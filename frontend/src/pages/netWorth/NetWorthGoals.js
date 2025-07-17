import { useEffect, useMemo, useState } from "react";
import { useDispatch, useSelector } from "react-redux";
import { addError } from "src/redux/slices/errorSlice";
import InternalPage from "../../components/InternalPage";
import useNetWorthServiceActions from "../../serviceActions/NetWorthServiceActions";
import { Grid } from "@mui/material";
import NetWorthGoalSidebar from "../../components/netWorth/NetWorthGoalSidebar";
import NetWorthGoalCard from "../../components/netWorth/NetWorthGoalCard";
import EmptyState from "../../components/utilities/EmptyState";
import SavingsRoundedIcon from "@mui/icons-material/SavingsRounded";
import AccountBalanceWalletRoundedIcon from "@mui/icons-material/AccountBalanceWalletRounded";
import LocalAtmRoundedIcon from "@mui/icons-material/LocalAtmRounded";

// ----------------------------------------------------------------------

export default function NetWorthGoals() {
  const dispatch = useDispatch();
  const netWorthServiceActions = useNetWorthServiceActions();

  const { data, dataFetched, isLoading, error } = useSelector(
    (state) => state.netWorth
  );

  dispatch(addError(error));

  const [openForm, setOpenForm] = useState(false);
  const [goal, setGoal] = useState({});

  const handleOpenForm = () => {
    setGoal({});
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setGoal({});
  };

  const handleChangeGoal = (goal) => {
    setGoal(goal);
    setOpenForm(true);
  };

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
    } else if (!data.entries || data.entries.length === 0)
      return (
        <EmptyState
          icon={LocalAtmRoundedIcon}
          text="Nie masz dodanych żadnych wartości majątku."
          buttonText="Przejdź do wartości majątku"
          buttonUrl="/networth/data"
        />
      );
    else
      return (
        <EmptyState
          icon={SavingsRoundedIcon}
          text="Nie masz dodanych żadnych celów."
          bodyText={
            <span>
              Przykłady:
              <br />- 2 mln zł na emeryturę do 2050 roku
              <br />- Całkowita spłata hipoteki do 2030 roku
              <br />- 50 tys. zł na nowy samochód do 2026 roku
            </span>
          }
          buttonText="Zdefiniuj swój pierwszy cel"
          buttonOnClick={handleOpenForm}
          showImportButton={false}
        />
      );
  };

  const showEmptyState = useMemo(() => {
    return (
      !data.parts ||
      data.parts.length === 0 ||
      !data.entries ||
      data.entries.length === 0 ||
      !data.goals ||
      data.goals.length === 0
    );
  }, [data.entries, data.goals, data.parts]);

  useEffect(() => {
    if (!dataFetched) {
      netWorthServiceActions.getData().then();
    }
  }, [dataFetched, netWorthServiceActions]);

  return (
    <InternalPage
      title="Cele"
      isLoading={isLoading}
      handleOpenForm={handleOpenForm}
      showEmptyState={showEmptyState}
      emptyState={emptyState()}
    >
      {dataFetched && (
        <>
          <Grid container spacing={3}>
            {data.goals.map((goal) => (
              <Grid key={goal.id} item xs={12} sm={6} md={3}>
                <NetWorthGoalCard
                  goal={goal}
                  handleChangeGoal={handleChangeGoal}
                  parts={data.parts}
                />
              </Grid>
            ))}
          </Grid>
          <NetWorthGoalSidebar
            isOpenForm={openForm}
            onCloseForm={handleCloseForm}
            goal={goal}
            parts={data.parts}
            entries={data.entries}
          />
        </>
      )}
    </InternalPage>
  );
}
