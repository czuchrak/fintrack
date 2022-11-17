import {useEffect, useState} from "react";
import {Box, Card, Container, Grid, Stack, Typography} from "@mui/material";
import {LoadingButton} from "@mui/lab";
import {styled} from "@mui/material/styles";
import Page from "../../components/Page";
import {useSelector} from "react-redux";
import {useAuth} from "../../navigation/PrivateRoute";

const RootStyle = styled(Card)(({ theme }) => ({
  padding: theme.spacing(3),
}));

export default function MailVerification() {
  const [isSent, setIsSent] = useState(false);
  const [isError, setIsError] = useState(false);
  const [isSubmitting, setIsSubmitting] = useState(false);
  const [isLoading, setIsLoading] = useState(true);
  let auth = useAuth();

  let mailVerificationSent = useSelector(
    (state) => state.profile.mailVerificationSent
  );

  const verifyEmail = async () => {
    setIsSubmitting(true);
    let result = await auth.verifyEmail();
    setIsError(!result);
    setIsSent(true);
    setIsSubmitting(false);
  };

  useEffect(() => {
    setIsSent(mailVerificationSent);
    setIsLoading(false);
  }, [mailVerificationSent]);

  return (
    <Page title="Weryfikacja adresu e-mail">
      <Container>
        <Stack
          direction="row"
          alignItems="center"
          justifyContent="space-between"
          mb={3}
        >
          <Grid container>
            <Grid item xs={12} sm={6}>
              <Typography variant="h4" gutterBottom>
                Weryfikacja adresu e-mail
              </Typography>
            </Grid>
          </Grid>
        </Stack>
        <RootStyle>
          <>
            <Typography sx={{ mt: 2 }}>
              Aby rozpocząć korzystanie z aplikacji{" "}
              <strong>fintrack.app</strong> musisz zweryfikować swój adres
              e-mail.
              <br />
              Naciśnij przycisk 'Wyślij', sprawdź swoją pocztę (w tym folder
              SPAM), kliknij znajdujący się w mailu link i odśwież tę stronę.
              <br />
              Jeśli wiadomość nie dotarła, spróbuj ponownie za kilka minut.
            </Typography>
            {isLoading || (
              <Box sx={{ textAlign: "left", mt: 2 }}>
                {isSent ? (
                  <Typography variant="subtitle2">
                    {isError
                      ? "Wystąpił błąd!"
                      : "Mail weryfikujący został wysłany"}
                  </Typography>
                ) : (
                  <LoadingButton
                    size="small"
                    type="button"
                    onClick={() => verifyEmail()}
                    variant="contained"
                    loading={isSubmitting}
                    data-cy="verifyEmail"
                  >
                    Wyślij
                  </LoadingButton>
                )}
              </Box>
            )}
          </>
        </RootStyle>
      </Container>
    </Page>
  );
}
