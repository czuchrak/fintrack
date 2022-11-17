import {Card, Stack, Typography} from "@mui/material";
import MoreMenu from "../utilities/MoreMenu";
import useNetWorthServiceActions from "../../serviceActions/NetWorthServiceActions";
import {getCurrencyFormatter, getMonthAndYear} from "../../utils/helpers";

export default function NetWorthGoalCard({ goal, handleChangeGoal, parts }) {
  const { deadline, value, returnRate } = goal;
  const netWorthServiceActions = useNetWorthServiceActions();

  const ps = parts.filter((x) => goal.parts.includes(x.id));

  return (
    <>
      <Card>
        <Stack spacing={2} sx={{ p: 3 }}>
          <Stack
            direction="row"
            alignItems="center"
            justifyContent="space-between"
          >
            <Typography variant="body2">
              {new Date(goal.deadline) < new Date() ? (
                <span style={{ color: "#FF4842" }}>
                  {getMonthAndYear(deadline)}
                </span>
              ) : (
                getMonthAndYear(deadline)
              )}
            </Typography>
            <span>
              <MoreMenu
                onEdit={() => handleChangeGoal(goal)}
                onDelete={() => netWorthServiceActions.deleteGoal(goal.id)}
              />
            </span>
          </Stack>
          <Stack
            direction="row"
            alignItems="center"
            justifyContent="space-between"
            style={{ marginTop: 0 }}
          >
            <Typography variant="h6" noWrap>
              {goal.name}
            </Typography>
          </Stack>
          <Typography variant="body2" noWrap>
            {ps.map((x) => x.name).join(", ")}
          </Typography>
          <Stack
            direction="row"
            alignItems="center"
            justifyContent="space-between"
            style={{ marginTop: 0 }}
          >
            <Typography variant="body2" noWrap>
              {getCurrencyFormatter("PLN").format(value)}
            </Typography>
            <Typography variant="body2" noWrap>
              {returnRate}%
            </Typography>
          </Stack>
        </Stack>
      </Card>
    </>
  );
}
