import HomeLayout from "src/components/home/HomeLayout";
import {Typography} from "@mui/material";

// ----------------------------------------------------------------------

const Title = ({ children }) => {
  return <Typography variant="h4">{children}</Typography>;
};
const Text = ({ children }) => {
  return (
    <>
      <Typography>{children}</Typography>
      <br />
    </>
  );
};

export default function Privacy() {
  return (
    <HomeLayout title="Polityka prywatności">
      <Typography variant="h3" textAlign="center">
        Polityka prywatności
      </Typography>
      <Title>1. Ciasteczka</Title>
      <Text>
        Do prawidłowego działania aplikacji <strong>fintrack.app</strong>{" "}
        wykorzystujemy tzw. ciasteczka. Są to personalne ustawienia potrzebne do
        zalogowania i korzystania z funkcjonalności aplikacji.
      </Text>
      <Title>2. Analiza statystyk</Title>
      <Text>
        Do analizy statystyk zbierane są anonimowe dane na temat Twojej wizyty.
        Są to np. witryny, które odwiedziłeś czy czas spędzony na danej
        podstronie.
      </Text>
      <Title>3. Bezpieczeństwo Twoich danych</Title>
      <Text>
        Do zarządzania użytkownikami korzystamy z Google Firebase. Po utworzeniu
        każdemu użytkownikowi konta przydzielany jest anonimowy identyfikator.
        <br />
        Zalecamy korzystanie z adresu e-mail, który nie pozwala na jednoznaczną
        identyfikację użytkownika.
        <br />
        Dane użytkowników są bezpiecznie przechowywane oraz nie są udostępniane
        osobom trzecim.
      </Text>
    </HomeLayout>
  );
}
