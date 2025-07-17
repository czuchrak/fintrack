import { useEffect, useState } from "react";
import { Link as RouterLink } from "react-router-dom";
import {
  Container,
  IconButton,
  Stack,
  TableCell,
  TableRow,
  Typography,
} from "@mui/material";
import { Icon } from "@iconify/react";
import Page from "src/components/Page";
import { useAuth } from "src/navigation/PrivateRoute";
import { getExchangeRates, fillExchangeRates } from "src/services";
import moment from "moment";
import "moment/locale/pl";
import Loader from "src/components/Loader";
import arrowBackOutline from "@iconify/icons-eva/arrow-back-outline";
import SimpleTable from "src/components/SimpleTable";
import RefreshIcon from "@mui/icons-material/Refresh";
import { addError, removeError } from "src/redux/slices/errorSlice";
import { useDispatch } from "react-redux";

// ----------------------------------------------------------------------

export default function ExchangeRates() {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const dispatch = useDispatch();

  let auth = useAuth();

  useEffect(() => {
    const getData = async () => {
      try {
        const data = await getExchangeRates(await auth.getToken());
        setData(data);
      } catch (error) {
        setData([]);
      }
      setIsLoading(false);
    };
    getData();
  }, [auth]);

  const handleRefreshRates = async () => {
    setIsLoading(true);
    dispatch(removeError());
    try {
      await fillExchangeRates(await auth.getToken());
      const data = await getExchangeRates(await auth.getToken());
      setData(data);
    } catch (error) {
      dispatch(addError(error));
    }
    setIsLoading(false);
  };

  return (
    <>
      <Page title="Waluty">
        <Container maxWidth="xl">
          <>
            <Stack
              direction="row"
              alignItems="center"
              justifyContent="space-between"
              mb={3}
            >
              <IconButton
                size="large"
                component={RouterLink}
                to={process.env.PUBLIC_URL + "/admin"}
              >
                <Icon icon={arrowBackOutline} />
              </IconButton>
              <Typography variant="h4" gutterBottom>
                Waluty
              </Typography>
              <IconButton
                size="large"
                color="primary"
                onClick={handleRefreshRates}
                title="Refresh"
              >
                <RefreshIcon />
              </IconButton>
            </Stack>
            {isLoading || (
              <SimpleTable
                headers={
                  <>
                    <TableCell align="center">Date</TableCell>
                    <TableCell align="center">Currency</TableCell>
                    <TableCell align="center">Rate</TableCell>
                  </>
                }
                data={data.sort((a, b) => {
                  if (a.date < b.date) return 1;
                  else if (a.date > b.date) return -1;
                  else return 0;
                })}
                mapping={(entry) => {
                  const { date, currency, rate } = entry;

                  return (
                    <TableRow hover key={`${date}-${currency}`} tabIndex={-1}>
                      <TableCell align="center">
                        {moment(date).format("yyyy-MM-DD")}
                      </TableCell>
                      <TableCell align="center">{currency}</TableCell>
                      <TableCell align="center">{rate}</TableCell>
                    </TableRow>
                  );
                }}
                paging={[10, 50, 100]}
              />
            )}
          </>
        </Container>
        <Loader open={isLoading} />
      </Page>
    </>
  );
}
