import { useEffect } from "react";
import { Alert, AlertTitle, Stack } from "@mui/material";
import { useDispatch, useSelector } from "react-redux";
import { removeError } from "src/redux/slices/errorSlice";
import { useLocation } from "react-router-dom";

export default function ErrorAlert() {
  const location = useLocation();
  const dispatch = useDispatch();
  const { error } = useSelector((state) => state.error);

  useEffect(() => {
    dispatch(removeError());
  }, [dispatch, location]);

  return (
    error && (
      <Stack sx={{ width: "100%", mb: 2 }} spacing={2}>
        <Alert
          severity="error"
          onClose={() => {
            dispatch(removeError());
          }}
        >
          <AlertTitle>Wystąpił błąd</AlertTitle>
          {error}
        </Alert>
      </Stack>
    )
  );
}
