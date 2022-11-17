import {useState} from "react";
import {Stack, Typography} from "@mui/material";
import {LoadingButton} from "@mui/lab";
import {useAuth} from "src/navigation/PrivateRoute";
import {useDispatch} from "react-redux";
import {removeError} from "src/redux/slices/errorSlice";
import {TextInput} from "../form/Inputs";
import {useForm} from "../form/FormikHelper";
import {EmailValidation} from "../form/YupHelper";

// ----------------------------------------------------------------------

export default function ResetPasswordForm() {
  const dispatch = useDispatch();
  const [sent, setSent] = useState(false);
  let auth = useAuth();

  const onSave = async (values) => {
    dispatch(removeError());
    const result = await auth.resetPassword(values.email);
    if (result === "auth/user-not-found") {
      formik.setFieldError("email", "Użytkownik nie istnieje");
    } else setSent(true);
  };

  const formik = useForm(null, onSave, null, [
    {
      name: "email",
      default: "",
      validation: EmailValidation(),
    },
  ]);

  return (
    <>
      <form onSubmit={formik.handleSubmit} key={formik.isLoaded}>
        {sent ? (
          <Typography textAlign="center">
            Link do zresetowania hasła został wysłany na podany adres e-mail.
          </Typography>
        ) : (
          <>
            <Stack spacing={3} sx={{ mb: 3 }}>
              <TextInput name="email" label="Adres e-mail" formik={formik} />
            </Stack>

            <LoadingButton
              fullWidth
              size="large"
              type="submit"
              variant="contained"
              loading={formik.isSubmitting}
            >
              Zresetuj hasło
            </LoadingButton>
          </>
        )}
      </form>
    </>
  );
}
