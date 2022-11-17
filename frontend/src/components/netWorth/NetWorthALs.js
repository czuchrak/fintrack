import {Grid} from "@mui/material";
import NetWorthALChart from "./NetWorthALChart";
import NetWorthALPieChart from "./NetWorthALPieChart";
import {useMemo, useState} from "react";
import {getMonthAndYear, sortByDate} from "../../utils/helpers";

export default function NetWorthALs({ data, lastEntry }) {
  const [assetPieEntry, setAssetPieEntry] = useState(lastEntry);
  const [liabilityPieEntry, setLiabilityPieEntry] = useState(lastEntry);

  const entries = useMemo(() => {
    return data.entries.slice().sort(sortByDate());
  }, [data.entries]);

  const handleAssetPieDateChange = (e) => {
    setAssetPieEntry(
      entries.find((x) => getMonthAndYear(x.date) === e.target.value)
    );
  };

  const handleLiabilityPieDateChange = (e) => {
    setLiabilityPieEntry(
      entries.find((x) => getMonthAndYear(x.date) === e.target.value)
    );
  };

  return (
    <>
      <Grid item xs={12} md={6} lg={8}>
        <NetWorthALChart
          data={data}
          type="asset"
          currentDate={lastEntry.date}
        />
      </Grid>
      <Grid item xs={12} md={6} lg={4}>
        <NetWorthALPieChart
          entry={assetPieEntry}
          type="asset"
          parts={data.parts}
          dates={entries.map((x) => x.date)}
          handlePieDateChange={handleAssetPieDateChange}
        />
      </Grid>
      {data.parts.filter((x) => x.type === "liability").length > 0 && (
        <>
          <Grid item xs={12} md={6} lg={8}>
            <NetWorthALChart
              data={data}
              type="liability"
              currentDate={lastEntry.date}
            />
          </Grid>
          <Grid item xs={12} md={6} lg={4}>
            <NetWorthALPieChart
              entry={liabilityPieEntry}
              type="liability"
              parts={data.parts}
              dates={entries.map((x) => x.date)}
              handlePieDateChange={handleLiabilityPieDateChange}
            />
          </Grid>
        </>
      )}
    </>
  );
}
