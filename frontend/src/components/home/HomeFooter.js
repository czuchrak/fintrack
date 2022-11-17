import {Grid, Link, Typography} from "@mui/material";
import {useTheme} from "@mui/material/styles";
import {Link as RouterLink} from "react-router-dom";

// ----------------------------------------------------------------------

export default function HomeFooter() {
  const theme = useTheme();

  return (
    <Grid
      container
      sx={{
        backgroundColor: theme.palette.background.neutral,
        textAlign: "center",
        py: theme.spacing(2),
      }}
    >
      <Grid item xs={12} md={4} />
      <Grid item xs={12} md={4}>
        <Typography>© 2021-{new Date().getFullYear()} Fintrack.app</Typography>
        <Typography variant="caption">
          <Link
            underline="hover"
            sx={{ color: "text.primary", cursor: "pointer" }}
            component={RouterLink}
            to={process.env.PUBLIC_URL + "/terms"}
          >
            Regulamin
          </Link>
          &nbsp;|&nbsp;
          <Link
            underline="hover"
            sx={{ color: "text.primary", cursor: "pointer" }}
            component={RouterLink}
            to={process.env.PUBLIC_URL + "/privacy"}
          >
            Polityka prywatności
          </Link>
        </Typography>
      </Grid>
      <Grid item xs={12} md={4} />
    </Grid>
  );
}
