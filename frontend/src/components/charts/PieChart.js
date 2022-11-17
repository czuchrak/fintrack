import React from "react";
import {ResponsivePie} from "@nivo/pie";
import {getCurrencyFormatter} from "src/utils/helpers";
import {useTheme} from "@mui/material/styles";
import {Card, Grid, Typography} from "@mui/material";
import {fPercent} from "src/utils/formatNumber";

export default function PieChart({ data, all }) {
  const theme = useTheme();

  return (
    <>
      <ResponsivePie
        data={data}
        margin={{ right: 40, left: 40, top: 20, bottom: 20 }}
        sortByValue={true}
        innerRadius={0.5}
        padAngle={2}
        cornerRadius={5}
        fit={false}
        activeOuterRadiusOffset={8}
        colors={[
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
        ]}
        borderWidth={1}
        borderColor={{ from: "color", modifiers: [["darker", 0.2]] }}
        enableArcLabels={true}
        arcLabel={function (e) {
          return fPercent((e.value / all) * 100);
        }}
        arcLabelsTextColor={{ from: "color", modifiers: [["brighter", "3"]] }}
        arcLabelsSkipAngle={20}
        enableArcLinkLabels={false}
        arcLinkLabelsSkipAngle={15}
        arcLinkLabelsTextOffset={5}
        arcLinkLabelsDiagonalLength={10}
        arcLinkLabelsStraightLength={1}
        transitionMode="pushIn"
        tooltip={({ datum }) => (
          <Card>
            <Grid container spacing={0} justifyContent="center">
              <Grid item xs={12} textAlign="center">
                <Typography variant="subtitle2" color={datum.color}>
                  {datum.id}
                </Typography>
              </Grid>
              <Grid item xs={12} textAlign="center">
                <Typography variant="subtitle2">
                  {getCurrencyFormatter(datum.data.currency).format(
                    datum.data.valueNoRate
                  )}
                </Typography>

                <Typography variant="caption">
                  ({fPercent(datum.data.share * 100)})
                </Typography>
              </Grid>
            </Grid>
          </Card>
        )}
      />
    </>
  );
}
