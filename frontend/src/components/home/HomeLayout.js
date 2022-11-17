import {styled} from "@mui/material/styles";
import {HomeNavbar} from "src/components/home/HomeNavbar";
import HomeFooter from "src/components/home/HomeFooter";
import Page from "src/components/Page";

// ----------------------------------------------------------------------
const APPBAR_MOBILE = 64;
const APPBAR_DESKTOP = 92;

const RootStyle = styled(Page)(() => ({
  display: "flex",
  minHeight: "100%",
  overflow: "hidden",
  maxWidth: 2500,
  margin: "auto",
}));

const MainStyle = styled("div")(({ theme }) => ({
  flexGrow: 1,
  overflow: "auto",
  minHeight: "100%",
  paddingTop: APPBAR_MOBILE + 24,
  paddingBottom: theme.spacing(5),
  paddingRight: theme.spacing(4),
  paddingLeft: theme.spacing(4),
  [theme.breakpoints.up("lg")]: {
    marginTop: 30,
    paddingTop: APPBAR_DESKTOP + 24,
    paddingLeft: theme.spacing(10),
    paddingRight: theme.spacing(10),
  },
}));

// ----------------------------------------------------------------------

export default function HomeLayout({ children, title }) {
  return (
    <>
      <RootStyle title={title}>
        <HomeNavbar />
        <MainStyle>{children}</MainStyle>
      </RootStyle>
      <HomeFooter />
    </>
  );
}
