import {useEffect, useState} from "react";
import {Link as RouterLink} from "react-router-dom";
import {Container, IconButton, Stack, TableCell, TableRow, Typography,} from "@mui/material";
import {Icon} from "@iconify/react";
import Page from "src/components/Page";
import {useAuth} from "src/navigation/PrivateRoute";
import {getUsers} from "src/services";
import moment from "moment";
import "moment/locale/pl";
import Loader from "src/components/Loader";
import arrowBackOutline from "@iconify/icons-eva/arrow-back-outline";
import SimpleTable from "src/components/SimpleTable";

// ----------------------------------------------------------------------

export default function Users() {
  const [data, setData] = useState([]);
  const [isLoading, setIsLoading] = useState(true);

  let auth = useAuth();

  useEffect(() => {
    const getData = async () => {
      try {
        const data = await getUsers(await auth.getToken());
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
      <Page title="Użytkownicy">
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
                Użytkownicy
              </Typography>
              <Typography />
            </Stack>
            {isLoading || (
              <SimpleTable
                headers={
                  <>
                    <TableCell align="center">Id</TableCell>
                    <TableCell align="center">Name</TableCell>
                    <TableCell align="center">CreationDate</TableCell>
                    <TableCell align="center">LastActivity</TableCell>
                    <TableCell align="center">NewMonthEnabled</TableCell>
                    <TableCell align="center">NewsEnabled</TableCell>
                    <TableCell align="center">Parts</TableCell>
                    <TableCell align="center">Entries</TableCell>
                    <TableCell align="center">Goals</TableCell>
                    <TableCell align="center">Properties</TableCell>
                  </>
                }
                data={data.sort((a, b) => {
                  if (a.lastActivity < b.lastActivity) return 1;
                  else if (a.lastActivity > b.lastActivity) return -1;
                  else return 0;
                })}
                mapping={(entry) => {
                  const {
                    id,
                    name,
                    creationDate,
                    lastActivity,
                    newMonthEmailEnabled,
                    newsEmailEnabled,
                    partsCount,
                    goalsCount,
                    entriesCount,
                    propertiesCount,
                  } = entry;

                  return (
                    <TableRow hover key={id} tabIndex={-1}>
                      <TableCell align="center">
                        {id.substring(0, 10)}
                      </TableCell>
                      <TableCell align="center">{name}</TableCell>
                      <TableCell align="center">
                        {moment(creationDate).format("yyyy-MM-DD HH:mm:ss")}
                      </TableCell>
                      <TableCell align="center">
                        {moment(lastActivity).format("yyyy-MM-DD HH:mm:ss")}
                      </TableCell>
                      <TableCell align="center">
                        {newMonthEmailEnabled === true ? "Y" : "N"}
                      </TableCell>
                      <TableCell align="center">
                        {newsEmailEnabled === true ? "Y" : "N"}
                      </TableCell>
                      <TableCell align="center">{partsCount}</TableCell>
                      <TableCell align="center">{entriesCount}</TableCell>
                      <TableCell align="center">{goalsCount}</TableCell>
                      <TableCell align="center">{propertiesCount}</TableCell>
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
