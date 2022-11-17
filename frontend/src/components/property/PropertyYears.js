import {Grid} from "@mui/material";
import {onlyUnique, sortByDate} from "../../utils/helpers";
import {useMemo} from "react";
import PropertyYearSummary from "./PropertyYearSummary";
import PropertyYearChart from "./PropertyYearChart";

export default function PropertyYears({ property }) {
  const categories = useMemo(() => {
    const transCategories = property.transactions
      .map((x) => x.categoryId)
      .filter(onlyUnique);
    return property.categories.filter((x) => transCategories.includes(x.id));
  }, [property]);

  const buyCategoryId = useMemo(() => {
    return property.categories.find((x) => x.type === "buy").id;
  }, [property]);

  const buySum = useMemo(() => {
    return property.transactions
      .filter((x) => x.categoryId === buyCategoryId)
      .reduce((x, y) => x + y.value, 0);
  }, [buyCategoryId, property.transactions]);

  const data = useMemo(() => {
    const transactions = property.transactions.slice().sort(sortByDate(-1));
    if (transactions.length === 0) return [];

    const firstTran = transactions[0];
    const lastTran = transactions[transactions.length - 1];

    const firstYear = new Date(firstTran.date).getFullYear();
    const lastYear = new Date(lastTran.date).getFullYear();

    const result = [];

    for (let year = firstYear; year <= lastYear; year++) {
      const yearTransactions = transactions.filter(
        (x) => new Date(x.date).getFullYear() === year
      );
      if (yearTransactions.length === 0) continue;

      const costs = yearTransactions
        .filter((x) =>
          categories
            .filter((y) => y.isCost && y.id !== buyCategoryId)
            .map((y) => y.id)
            .includes(x.categoryId)
        )
        .reduce((x, y) => x + y.value, 0);

      const incomes = yearTransactions
        .filter((x) =>
          categories
            .filter((y) => !y.isCost)
            .map((y) => y.id)
            .includes(x.categoryId)
        )
        .reduce((x, y) => x + y.value, 0);

      const balance = incomes - costs;
      const rate = buySum === 0 ? 0 : balance / buySum;

      result.push({ year, costs, incomes, balance, rate });
    }

    return result;
  }, [buyCategoryId, buySum, categories, property.transactions]);

  const isIncome = useMemo(() => {
    for (let i = 0; i < data.length; i++) {
      if (data[i].incomes > 0) return true;
    }
    return false;
  }, [data]);

  return (
    isIncome && (
      <>
        <Grid item xs={12} md={6}>
          <PropertyYearSummary data={data} />
        </Grid>
        <Grid item xs={12} md={6}>
          <PropertyYearChart data={data} />
        </Grid>
      </>
    )
  );
}
