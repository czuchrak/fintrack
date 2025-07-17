import { Card, Divider, Grid, Typography } from "@mui/material";
import { styled } from "@mui/material/styles";
import { getMonthAndYear } from "../../utils/helpers";
import React, { useMemo } from "react";
import { DecimalLabel } from "../Label";
import Scrollbar from "../Scrollbar";
import { InlineIcon } from "@iconify/react";
import arrowForward from "@iconify/icons-eva/arrow-forward-outline";
import EmptyState from "../utilities/EmptyState";
import HourglassBottomRoundedIcon from "@mui/icons-material/HourglassBottomRounded";
// ----------------------------------------------------------------------

const RootStyle = styled(Card)(({ theme }) => ({
  textAlign: "center",
  padding: theme.spacing(3, 0),
}));

export default function NetWorthLastSummary({ data, lastEntry, last2Entry }) {
  const changes = useMemo(() => {
    if (!lastEntry || !last2Entry) return { assets: [], liabilities: [] };
    let assets = [];
    let liabilities = [];

    data.parts.slice().forEach((part) => {
      const lastValue = lastEntry.partValues[part.id];
      const last2Value = last2Entry.partValues[part.id];

      const obj = {
        id: part.id,
        name: part.name,
        value: lastValue.value - last2Value.value,
        valueRate: lastValue.valueRate - last2Value.valueRate,
        type: part.type,
        currency: part.currency,
      };

      if (lastValue.value - last2Value.value !== 0)
        if (part.type === "asset") assets.push(obj);
        else liabilities.push(obj);
    });

    assets.sort((a, b) => b.valueRate - a.valueRate);
    liabilities.sort((a, b) => a.valueRate - b.valueRate);

    return { assets, liabilities };
  }, [data.parts, last2Entry, lastEntry]);

  const Element = ({ el }) => (
    <>
      <Grid item xs={6}>
        <Typography variant="caption" sx={{ opacity: 0.72 }}>
          {el.name}
        </Typography>
      </Grid>
      <Grid item xs={6}>
        <DecimalLabel
          value={el.value}
          invert={el.type === "liability"}
          plus={true}
          currency={el.currency}
        />
      </Grid>
    </>
  );

  return (
    <>
      <Grid item xs={12} md={4}>
        <RootStyle sx={{ height: "330px" }}>
          {changes.assets.length > 0 || changes.liabilities.length > 0 ? (
            <Scrollbar>
              <Typography variant="h4">Ostatnie zmiany</Typography>
              <Typography variant="caption" sx={{ opacity: 0.72 }}>
                {getMonthAndYear(last2Entry.date)}{" "}
                <InlineIcon icon={arrowForward} />{" "}
                {getMonthAndYear(lastEntry.date)}
              </Typography>
              <Divider sx={{ my: 2 }} />

              {changes.assets.length > 0 && (
                <>
                  <Typography variant="subtitle2" sx={{ opacity: 0.72 }} mt={1}>
                    Aktywa
                  </Typography>
                  <Grid container>
                    {changes.assets.map((el) => (
                      <Element el={el} key={el.id} />
                    ))}
                  </Grid>
                </>
              )}

              {changes.liabilities.length > 0 && (
                <>
                  <Typography variant="subtitle2" sx={{ opacity: 0.72 }} mt={1}>
                    Zobowiązania
                  </Typography>
                  <Grid container>
                    {changes.liabilities.map((el) => (
                      <Element el={el} key={el.id} />
                    ))}
                  </Grid>
                </>
              )}
            </Scrollbar>
          ) : (
            <EmptyState
              title="Ostatnie zmiany"
              icon={HourglassBottomRoundedIcon}
              text="Uzupełnij wartości składników jeszcze raz, aby&nbsp;zobaczyć ostatnie zmiany"
              showImportButton={false}
            />
          )}
        </RootStyle>
      </Grid>
    </>
  );
}
