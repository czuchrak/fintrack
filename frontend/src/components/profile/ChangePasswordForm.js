import {useState} from "react";
import {styled} from "@mui/material/styles";
import {Box, Card, Stack, Typography} from "@mui/material";
import {useAuth} from "src/navigation/PrivateRoute";
import {LoadingButton} from "@mui/lab";
import {useDispatch} from "react-redux";
import {addError, removeError} from "src/redux/slices/errorSlice";
import {PasswordInput} from "../form/Inputs";
import {useForm} from "../form/FormikHelper";
import {PasswordValidation} from "../form/YupHelper";

// ----------------------------------------------------------------------

const RootStyle = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3),
  height: "100%",
}));

export default function ChangePasswordForm() {
  const dispatch = useDispatch();
  const [isSet, setIsSet] = useState(false);
  let auth = useAuth();

  const onSave = async (values) => {
    dispatch(removeError());
    const result = await auth.changePassword(
      values.oldPassword,
      values.newPassword,
      (error) => dispatch(addError(error))
    );
    if (result === "auth/wrong-password") {
      formik.setFieldError("oldPassword", "Niepoprawne hasło");
    } else {
      formik.resetForm();
      setIsSet(true);
      setTimeout(() => setIsSet(false), 3000);
    }
  };

  const formik = useForm(null, onSave, null, [
    {
      name: "oldPassword",
      default: "",
      validation: PasswordValidation({
        error: "To pole jest wymagane",
        test: (item) => item && item.length > 0,
      }),
    },
    {
      name: "newPassword",
      default: "",
      validation: PasswordValidation({
        error: "Hasło jest za krótkie - wymagane jest minimum 8 znaków",
        test: (item) => {
          return item && item.length >= 8;
        },
      }),
    },
    {
      name: "newPassword2",
      default: "",
      validation: PasswordValidation({
        error: "Hasła nie są identyczne",
        test: (item) => item === formik.values.newPassword,
      }),
    },
  ]);

  return (
    <RootStyle>
      <Typography variant="h6">Zmiana hasła</Typography>
      <Stack sx={{ my: 3 }} key={formik.isLoaded}>
        <PasswordInput
          name="oldPassword"
          label="Stare hasło"
          formik={formik}
          sx={{ mb: 2 }}
        />
        <PasswordInput
          name="newPassword"
          label="Nowe hasło"
          formik={formik}
          sx={{ mb: 2 }}
        />
        <PasswordInput
          name="newPassword2"
          label="Potwierdź nowe hasło"
          formik={formik}
          sx={{ mb: 2 }}
        />
      </Stack>
      <Box
        sx={{
          right: 0,
          position: "absolute",
          bottom: 0,
          mb: 2,
          mr: 2,
        }}
      >
        {isSet ? (
          <Typography variant="subtitle2" textAlign="center">
            Hasło zostało zmienione
          </Typography>
        ) : (
          <LoadingButton
            size="small"
            type="button"
            variant="contained"
            loading={formik.isSubmitting}
            onClick={formik.handleSubmit}
          >
            Zapisz
          </LoadingButton>
        )}
      </Box>
    </RootStyle>
  );
}
