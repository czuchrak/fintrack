import {useEffect, useMemo, useState} from "react";
import {IconButton, TableCell, TableRow, Tooltip} from "@mui/material";
import {DecimalLabel} from "src/components/Label";
import {getCurrencyFormatter, getCurrencySymbol, getMonth3AndYear, sortByDate, sortByOrder,} from "src/utils/helpers";
import NetWorthDataSidebar from "src/components/netWorth/NetWorthDataSidebar";
import {useDispatch, useSelector} from "react-redux";
import {addError} from "src/redux/slices/errorSlice";
import SimpleTable from "../../components/SimpleTable";
import MoreMenu from "../../components/utilities/MoreMenu";
import useNetWorthServiceActions from "../../serviceActions/NetWorthServiceActions";
import InternalPage from "../../components/InternalPage";
import eyeFill from "@iconify/icons-eva/eye-fill";
import eyeOffFill from "@iconify/icons-eva/eye-off-fill";
import {Icon} from "@iconify/react";
import EmptyState from "../../components/utilities/EmptyState";
import AccountBalanceWalletRoundedIcon from "@mui/icons-material/AccountBalanceWalletRounded";
import LocalAtmRoundedIcon from "@mui/icons-material/LocalAtmRounded";

export default function NetWorthData() {
  const dispatch = useDispatch();
  const netWorthServiceActions = useNetWorthServiceActions();

  const { data, dataFetched, isLoading, error } = useSelector(
    (state) => state.netWorth
  );

  let { currency } = useSelector((state) => state.profile);

  dispatch(addError(error));

  const [showAll, setShowAll] = useState(false);
  const [openForm, setOpenForm] = useState(false);
  const [entry, setEntry] = useState({});

  const handleShowAll = () => {
    setShowAll(!showAll);
  };

  const handleOpenForm = () => {
    setEntry({});
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setEntry({});
  };

  const handleChangePart = (entry) => {
    setEntry(entry);
    setOpenForm(true);
  };

  const lastEntry = useMemo(() => {
    return dataFetched && data.entries.slice().sort(sortByDate())[0];
  }, [data.entries, dataFetched]);

  const showAllIcon = useMemo(() => {
    return (
      dataFetched &&
      data.entries.length > 0 &&
      data.parts.some((x) => !x.isVisible)
    );
  }, [data, dataFetched]);

  const emptyState = () => {
    if (!data.parts || data.parts.length === 0) {
      return (
        <EmptyState
          icon={AccountBalanceWalletRoundedIcon}
          text="Nie masz dodanych żadnych składników majątku."
          buttonText="Przejdź do składników"
          buttonUrl="/networth/parts"
        />
      );
    } else
      return (
        <EmptyState
          icon={LocalAtmRoundedIcon}
          text="Nie masz dodanych żadnych wartości majątku."
          bodyText={
            <span>
              W&nbsp;tym miejscu raz w&nbsp;miesiącu podawaj aktualną wartość
              składników Twojego majątku.
              <br />
              <br />
              Ukryte kolumny zobaczysz po&nbsp;kliknięciu{" "}
              <strong>Pokaż wszystkie</strong> w&nbsp;prawym górnym rogu.
              <br />
              Ta&nbsp;możliwość pojawia się tylko wtedy, gdy istnieje
              co&nbsp;najmniej jedna ukryta kolumna.
              <br />
              <br />
              Podczas dodawania lub edytowania wierszy zapisywane są tylko
              aktualnie widoczne pola formularza.
            </span>
          }
          buttonText="Uzupełnij aktualne wartości majątku"
          buttonOnClick={handleOpenForm}
        />
      );
  };

  useEffect(() => {
    if (!dataFetched) {
      netWorthServiceActions.getData().then();
    }
  }, [dataFetched, netWorthServiceActions]);

  return (
    <InternalPage
      title="Wartości"
      isLoading={isLoading}
      handleOpenForm={handleOpenForm}
      showEmptyState={
        !data.parts ||
        data.parts.length === 0 ||
        !data.entries ||
        data.entries.length === 0
      }
      emptyState={emptyState()}
      actionPrefix={
        showAllIcon && (
          <Tooltip
            title={showAll ? "Schowaj ukryte" : "Pokaż ukryte"}
            placement="bottom"
            sx={{ mr: 2 }}
          >
            <IconButton onClick={handleShowAll} data-cy="showAll">
              <Icon icon={showAll ? eyeFill : eyeOffFill} />
            </IconButton>
          </Tooltip>
        )
      }
    >
      {dataFetched && (
        <>
          {data.entries.length > 0 && (
            <SimpleTable
              height={530}
              headers={
                <>
                  <TableCell align="center">Miesiąc</TableCell>
                  <TableCell align="center">Wartość netto</TableCell>
                  {data.parts
                    .filter((x) => showAll || x.isVisible)
                    .sort(sortByOrder())
                    .map((part) => (
                      <TableCell key={part.id} align="center">
                        <Tooltip
                          title={
                            part.type === "asset" ? "Aktywo" : "Zobowiązanie"
                          }
                          placement="top"
                        >
                          <strong>{part.name}</strong>
                        </Tooltip>
                      </TableCell>
                    ))}
                  <TableCell />
                </>
              }
              data={data.entries.slice().sort(sortByDate())}
              mapping={(entry) => {
                const { id, date, value, partValues } = entry;

                return (
                  <TableRow hover key={id} tabIndex={-1}>
                    <TableCell align="center">
                      {getMonth3AndYear(date)}
                    </TableCell>
                    <TableCell align="center">
                      <DecimalLabel value={value} currency={currency} />
                    </TableCell>
                    {data.parts
                      .slice()
                      .sort(sortByOrder())
                      .map((part) => {
                        return showAll || part.isVisible ? (
                          <TableCell key={part.id} align="center">
                            {part.currency === "PLN" ? (
                              getCurrencyFormatter(part.currency).format(
                                partValues[part.id].value
                              )
                            ) : (
                              <Tooltip
                                title={`1${getCurrencySymbol(
                                  part.currency
                                )} = ${getCurrencyFormatter(currency, 4).format(
                                  partValues[part.id].rate
                                )} [${entry.exchangeRateDate.substring(
                                  0,
                                  10
                                )}]`}
                                placement="top"
                              >
                                <span>
                                  {getCurrencyFormatter(part.currency).format(
                                    partValues[part.id].value
                                  )}
                                </span>
                              </Tooltip>
                            )}
                          </TableCell>
                        ) : null;
                      })}
                    <TableCell align="center">
                      <MoreMenu
                        onEdit={() => handleChangePart(entry)}
                        onDelete={() =>
                          netWorthServiceActions.deleteEntry(entry.id)
                        }
                      />
                    </TableCell>
                  </TableRow>
                );
              }}
              paging={[12, 24, 48]}
            />
          )}
          <NetWorthDataSidebar
            isOpenForm={openForm}
            onCloseForm={handleCloseForm}
            parts={data.parts}
            entry={entry}
            dates={data.entries.map((x) => x.date)}
            lastEntry={lastEntry}
          />
        </>
      )}
    </InternalPage>
  );
}
