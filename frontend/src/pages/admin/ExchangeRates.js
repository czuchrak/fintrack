import {useEffect, useState} from "react";
import {Link as RouterLink} from "react-router-dom";
import {Container, IconButton, Stack, TableCell, TableRow, Typography,} from "@mui/material";
import {Icon} from "@iconify/react";
import Page from "src/components/Page";
import {useAuth} from "src/navigation/PrivateRoute";
import {getExchangeRates} from "src/services";
import moment from "moment";
import "moment/locale/pl";
import Loader from "src/components/Loader";
import arrowBackOutline from "@iconify/icons-eva/arrow-back-outline";
import SimpleTable from "src/components/SimpleTable";

// ----------------------------------------------------------------------

export default function ExchangeRates() {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

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
              <Typography />
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
