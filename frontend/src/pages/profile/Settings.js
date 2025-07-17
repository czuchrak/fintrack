import { Container, Grid, Typography } from "@mui/material";
import ChangePasswordForm from "src/components/profile/ChangePasswordForm";
import DeleteAccountForm from "src/components/profile/DeleteAccountForm";
import Page from "src/components/Page";
import { useAuth } from "src/navigation/PrivateRoute";
import NotificationSettingsForm from "src/components/profile/NotificationSettingsForm";
import ExportDataForm from "src/components/profile/ExportDataForm";
import ImportDataForm from "src/components/profile/ImportDataForm";

// ----------------------------------------------------------------------

export default function Settings() {
  let auth = useAuth();

  return (
    <>
      <Page title="Ustawienia">
        <Container maxWidth="xl" sx={{ mb: 3 }}>
          <Typography variant="h4" gutterBottom>
            Twoje konto
          </Typography>
          <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
              <NotificationSettingsForm />
            </Grid>
            {auth.user.providerData[0].providerId === "password" && (
              <Grid item xs={12} md={6}>
                <ChangePasswordForm />
              </Grid>
            )}
            <Grid item xs={12} md={6}>
              <DeleteAccountForm />
            </Grid>
          </Grid>
        </Container>
        <Container maxWidth="xl">
          <Typography variant="h4" gutterBottom>
            Twoje dane
          </Typography>
          <Grid container spacing={3}>
            <Grid item xs={12} md={6}>
              <ExportDataForm />
            </Grid>
            <Grid item xs={12} md={6}>
              <ImportDataForm />
            </Grid>
          </Grid>
        </Container>
      </Page>
    </>
  );
}
