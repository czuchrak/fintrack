import PropTypes from "prop-types";
// material
import {alpha, styled} from "@mui/material/styles";
import {getCurrencyFormatter} from "../utils/helpers";
import {useMemo} from "react";

// ----------------------------------------------------------------------

const RootStyle = styled("span")(({ theme, styleprops }) => {
  const { color, variant } = styleprops;

  const styleFilled = (color) => ({
    color: theme.palette[color].contrastText,
    backgroundColor: theme.palette[color].main,
  });

  const styleOutlined = (color) => ({
    color: theme.palette[color].main,
    backgroundColor: "transparent",
    border: `1px solid ${theme.palette[color].main}`,
  });

  const styleGhost = (color) => ({
    color: theme.palette[color].dark,
    backgroundColor: alpha(theme.palette[color].main, 0.16),
  });

  return {
    height: 22,
    minWidth: 22,
    lineHeight: 0,
    borderRadius: 8,
    cursor: "default",
    alignItems: "center",
    whiteSpace: "nowrap",
    display: "inline-flex",
    justifyContent: "center",
    padding: theme.spacing(0, 1),
    color: theme.palette.grey[800],
    fontSize: theme.typography.pxToRem(12),
    fontFamily: theme.typography.fontFamily,
    backgroundColor: theme.palette.grey[300],
    fontWeight: theme.typography.fontWeightBold,

    ...(color !== "default"
      ? {
          ...(variant === "filled" && { ...styleFilled(color) }),
          ...(variant === "outlined" && { ...styleOutlined(color) }),
          ...(variant === "ghost" && { ...styleGhost(color) }),
        }
      : {
          ...(variant === "outlined" && {
            backgroundColor: "transparent",
            color: theme.palette.text.primary,
            border: `1px solid ${theme.palette.grey[500_32]}`,
          }),
          ...(variant === "ghost" && {
            color: theme.palette.text.secondary,
            backgroundColor: theme.palette.grey[500_16],
          }),
        }),
  };
});

// ----------------------------------------------------------------------

export default function Label({
  color = "default",
  variant = "ghost",
  children,
  ...other
}) {
  return (
    <RootStyle styleprops={{ color, variant }} {...other}>
      {children}
    </RootStyle>
  );
}

Label.propTypes = {
  children: PropTypes.node,
  color: PropTypes.oneOf([
    "default",
    "primary",
    "secondary",
    "info",
    "success",
    "warning",
    "error",
  ]),
  variant: PropTypes.oneOf(["filled", "outlined", "ghost"]),
};

export function DecimalLabel({
  currency,
  value,
  plus = false,
  invert = false,
}) {
  const color = useMemo(() => {
    const sign = invert ? -1 : 1;
    return (sign * value < 0 && "error") || "success";
  }, [value, invert]);

  return (
    <Label variant="ghost" color={color}>
      {plus && value > 0 ? "+" : ""}
      {getCurrencyFormatter(currency).format(value)}
    </Label>
  );
}

export function PercentLabel({ value, plus = false }) {
  return (
    <Label variant="ghost" color={(value < 0 && "error") || "success"}>
      {plus && value > 0 ? "+" : ""}
      {(value * 100).toFixed(2)}%
    </Label>
  );
}
