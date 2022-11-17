import React, {useMemo} from "react";
import {styled} from "@mui/material/styles";
import {Card, Grid, Typography} from "@mui/material";
import {getCurrencyFormatter, onlyUnique, sortByDate,} from "../../utils/helpers";
import {DecimalLabel} from "../Label";

// ----------------------------------------------------------------------

const RootStyle = styled(Card)(({ theme }) => ({
  textAlign: "center",
  padding: theme.spacing(3, 0),
  height: "100%",
}));

export default function PropertySummary({ property }) {
  const categoryValues = useMemo(() => {
    const transCategories = property.transactions
      .map((x) => x.categoryId)
      .filter(onlyUnique);
    const categories = property.categories.filter((x) =>
      transCategories.includes(x.id)
    );

    return categories
      .map((cat) => {
        const transFilter = property.transactions
          .slice()
          .filter((x) => x.categoryId === cat.id);
        return {
          category: cat,
          trans: transFilter,
          sum: transFilter.reduce((partialSum, a) => partialSum + a.value, 0),
        };
      })
      .sort((a, b) => {
        if (a.sum > b.sum) return -1;
        else return 1;
      });
  }, [property]);

  const costSum = useMemo(() => {
    return categoryValues
      .slice()
      .filter((x) => x.category.isCost)
      .reduce((partialSum, a) => partialSum + a.sum, 0);
  }, [categoryValues]);

  const incomeSum = useMemo(() => {
    return categoryValues
      .slice()
      .filter((x) => !x.category.isCost)
      .reduce((partialSum, a) => partialSum + a.sum, 0);
  }, [categoryValues]);

  const investmentData = useMemo(() => {
    const buyCategory = categoryValues
      .slice()
      .find((x) => x.category.type === "buy");

    const data = {};

    if (buyCategory) {
      const startDate = new Date(
        buyCategory.trans.sort(sortByDate(-1))[0].date
      );
      const endDate = new Date(
        property.transactions.slice().sort(sortByDate())[0].date
      );

      const diff = endDate.getTime() - startDate.getTime();
      const days = diff / (1000 * 3600 * 24);

      if (days > 0) {
        data.rate = ((incomeSum / costSum) * 100).toFixed(2);
        data.rateYearly = ((incomeSum / costSum / days) * 365.25 * 100).toFixed(
          2
        );
      }
    }

    return data;
  }, [categoryValues, costSum, incomeSum, property]);

  const CategoryList = ({ isCost }) => {
    const values = categoryValues
      .slice()
      .filter((x) => x.category.isCost === isCost);

    let valuesLeft = values.slice(0, Math.ceil(values.length / 2));
    let valuesRight = values.slice(Math.ceil(values.length / 2), values.length);

    const Values = ({ values }) => {
      return (
        <>
          {values.map((obj) => {
            const { category, sum } = obj;
            return (
              <Grid container key={category.id} spacing={1}>
                <Grid item xs={6} textAlign={"right"}>
                  <Typography variant="caption" sx={{ opacity: 0.72 }}>
                    {category.name}
                  </Typography>
                </Grid>
                <Grid item xs={6} textAlign={"left"}>
                  <DecimalLabel
                    value={sum}
                    invert={category.isCost}
                    currency="PLN"
                  />
                </Grid>
              </Grid>
            );
          })}
        </>
      );
    };

    return (
      <>
        {values.length > 0 && (
          <>
            <Typography variant="subtitle2" sx={{ opacity: 0.72 }} mt={1}>
              {isCost ? "Koszty" : "Przychody"}
            </Typography>
            <Grid container px={2} mt={1}>
              <Grid item xs={12} sm={6}>
                <Values values={valuesLeft} />
              </Grid>
              <Grid item xs={12} sm={6}>
                <Values values={valuesRight} />
              </Grid>
            </Grid>
          </>
        )}
      </>
    );
  };

  return (
    <RootStyle>
      <Grid container mb={2} spacing={1}>
        {incomeSum > 0 && (
          <Grid item xs={6}>
            <Typography variant="h5">
              {getCurrencyFormatter("PLN").format(incomeSum)}
            </Typography>
            <Typography variant="subtitle2" sx={{ opacity: 0.72 }}>
              Suma przychodów
            </Typography>
          </Grid>
        )}
        <Grid item xs={incomeSum > 0 ? 6 : 12}>
          <Typography variant="h5">
            {getCurrencyFormatter("PLN").format(costSum)}
          </Typography>
          <Typography variant="subtitle2" sx={{ opacity: 0.72 }}>
            Suma kosztów
          </Typography>
        </Grid>
        {incomeSum > 0 && investmentData.rate && (
          <>
            <Grid item xs={6}>
              <Typography variant="h6">{investmentData.rate}%</Typography>
              <Typography variant="subtitle2" sx={{ opacity: 0.72 }}>
                Stopa zwrotu
              </Typography>
            </Grid>
            <Grid item xs={6}>
              <Typography variant="h6">{investmentData.rateYearly}%</Typography>
              <Typography variant="subtitle2" sx={{ opacity: 0.72 }}>
                Stopa zwrotu rocznie
              </Typography>
            </Grid>
          </>
        )}
      </Grid>

      <CategoryList isCost={false} />
      <CategoryList isCost={true} />
    </RootStyle>
  );
}
