import {Stack} from "@mui/material";
import {LoadingButton} from "@mui/lab";
import {useAuth} from "src/navigation/PrivateRoute";
import {useDispatch} from "react-redux";
import {removeError} from "src/redux/slices/errorSlice";
import {PasswordInput, TextInput} from "../form/Inputs";
import {useForm} from "../form/FormikHelper";
import {EmailValidation, PasswordValidation} from "../form/YupHelper";

// ----------------------------------------------------------------------

export default function RegisterForm() {
  const dispatch = useDispatch();
  let auth = useAuth();

  const onSave = async (values) => {
    dispatch(removeError());
    const result = await auth.register(values.email, values.password);
    if (result === "auth/email-already-in-use") {
      formik.setFieldError("email", "Konto z tym adresem email już istnieje");
    }
  };

  const formik = useForm(null, onSave, null, [
    {
      name: "email",
      default: "",
      validation: EmailValidation(),
    },
    {
      name: "password",
      default: "",
      validation: PasswordValidation({
        error: "Hasło jest za krótkie - wymagane jest minimum 8 znaków",
        test: (item) => {
          return item && item.length >= 8;
        },
      }),
    },
  ]);

  return (
    <>
      <form onSubmit={formik.handleSubmit}>
        <Stack spacing={3} key={formik.isLoaded}>
          <TextInput name="email" label="Adres e-mail" formik={formik} />
          <PasswordInput name="password" label="Hasło" formik={formik} />

          <LoadingButton
            fullWidth
            size="large"
            type="submit"
            variant="contained"
            loading={formik.isSubmitting}
          >
            Zarejestruj się
          </LoadingButton>
        </Stack>
      </form>
    </>
  );
}
