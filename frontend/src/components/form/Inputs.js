import {
    Checkbox,
    FormControlLabel,
    IconButton,
    InputAdornment,
    MenuItem,
    Select,
    TextField,
    Tooltip,
} from "@mui/material";
import {Icon} from "@iconify/react";
import copyOutline from "@iconify/icons-eva/copy-outline";
import {getCurrencySymbol} from "../../utils/helpers";
import {useMemo, useState} from "react";
import eyeFill from "@iconify/icons-eva/eye-fill";
import eyeOffFill from "@iconify/icons-eva/eye-off-fill";

function InputProps(name, label, formik, onChange) {
  return {
    id: name,
    name: name,
    label: label,
    variant: "standard",
    value: formik.values[name],
    onChange: onChange ?? formik.handleChange,
    onBlur: formik.handleBlur,
    error: formik.touched[name] && Boolean(formik.errors[name]),
    helperText:
      formik.touched[name] &&
      Boolean(formik.errors[name]) &&
      formik.errors[name],
  };
}

export function TextInput({ name, label, formik, ...customParams }) {
  return (
    <TextField
      {...customParams}
      {...InputProps(name, label, formik)}
      type="text"
    />
  );
}

export function PasswordInput({ name, label, formik, ...customParams }) {
  const [showPassword, setShowPassword] = useState(false);
  return (
    <TextField
      {...customParams}
      {...InputProps(name, label, formik)}
      type={showPassword ? "text" : "password"}
      InputProps={{
        endAdornment: (
          <InputAdornment position="end">
            <IconButton
              onClick={() => setShowPassword(!showPassword)}
              edge="end"
            >
              <Icon icon={showPassword ? eyeFill : eyeOffFill} />
            </IconButton>
          </InputAdornment>
        ),
      }}
    />
  );
}

export function SelectInput({ children, name, label, formik, disabled }) {
  return (
    <TextField
      select
      {...InputProps(name, label, formik)}
      type="text"
      disabled={disabled}
    >
      {children}
    </TextField>
  );
}

export function DecimalInput({
  name,
  label,
  formik,
  lastValue,
  currency,
  percent = false,
}) {
  const endAdornment = useMemo(() => {
    return (
      <InputAdornment position="end">
        {getCurrencySymbol(currency, percent ? "percent" : "currency")}
        {lastValue && (
          <>
            &nbsp;
            <Tooltip title="Skopiuj ostatnie" placement="bottom">
              <IconButton
                size="small"
                onClick={() => {
                  formik.setFieldValue(name, lastValue);
                }}
              >
                <Icon icon={copyOutline} />
              </IconButton>
            </Tooltip>
          </>
        )}
      </InputAdornment>
    );
  }, [currency, percent, lastValue, formik, name]);

  return (
    <TextField
      {...InputProps(name, label, formik, (e) => {
        let value = e.target.value
          .replace(/\s/g, "")
          .replace(/[^0-9,.]/g, "")
          .replace(",", ".");
        if (percent) {
          if (/^\d{0,2}([.]\d{0,2})?$/.test(value) || value === "") {
            formik.setFieldValue(name, value);
          } else return;
        }

        if (/^\d+([.]?\d{0,2})?$/.test(value) || value === "")
          formik.setFieldValue(name, value);
      })}
      inputProps={{
        inputMode: "decimal",
      }}
      InputProps={{
        endAdornment,
      }}
      type="text"
      data-cy="decimalInput"
    />
  );
}

export function CheckboxInput({ name, label, formik }) {
  return (
    <FormControlLabel
      control={
        <Checkbox
          id={name}
          name={name}
          checked={formik.values[name]}
          onChange={formik.handleChange}
        />
      }
      label={label}
    />
  );
}

export function ChartRangeInput({ chartRange, handleRangeChange }) {
  return (
    <TextField
      select
      value={chartRange}
      onChange={handleRangeChange}
      variant="standard"
    >
      <MenuItem value="last12months">Ostatnie 12 miesięcy</MenuItem>
      <MenuItem value="thisyear">Bieżący rok</MenuItem>
      <MenuItem value="lastyear">Poprzedni rok</MenuItem>
      <MenuItem value="max">Od początku</MenuItem>
    </TextField>
  );
}

export function MultipleSelect({
  children,
  selected,
  handleChangeSelected,
  render,
}) {
  return (
    <Select
      variant="standard"
      multiple
      displayEmpty
      value={selected}
      onChange={handleChangeSelected}
      renderValue={render}
      MenuProps={{
        PaperProps: {
          style: {
            maxWidth: 250,
            maxHeight: 200,
          },
        },
        anchorOrigin: {
          vertical: "bottom",
          horizontal: "center",
        },
        transformOrigin: {
          vertical: "top",
          horizontal: "center",
        },
        variant: "menu",
      }}
    >
      {children}
    </Select>
  );
}
