import {Link as RouterLink} from "react-router-dom";
import {styled} from "@mui/material/styles";
import {Box, Container, Link, Stack, Typography} from "@mui/material";
import Page from "src/components/Page";
import LoginForm from "src/components/authentication/LoginForm";
import AuthSocial from "src/components/authentication/AuthSocial";
import Logo from "src/components/Logo";
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
  padding: theme.spacing(3, 0),
}));

// ----------------------------------------------------------------------

export default function Login() {
  return (
    <RootStyle title="Logowanie">
      <Container maxWidth="sm">
        <ContentStyle>
          <Box
            sx={{ pb: 2, textAlign: "-webkit-center" }}
            to={process.env.PUBLIC_URL + "/"}
            component={RouterLink}
          >
            <Logo sx={{ height: 100 }} />
          </Box>
          <Stack sx={{ mb: 0, textAlign: "center" }}>
            <Typography variant="h5" gutterBottom>
              Logowanie
            </Typography>
          </Stack>

          <ErrorAlert />

          <AuthSocial />
          <LoginForm />

          <Typography variant="body2" align="center" sx={{ mt: 3 }}>
            Nie masz konta?&nbsp;
            <Link
              variant="subtitle2"
              component={RouterLink}
              to={process.env.PUBLIC_URL + "/register"}
            >
              Utwórz je teraz
            </Link>
          </Typography>
        </ContentStyle>
      </Container>
    </RootStyle>
  );
}
