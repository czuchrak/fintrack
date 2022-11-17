import {useEffect, useState} from "react";
import {Link as RouterLink} from "react-router-dom";
import {Button, Container, IconButton, Stack, TableCell, TableRow, Typography,} from "@mui/material";
import {Icon} from "@iconify/react";
import Page from "src/components/Page";
import {useAuth} from "src/navigation/PrivateRoute";
import {addPropertyCategory, getPropertyCategories, updatePropertyCategory,} from "src/services";
import "moment/locale/pl";
import Loader from "src/components/Loader";
import arrowBackOutline from "@iconify/icons-eva/arrow-back-outline";
import SimpleTable from "src/components/SimpleTable";
import PropertyCategorySidebar from "src/components/admin/PropertyCategorySidebar";
import MoreMenu from "../../components/utilities/MoreMenu";

// ----------------------------------------------------------------------

export default function PropertyCategories() {
  const [openForm, setOpenForm] = useState(false);
  const [propertyCategory, setPropertyCategory] = useState({});
  const [data, setData] = useState([]);
  const [change, setChange] = useState(false);
  const [isLoading, setIsLoading] = useState(true);

  let auth = useAuth();

  const handleOpenForm = () => {
    setPropertyCategory({});
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setPropertyCategory({});
  };

  const handleChangePropertyCategory = (propertyCategory) => {
    setPropertyCategory(propertyCategory);
    setOpenForm(true);
  };

  const handleSave = async (values) => {
    setIsLoading(true);
    try {
      if (values.id === undefined) {
        await addPropertyCategory(values, await auth.getToken());
      } else {
        await updatePropertyCategory(values, await auth.getToken());
      }
      setChange(!change);
    } catch (error) {
      setIsLoading(false);
    }
  };

  useEffect(() => {
    const getData = async () => {
      try {
        const data = await getPropertyCategories(await auth.getToken());
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
      <Page title="PropertyCategories">
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
                PropertyCategories
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
                    <TableCell align="center">Name</TableCell>
                    <TableCell align="center">Type</TableCell>
                    <TableCell align="center">IsCost</TableCell>
                    <TableCell align="center">Count</TableCell>
                    <TableCell align="center" />
                  </>
                }
                data={data}
                mapping={(entry) => {
                  const { id, name, type, isCost, count } = entry;

                  return (
                    <TableRow hover key={id} tabIndex={-1}>
                      <TableCell align="center">{id.substring(0, 8)}</TableCell>
                      <TableCell align="center">{name}</TableCell>
                      <TableCell align="center">{type}</TableCell>
                      <TableCell align="center">
                        <Typography color={isCost ? "primary" : "error"}>
                          {isCost ? "Koszt" : "Przychód"}
                        </Typography>
                      </TableCell>
                      <TableCell align="center">{count}</TableCell>
                      <TableCell align="center">
                        <MoreMenu
                          onEdit={() => handleChangePropertyCategory(entry)}
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
        <PropertyCategorySidebar
          isOpenForm={openForm}
          onCloseForm={handleCloseForm}
          onSave={handleSave}
          propertyCategory={propertyCategory}
        />
        <Loader open={isLoading} />
      </Page>
    </>
  );
}
