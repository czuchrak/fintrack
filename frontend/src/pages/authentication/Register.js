import {Link as RouterLink} from "react-router-dom";
import {styled} from "@mui/material/styles";
import {Box, Container, Link, Typography} from "@mui/material";
import Page from "src/components/Page";
import RegisterForm from "src/components/authentication/RegisterForm";
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

export default function Register() {
  return (
    <RootStyle title="Rejestracja">
      <Container>
        <ContentStyle>
          <Box
            sx={{ pb: 2, textAlign: "-webkit-center" }}
            to={process.env.PUBLIC_URL + "/"}
            component={RouterLink}
          >
            <Logo sx={{ height: 100 }} />
          </Box>
          <Box sx={{ mb: 0, textAlign: "center" }}>
            <Typography variant="h5" gutterBottom>
              Rejestracja
            </Typography>
          </Box>

          <ErrorAlert />

          <AuthSocial />
          <RegisterForm />

          <Typography
            variant="body2"
            align="center"
            sx={{ color: "text.secondary", mt: 3 }}
          >
            Rejestrując się, akceptujesz&nbsp;
            <Link
              underline="always"
              sx={{ color: "text.primary" }}
              component={RouterLink}
              to={process.env.PUBLIC_URL + "/terms"}
            >
              Regulamin
            </Link>
            &nbsp;oraz&nbsp;
            <Link
              underline="always"
              sx={{ color: "text.primary" }}
              component={RouterLink}
              to={process.env.PUBLIC_URL + "/privacy"}
            >
              Politykę prywatności
            </Link>
            .
          </Typography>

          <Typography variant="body2" sx={{ mt: 3, textAlign: "center" }}>
            Masz już konto?&nbsp;
            <Link
              variant="subtitle2"
              to={process.env.PUBLIC_URL + "/login"}
              component={RouterLink}
            >
              Zaloguj się
            </Link>
          </Typography>
        </ContentStyle>
      </Container>
    </RootStyle>
  );
}
