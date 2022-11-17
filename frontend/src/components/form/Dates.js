import {TextField, Zoom} from "@mui/material";
import {getFnsLocale, getFullDate} from "../../utils/helpers";
import {AdapterDateFns} from "@mui/x-date-pickers/AdapterDateFns";
import {DatePicker, DateTimePicker, LocalizationProvider,} from "@mui/x-date-pickers";
import {useMemo} from "react";

const globalMinDate = new Date("2020-01-01");
const date = new Date();
date.setDate(1);
const globalMaxDate = date.setMonth(date.getMonth() + 1);

function TextInputProps(name, formik) {
  return {
    id: name,
    name: name,
    variant: "standard",
    type: "date",
    onBlur: formik.handleBlur,
    error: formik.touched[name] && Boolean(formik.errors[name]),
    helperText:
      formik.touched[name] &&
      Boolean(formik.errors[name]) &&
      formik.errors[name],
  };
}

export function DateInput({
  name,
  label,
  formik,
  inputFormat,
  views,
  disabled,
  minDate,
  maxDate,
}) {
  const customProps = useMemo(() => {
    const result = {};
    if (inputFormat) result.inputFormat = inputFormat;
    if (views) result.views = views;
    return result;
  }, [inputFormat, views]);

  return (
    <LocalizationProvider
      dateAdapter={AdapterDateFns}
      adapterLocale={getFnsLocale()}
    >
      <DatePicker
        {...customProps}
        disableMaskedInput
        label={label}
        disabled={disabled}
        minDate={minDate ?? globalMinDate}
        maxDate={maxDate ?? globalMaxDate}
        value={formik.values[name]}
        onChange={(newValue) => {
          if (views) newValue.setDate(1);
          formik.setFieldValue(name, newValue);
        }}
        desktopModeMediaQuery=""
        renderInput={(params) => (
          <TextField {...params} {...TextInputProps(name, formik)} />
        )}
        inputProps={{
          readOnly: true,
        }}
        TransitionComponent={Zoom}
      />
    </LocalizationProvider>
  );
}

export function DateTimeInput({ name, label, formik }) {
  return (
    <LocalizationProvider
      dateAdapter={AdapterDateFns}
      adapterLocale={getFnsLocale()}
    >
      <DateTimePicker
        mask="__.__.____ __:__"
        label={label}
        value={formik.values[name]}
        onChange={(newValue) => {
          formik.setFieldValue(name, getFullDate(newValue));
        }}
        renderInput={(params) => (
          <TextField {...params} {...TextInputProps(name, formik)} />
        )}
        TransitionComponent={Zoom}
      />
    </LocalizationProvider>
  );
}
