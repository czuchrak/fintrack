import {useMemo, useState} from "react";
import {Icon} from "@iconify/react";
import {matchPath, NavLink as RouterLink, useLocation,} from "react-router-dom";
import arrowIosForwardFill from "@iconify/icons-eva/arrow-ios-forward-fill";
import arrowIosDownwardFill from "@iconify/icons-eva/arrow-ios-downward-fill";
import {alpha, styled, useTheme} from "@mui/material/styles";
import {Box, Collapse, List, ListItemButton, ListItemIcon, ListItemText,} from "@mui/material";
import trendingUpFill from "@iconify/icons-eva/trending-up-fill";
import homeFill from "@iconify/icons-eva/home-fill";
import starFill from "@iconify/icons-eva/star-fill";
import {useSelector} from "react-redux";
import {useAuth} from "../navigation/PrivateRoute";
import {appConfig} from "../config/config";

const demo = appConfig.demo;

// ----------------------------------------------------------------------

const ListItemStyle = styled((props) => (
  <ListItemButton disableGutters {...props} />
))(({ theme }) => ({
  ...theme.typography.body2,
  height: 48,
  position: "relative",
  paddingLeft: theme.spacing(5),
  paddingRight: theme.spacing(2.5),
  color: theme.palette.text.secondary,
  "&:before": {
    top: 0,
    right: 0,
    width: 3,
    bottom: 0,
    content: "''",
    display: "none",
    position: "absolute",
    borderTopLeftRadius: 4,
    borderBottomLeftRadius: 4,
    backgroundColor: theme.palette.primary.main,
  },
}));

const ListItemIconStyle = styled(ListItemIcon)({
  width: 22,
  height: 22,
  display: "flex",
  alignItems: "center",
  justifyContent: "center",
});

function NavItem({ item, active }) {
  const theme = useTheme();
  const isActiveRoot = active(item.path);
  const { title, path, icon, info, children, disabled } = item;
  const [open, setOpen] = useState(isActiveRoot);

  const handleOpen = () => {
    setOpen((prev) => !prev);
  };

  const activeRootStyle = {
    color: "primary.main",
    fontWeight: "fontWeightMedium",
    bgcolor: alpha(
      theme.palette.primary.main,
      theme.palette.action.selectedOpacity
    ),
    "&:before": { display: "block" },
  };

  const activeSubStyle = {
    color: "text.primary",
    fontWeight: "fontWeightMedium",
  };

  if (children) {
    return (
      <>
        <ListItemStyle
          disabled={disabled}
          onClick={handleOpen}
          sx={{
            ...(isActiveRoot && activeRootStyle),
          }}
        >
          <ListItemIconStyle>{icon && icon}</ListItemIconStyle>
          <ListItemText disableTypography primary={title} />
          {info && info}
          <Box
            component={Icon}
            icon={open ? arrowIosDownwardFill : arrowIosForwardFill}
            sx={{ width: 16, height: 16, ml: 1 }}
          />
        </ListItemStyle>

        <Collapse in={open} timeout="auto" unmountOnExit>
          <List component="div" disablePadding>
            {children.map((item) => {
              const { title, path } = item;
              const isActiveSub = active(path);

              return (
                <ListItemStyle
                  key={title}
                  component={RouterLink}
                  to={path}
                  sx={{
                    ...(isActiveSub && activeSubStyle),
                  }}
                >
                  <ListItemIconStyle>
                    <Box
                      component="span"
                      sx={{
                        width: 4,
                        height: 4,
                        display: "flex",
                        borderRadius: "50%",
                        alignItems: "center",
                        justifyContent: "center",
                        bgcolor: "text.disabled",
                        transition: (theme) =>
                          theme.transitions.create("transform"),
                        ...(isActiveSub && {
                          transform: "scale(2)",
                          bgcolor: "primary.main",
                        }),
                      }}
                    />
                  </ListItemIconStyle>
                  <ListItemText disableTypography primary={title} />
                </ListItemStyle>
              );
            })}
          </List>
        </Collapse>
      </>
    );
  }

  return (
    <ListItemStyle
      component={RouterLink}
      to={path}
      sx={{
        ...(isActiveRoot && activeRootStyle),
      }}
    >
      <ListItemIconStyle>{icon && icon}</ListItemIconStyle>
      <ListItemText disableTypography primary={title} />
      {info && info}
    </ListItemStyle>
  );
}

export default function NavSection({ ...other }) {
  const { pathname } = useLocation();
  let auth = useAuth();
  let properties = useSelector((state) => state.profile.properties);

  const navConfig = useMemo(() => {
    const getIcon = (name) => <Icon icon={name} width={22} height={22} />;
    const start = !demo && auth.user && !auth.user.emailVerified;

    const result = [];

    if (start) {
      result.push({
        title: "Start",
        path: process.env.PUBLIC_URL + "/onboarding",
        icon: getIcon(starFill),
      });
    }

    result.push({
      title: "Wartość netto",
      path: process.env.PUBLIC_URL + "/networth",
      disabled: start,
      icon: getIcon(trendingUpFill),
      children: [
        {
          title: "Przegląd",
          path: process.env.PUBLIC_URL + "/networth/dashboard",
        },
        {
          title: "Cele",
          path: process.env.PUBLIC_URL + "/networth/goals",
        },
        {
          title: "Dane",
          path: process.env.PUBLIC_URL + "/networth/data",
        },
        {
          title: "Składniki",
          path: process.env.PUBLIC_URL + "/networth/parts",
        },
      ],
    });

    let propertiesChildren = properties
      .slice()
      .filter((x) => x.isActive)
      .map((property) => {
        return {
          title: property.name,
          path: process.env.PUBLIC_URL + `/property/${property.id}`,
        };
      });

    propertiesChildren.push({
      title: "Ustawienia",
      path: process.env.PUBLIC_URL + "/property/settings",
    });

    result.push({
      title: "Nieruchomości",
      path: process.env.PUBLIC_URL + "/property",
      disabled: start,
      icon: getIcon(homeFill),
      children: propertiesChildren,
    });

    return result;
  }, [properties]);

  const match = (path) => {
    return path ? !!matchPath({ path, end: false }, pathname) : false;
  };

  return (
    <Box {...other}>
      <List disablePadding key={pathname}>
        {navConfig.map((item) => (
          <NavItem key={item.title} item={item} active={match} />
        ))}
      </List>
    </Box>
  );
}
