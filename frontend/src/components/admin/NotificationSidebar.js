import { MenuItem } from "@mui/material";
import { getFullDate } from "src/utils/helpers";
import Sidebar from "../utilities/Sidebar";
import { CheckboxInput, SelectInput, TextInput } from "../form/Inputs";
import { DateTimeInput } from "../form/Dates";
import { useForm } from "../form/FormikHelper";
import { DateValidation, TextValidation } from "../form/YupHelper";

export default function NotificationSidebar({
  isOpenForm,
  onCloseForm,
  onSave,
  notification,
}) {
  const formik = useForm(onCloseForm, onSave, notification, [
    { name: "id" },
    { name: "type", default: "maintenance" },
    { name: "message", default: "", validation: TextValidation(null, true) },
    { name: "url", default: "" },
    {
      name: "validFrom",
      default: getFullDate(new Date()),
      validation: DateValidation(true),
    },
    {
      name: "validUntil",
      default: getFullDate(new Date()),
      validation: DateValidation(true),
    },
    { name: "isActive", default: false },
  ]);

  return (
    <Sidebar
      isOpenForm={isOpenForm}
      onCloseForm={onCloseForm}
      formik={formik}
      isAdding={!notification.id}
    >
      <SelectInput name="type" label="Typ" formik={formik}>
        <MenuItem value="maintenance">Przerwa techniczna</MenuItem>
        <MenuItem value="update">Nowa wersja aplikacji</MenuItem>
        <MenuItem value="networthdata">Dodawanie wartości majątku</MenuItem>
        <MenuItem value="contact">Kontakt</MenuItem>
        <MenuItem value="propertySettings">Ustawienia nieruchomości</MenuItem>
      </SelectInput>

      <TextInput name="message" label="Message" formik={formik} />

      <SelectInput name="url" label="Url" formik={formik}>
        <MenuItem value="">-</MenuItem>
        <MenuItem value="https://github.com/czuchrak/fintrack/blob/main/CHANGELOG.md">
          Changelog
        </MenuItem>
        <MenuItem value="/networth/data">NetWorthData</MenuItem>
        <MenuItem value="/contact">Contact</MenuItem>
        <MenuItem value="/settings">Ustawienia</MenuItem>
      </SelectInput>

      <DateTimeInput name="validFrom" label="ValidFrom" formik={formik} />
      <DateTimeInput name="validUntil" label="ValidUntil" formik={formik} />

      <CheckboxInput name="isActive" label="Aktywne" formik={formik} />
    </Sidebar>
  );
}
