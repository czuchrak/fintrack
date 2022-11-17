import {Link as RouterLink} from "react-router-dom";
import {styled} from "@mui/material/styles";
import {Box, Container, Link, Typography} from "@mui/material";
import Page from "../../components/Page";
import Logo from "src/components/Logo";
import AuthForm from "src/components/authentication/AuthForm";
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

export default function Auth() {
  return (
    <RootStyle>
      <Container maxWidth="sm">
        <ContentStyle>
          <Box
            sx={{ py: 2, textAlign: "-webkit-center" }}
            to={process.env.PUBLIC_URL + "/"}
            component={RouterLink}
          >
            <Logo sx={{ height: 100 }} />
          </Box>

          <ErrorAlert />

          <AuthForm />

          <Typography variant="body2" align="center" sx={{ mt: 3 }}>
            <Link
              variant="subtitle2"
              component={RouterLink}
              onClick={() => {
                window.location.href = process.env.PUBLIC_URL + "/login";
              }}
              to={process.env.PUBLIC_URL + "/login"}
            >
              Powrót
            </Link>
          </Typography>
        </ContentStyle>
      </Container>
    </RootStyle>
  );
}
