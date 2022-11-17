import {AppBar, Box, Button, Stack, Toolbar} from "@mui/material";
import {alpha, styled} from "@mui/material/styles";
import {Link as RouterLink} from "react-router-dom";
import Logo from "../Logo";

const APPBAR_MOBILE = 64;
const APPBAR_DESKTOP = 92;

const Navbar = styled(AppBar)(({ theme }) => ({
  backdropFilter: "blur(6px)",
  WebkitBackdropFilter: "blur(6px)", // Fix on Mobile
  backgroundColor: alpha(theme.palette.background.default, 0.72),
  [theme.breakpoints.up("md")]: {
    display: "flex",
  },
}));

const ToolbarStyle = styled(Toolbar)(({ theme }) => ({
  minHeight: APPBAR_MOBILE,
  [theme.breakpoints.up("lg")]: {
    minHeight: APPBAR_DESKTOP,
    padding: theme.spacing(0, 10),
  },
}));

export function HomeNavbar() {
  return (
    <Navbar>
      <ToolbarStyle>
        <Box sx={{ textAlign: "-webkit-center" }}>
          <Box
            to={process.env.PUBLIC_URL + "/"}
            component={RouterLink}
            onClick={() => window.scrollTo({ top: 0, behavior: "smooth" })}
          >
            <Logo sx={{ height: 50 }} />
          </Box>
        </Box>
        <Box sx={{ flexGrow: 1 }} />

        <Stack direction="row" alignItems="center" spacing={{ xs: 1.5 }}>
          <Button variant="outlined" href="https://fintrack.app/demo">
            Demo
          </Button>
          <Button
            variant="contained"
            component={RouterLink}
            to={process.env.PUBLIC_URL + "/login"}
          >
            Zaloguj się
          </Button>
        </Stack>
      </ToolbarStyle>
    </Navbar>
  );
}
