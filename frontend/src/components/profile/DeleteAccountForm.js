import {styled} from "@mui/material/styles";
import {Box, Card, Stack, Typography} from "@mui/material";
import {useAuth} from "src/navigation/PrivateRoute";
import {LoadingButton} from "@mui/lab";
import {useDispatch} from "react-redux";
import {removeError} from "src/redux/slices/errorSlice";
import {CheckboxInput, PasswordInput} from "../form/Inputs";
import {useForm} from "../form/FormikHelper";
import {PasswordValidation} from "../form/YupHelper";
// ----------------------------------------------------------------------

const RootStyle = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3, 3, 8, 3),
  height: "100%",
  minHeight: 200,
  position: "relative",
}));

export default function DeleteAccountForm() {
  const dispatch = useDispatch();
  let auth = useAuth();

  const onSave = async (values) => {
    dispatch(removeError());
    const result = await auth.deleteAccount(
      auth.user.providerData[0].providerId,
      values.password
    );
    if (result === "auth/wrong-password") {
      formik.setFieldError("password", "Niepoprawne hasło");
    }
  };

  const formik = useForm(null, onSave, null, [
    {
      name: "password",
      default: "",
      validation: PasswordValidation({
        error: "To pole jest wymagane",
        test: (item) => {
          return auth.user.providerData[0].providerId !== "password" || item;
        },
      }),
    },
    { name: "checked", default: false },
  ]);

  return (
    <RootStyle>
      <Typography variant="h6">Usuwanie konta</Typography>
      <Typography>
        Usunięcie konta spowoduje wyczyszczenie wszystkich danych wprowadzonych
        przez Ciebie w tej aplikacji.
        <br />
        <br />
        <b>Tej czynności nie można cofnąć.</b>
      </Typography>
      {auth.user.providerData[0].providerId === "password" && (
        <Stack sx={{ my: 3 }} key={formik.isLoaded}>
          <PasswordInput name="password" label="Hasło" formik={formik} />
        </Stack>
      )}
      <Box
        sx={{
          position: "absolute",
          bottom: 0,
          mb: 2,
          width: "100%",
          pr: 6,
        }}
      >
        <Stack
          direction="row"
          alignItems="center"
          justifyContent="space-between"
          key={formik.isLoaded}
        >
          <CheckboxInput
            name="checked"
            label="Chcę usunąć konto"
            formik={formik}
          />
          <LoadingButton
            size="small"
            type="button"
            onClick={formik.handleSubmit}
            variant="contained"
            loading={formik.isSubmitting}
            disabled={!formik.values.checked}
          >
            Usuń konto
          </LoadingButton>
        </Stack>
      </Box>
    </RootStyle>
  );
}
