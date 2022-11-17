import * as Yup from "yup";

const RequiredText = "To pole jest wymagane";
const MaxDecimalValueText = (max) =>
  `Wartość nie może być większa niż ${max / 1000000} mln`;
const MaxPercentDecimalValueText = (max) =>
  `Wartość nie może być większa niż ${max}%`;
const MaxTextValueText = (max) => `Tekst musi być krótszy niż ${max} znaków.`;

export function DecimalValidation(max, required) {
  let result = Yup.number();
  if (max) result = result.max(max, MaxDecimalValueText(max));
  if (required) result = result.required(RequiredText);
  return result;
}

export function PercentDecimalValidation(max, required) {
  let result = Yup.number();
  if (max) result = result.max(max, MaxPercentDecimalValueText(max));
  if (required) result = result.required(RequiredText);
  return result;
}

export function TextValidation(max, required) {
  let result = Yup.string();
  if (max) result = result.max(max, MaxTextValueText(max));
  if (required) result = result.required(RequiredText);
  return result;
}

export function DateValidation(required, isValidDate) {
  let result = Yup.date();
  if (isValidDate) {
    result = result.test("isValidDate", isValidDate.error, isValidDate.test);
  }
  if (required) result = result.required(RequiredText);
  return result;
}

export function EmailValidation() {
  return Yup.string()
    .matches(
      /^(([^<>()[\]\\.,;:\s@"]+(\.[^<>()[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/,
      "Nieprawidłowy adres e-mail"
    )
    .max(100, "Maksymalna  długość  adresu e-mail wynosi 100 znaków")
    .required("Adres e-mail jest wymagany");
}

export function PasswordValidation(isValidPassword) {
  let result = Yup.string();
  if (isValidPassword) {
    result = result.test(
      "isValidPassword",
      isValidPassword.error,
      isValidPassword.test
    );
  }
  return result;
}

export function ObjectValidation(required) {
  let result = Yup.object().nullable();
  if (required) result = result.required(RequiredText);
  return result;
}

export function ArrayValidation(required) {
  let result = Yup.array().nullable();
  if (required) result = result.required(RequiredText).min(1, RequiredText);
  return result;
}
