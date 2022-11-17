import React from "react";
import {ResponsiveBar} from "@nivo/bar";
import {getCurrencyFormatter} from "src/utils/helpers";
import {Card, Grid, Typography} from "@mui/material";
import {styled} from "@mui/material/styles";
import {fShortenNumber} from "src/utils/formatNumber";

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

export default function BasicChart({ data, currency }) {
  return (
    <ResponsiveBar
      data={data}
      keys={["value"]}
      indexBy="year"
      margin={{ top: 10, right: 20, bottom: 30, left: 35 }}
      padding={0.3}
      valueScale={{ type: "linear" }}
      indexScale={{ type: "band", round: true }}
      colors={({ id, data }) => String(data[`${id}Color`])}
      borderColor={{
        from: "color",
        modifiers: [["darker", 1.6]],
      }}
      labelTextColor="#ffffff"
      valueFormat={(value) => {
        return (value > 0 ? "+" : "") + fShortenNumber(value);
      }}
      axisTop={null}
      axisRight={null}
      axisBottom={{
        tickSize: 5,
        tickPadding: 5,
        tickRotation: 0,
      }}
      axisLeft={{
        tickSize: 5,
        tickPadding: 5,
        tickRotation: 0,
        format: (value) => fShortenNumber(value),
      }}
      labelSkipWidth={12}
      labelSkipHeight={12}
      role="application"
      tooltip={({ value, color, indexValue }) => {
        return (
          <Tooltip>
            <Grid container spacing={0} justifyContent="center">
              <Grid item xs={12} textAlign="center">
                <Typography variant="subtitle2">Rok {indexValue}</Typography>
              </Grid>
              <Grid item xs={12} textAlign="center">
                <Typography variant="subtitle2" color={color}>
                  {value > 0 && "+"}
                  {getCurrencyFormatter(currency).format(value)}
                </Typography>
              </Grid>
            </Grid>
          </Tooltip>
        );
      }}
    />
  );
}
