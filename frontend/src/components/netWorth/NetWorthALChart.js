import {useMemo, useState} from "react";
import {Box, Card, Grid, Stack, Typography} from "@mui/material";
import {handleChartRange, sortByDate} from "src/utils/helpers";
import BasicChart from "../charts/BasicChart";
import {ChartRangeInput} from "../form/Inputs";
import HourglassBottomRoundedIcon from "@mui/icons-material/HourglassBottomRounded";

export default function NetWorthALChart({ data, type, currentDate }) {
  const [chartRange, setChartRange] = useState("last12months");

  const handleRangeChange = (e) => {
    setChartRange(e.target.value);
  };
  const areaChartData = useMemo(() => {
    const entries = data.entries.slice().sort(sortByDate());

    return data.parts
      .filter((x) => x.type === type)
      .map((x) => ({
        id: x.name,
        data: entries
          .filter((x) => handleChartRange(x, chartRange, currentDate))
          .map((y) => ({
            x: y.date,
            y: y.partValues[x.id].value,
            valueRate: y.partValues[x.id].valueRate,
            currency: x.currency,
          })),
      }));
  }, [chartRange, currentDate, data, type]);

  return (
    <Card sx={{ p: 3 }}>
      <Stack direction="row" alignItems="center" justifyContent="space-between">
        <Grid container>
          <Grid item xs={12} sm={6}>
            <Typography variant="h4" gutterBottom>
              {type === "asset" ? "Twoje aktywa" : "Twoje zobowiązania"}
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
        {areaChartData.length === 0 || areaChartData[0].data.length < 2 ? (
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
            data={areaChartData}
            stacked={true}
            area={true}
            showZero={false}
            sum={true}
          />
        )}
      </Box>
    </Card>
  );
}
