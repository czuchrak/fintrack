import {Icon} from "@iconify/react";
import googleFill from "@iconify/icons-eva/google-fill";
import {Button, Divider, Typography} from "@mui/material";
import {useAuth} from "src/navigation/PrivateRoute";

// ----------------------------------------------------------------------

export default function AuthSocial() {
  let auth = useAuth();

  return (
    <>
      <Button
        fullWidth
        size="large"
        color="inherit"
        variant="outlined"
        onClick={auth.signInWithGoogle}
      >
        <Icon icon={googleFill} color="#DF3E30" height={24} />
        &nbsp;&nbsp;
        <Typography variant="subtitle2" sx={{ opacity: 0.7 }}>
          KONTYNUUJ Z GOOGLE
        </Typography>
      </Button>

      <Divider sx={{ my: 3 }}>
        <Typography variant="body2" sx={{ color: "text.secondary" }}>
          LUB
        </Typography>
      </Divider>
    </>
  );
}
