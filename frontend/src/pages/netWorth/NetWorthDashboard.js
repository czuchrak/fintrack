import {Grid} from "@mui/material";
import {useEffect, useMemo} from "react";
import {useDispatch, useSelector} from "react-redux";
import {addError} from "src/redux/slices/errorSlice";
import useNetWorthServiceActions from "../../serviceActions/NetWorthServiceActions";
import InternalPage from "../../components/InternalPage";
import NetWorthValues from "../../components/netWorth/NetWorthValues";
import {sortByDate} from "../../utils/helpers";
import NetWorthChart from "../../components/netWorth/NetWorthChart";
import NetWorthALs from "../../components/netWorth/NetWorthALs";
import {NetWorthHeatMap} from "../../components/netWorth/NetWorthHeatMap";
import NetWorthYears from "../../components/netWorth/NetWorthYears";
import NetWorthGoalsSummary from "../../components/netWorth/NetWorthGoalsSummary";
import NetWorthLastSummary from "../../components/netWorth/NetWorthLastSummary";
import EmptyState from "../../components/utilities/EmptyState";
import AccountBalanceWalletRoundedIcon from "@mui/icons-material/AccountBalanceWalletRounded";
import LocalAtmRoundedIcon from "@mui/icons-material/LocalAtmRounded";

// ----------------------------------------------------------------------

export default function NetWorthDashboard() {
  const dispatch = useDispatch();
  const netWorthServiceActions = useNetWorthServiceActions();

  const { data, dataFetched, isLoading, error } = useSelector(
    (state) => state.netWorth
  );

  dispatch(addError(error));

  const lastEntry = useMemo(() => {
    return dataFetched && data.entries.slice().sort(sortByDate())[0];
  }, [data.entries, dataFetched]);

  const last2Entry = useMemo(() => {
    return (
      dataFetched &&
      data.entries.length > 1 &&
      data.entries.slice().sort(sortByDate())[1]
    );
  }, [data.entries, dataFetched]);

  const emptyState = () => {
    if (!data.parts || data.parts.length === 0) {
      return (
        <EmptyState
          icon={AccountBalanceWalletRoundedIcon}
          text="Nie masz dodanych żadnych składników majątku."
          buttonText="Przejdź do składników"
          buttonUrl="/networth/parts"
        />
      );
    } else
      return (
        <EmptyState
          icon={LocalAtmRoundedIcon}
          text="Nie masz dodanych żadnych wartości majątku."
          buttonText="Przejdź do wartości majątku"
          buttonUrl="/networth/data"
        />
      );
  };

  useEffect(() => {
    if (!dataFetched) {
      netWorthServiceActions.getData().then();
    }
  }, [dataFetched, netWorthServiceActions]);

  return (
    <InternalPage
      title="Twój majątek"
      isLoading={isLoading}
      showEmptyState={
        !data.parts ||
        data.parts.length === 0 ||
        !data.entries ||
        data.entries.length === 0
      }
      emptyState={emptyState()}
    >
      {dataFetched && data.entries.length > 0 && (
        <Grid container spacing={3}>
          <NetWorthValues data={data} lastEntry={lastEntry} />
          <NetWorthLastSummary
            data={data}
            lastEntry={lastEntry}
            last2Entry={last2Entry}
          />
          <NetWorthGoalsSummary data={data} lastEntry={lastEntry} />
          <NetWorthChart data={data} currentDate={lastEntry.date} />
          <NetWorthALs data={data} lastEntry={lastEntry} />
          <NetWorthYears data={data} />
          <NetWorthHeatMap data={data} />
        </Grid>
      )}
    </InternalPage>
  );
}
