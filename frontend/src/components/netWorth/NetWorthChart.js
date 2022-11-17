import {useMemo, useState} from "react";
import {Box, Card, Grid, Stack, Typography} from "@mui/material";
import {handleChartRange, sortByDate} from "src/utils/helpers";
import BasicChart from "../charts/BasicChart";
import {useTheme} from "@mui/material/styles";
import {ChartRangeInput} from "../form/Inputs";
import {useSelector} from "react-redux";
import HourglassBottomRoundedIcon from "@mui/icons-material/HourglassBottomRounded";
// ----------------------------------------------------------------------

export default function NetWorthChart({ data, currentDate }) {
  const theme = useTheme();
  const [chartRange, setChartRange] = useState("last12months");
  let { currency } = useSelector((state) => state.profile);

  const handleRangeChange = (e) => {
    setChartRange(e.target.value);
  };

  const basicChartData = useMemo(() => {
    const entries = data.entries.slice().sort(sortByDate());

    const getData = (field) => {
      return entries
        .filter((x) => handleChartRange(x, chartRange, currentDate))
        .map((y) => ({
          x: y.date,
          y: y[field],
          currency: currency,
        }));
    };

    return [
      {
        id: "Wartość netto",
        data: getData("value"),
      },
      {
        id: "Zobowiązania",
        data: getData("liabilities"),
      },
      {
        id: "Aktywa",
        data: getData("assets"),
      },
    ];
  }, [chartRange, currency, currentDate, data.entries]);

  return (
    data.entries.length > 1 && (
      <Grid item xs={12}>
        <Card sx={{ p: 3 }}>
          <Stack
            direction="row"
            alignItems="center"
            justifyContent="space-between"
          >
            <Grid container>
              <Grid item xs={12} sm={6}>
                <Typography variant="h4" gutterBottom>
                  Twoja wartość netto
                </Typography>
              </Grid>
              <Grid item xs={12} sm={6} textAlign="right">
                <ChartRangeInput
                  chartRange={chartRange}
                  handleRangeChange={handleRangeChange}
                />
              </Grid>
            </Grid>
          </Stack>
          <Box sx={{ height: "400px" }}>
            {basicChartData[0].data.length < 2 ? (
              <>
                <Typography variant="h6" textAlign="center" sx={{ pt: 10 }}>
                  <HourglassBottomRoundedIcon
                    sx={{ fontSize: "100px", opacity: 0.5, mt: 1 }}
                  />
                  <br />
                  Za mało danych do wyświetlenia wykresu.
                </Typography>
              </>
            ) : (
              <BasicChart
                data={basicChartData}
                stacked={false}
                area={false}
                showZero={true}
                colors={[
                  theme.palette.secondary.main,
                  theme.palette.error.main,
                  theme.palette.success.main,
                ]}
              />
            )}
          </Box>
        </Card>
      </Grid>
    )
  );
}
