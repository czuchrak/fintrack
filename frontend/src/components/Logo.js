import PropTypes from "prop-types";
// material
import {Box} from "@mui/material";

// ----------------------------------------------------------------------

Logo.propTypes = {
  sx: PropTypes.object,
};

export default function Logo({ sx }) {
  return (
    <Box
      component="img"
      src={process.env.PUBLIC_URL + "/static/logo.png"}
      sx={{ ...sx }}
    />
  );
}
