import PropTypes from "prop-types";
import {useRef, useState} from "react";
import {useLocation, useNavigate} from "react-router-dom";
import {Icon} from "@iconify/react";
import bellFill from "@iconify/icons-eva/bell-fill";
import clockFill from "@iconify/icons-eva/clock-fill";
import alertTriangleOutline from "@iconify/icons-eva/alert-triangle-outline";
import checkmarkCircleOutline from "@iconify/icons-eva/checkmark-circle-outline";
import doneAllFill from "@iconify/icons-eva/done-all-fill";
// material
import {alpha} from "@mui/material/styles";
import {
    Avatar,
    Badge,
    Box,
    Divider,
    IconButton,
    List,
    ListItemAvatar,
    ListItemButton,
    ListItemText,
    Tooltip,
    Typography,
    Zoom,
} from "@mui/material";
// components
import Scrollbar from "src/components/Scrollbar";
import MenuPopover from "src/components/MenuPopover";
import {useAuth} from "src/navigation/PrivateRoute";
import {getFullDateInText} from "src/utils/helpers";
import {markNotificationAsRead} from "src/services";
import databaseFilled from "@iconify/icons-ant-design/database-filled";
import emailFill from "@iconify/icons-eva/email-fill";
import {useSelector} from "react-redux";
import {appConfig} from "src/config/config";
import homeFill from "@iconify/icons-eva/home-fill";

const demo = appConfig.demo;

// ----------------------------------------------------------------------

export default function NotificationsPopover() {
  let auth = useAuth();
  let navigate = useNavigate();
  let location = useLocation();

  let notifications = useSelector((state) => state.profile.notifications);
  const totalUnRead = notifications.filter((x) => !x.isRead).length;

  const anchorRef = useRef(null);
  const [open, setOpen] = useState(false);

  const handleOpen = () => {
    setOpen(true);
  };

  const handleClose = () => {
    setOpen(false);
  };

  const handleMarkAllAsRead = async () => {
    if (!demo) {
      const token = await auth.getToken();
      await Promise.all(
        notifications.map(async (notification) => {
          if (!notification.isRead) {
            await markNotificationAsRead(notification.id, token);
          }
        })
      );
      auth.refreshBckUser();
    }
  };

  const navigateToUrl = (url) => {
    if (url && location.pathname !== process.env.PUBLIC_URL + url) {
      handleClose();
      navigate(process.env.PUBLIC_URL + url);
    }
  };

  const handleMarkAsRead = async (notification) => {
    navigateToUrl(notification.url);
    if (!notification.isRead && !demo) {
      await markNotificationAsRead(notification.id, await auth.getToken());
      auth.refreshBckUser();
    }
  };

  function renderContent(notification) {
    if (notification.type === "maintenance") {
      return {
        avatar: <Icon icon={alertTriangleOutline} />,
        title: "Przerwa techniczna",
      };
    } else if (notification.type === "update") {
      return {
        avatar: <Icon icon={checkmarkCircleOutline} />,
        title: "Nowa wersja aplikacji",
      };
    } else if (notification.type === "networthdata") {
      return {
        avatar: <Icon icon={databaseFilled} />,
        title: "Nowy miesiąc!",
      };
    } else if (notification.type === "contact") {
      return {
        avatar: <Icon icon={emailFill} />,
        title: "Prośba o opinię",
      };
    } else if (notification.type === "propertySettings") {
      return {
        avatar: <Icon icon={homeFill} />,
        title: "Twoje nieruchomości",
      };
    }
    return {
      avatar: null,
      title: null,
    };
  }

  NotificationItem.propTypes = {
    notification: PropTypes.object.isRequired,
  };

  function NotificationItem({ notification }) {
    const { avatar, title } = renderContent(notification);

    return (
      <ListItemButton
        disableGutters
        onClick={() => handleMarkAsRead(notification)}
        sx={{
          py: 1.5,
          px: 2.5,
          mt: "1px",
          ...(!notification.isRead && {
            bgcolor: "action.selected",
          }),
        }}
      >
        <ListItemAvatar>
          <Avatar sx={{ bgcolor: "green" }}>{avatar}</Avatar>
        </ListItemAvatar>
        <ListItemText
          primary={
            <>
              <Typography variant="subtitle2">{title}</Typography>
              <Typography
                component="span"
                variant="body2"
                sx={{ color: "text.secondary" }}
              >
                {notification.message}
              </Typography>
            </>
          }
          secondary={
            demo ? (
              ""
            ) : (
              <Typography
                variant="caption"
                sx={{
                  mt: 0.5,
                  display: "flex",
                  alignItems: "center",
                  color: "text.disabled",
                }}
              >
                <Box
                  component={Icon}
                  icon={clockFill}
                  sx={{ mr: 0.5, width: 16, height: 16 }}
                />
                {getFullDateInText(notification.date)}
              </Typography>
            )
          }
        />
      </ListItemButton>
    );
  }

  return (
    <>
      {notifications.length > 0 && (
        <IconButton
          ref={anchorRef}
          size="large"
          color={open ? "primary" : "default"}
          onClick={handleOpen}
          sx={{
            ...(open && {
              bgcolor: (theme) =>
                alpha(
                  theme.palette.primary.main,
                  theme.palette.action.focusOpacity
                ),
            }),
          }}
        >
          <Badge badgeContent={totalUnRead} color="error">
            <Icon icon={bellFill} width={20} height={20} />
          </Badge>
        </IconButton>
      )}

      <MenuPopover
        open={open}
        onClose={handleClose}
        anchorEl={anchorRef.current}
        sx={{ width: 360 }}
        TransitionComponent={Zoom}
      >
        <Box sx={{ display: "flex", alignItems: "center", py: 2, px: 2.5 }}>
          <Box sx={{ flexGrow: 1 }}>
            <Typography variant="subtitle1">Powiadomienia</Typography>
          </Box>

          {totalUnRead > 0 && (
            <Tooltip title="Oznacz wszystkie jako przeczytane">
              <span>
                <IconButton
                  color="primary"
                  onClick={handleMarkAllAsRead}
                  disabled={demo}
                >
                  <Icon icon={doneAllFill} width={20} height={20} />
                </IconButton>
              </span>
            </Tooltip>
          )}
        </Box>

        <Divider />

        <Scrollbar sx={{ height: { xs: "auto" } }}>
          <List disablePadding>
            {notifications.map((notification) => (
              <NotificationItem
                key={notification.id}
                notification={notification}
              />
            ))}
          </List>
        </Scrollbar>
      </MenuPopover>
    </>
  );
}
