import {Box, Card} from "@mui/material";
import BarChart from "../charts/BarChart";
import {useTheme} from "@mui/material/styles";
import {useMemo} from "react";

export default function PropertyYearChart({ data }) {
  const theme = useTheme();

  const barChartData = useMemo(() => {
    const result = [];

    data
      .slice()
      .reverse()
      .forEach((x) => {
        result.push({
          year: x.year,
          value: x.balance,
          valueColor:
            x.balance >= 0
              ? theme.palette.primary.main
              : theme.palette.error.main,
        });
      });

    return result;
  }, [data, theme.palette]);

  return (
    <Card sx={{ p: 3 }}>
      <Box sx={{ height: "300px" }}>
        <BarChart data={barChartData} currency="PLN" />
      </Box>
    </Card>
  );
}
