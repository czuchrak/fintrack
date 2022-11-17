import {Card, IconButton, TableCell, TableRow, TextField, Tooltip,} from "@mui/material";
import {getCurrencyFormatter, getFullDateInText, sortByDate,} from "../../utils/helpers";
import {DecimalLabel} from "../Label";
import SimpleTable from "../SimpleTable";
import {useEffect, useMemo, useRef, useState} from "react";
import MoreMenu from "../utilities/MoreMenu";
import usePropertyTransactionsServiceActions from "../../serviceActions/PropertyTransactionsServiceActions";
import {Icon} from "@iconify/react/dist/iconify";
import searchFill from "@iconify/icons-eva/search-fill";

// ----------------------------------------------------------------------

export default function PropertyTransactions({
  property,
  handleChangePropertyTransaction,
}) {
  const propertyTransactionsServiceActions =
    usePropertyTransactionsServiceActions();
  const [filter, setFilter] = useState("");
  const [search, setSearch] = useState(false);
  const inputRef = useRef(null);

  const transactions = useMemo(() => {
    const fil = filter.toLowerCase();
    const cats = property.categories;

    return property.transactions
      .slice()
      .filter((x) => {
        const value = getCurrencyFormatter("PLN").format(x.value);
        const date = getFullDateInText(x.date);
        const categoryName = cats.find((y) => y.id === x.categoryId).name;
        return (
          !fil ||
          categoryName.toString().toLowerCase().includes(fil) ||
          date.toLowerCase().includes(fil) ||
          value.toLowerCase().includes(fil) ||
          (x.details && x.details.toLowerCase().includes(fil))
        );
      })
      .sort(sortByDate());
  }, [property, filter]);

  const toggleSearch = () => {
    setSearch(!search);
    setFilter("");
    if (!search) setTimeout(() => inputRef.current.focus(), 100);
  };

  useEffect(() => {
    setSearch(false);
    setFilter("");
  }, [property]);

  return (
    <Card sx={{ textAlign: "right" }}>
      {search && (
        <TextField
          inputRef={inputRef}
          variant="standard"
          placeholder="Szukaj..."
          value={filter}
          onChange={(e) => setFilter(e.target.value)}
          sx={{ m: 2, textAlign: "right" }}
        />
      )}
      <SimpleTable
        height={400}
        minWidth={0}
        card={false}
        headers={
          <>
            <TableCell align="center">Data</TableCell>
            <TableCell align="center">Transakcja</TableCell>
            <TableCell align="center">Kwota</TableCell>
            <TableCell align="center">Uwagi</TableCell>
            <TableCell align="center">
              <Tooltip title="Przeszukaj" placement="bottom">
                <IconButton onClick={toggleSearch}>
                  <Icon icon={searchFill} />
                </IconButton>
              </Tooltip>
            </TableCell>
          </>
        }
        data={transactions}
        mapping={(transaction) => {
          const { id, date, categoryId, value, details } = transaction;
          const categories = property.categories;
          const category = categories.find((x) => x.id === categoryId);

          return (
            <TableRow hover key={id} tabIndex={-1}>
              <TableCell align="center">{getFullDateInText(date)}</TableCell>
              <TableCell align="center">{category.name}</TableCell>
              <TableCell align="center">
                <DecimalLabel
                  value={value}
                  invert={category.isCost}
                  currency="PLN"
                />
              </TableCell>
              <TableCell align="center">{details}</TableCell>
              <TableCell align="center">
                <MoreMenu
                  onEdit={() => handleChangePropertyTransaction(transaction)}
                  onDelete={() =>
                    propertyTransactionsServiceActions.deletePropertyTrans(
                      transaction.propertyId,
                      transaction.id
                    )
                  }
                />
              </TableCell>
            </TableRow>
          );
        }}
        paging={[10, 25, 50]}
      />
    </Card>
  );
}
