import {useState} from "react";
import {styled} from "@mui/material/styles";
import {Box, Card, Stack, Typography} from "@mui/material";
import {useAuth} from "src/navigation/PrivateRoute";
import {LoadingButton} from "@mui/lab";
import {sendMessage} from "src/services";
import {useDispatch} from "react-redux";
import {addError, removeError} from "src/redux/slices/errorSlice";
import {useForm} from "../form/FormikHelper";
import {TextValidation} from "../form/YupHelper";
import {TextInput} from "../form/Inputs";

// ----------------------------------------------------------------------

const RootStyle = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3),
  height: "100%",
}));

export default function ContactForm() {
  const dispatch = useDispatch();
  const [isSent, setIsSent] = useState(false);
  let auth = useAuth();

  const onSave = async (values) => {
    dispatch(removeError());
    try {
      await sendMessage(values, await auth.getToken());
      setIsSent(true);
      formik.resetForm();
      setTimeout(() => setIsSent(false), 3000);
    } catch (error) {
      dispatch(addError(error.message));
    }
  };

  const formik = useForm(null, onSave, null, [
    { name: "email", default: auth.user.email },
    { name: "topic", default: "", validation: TextValidation(null, true) },
    { name: "message", default: "", validation: TextValidation(null, true) },
  ]);

  return (
    <RootStyle>
      <Stack sx={{ mt: 1, mb: 5 }} key={formik.isLoaded}>
        <TextInput name="email" label="Email" formik={formik} disabled />
        <TextInput name="topic" label="Tytuł" formik={formik} sx={{ mt: 2 }} />
        <TextInput
          name="message"
          label="Treść wiadomości"
          formik={formik}
          multiline
          maxRows={10}
          sx={{ mt: 2 }}
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
        {isSent ? (
          <Typography variant="subtitle2" textAlign="center">
            Wiadomość została wysłana.
          </Typography>
        ) : (
          <LoadingButton
            size="small"
            type="button"
            variant="contained"
            loading={formik.isSubmitting}
            onClick={formik.handleSubmit}
          >
            Wyślij
          </LoadingButton>
        )}
      </Box>
    </RootStyle>
  );
}
