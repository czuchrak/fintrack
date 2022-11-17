import {Link as RouterLink} from "react-router-dom";
import {Link, Stack} from "@mui/material";
import {LoadingButton} from "@mui/lab";
import {useAuth} from "src/navigation/PrivateRoute";
import {useDispatch} from "react-redux";
import {removeError} from "src/redux/slices/errorSlice";
import {CheckboxInput, PasswordInput, TextInput} from "../form/Inputs";
import {useForm} from "../form/FormikHelper";
import {EmailValidation, PasswordValidation} from "../form/YupHelper";

// ----------------------------------------------------------------------

export default function LoginForm() {
  const dispatch = useDispatch();
  let auth = useAuth();

  const onSave = async (values) => {
    dispatch(removeError());
    const result = await auth.login(
      values.email,
      values.password,
      values.remember
    );
    if (result === "auth/wrong-password") {
      formik.setFieldError("password", "Niepoprawne hasło");
    } else if (result === "auth/user-not-found") {
      formik.setFieldError("email", "Użytkownik nie istnieje");
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
        error: "Hasło jest wymagane",
        test: (item) => item && item.length > 0,
      }),
    },
    { name: "remember", default: true },
  ]);

  return (
    <>
      <form onSubmit={formik.handleSubmit} key={formik.isLoaded}>
        <Stack spacing={3}>
          <TextInput name="email" label="Adres e-mail" formik={formik} />
          <PasswordInput name="password" label="Hasło" formik={formik} />
        </Stack>

        <Stack
          direction="row"
          alignItems="center"
          justifyContent="space-between"
          sx={{ my: 2 }}
        >
          <CheckboxInput
            name="remember"
            label="Pamiętaj mnie"
            formik={formik}
          />

          <Link
            component={RouterLink}
            variant="subtitle1"
            to={process.env.PUBLIC_URL + "/resetpassword"}
          >
            Zresetuj hasło
          </Link>
        </Stack>

        <LoadingButton
          fullWidth
          size="large"
          type="submit"
          variant="contained"
          loading={formik.isSubmitting}
        >
          Zaloguj się
        </LoadingButton>
      </form>
    </>
  );
}
