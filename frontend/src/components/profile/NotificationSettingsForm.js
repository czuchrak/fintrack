import {useState} from "react";
import {styled} from "@mui/material/styles";
import {Card, Stack, Switch, Typography} from "@mui/material";
import {useAuth} from "src/navigation/PrivateRoute";
import {useSelector} from "react-redux";
import {setUserSetting} from "src/services";

// ----------------------------------------------------------------------

const RootStyle = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3),
  height: "100%",
}));

export default function NotificationSettingsForm() {
  let auth = useAuth();

  let { newMonthEmailEnabled, newsEmailEnabled } = useSelector(
    (state) => state.profile.userSettings
  );

  const [newMonth, setNewMonth] = useState(newMonthEmailEnabled);
  const [news, setNews] = useState(newsEmailEnabled);

  const setSetting = async (e) => {
    await setUserSetting(
      e.target.name,
      e.target.checked,
      await auth.getToken()
    );
    auth.refreshBckUser();
  };

  const newMonthEmailEnabledChange = async (e) => {
    setNewMonth(!newMonth);
    setSetting(e);
  };

  const newsEmailEnabledChange = async (e) => {
    setNews(!news);
    setSetting(e);
  };

  return (
    <RootStyle>
      <Typography variant="h6" mb={2}>
        Powiadomienia e-mail
      </Typography>
      <Stack direction="row" alignItems="center" justifyContent="space-between">
        <Typography gutterBottom>
          Powiadomienia o rozpoczęciu miesiąca
        </Typography>
        <Switch
          id="NewMonthEmailEnabled"
          name="NewMonthEmailEnabled"
          checked={newMonth}
          onChange={newMonthEmailEnabledChange}
        />
      </Stack>
      <Stack direction="row" alignItems="center" justifyContent="space-between">
        <Typography gutterBottom>
          Powiadomienia o nowościach w aplikacji
        </Typography>
        <Switch
          id="NewsEmailEnabled"
          name="NewsEmailEnabled"
          checked={news}
          onChange={newsEmailEnabledChange}
        />
      </Stack>
    </RootStyle>
  );
}
