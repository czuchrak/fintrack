import {Autocomplete, Chip, TextField} from "@mui/material";
import {getCurrencySymbol} from "../../utils/helpers";
import {Fragment} from "react";

function TextInputProps(name, label, formik) {
  return {
    label: label,
    variant: "standard",
    onBlur: formik.handleBlur,
    error: formik.touched[name] && Boolean(formik.errors[name]),
    helperText:
      formik.touched[name] &&
      Boolean(formik.errors[name]) &&
      formik.errors[name],
  };
}

export function AutoCompleteInput({ name, label, formik, options, disabled }) {
  return (
    <Autocomplete
      id={name}
      name={name}
      options={options}
      disabled={disabled}
      isOptionEqualToValue={(option, value) => option.id === value.id}
      groupBy={(option) => option.groupBy}
      getOptionLabel={(option) => option.name}
      value={formik.values[name]}
      onChange={(event, newValue) => {
        formik.setFieldValue(name, newValue);
      }}
      clearText="Wyczyść"
      closeText="Zamknij"
      openText="Otwórz"
      noOptionsText="Brak wyników"
      renderInput={(params) => (
        <TextField {...params} {...TextInputProps(name, label, formik)} />
      )}
    />
  );
}

export function AutoCompleteMultiInput({ name, label, formik, options }) {
  return (
    <Autocomplete
      multiple
      id={name}
      name={name}
      options={options}
      disableCloseOnSelect
      limitTags={1}
      size="small"
      groupBy={(option) => option.groupBy}
      getOptionLabel={(option) => option.name}
      isOptionEqualToValue={(option, value) => option.id === value.id}
      value={formik.values[name]}
      onChange={(event, newValue) => {
        formik.setFieldValue(name, newValue);
      }}
      renderOption={(props, option) => (
        <li {...props}>
          {option.name} ({getCurrencySymbol(option.currency)})
        </li>
      )}
      clearText="Wyczyść"
      closeText="Zamknij"
      openText="Otwórz"
      noOptionsText="Brak wyników"
      renderTags={(value, getTagProps) => {
        const numTags = value.length;
        return value.slice(0, 1).map((option, index) => (
          <Fragment key={option.name + index}>
            <Chip
              variant="filled"
              label={option.name}
              size="small"
              {...getTagProps({ index })}
            />
            {numTags > 1 && ` +${numTags - 1}`}
          </Fragment>
        ));
      }}
      renderInput={(params) => (
        <TextField {...params} {...TextInputProps(name, label, formik)} />
      )}
    />
  );
}
