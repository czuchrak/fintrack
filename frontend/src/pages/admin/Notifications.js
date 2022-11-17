import {useEffect, useState} from "react";
import {Link as RouterLink} from "react-router-dom";
import {Button, Container, IconButton, Stack, TableCell, TableRow, Typography,} from "@mui/material";
import {Icon} from "@iconify/react";
import Page from "src/components/Page";
import {useAuth} from "src/navigation/PrivateRoute";
import {
    addNotification,
    deleteNotification,
    duplicateNotification,
    getNotifications,
    updateNotification,
} from "src/services";
import moment from "moment";
import "moment/locale/pl";
import Loader from "src/components/Loader";
import arrowBackOutline from "@iconify/icons-eva/arrow-back-outline";
import NotificationSidebar from "src/components/admin/NotificationSidebar";
import SimpleTable from "src/components/SimpleTable";
import MoreMenu from "../../components/utilities/MoreMenu";

// ----------------------------------------------------------------------

export default function Notifications() {
  const [openForm, setOpenForm] = useState(false);
  const [notification, setNotification] = useState({});
  const [data, setData] = useState([]);
  const [change, setChange] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  let auth = useAuth();

  const handleOpenForm = () => {
    setNotification({});
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setNotification({});
  };

  const handleChangeNotification = (notification) => {
    setNotification(notification);
    setOpenForm(true);
  };

  const handleSave = async (values) => {
    setIsLoading(true);
    try {
      if (values.id === undefined) {
        await addNotification(values, await auth.getToken());
      } else {
        await updateNotification(values, await auth.getToken());
      }
      setChange(!change);
    } catch (error) {
      setIsLoading(false);
    }
  };

  const handleDuplicate = async (id) => {
    setIsLoading(true);
    try {
      await duplicateNotification(id, await auth.getToken());
      setChange(!change);
    } catch (error) {
      setIsLoading(false);
    }
  };

  const handleDelete = async (id) => {
    setIsLoading(true);
    try {
      await deleteNotification(id, await auth.getToken());
      setChange(!change);
    } catch (error) {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    const getData = async () => {
      try {
        const data = await getNotifications(await auth.getToken());
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
      <Page title="Powiadomienia">
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
                Powiadomienia
              </Typography>

              <Button variant="contained" onClick={handleOpenForm}>
                Dodaj
              </Button>
            </Stack>
            {isLoading || (
              <SimpleTable
                headers={
                  <>
                    <TableCell align="center">Id</TableCell>
                    <TableCell align="center">Type</TableCell>
                    <TableCell align="center">Message</TableCell>
                    <TableCell align="center">Url</TableCell>
                    <TableCell align="center">ValidFrom</TableCell>
                    <TableCell align="center">ValidUntil</TableCell>
                    <TableCell align="center">IsActive</TableCell>
                    <TableCell align="center">ReadCount</TableCell>
                    <TableCell align="center" />
                  </>
                }
                data={data.sort((a, b) => {
                  if (a.validUntil < b.validUntil) return -1;
                  else if (a.validUntil > b.validUntil) return 1;
                  else return 0;
                })}
                mapping={(entry) => {
                  const {
                    id,
                    type,
                    message,
                    url,
                    validFrom,
                    validUntil,
                    isActive,
                    readCount,
                    usersCount,
                  } = entry;

                  return (
                    <TableRow hover key={id} tabIndex={-1}>
                      <TableCell align="center">{id.substring(0, 8)}</TableCell>
                      <TableCell align="center">{type}</TableCell>
                      <TableCell align="center">{message}</TableCell>
                      <TableCell align="center">{url}</TableCell>
                      <TableCell align="center">
                        <Typography
                          color={
                            moment(new Date()) > moment(validFrom)
                              ? "primary"
                              : "error"
                          }
                        >
                          {moment(validFrom).format("yyyy-MM-DD HH:mm")}
                        </Typography>
                      </TableCell>
                      <TableCell align="center">
                        <Typography
                          color={
                            moment(new Date()) < moment(validUntil)
                              ? "primary"
                              : "error"
                          }
                        >
                          {moment(validUntil).format("yyyy-MM-DD HH:mm")}
                        </Typography>
                      </TableCell>
                      <TableCell align="center">
                        <Typography color={isActive ? "primary" : "error"}>
                          {isActive ? "Aktywne" : "Nieaktywne"}
                        </Typography>
                      </TableCell>
                      <TableCell align="center">
                        {readCount}/{usersCount}
                      </TableCell>
                      <TableCell align="center">
                        <MoreMenu
                          onEdit={() => handleChangeNotification(entry)}
                          onDelete={() => handleDelete(entry.id)}
                          onDuplicate={() => handleDuplicate(entry.id)}
                        />
                      </TableCell>
                    </TableRow>
                  );
                }}
                paging={[10, 50, 100]}
              />
            )}
          </>
        </Container>
        <NotificationSidebar
          isOpenForm={openForm}
          onCloseForm={handleCloseForm}
          onSave={handleSave}
          notification={notification}
        />
        <Loader open={isLoading} />
      </Page>
    </>
  );
}
