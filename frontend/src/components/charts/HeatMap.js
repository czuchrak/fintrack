import {ResponsiveHeatMap} from "@nivo/heatmap";
import {getCurrencyFormatter, getFullMonthNameFromMonth3, getMonthName,} from "src/utils/helpers";
import {Box, Card, Grid, Stack, Typography} from "@mui/material";
import {useMemo} from "react";
import {useSelector} from "react-redux";

const keys = [
  "sty",
  "lut",
  "mar",
  "kwi",
  "maj",
  "cze",
  "lip",
  "sie",
  "wrz",
  "paź",
  "lis",
  "gru",
];

const greens = [
  "#f9fffc",
  "#bfffdf",
  "#84ffc1",
  "#49ffa3",
  "#0eff86",
  "#00f97c",
  "#00e672",
  "#00d269",
  "#00bf5f",
  "#00ab55",
];

const reds = [
  "#fff3f3",
  "#ffbab8",
  "#ff817d",
  "#ff928f",
  "#ff7975",
  "#ff615c",
  "#ff4842",
  "#ff0f07",
  "#f30800",
  "#df0700",
];

export default function HeatMapWrapper({ diffs, header }) {
  function getColor(value, min, max) {
    if (value === null) {
      return "#D3D3D3";
    } else if (value < 0) {
      if (min === 0) return reds[0];
      let index = ((value / min) * (reds.length - 1)).toFixed(0);
      return reds[index];
    } else {
      if (max === 0) return greens[0];
      let index = ((value / max) * (greens.length - 1)).toFixed(0);
      return greens[index];
    }
  }

  function getMonth(month) {
    const date = new Date();
    date.setMonth(month - 1);
    return getMonthName(month);
  }

  const heatMapData = useMemo(() => {
    const min = Math.min.apply(
      null,
      diffs.map((item) => item.value)
    );
    const max = Math.max.apply(
      null,
      diffs.map((item) => item.value)
    );

    const result = [];

    diffs.forEach((x) => {
      let q = result.find((y) => y.year === x.year);
      if (q === undefined) result.push({ year: x.year });
      q = result.find((y) => y.year === x.year);
      q[getMonth(x.month)] = x.value;
      q[getMonth(x.month) + "Color"] = getColor(x.value, min, max);
    });

    return result;
  }, [diffs]);

  return (
    <Card sx={{ p: 3 }}>
      <Stack direction="row" alignItems="center" justifyContent="space-between">
        <Grid container sx={{ mb: 2 }}>
          {header}
        </Grid>
      </Stack>
      <Box sx={{ height: `${heatMapData.length * 60}px` }}>
        <HeatMap data={heatMapData.reverse()} />
      </Box>
    </Card>
  );
}

export function HeatMap({ data }) {
  let { currency } = useSelector((state) => state.profile);

  const scale = useMemo(() => {
    const colors = data
      .map((item) => keys.map((key) => item[`${key}Color`]))
      .flat();

    function result() {}

    result.domain = () => {
      const _colors = colors.slice(0);

      return () => {
        return _colors.shift();
      };
    };

    return result;
  }, [data]);

  return (
    <>
      <ResponsiveHeatMap
        data={data}
        keys={keys}
        indexBy="year"
        colors={scale}
        margin={{ top: 20, right: 10, bottom: 0, left: 35 }}
        axisTop={{
          orient: "top",
          tickSize: 5,
          tickPadding: 5,
          tickRotation: 0,
          legend: "",
          legendOffset: 36,
        }}
        axisRight={null}
        axisBottom={null}
        axisLeft={{
          orient: "left",
          tickSize: 5,
          tickPadding: 5,
          tickRotation: 0,
        }}
        cellOpacity={1}
        cellBorderWidth={1}
        cellBorderColor="#ffffff"
        label={(datum, key) => {
          let value = datum[key];
          return value === null
            ? ""
            : getCurrencyFormatter(currency).format(value);
        }}
        labelTextColor={{ from: "color", modifiers: [["darker", "3"]] }}
        defs={[
          {
            id: "lines",
            type: "patternLines",
            background: "inherit",
            color: "rgba(0, 0, 0, 0.1)",
            rotation: -45,
            lineWidth: 4,
            spacing: 7,
          },
        ]}
        fill={[{ id: "lines" }]}
        animate={true}
        hoverTarget="rowColumn"
        cellHoverOthersOpacity={0.2}
        tooltip={({ xKey, yKey, value, color }) => (
          <Card>
            <Grid container spacing={0} justifyContent="center">
              <Grid item xs={12} textAlign="center" sx={{ opacity: 0.72 }}>
                <Typography variant="subtitle2">
                  {getFullMonthNameFromMonth3(xKey)} {yKey}
                </Typography>
              </Grid>
              <Grid item xs={12} textAlign="center">
                <Typography
                  variant="subtitle2"
                  color={
                    value === null ? color : value < 0 ? "#ff4842" : "#00ab55"
                  }
                >
                  {value === null
                    ? "Brak danych"
                    : getCurrencyFormatter(currency).format(value)}
                </Typography>
              </Grid>
            </Grid>
          </Card>
        )}
        theme={{
          tooltip: {
            container: {
              background: "none",
              boxShadow: "none",
            },
          },
          labels: {
            text: {
              display: window.innerWidth < 1200 ? "none" : "block",
              fontWeight: "bold",
            },
          },
        }}
      />
    </>
  );
}
