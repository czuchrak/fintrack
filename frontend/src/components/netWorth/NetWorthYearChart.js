import {Box, Card} from "@mui/material";
import BarChart from "../charts/BarChart";
import {useTheme} from "@mui/material/styles";
import {useMemo} from "react";
import {useSelector} from "react-redux";

// ----------------------------------------------------------------------

export default function NetWorthYearChart({ years }) {
  let { currency } = useSelector((state) => state.profile);
  const theme = useTheme();

  const barChartData = useMemo(() => {
    const result = [];

    years
      .slice()
      .reverse()
      .forEach((x) => {
        result.push({
          year: x.year,
          value: x.value,
          valueColor:
            x.value >= 0
              ? theme.palette.primary.main
              : theme.palette.error.main,
        });
      });

    return result;
  }, [years, theme.palette]);

  return (
    <Card sx={{ p: 3 }}>
      <Box sx={{ height: "300px" }}>
        <BarChart data={barChartData} currency={currency} />
      </Box>
    </Card>
  );
}
