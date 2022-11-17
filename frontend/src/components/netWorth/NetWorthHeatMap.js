import HeatMapWrapper from "../charts/HeatMap";
import {Grid, Typography} from "@mui/material";
import {useMemo} from "react";
import {getDate, sortByDate} from "../../utils/helpers";

export function NetWorthHeatMap({ data }) {
  const diffs = useMemo(() => {
    const entries = data.entries.slice().sort(sortByDate(-1));
    if (entries.length === 0) return [];

    const firstEntry = entries[0];
    const lastEntry = entries[entries.length - 1];

    const firstEntryDate = new Date(firstEntry.date);
    const lastEntryDate = new Date(lastEntry.date);

    let currentDate = new Date(firstEntryDate.getFullYear(), 0, 1);
    const maxDate = new Date(lastEntryDate.getFullYear() + 1, 0, 1);

    let result = [];

    while (currentDate < maxDate) {
      const nextDate = new Date(
        currentDate.getFullYear(),
        currentDate.getMonth(),
        1
      );
      nextDate.setMonth(nextDate.getMonth() + 1);

      const currentEntry = entries.find((x) => x.date === getDate(currentDate));
      const nextEntry = entries.find((x) => x.date === getDate(nextDate));

      let value = 0;

      if (!currentEntry || !nextEntry) value = null;
      else value = nextEntry.value - currentEntry.value;

      result.push({
        year: currentDate.getFullYear(),
        month: currentDate.getMonth() + 1,
        value,
      });

      currentDate.setMonth(currentDate.getMonth() + 1);
    }

    for (
      let year = firstEntryDate.getFullYear();
      year <= lastEntryDate.getFullYear();
      year++
    ) {
      const validCells = result.filter(
        (x) => x.year === year && x.value !== null
      );
      if (validCells.length === 0) {
        result = result.filter((x) => x.year !== year);
      }
    }

    return result;
  }, [data.entries]);

  return (
    diffs.length > 0 && (
      <Grid item xs={12}>
        <HeatMapWrapper
          diffs={diffs}
          header={
            <Grid item xs={12} sm={6}>
              <Typography variant="h4" gutterBottom>
                Zmiany wartości netto
              </Typography>
            </Grid>
          }
        />
      </Grid>
    )
  );
}
