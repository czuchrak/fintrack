import {Link as RouterLink} from "react-router-dom";
import {styled} from "@mui/material/styles";
import {Box, Container, Link, Stack, Typography} from "@mui/material";
import Page from "src/components/Page";
import Logo from "src/components/Logo";
import ResetPasswordForm from "src/components/authentication/ResetPasswordForm";
import ErrorAlert from "src/components/ErrorAlert";

// ----------------------------------------------------------------------

const RootStyle = styled(Page)(({ theme }) => ({
  [theme.breakpoints.up("md")]: {
    display: "flex",
  },
}));

const ContentStyle = styled("div")(({ theme }) => ({
  maxWidth: 480,
  margin: "auto",
  display: "flex",
  minHeight: "100vh",
  flexDirection: "column",
  padding: theme.spacing(6, 0),
}));

// ----------------------------------------------------------------------

export default function ResetPassword() {
  return (
    <RootStyle title="Resetowanie hasła">
      <Container maxWidth="sm">
        <ContentStyle>
          <Box
            sx={{ py: 2, textAlign: "-webkit-center" }}
            to={process.env.PUBLIC_URL + "/"}
            component={RouterLink}
          >
            <Logo sx={{ height: 100 }} />
          </Box>
          <Stack sx={{ mb: 0, textAlign: "center" }}>
            <Typography variant="h5" gutterBottom>
              Resetowanie hasła
            </Typography>
          </Stack>

          <ErrorAlert />

          <ResetPasswordForm />

          <Typography variant="body2" align="center" sx={{ mt: 3 }}>
            <Link
              variant="subtitle2"
              component={RouterLink}
              to={process.env.PUBLIC_URL + "/login"}
            >
              Wróć na stronę logowania
            </Link>
          </Typography>
        </ContentStyle>
      </Container>
    </RootStyle>
  );
}
