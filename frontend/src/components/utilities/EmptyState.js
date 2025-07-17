import { Box, Button, Divider, Typography } from "@mui/material";
import { Link as RouterLink } from "react-router-dom";

export default function EmptyState({
  title,
  icon: Icon,
  text,
  bodyText,
  buttonUrl,
  buttonText,
  buttonOnClick,
  showImportButton = true,
}) {
  return (
    <Box sx={{ height: "100%", minHeight: 200 }}>
      {title && (
        <>
          <Typography variant="h4">{title}</Typography>
          <Divider sx={{ my: 2 }} />
        </>
      )}
      <Icon sx={{ fontSize: "100px", opacity: 0.5, mt: 1 }} />
      <Typography variant="subtitle2" sx={{ px: 2 }}>
        {text}
      </Typography>
      {bodyText && (
        <Typography variant="body2" sx={{ mt: 1, px: 2 }}>
          {bodyText}
        </Typography>
      )}
      {showImportButton && (
        <>
          <Button
            variant="outlined"
            component={RouterLink}
            to={process.env.PUBLIC_URL + "/settings"}
            sx={{ mt: 4 }}
          >
            Importuj dane
          </Button>
          <br />
        </>
      )}
      {buttonText &&
        (buttonOnClick ? (
          <Button variant="outlined" onClick={buttonOnClick} sx={{ mt: 1 }}>
            {buttonText}
          </Button>
        ) : (
          <Button
            variant="outlined"
            component={RouterLink}
            to={process.env.PUBLIC_URL + buttonUrl}
            sx={{ mt: 1 }}
          >
            {buttonText}
          </Button>
        ))}
    </Box>
  );
}
