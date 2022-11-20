import { Icon } from "@iconify/react";
import { useRef, useState } from "react";
import settingsFill from "@iconify/icons-eva/settings-fill";
import emailFill from "@iconify/icons-eva/email-fill";
import briefcaseOutline from "@iconify/icons-eva/briefcase-outline";
import person from "@iconify/icons-eva/person-fill";
import { Link as RouterLink } from "react-router-dom";
// material
import { alpha } from "@mui/material/styles";
import {
  Box,
  Button,
  Divider,
  IconButton,
  MenuItem,
  Typography,
  Zoom,
} from "@mui/material";
// components
import MenuPopover from "src/components/MenuPopover";
//
import { useAuth } from "src/navigation/PrivateRoute";
import { appConfig } from "src/config/config";

const demo = appConfig.demo;

// ----------------------------------------------------------------------

export default function AccountPopover() {
  const anchorRef = useRef(null);
  const [open, setOpen] = useState(false);
  let auth = useAuth();

  const handleOpen = () => {
    setOpen(true);
  };
  const handleClose = () => {
    setOpen(false);
  };

  const MENU_OPTIONS = [
    {
      label: "Administracja",
      icon: briefcaseOutline,
      linkTo: process.env.PUBLIC_URL + "/admin",
      visible: !demo && auth.user && auth.user.bck && auth.user.bck.isAdmin,
    },
    {
      label: "Ustawienia",
      icon: settingsFill,
      linkTo: process.env.PUBLIC_URL + "/settings",
      visible: true,
    },
    {
      label: "Kontakt",
      icon: emailFill,
      linkTo: process.env.PUBLIC_URL + "/contact",
      visible: true,
    },
  ];

  return (
    <>
      <IconButton
        ref={anchorRef}
        color={open ? "primary" : "default"}
        onClick={handleOpen}
        sx={{
          padding: 0,
          width: 44,
          height: 44,
          ...(open && {
            "&:before": {
              zIndex: 1,
              content: "''",
              width: "100%",
              height: "100%",
              borderRadius: "50%",
              position: "absolute",
              bgcolor: (theme) =>
                alpha(
                  theme.palette.primary.main,
                  theme.palette.action.focusOpacity
                ),
            },
          }),
        }}
        data-cy="profile"
      >
        <Icon icon={person} width={20} height={20} />
      </IconButton>

      <MenuPopover
        open={open}
        onClose={handleClose}
        anchorEl={anchorRef.current}
        sx={{ width: 220 }}
        TransitionComponent={Zoom}
      >
        <Box sx={{ my: 1.5, px: 2.5 }}>
          <Typography variant="body2" sx={{ color: "text.secondary" }} noWrap>
            {demo ? "mail@example.com" : auth.user.email}
          </Typography>
        </Box>

        <Divider sx={{ my: 1 }} />

        {MENU_OPTIONS.map(
          (option) =>
            option.visible && (
              <MenuItem
                key={option.label}
                to={option.linkTo}
                component={RouterLink}
                onClick={handleClose}
                sx={{ typography: "body2", py: 1, px: 2.5 }}
                disabled={demo}
              >
                <Box
                  component={Icon}
                  icon={option.icon}
                  sx={{
                    mr: 2,
                    width: 24,
                    height: 24,
                  }}
                />

                {option.label}
              </MenuItem>
            )
        )}

        <Box sx={{ p: 2, pt: 1.5 }}>
          <Button
            fullWidth
            color="inherit"
            variant="outlined"
            onClick={!demo ? auth.logout : () => {}}
            disabled={demo}
          >
            Wyloguj
          </Button>
        </Box>
      </MenuPopover>
    </>
  );
}
