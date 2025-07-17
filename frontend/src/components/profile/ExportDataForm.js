import { styled } from "@mui/material/styles";
import { Card, Button, Box, Typography, Stack } from "@mui/material";
import { useAuth } from "src/navigation/PrivateRoute";
import { exportUserData } from "src/services/UserService";
import { CheckboxInput } from "../form/Inputs";
import { LoadingButton } from "@mui/lab";

const RootStyle = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3),
  height: "100%",
}));

export default function ExportDataForm() {
  const auth = useAuth();

  const handleExport = async () => {
    const token = auth.user.stsTokenManager.accessToken;
    const response = await exportUserData(token);
    const url = window.URL.createObjectURL(
      new Blob([response], { type: "text/csv" })
    );
    const link = document.createElement("a");
    link.href = url;
    const today = new Date();
    const yyyyMMdd =
      today.getFullYear().toString() +
      ("0" + (today.getMonth() + 1)).slice(-2) +
      ("0" + today.getDate()).slice(-2);
    link.setAttribute("download", `fintrack_export_${yyyyMMdd}.csv`);
    document.body.appendChild(link);
    link.click();
    link.parentNode.removeChild(link);
  };

  return (
    <RootStyle>
      <Typography variant="h6" mb={2}>
        Eksport danych
      </Typography>
      <Box mb={5}>
        <Typography mb={5}>
          Możesz pobrać swoje dane w formacie CSV. Plik będzie zawierał
          wszystkie dane wprowadzone do aplikacji.
        </Typography>
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
          >
            <Typography gutterBottom></Typography>
            <Button variant="outlined" onClick={handleExport}>
              Pobierz dane
            </Button>
          </Stack>
        </Box>
      </Box>
    </RootStyle>
  );
}
