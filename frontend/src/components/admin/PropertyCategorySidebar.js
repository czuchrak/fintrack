import Sidebar from "../utilities/Sidebar";
import {CheckboxInput, TextInput} from "../form/Inputs";
import {useForm} from "../form/FormikHelper";
import {TextValidation} from "../form/YupHelper";

export default function PropertyCategorySidebar({
  isOpenForm,
  onCloseForm,
  onSave,
  propertyCategory,
}) {
  const formik = useForm(onCloseForm, onSave, propertyCategory, [
    { name: "id" },
    { name: "type", default: "", validation: TextValidation(null, true) },
    { name: "name", default: "", validation: TextValidation(null, true) },
    { name: "isCost", default: false },
  ]);

  return (
    <Sidebar
      isOpenForm={isOpenForm}
      onCloseForm={onCloseForm}
      formik={formik}
      isAdding={!propertyCategory.id}
    >
      <TextInput name="name" label="Nazwa" formik={formik} />
      <TextInput name="type" label="Typ" formik={formik} />
      <CheckboxInput name="isCost" label="Koszt" formik={formik} />
    </Sidebar>
  );
}
