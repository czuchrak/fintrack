import {MenuItem} from "@mui/material";
import Sidebar from "../utilities/Sidebar";
import {SelectInput, TextInput} from "../form/Inputs";
import {useForm} from "../form/FormikHelper";
import {TextValidation} from "../form/YupHelper";
import usePropertyServiceActions from "../../serviceActions/PropertyServiceActions";

export default function PropertySidebar({ isOpenForm, onCloseForm, property }) {
  const propertyServiceActions = usePropertyServiceActions();
  const formik = useForm(
    onCloseForm,
    propertyServiceActions.saveProperty,
    property,
    [
      { name: "id" },
      { name: "name", default: "", validation: TextValidation(40, true) },
      { name: "isActive", default: true },
    ]
  );

  return (
    <Sidebar
      isOpenForm={isOpenForm}
      onCloseForm={onCloseForm}
      formik={formik}
      isAdding={!property.id}
    >
      <TextInput name="name" label="Nazwa" formik={formik} autoFocus />
      <SelectInput name="isActive" label="Aktywność" formik={formik}>
        <MenuItem value={true}>Aktywne</MenuItem>
        <MenuItem value={false}>Nieaktywne</MenuItem>
      </SelectInput>
    </Sidebar>
  );
}
