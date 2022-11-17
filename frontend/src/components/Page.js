import PropTypes from "prop-types";
import {Helmet} from "react-helmet-async";
import {forwardRef} from "react";
// material
import {Box} from "@mui/material";

// ----------------------------------------------------------------------

const Page = forwardRef(({ children, title = "", ...other }, ref) => (
  <Box ref={ref} {...other}>
    <Helmet>
      <title>
        {title === ""
          ? "Fintrack.app — zyskaj świadomość wartości Twojego majątku"
          : title + " | Fintrack.app"}
      </title>
    </Helmet>
    {children}
  </Box>
));

Page.propTypes = {
  children: PropTypes.node.isRequired,
  title: PropTypes.string,
};

export default Page;
