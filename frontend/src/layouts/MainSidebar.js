import PropTypes from "prop-types";
import {useEffect} from "react";
import {Link as RouterLink, useLocation} from "react-router-dom";
// material
import {Icon} from "@iconify/react";
import {styled} from "@mui/material/styles";
import {Box, Button, Drawer, IconButton, Stack, Typography,} from "@mui/material";
import facebookOutline from "@iconify/icons-eva/facebook-outline";
// components
import Logo from "src/components/Logo";
import Scrollbar from "src/components/Scrollbar";
import NavSection from "src/components/NavSection";
import {MHidden} from "src/components/@material-extend";
//
import {appConfig} from "src/config/config";

const demo = appConfig.demo;

// ----------------------------------------------------------------------

const DRAWER_WIDTH = 280;

const RootStyle = styled("div")(({ theme }) => ({
  [theme.breakpoints.up("lg")]: {
    flexShrink: 0,
    width: DRAWER_WIDTH,
  },
}));

// ----------------------------------------------------------------------

MainSidebar.propTypes = {
  isOpenSidebar: PropTypes.bool,
  onCloseSidebar: PropTypes.func,
};

export default function MainSidebar({ isOpenSidebar, onCloseSidebar }) {
  const { pathname } = useLocation();

  useEffect(() => {
    if (isOpenSidebar) {
      onCloseSidebar();
    }
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [pathname]);

  const renderContent = (
    <Scrollbar
      sx={{
        height: "100%",
        "& .simplebar-content": {
          height: "100%",
          display: "flex",
          flexDirection: "column",
        },
      }}
    >
      <Box sx={{ py: 4, textAlign: "-webkit-center" }}>
        <Box to={process.env.PUBLIC_URL + "/"} component={RouterLink}>
          <Logo sx={{ height: 100 }} />
        </Box>
      </Box>

      <NavSection />

      <Box sx={{ flexGrow: 1 }} />

      <Box sx={{ px: 2.5, pb: 3, mt: 10 }}>
        <Stack
          alignItems="center"
          spacing={3}
          sx={{
            px: 2.5,
            pb: 0,
            pt: 2,
            borderRadius: 2,
            position: "relative",
            bgcolor: "grey.200",
          }}
        >
          {demo ? (
            <>
              <Box sx={{ textAlign: "center" }}>
                <Typography gutterBottom variant="h6">
                  To jest wersja demo
                </Typography>
                <Typography variant="body2" sx={{ color: "text.secondary" }}>
                  Zarejestruj się, aby odblokować wszystkie funkcjonalności
                </Typography>
              </Box>

              <Button
                fullWidth
                href="https://fintrack.app/register"
                variant="contained"
              >
                Rejestracja
              </Button>
            </>
          ) : (
            <>
              <Box sx={{ textAlign: "center" }}>
                <Typography gutterBottom variant="h6">
                  Open source
                </Typography>
                <Typography variant="body2" sx={{ color: "text.secondary" }}>
                  Kod źródłowy aplikacji fintrack.app jest otwarty.
                </Typography>
              </Box>

              <Button
                fullWidth
                target="_blank"
                href={"https://github.com/czuchrak/fintrack"}
                variant="contained"
              >
                Zobacz na GitHub
              </Button>
            </>
          )}
          <IconButton
            size="small"
            component="a"
            href="https://www.facebook.com/appfintrack"
            target="_blank"
            style={{ margin: 0, marginTop: 1, marginBottom: 1 }}
          >
            <Icon icon={facebookOutline} sx={{ margin: 0 }} />
          </IconButton>
        </Stack>
      </Box>
    </Scrollbar>
  );

  return (
    <RootStyle>
      <MHidden width="lgUp">
        <Drawer
          open={isOpenSidebar}
          onClose={onCloseSidebar}
          PaperProps={{
            sx: { width: DRAWER_WIDTH },
          }}
        >
          {renderContent}
        </Drawer>
      </MHidden>

      <MHidden width="lgDown">
        <Drawer
          open
          variant="persistent"
          PaperProps={{
            sx: {
              width: DRAWER_WIDTH,
              bgcolor: "background.default",
            },
          }}
        >
          {renderContent}
        </Drawer>
      </MHidden>
    </RootStyle>
  );
}
