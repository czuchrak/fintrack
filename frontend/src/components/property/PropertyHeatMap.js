import HeatMapWrapper from "../charts/HeatMap";
import {useEffect, useMemo, useState} from "react";
import {onlyUnique, sortByDate} from "../../utils/helpers";
import {Checkbox, Grid, ListItemIcon, ListItemText, MenuItem, Typography,} from "@mui/material";
import {MultipleSelect} from "../form/Inputs";

export function PropertyHeatMap({ property }) {
  const [selected, setSelected] = useState([]);

  const categories = useMemo(() => {
    const transCategories = property.transactions
      .map((x) => x.categoryId)
      .filter(onlyUnique);
    return property.categories
      .filter((x) => transCategories.includes(x.id))
      .sort((a, b) => a.name.localeCompare(b.name));
  }, [property]);

  const isAllSelected = useMemo(() => {
    return categories.length > 0 && selected.length === categories.length;
  }, [categories.length, selected.length]);

  const handleChange = (event) => {
    const value = event.target.value;
    if (value[value.length - 1] === "all") {
      setSelected(
        selected.length === categories.length
          ? []
          : categories.map((x) => x.name)
      );
      return;
    }
    setSelected(value);
  };

  useEffect(() => {
    setSelected(categories.map((x) => x.name));
  }, [categories]);

  const diffs = useMemo(() => {
    const transactions = property.transactions.slice().sort(sortByDate(-1));
    if (transactions.length === 0) return [];

    const firstTran = transactions[0];
    const lastTran = transactions[transactions.length - 1];

    const currentDate = new Date(new Date(firstTran.date).getFullYear(), 0, 1);
    const maxDate = new Date(new Date(lastTran.date).getFullYear() + 1, 0, 1);

    let result = [];
    while (currentDate < maxDate) {
      const month = currentDate.getMonth();
      const year = currentDate.getFullYear();

      const trans = transactions.filter(
        (x) =>
          new Date(x.date).getFullYear() === year &&
          new Date(x.date).getMonth() === month
      );

      let value = 0;

      if (trans.length === 0) value = null;
      else {
        const costs = trans
          .filter((x) =>
            categories
              .filter((y) => y.isCost && selected.includes(y.name))
              .map((y) => y.id)
              .includes(x.categoryId)
          )
          .reduce((x, y) => x + y.value, 0);
        const incomes = trans
          .filter((x) =>
            categories
              .filter((y) => !y.isCost && selected.includes(y.name))
              .map((y) => y.id)
              .includes(x.categoryId)
          )
          .reduce((x, y) => x + y.value, 0);

        if (incomes === 0 && costs === 0) value = null;
        else value = incomes - costs;
      }

      result.push({ month: month + 1, year, value });

      currentDate.setMonth(month + 1);
    }

    return result;
  }, [property, selected, categories]);

  return (
    <>
      <HeatMapWrapper
        diffs={diffs}
        header={
          <>
            <Grid item xs={12} sm={6}>
              <Typography variant="h4" gutterBottom>
                Bilans miesięczny
              </Typography>
            </Grid>
            <Grid item xs={12} sm={6} textAlign="right">
              <MultipleSelect
                selected={selected}
                handleChangeSelected={handleChange}
                render={(selected) => {
                  const count = selected.length;
                  if (count === 0 || count > 4) {
                    return `${selected.length} kategorii`;
                  } else if (count === 1) {
                    return `${selected.length} kategoria`;
                  }
                  return `${selected.length} kategorie`;
                }}
              >
                {categories.length > 0 && (
                  <MenuItem value="all">
                    <ListItemIcon>
                      <Checkbox
                        checked={isAllSelected}
                        indeterminate={
                          selected.length > 0 &&
                          selected.length < categories.length
                        }
                      />
                    </ListItemIcon>
                    <ListItemText primary="Zaznacz wszystko" />
                  </MenuItem>
                )}
                {categories.map((cat) => (
                  <MenuItem key={cat.id} value={cat.name}>
                    <ListItemIcon>
                      <Checkbox checked={selected.indexOf(cat.name) > -1} />
                    </ListItemIcon>
                    <ListItemText primary={cat.name} />
                  </MenuItem>
                ))}
              </MultipleSelect>
            </Grid>
          </>
        }
      />
    </>
  );
}
