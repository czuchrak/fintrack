import {MenuItem} from "@mui/material";
import Sidebar from "../utilities/Sidebar";
import {SelectInput, TextInput} from "../form/Inputs";
import {useForm} from "../form/FormikHelper";
import {TextValidation} from "../form/YupHelper";
import useNetWorthServiceActions from "../../serviceActions/NetWorthServiceActions";
import {useSelector} from "react-redux";

export default function NetWorthPartSidebar({ isOpenForm, onCloseForm, part }) {
  const netWorthServiceActions = useNetWorthServiceActions();

  let { currencies, currency } = useSelector((state) => state.profile);

  const formik = useForm(onCloseForm, netWorthServiceActions.savePart, part, [
    { name: "id" },
    { name: "name", default: "", validation: TextValidation(40, true) },
    { name: "type", default: "asset" },
    { name: "currency", default: currency },
    { name: "isVisible", default: true },
  ]);

  return (
    <Sidebar
      isOpenForm={isOpenForm}
      onCloseForm={onCloseForm}
      formik={formik}
      isAdding={!part.id}
    >
      <TextInput name="name" label="Nazwa" formik={formik} autoFocus />
      <SelectInput name="type" label="Typ" formik={formik}>
        <MenuItem value="asset">Aktywo</MenuItem>
        <MenuItem value="liability">Zobowiązanie</MenuItem>
      </SelectInput>
      <SelectInput
        name="currency"
        label="Waluta"
        formik={formik}
        disabled={part.id}
      >
        {currencies.map((c) => {
          return (
            <MenuItem key={c} value={c}>
              {c}
            </MenuItem>
          );
        })}
      </SelectInput>
      <SelectInput name="isVisible" label="Widoczność" formik={formik}>
        <MenuItem value={true}>Widoczne</MenuItem>
        <MenuItem value={false}>Ukryte</MenuItem>
      </SelectInput>
    </Sidebar>
  );
}
