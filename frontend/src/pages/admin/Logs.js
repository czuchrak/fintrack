import {useEffect, useState} from "react";
import {Link as RouterLink} from "react-router-dom";
import {Button, Container, IconButton, Stack, TableCell, TableRow, Typography,} from "@mui/material";
import {Icon} from "@iconify/react";
import Page from "src/components/Page";
import {useAuth} from "src/navigation/PrivateRoute";
import {deleteLogs, getLogs} from "src/services";
import moment from "moment";
import "moment/locale/pl";
import Loader from "src/components/Loader";
import arrowBackOutline from "@iconify/icons-eva/arrow-back-outline";
import SimpleTable from "src/components/SimpleTable";

// ----------------------------------------------------------------------

export default function Logs() {
  const [data, setData] = useState([]);
  const [change, setChange] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  let auth = useAuth();

  const handleDelete = async () => {
    await deleteLogs(await auth.getToken());
    setChange(!change);
  };

  useEffect(() => {
    const getData = async () => {
      try {
        const data = await getLogs(await auth.getToken());
        setData(data);
      } catch (error) {
        setData([]);
      }
      setIsLoading(false);
    };
    getData();
  }, [auth, change]);

  return (
    <>
      <Page title="Logi">
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
                Logi
              </Typography>

              <Button variant="contained" onClick={handleDelete}>
                Usuń wszystkie
              </Button>
            </Stack>
            {isLoading || (
              <SimpleTable
                headers={
                  <>
                    <TableCell align="center">Id</TableCell>
                    <TableCell align="center">Level</TableCell>
                    <TableCell align="center">Timestamp</TableCell>
                    <TableCell align="center">Message</TableCell>
                    <TableCell align="center">Exception</TableCell>
                  </>
                }
                data={data.sort((a, b) => {
                  if (a.id < b.id) return 1;
                  else if (a.id > b.id) return -1;
                  else return 0;
                })}
                mapping={(entry) => {
                  const { id, message, exception, timestamp, level } = entry;

                  return (
                    <TableRow hover key={id} tabIndex={-1}>
                      <TableCell align="center">{id}</TableCell>
                      <TableCell align="center">{level}</TableCell>
                      <TableCell align="center">
                        {moment(timestamp).format("yyyy-MM-DD HH:mm:ss")}
                      </TableCell>
                      <TableCell align="center">
                        {message && message.substring(0, 500)}
                      </TableCell>
                      <TableCell align="center">
                        {exception && exception.substring(0, 500)}
                      </TableCell>
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
