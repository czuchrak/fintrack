import {useEffect, useState} from "react";
import {Stack, Typography} from "@mui/material";
import {LoadingButton} from "@mui/lab";
import {useAuth} from "src/navigation/PrivateRoute";
import {useLocation} from "react-router-dom";
import {useDispatch} from "react-redux";
import {removeError} from "src/redux/slices/errorSlice";
import {PasswordInput} from "../form/Inputs";
import {useForm} from "../form/FormikHelper";
import {PasswordValidation} from "../form/YupHelper";

// ----------------------------------------------------------------------

function useQuery() {
  return new URLSearchParams(useLocation().search);
}

export default function AuthForm() {
  const dispatch = useDispatch();
  const [isCodeValid, setIsCodeValid] = useState(true);
  const [sent, setSent] = useState(false);

  let auth = useAuth();
  let query = useQuery();
  const code = query.get("oobCode");
  const mode = query.get("mode");

  const onSave = async (values) => {
    dispatch(removeError());
    await auth.setNewPasswordReset(code, values.password);
    setSent(true);
  };

  const formik = useForm(null, onSave, null, [
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

  useEffect(() => {
    const verifyCode = async () => {
      if (mode === "verifyEmail") {
        const result = await auth.applyVerificationCode(code);
        setIsCodeValid(result);
      } else {
        const result = await auth.verifyResetCode(code);
        setIsCodeValid(result);
      }
    };

    verifyCode();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  return (
    <>
      <form onSubmit={formik.handleSubmit} key={formik.isLoaded}>
        {!isCodeValid ? (
          <Typography textAlign="center">
            Błędny link. Wygeneruj go jeszcze raz.
          </Typography>
        ) : mode === "verifyEmail" ? (
          <Typography textAlign="center">Mail został zweryfikowany.</Typography>
        ) : sent ? (
          <Typography textAlign="center">
            Hasło zostało zmienione. Teraz możesz się zalogować.
          </Typography>
        ) : (
          <>
            <Stack spacing={3} sx={{ mb: 3 }}>
              <PasswordInput name="password" label="Hasło" formik={formik} />
            </Stack>

            <LoadingButton
              fullWidth
              size="large"
              type="submit"
              variant="contained"
              loading={formik.isSubmitting}
            >
              Ustaw hasło
            </LoadingButton>
          </>
        )}
      </form>
    </>
  );
}
