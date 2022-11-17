import {useFormik} from "formik";
import {useEffect, useState} from "react";
import * as Yup from "yup";

const ValidationSchema = (fields) => {
  return Yup.object().shape(
    Object.fromEntries(fields.map((field) => [field.name, field.validation]))
  );
};

export function useForm(onCloseForm, onSave, entity, fields) {
  const [isLoaded, setIsLoaded] = useState(false);

  const formik = useFormik({
    initialValues: {},
    validationSchema: ValidationSchema(fields),
    onSubmit: async (values) => {
      handleCloseForm();
      await onSave(values);
    },
    validateOnChange: true,
    validateOnBlur: true,
  });

  useEffect(() => {
    if (!formik.isSubmitting) {
      fields.forEach((field) => {
        formik.initialValues[field.name] =
          field.value ?? (entity && entity[field.name]) ?? field.default;
      });

      formik.resetForm();
      setIsLoaded(true);
    }
  }, [entity]);

  const handleCloseForm = () => {
    if (onCloseForm) {
      onCloseForm();
      formik.resetForm();
    }
  };
  formik.isLoaded = isLoaded.toString();
  return formik;
}
