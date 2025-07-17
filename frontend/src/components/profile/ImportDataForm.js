import { useRef } from "react";
import { styled } from "@mui/material/styles";
import { Card, Button, Box, Typography, Stack } from "@mui/material";
import { useAuth } from "src/navigation/PrivateRoute";
import { importUserData } from "src/services/UserService";
import { useDispatch } from "react-redux";
import { addError, removeError } from "../../redux/slices/errorSlice";
import { reset as resetProfile } from "../../redux/slices/profileSlice";
import { reset as resetNetWorth } from "../../redux/netWorth/netWorthSlice";
import { reset as resetProperty } from "../../redux/property/propertySlice";
import { reset as resetPropertySettings } from "../../redux/property/propertySettingsSlice";

const RootStyle = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3),
  height: "100%",
}));

export default function ImportDataForm() {
  const auth = useAuth();
  const fileInputRef = useRef();
  const dispatch = useDispatch();

  const handleImport = async (event) => {
    dispatch(removeError());
    const file = event.target.files[0];
    event.target.value = null; // Allow re-uploading the same file
    if (!file) return;
    try {
      const token = auth.user.stsTokenManager.accessToken;
      const result = await importUserData(file, token);
      dispatch(resetProfile());
      dispatch(resetNetWorth());
      dispatch(resetProperty());
      dispatch(resetPropertySettings());
      auth.refreshBckUser();
      if (result && result.data) {
        if (
          Array.isArray(result.data.errors) &&
          result.data.errors.length > 0
        ) {
          dispatch(addError(result.data.errors.join("\n")));
        } else {
          const map = {
            partsAdded: "Składniki dodane",
            entriesAdded: "Wpisy dodane",
            propertiesAdded: "Nieruchomości dodane",
            propertyTransactionsAdded: "Transakcje dodane",
          };
          let message = Object.entries(result.data)
            .filter(([key]) => key !== "errors")
            .map(
              ([key, value]) =>
                `${map[key] ? `${map[key]}: ${value}` : `${key}: ${value}`}`
            )
            .join("\n");
          alert(message);
        }
      }
    } catch (error) {
      dispatch(addError(error));
    }
  };

  return (
    <RootStyle>
      <Typography variant="h6" mb={2}>
        Import danych
      </Typography>
      <Box mb={5}>
        <Typography>
          Możesz zaimportować swoje dane w formacie CSV. Upewnij się, że plik
          jest zgodny z wymaganym formatem.
          <br />
          <br />
          <b>Uwaga! Wszystkie dane zostaną nadpisane!</b>
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
            <Typography>
              <Button
                component="a"
                href={
                  process.env.PUBLIC_URL +
                  "/static/data/fintrack_export_example.csv"
                }
                target="_blank"
                rel="noopener noreferrer"
                variant="text"
                style={{ textDecoration: "underline", padding: 0, minWidth: 0 }}
                download
              >
                Przykładowy plik
              </Button>
            </Typography>
            <input
              type="file"
              accept="text/csv"
              style={{ display: "none" }}
              ref={fileInputRef}
              onChange={handleImport}
            />
            <Button
              variant="outlined"
              onClick={() =>
                fileInputRef.current && fileInputRef.current.click()
              }
            >
              Importuj dane
            </Button>
          </Stack>
        </Box>
      </Box>
    </RootStyle>
  );
}
