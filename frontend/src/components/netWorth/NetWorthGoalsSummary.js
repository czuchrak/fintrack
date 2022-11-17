import {Box, Card, Divider, Grid, Tooltip, Typography} from "@mui/material";
import Carousel from "react-material-ui-carousel";
import {getCurrencyFormatter, getGoalSummary, getMonth3AndYear, getMonthAndYear,} from "../../utils/helpers";
import {styled} from "@mui/material/styles";
import {useMemo} from "react";
import SavingsRoundedIcon from "@mui/icons-material/SavingsRounded";
import EmptyState from "../utilities/EmptyState";
// ----------------------------------------------------------------------

const RootStyle = styled(Card)(({ theme }) => ({
  textAlign: "center",
  padding: theme.spacing(3, 1),
}));

export default function NetWorthGoalsSummary({ data, lastEntry }) {
  const goalSummaries = useMemo(() => {
    return (
      data.goals &&
      data.goals.slice().filter((x) => new Date(x.deadline) > new Date())
    );
  }, [data.goals]);

  return (
    <>
      <Grid item xs={12} md={4}>
        <RootStyle sx={{ height: "330px" }}>
          {!data.goals || data.goals.length === 0 ? (
            <EmptyState
              title="Twoje cele"
              icon={SavingsRoundedIcon}
              text="Nie masz dodanych żadnych celów."
              buttonText="Zdefiniuj swój pierwszy cel"
              buttonUrl="/networth/goals"
            />
          ) : data.goals.length === 1 ? (
            <GoalSummary
              goal={data.goals[0]}
              key={data.goals[0].id}
              lastEntry={lastEntry}
              parts={data.parts}
            />
          ) : (
            <Carousel animation="fade" autoPlay={false} height="248px">
              {goalSummaries.map((goal) => (
                <GoalSummary
                  goal={goal}
                  key={goal.id}
                  lastEntry={lastEntry}
                  parts={data.parts}
                />
              ))}
            </Carousel>
          )}
        </RootStyle>
      </Grid>
    </>
  );
}

function GoalSummary({ goal, lastEntry, parts }) {
  const goalParts = parts.filter((x) => goal.parts.includes(x.id));
  const obj = useMemo(() => {
    return getGoalSummary(parts, goal, lastEntry.partValues);
  }, [goal, lastEntry, parts]);

  const Title = ({ children, xs = 6 }) => {
    return (
      <Grid item xs={xs}>
        <Typography variant="subtitle2" sx={{ opacity: 0.72 }}>
          {children}
        </Typography>
      </Grid>
    );
  };

  const Value = ({ children, xs = 6 }) => {
    return (
      <Grid item xs={xs}>
        <Typography variant="subtitle1">{children}</Typography>
      </Grid>
    );
  };

  return (
    <Box>
      <Tooltip title={goalParts.map((x) => x.name).join(", ")} placement="top">
        <Typography variant="h4">{goal.name}</Typography>
      </Tooltip>
      <Typography variant="caption" sx={{ opacity: 0.72 }}>
        Stan celu na {getMonthAndYear(new Date())}
      </Typography>
      <Divider sx={{ my: 2 }} />
      <Grid container>
        <Title>Aktualna wartość</Title>
        <Title>Docelowa wartość</Title>
        <Value>{getCurrencyFormatter("PLN").format(obj.currentValue)}</Value>
        <Value>{getCurrencyFormatter("PLN").format(goal.value)}</Value>

        <Divider sx={{ my: 2 }} />

        {goal.returnRate > 0 ? (
          <>
            <Title xs={4}>Termin</Title>
            <Title xs={4}>Stopa zwrotu</Title>
            <Title xs={4}>Pozostało</Title>
            <Value xs={4}>{getMonth3AndYear(goal.deadline)}</Value>
            <Value xs={4}>{goal.returnRate}%</Value>
            <Value xs={4}>{obj.months} mies.</Value>
          </>
        ) : (
          <>
            <Title>Termin</Title>
            <Title>Pozostało</Title>
            <Value>{getMonth3AndYear(goal.deadline)}</Value>
            <Value>{obj.months} mies.</Value>
          </>
        )}

        <Divider sx={{ my: 2 }} />

        <Title xs={12}>
          {obj.avgMonthValue > 0
            ? "Zwiększaj miesięcznie o"
            : "Zmniejszaj miesięcznie o"}
        </Title>
        <Value xs={12}>
          {getCurrencyFormatter("PLN").format(Math.abs(obj.avgMonthValue))}
        </Value>
      </Grid>
    </Box>
  );
}
