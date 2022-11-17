import {styled} from "@mui/material/styles";
import {Box, Card, Divider, Grid, Typography} from "@mui/material";
import {
    checkPreviousMonthFromCurrentEntry,
    getCurrencyFormatter,
    getMonthAndYear,
    sortByDate,
} from "src/utils/helpers";
import {DecimalLabel} from "src/components/Label";
import {useCallback, useMemo} from "react";
import {useSelector} from "react-redux";
import Carousel from "react-material-ui-carousel";

// ----------------------------------------------------------------------

const RootStyle = styled(Card)(({ theme }) => ({
  textAlign: "center",
  padding: theme.spacing(3, 0),
}));

function NetWorthValue({
  value,
  date,
  currentOne,
  currentThree,
  currentSix,
  currentTwelve,
  currentTwoFour,
  type,
}) {
  let { currency } = useSelector((state) => state.profile);

  const MonthValue = ({ value, current }) => {
    let diff = 0;
    if (type === "value") diff = value - current.value;
    if (type === "assets") diff = value - current.assets;
    if (type === "liabilities") diff = value - current.liabilities;
    return (
      <>
        <Grid item xs={6}>
          <Typography variant="caption" sx={{ opacity: 0.72 }}>
            {getMonthAndYear(current.date)}
          </Typography>
        </Grid>
        <Grid item xs={6}>
          <DecimalLabel
            value={diff}
            invert={type === "liabilities"}
            plus={true}
            currency={currency}
          />
        </Grid>
      </>
    );
  };

  return (
    <Box>
      <Typography variant="h3">
        {getCurrencyFormatter(currency).format(value)}
      </Typography>
      <Typography variant="subtitle2" sx={{ opacity: 0.72 }}>
        {(type === "value" && "Twoja wartość netto") ||
          (type === "assets" && "Wartość aktywów") ||
          "Wartość zobowiązań"}
      </Typography>
      <Typography variant="caption" sx={{ opacity: 0.72 }}>
        Stan na {getMonthAndYear(date)}
      </Typography>
      {(currentOne ||
        currentThree ||
        currentSix ||
        currentTwelve ||
        currentTwoFour) && (
        <>
          <Divider sx={{ my: 2 }} />
          <Grid container>
            {currentOne && <MonthValue value={value} current={currentOne} />}
            {currentThree && (
              <MonthValue value={value} current={currentThree} />
            )}
            {currentSix && <MonthValue value={value} current={currentSix} />}
            {currentTwelve && (
              <MonthValue value={value} current={currentTwelve} />
            )}
            {currentTwoFour && (
              <MonthValue value={value} current={currentTwoFour} />
            )}
          </Grid>
        </>
      )}
    </Box>
  );
}

export default function NetWorthValues({ data, lastEntry }) {
  const entries = useMemo(() => {
    return data.entries.slice().sort(sortByDate());
  }, [data.entries]);

  const getMonthEntry = useCallback(
    (month) => {
      return entries.find((x) =>
        checkPreviousMonthFromCurrentEntry(
          new Date(x.date),
          lastEntry.date,
          month
        )
      );
    },
    [entries, lastEntry.date]
  );

  const currentOneEntry = useMemo(() => {
    return getMonthEntry(1);
  }, [getMonthEntry]);

  const currentThreeEntry = useMemo(() => {
    return getMonthEntry(3);
  }, [getMonthEntry]);

  const currentSixEntry = useMemo(() => {
    return getMonthEntry(6);
  }, [getMonthEntry]);

  const currentTwelveEntry = useMemo(() => {
    return getMonthEntry(12);
  }, [getMonthEntry]);

  const currentTwoFourEntry = useMemo(() => {
    return getMonthEntry(24);
  }, [getMonthEntry]);

  return (
    <>
      <Grid item xs={12} md={4}>
        <RootStyle sx={{ height: "100%" }}>
          <Carousel animation="fade" autoPlay={false} height="248px">
            <NetWorthValue
              value={lastEntry.value}
              date={lastEntry.date}
              currentOne={currentOneEntry}
              currentThree={currentThreeEntry}
              currentSix={currentSixEntry}
              currentTwelve={currentTwelveEntry}
              currentTwoFour={currentTwoFourEntry}
              type="value"
            />
            <NetWorthValue
              value={lastEntry.assets}
              date={lastEntry.date}
              currentOne={currentOneEntry}
              currentThree={currentThreeEntry}
              currentSix={currentSixEntry}
              currentTwelve={currentTwelveEntry}
              currentTwoFour={currentTwoFourEntry}
              type="assets"
            />
            <NetWorthValue
              value={lastEntry.liabilities}
              date={lastEntry.date}
              currentOne={currentOneEntry}
              currentThree={currentThreeEntry}
              currentSix={currentSixEntry}
              currentTwelve={currentTwelveEntry}
              currentTwoFour={currentTwoFourEntry}
              type="liabilities"
            />
          </Carousel>
        </RootStyle>
      </Grid>
    </>
  );
}
