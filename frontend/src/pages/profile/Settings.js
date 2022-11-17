import {Container, Grid, Typography} from "@mui/material";
import ChangePasswordForm from "src/components/profile/ChangePasswordForm";
import DeleteAccountForm from "src/components/profile/DeleteAccountForm";
import Page from "src/components/Page";
import {useAuth} from "src/navigation/PrivateRoute";
import NotificationSettingsForm from "src/components/profile/NotificationSettingsForm";

// ----------------------------------------------------------------------

export default function Settings() {
  let auth = useAuth();

  return (
    <>
      <Page title="Ustawienia">
        <Container maxWidth="xl">
          <Typography variant="h4" gutterBottom>
            Ustawienia
          </Typography>
          <Grid container spacing={3} mt={1}>
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
      </Page>
    </>
  );
}
