import {Link as RouterLink} from "react-router-dom";
import {Box, Card, Container, Grid, Stack, Typography} from "@mui/material";
import Page from "src/components/Page";

// ----------------------------------------------------------------------

const links = [
  {
    title: "Logi",
    url: process.env.PUBLIC_URL + "/admin/logs",
  },
  {
    title: "Powiadomienia",
    url: process.env.PUBLIC_URL + "/admin/notifications",
  },
  {
    title: "Użytkownicy",
    url: process.env.PUBLIC_URL + "/admin/users",
  },
  {
    title: "PropertyCategories",
    url: process.env.PUBLIC_URL + "/admin/propertyCategories",
  },
  {
    title: "Waluty",
    url: process.env.PUBLIC_URL + "/admin/exchangeRates",
  },
];

export default function Admin() {
  return (
    <>
      <Page title="Panel administracyjny">
        <Container maxWidth="xl">
          <>
            <Typography variant="h4" gutterBottom>
              Panel administracyjny
            </Typography>
            <Grid container spacing={3} sx={{ mt: 2 }}>
              {links.map((x) => {
                return (
                  <Grid item xs={12} sm={6} md={3} key={x.title}>
                    <Card>
                      <Stack spacing={2} sx={{ p: 3 }}>
                        <Stack
                          direction="row"
                          alignItems="center"
                          justifyContent="center"
                          style={{ marginTop: 0 }}
                        >
                          <Box component={RouterLink} to={x.url}>
                            <Typography variant="h4" noWrap>
                              {x.title}
                            </Typography>
                          </Box>
                        </Stack>
                      </Stack>
                    </Card>
                  </Grid>
                );
              })}
            </Grid>
          </>
        </Container>
      </Page>
    </>
  );
}
