import React from "react";
import {ResponsiveLine} from "@nivo/line";
import {getCurrencyFormatter, getMonth3AndYear, getMonthAndYear,} from "src/utils/helpers";
import {Card, Grid, Typography} from "@mui/material";
import {styled, useTheme} from "@mui/material/styles";
import {fShortenNumber} from "src/utils/formatNumber";
import {useSelector} from "react-redux";

const Tooltip = styled(Card)(({ theme }) => ({
  padding: theme.spacing(1),
  maxWidth: 200,
  [theme.breakpoints.up("md")]: {
    maxWidth: 250,
  },
  [theme.breakpoints.up("lg")]: {
    maxWidth: 300,
  },
}));

export default function BasicChart({ data, area, sum, showZero, colors }) {
  const theme = useTheme();

  let { currency } = useSelector((state) => state.profile);

  return (
    <>
      <ResponsiveLine
        data={data}
        margin={{ top: 30, right: 10, bottom: 60, left: 40 }}
        xScale={{
          type: "time",
          format: "%Y-%m-%d",
          useUTC: false,
          precision: "day",
        }}
        xFormat="time:%Y-%m-%d"
        yScale={{
          type: "linear",
          stacked: area,
          reverse: false,
          min: area ? 0 : "auto",
          max: "auto",
        }}
        axisLeft={{
          orient: "left",
          tickSize: 6,
          tickPadding: 5,
          legendOffset: -40,
          legendPosition: "middle",
          format: (value) => fShortenNumber(value),
        }}
        axisBottom={{
          format: (value) => getMonth3AndYear(value),
          tickValues: "every 1 month",
          tickSize: 5,
          legendOffset: -12,
          tickRotation: -45,
          tickPadding: 10,
        }}
        lineWidth={area ? 2 : 5}
        curve="linear"
        enableGridX={false}
        colors={
          colors || [
            theme.palette.warning.main,
            theme.palette.primary.main,
            theme.palette.secondary.main,
            theme.palette.error.main,
            theme.palette.warning.light,
            theme.palette.primary.light,
            theme.palette.secondary.light,
            theme.palette.error.light,
            theme.palette.warning.dark,
            theme.palette.primary.dark,
            theme.palette.secondary.dark,
            theme.palette.error.dark,
            theme.palette.warning.lighter,
            theme.palette.primary.lighter,
            theme.palette.secondary.lighter,
            theme.palette.error.lighter,
            theme.palette.warning.darker,
            theme.palette.primary.darker,
            theme.palette.secondary.darker,
            theme.palette.error.darker,
          ]
        }
        enablePoints={!area}
        pointSize={11}
        pointBorderColor={{ from: "serieColor" }}
        enableArea={area}
        areaOpacity={1}
        enableCrosshair={true}
        enableSlices="x"
        useMesh={true}
        animate={true}
        sliceTooltip={({ slice }) => {
          return (
            <Tooltip>
              <Grid container>
                <Grid
                  item
                  xs={12}
                  sx={{ mb: 1, textAlign: "center", opacity: 0.72 }}
                >
                  <Typography variant="subtitle2">
                    {getMonthAndYear(slice.points[0].data.x)}
                  </Typography>
                </Grid>
                <Grid container>
                  {slice.points
                    .filter((x) => showZero || x.data.y !== 0)
                    .map((point) => (
                      <Grid container key={point.serieId}>
                        <Grid item xs={6}>
                          <Typography
                            variant="subtitle2"
                            noWrap
                            color={point.serieColor}
                          >
                            {point.serieId}
                          </Typography>
                        </Grid>
                        <Grid item xs={6} textAlign="right">
                          <Typography variant="subtitle2">
                            {getCurrencyFormatter(point.data.currency).format(
                              point.data.yFormatted
                            )}
                          </Typography>
                        </Grid>
                      </Grid>
                    ))}
                  {sum && (
                    <>
                      <Grid item xs={6} />
                      <Grid
                        item
                        xs={6}
                        textAlign="right"
                        sx={{ borderTop: "1px solid" }}
                      >
                        <Typography variant="subtitle2">
                          {getCurrencyFormatter(currency).format(
                            slice.points.reduce(
                              (acc, curr) => acc + curr.data.valueRate,
                              0
                            )
                          )}
                        </Typography>
                      </Grid>
                    </>
                  )}
                </Grid>
              </Grid>
            </Tooltip>
          );
        }}
      />
    </>
  );
}
