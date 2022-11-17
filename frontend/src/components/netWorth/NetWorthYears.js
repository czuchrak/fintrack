import {Grid} from "@mui/material";
import NetWorthYearSummary from "./NetWorthYearSummary";
import NetWorthYearChart from "./NetWorthYearChart";
import {useMemo} from "react";
import {sortByDate} from "../../utils/helpers";
// ----------------------------------------------------------------------

export default function NetWorthYears({ data }) {
  const years = useMemo(() => {
    const entries = data.entries.slice().sort(sortByDate(-1));
    if (entries.length === 0) return [];

    const firstYear = new Date(entries[0].date).getFullYear();
    const lastYear = new Date(entries[entries.length - 1].date).getFullYear();

    const result = [];

    for (let year = firstYear; year <= lastYear; year++) {
      const yearEntries = entries.filter(
        (x) =>
          new Date(x.date).getFullYear() === year ||
          (new Date(x.date).getFullYear() === year + 1 &&
            new Date(x.date).getMonth() === 0)
      );

      if (yearEntries.length === 0) continue;

      const firstEntry = yearEntries[0];
      const lastEntry = yearEntries[yearEntries.length - 1];
      const firstEntryDate = new Date(firstEntry.date);
      const lastEntryDate = new Date(lastEntry.date);

      const months =
        (lastEntryDate.getFullYear() - firstEntryDate.getFullYear()) * 12 +
        lastEntryDate.getMonth() -
        firstEntryDate.getMonth();

      if (months === 0) continue;

      result.push({
        year,
        value: lastEntry.value - firstEntry.value,
        valuePercent:
          firstEntry.value === 0
            ? 0
            : (lastEntry.value - firstEntry.value) / firstEntry.value,
        valueAvg: ((lastEntry.value - firstEntry.value) / months).toFixed(2),
      });
    }

    return result;
  }, [data.entries]);

  return (
    years.length > 0 && (
      <>
        <Grid item xs={12} md={6}>
          <NetWorthYearSummary years={years} />
        </Grid>
        <Grid item xs={12} md={6}>
          <NetWorthYearChart years={years} />
        </Grid>
      </>
    )
  );
}
