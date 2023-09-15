import HomeLayout from "src/components/home/HomeLayout";
import {Typography} from "@mui/material";

// ----------------------------------------------------------------------

export default function Terms() {
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

  return (
    <HomeLayout title="Regulamin">
      <Typography variant="h3" textAlign="center">
        Regulamin
      </Typography>
      <Title>1. Twórca i właściciel</Title>
      <Text>
        Twórcą i&nbsp;właścicielem aplikacji jest firma: Krzysztof Czuchra, NIP: 7010701713.
      </Text>
      <Title>2. Postanowienia ogólne</Title>
      <Text>
        Regulamin określa warunki korzystania z&nbsp;aplikacji{" "}
        <strong>fintrack.app</strong>.<br />
        Korzystając z&nbsp;aplikacji <strong>fintrack.app</strong> użytkownik
        potwierdza znajomość regulaminu oraz go&nbsp;akceptuje.
      </Text>
      <Title>3. Warunki korzystania</Title>
      <Text>
        Korzystanie z&nbsp;aplikacji <strong>fintrack.app</strong> jest
        bezpłatne i&nbsp;dostępne od&nbsp;razu po&nbsp;rejestracji.
        <br />
        Dostęp do aplikacji <strong>fintrack.app</strong> jest możliwy
        za&nbsp;pomocą Internetu poprzez przeglądarkę internetową na komputerze
        oraz na&nbsp;urządzeniu mobilnym.
        <br />
        Twórca zastrzega sobie prawo do wprowadzania zmian w&nbsp;aplikacji oraz
        dodawania nowych funkcjonalności.
        <br />
        Wszelkie aktualizacje aplikacji będą wdrażane po&nbsp;wcześniejszym
        poinformowaniu Użytkowników o planowanej przerwie technicznej.
        <br />
        Twórca zastrzega sobie prawo do usunięcia konta Użytkownika w sytuacji
        rażącego naruszenia przez niego regulaminu oraz po 12 miesiącach braku
        aktywności.
        <br />
        Twórca zastrzega sobie prawo do usunięcia konta Użytkownika w sytuacji,
        gdy będzie on zakłócał działanie aplikacji <strong>
          fintrack.app
        </strong>{" "}
        lub wykorzystywał ją niezgodnie z&nbsp;przeznaczeniem.
      </Text>
      <Title>4. Funkcjonalność aplikacji</Title>
      <Text>
        Aplikacja <strong>fintrack.app</strong> pozwala na monitorowanie
        wartości majątku Użytkownika.
        <br />
        Użytkownik ma możliwość podawania/edytowania/usuwania wartości
        składników swojego majątku (aktywów oraz zobowiązań), a&nbsp;aplikacja
        na ich podstawie wygeneruje podsumowanie w&nbsp;postaci statystyk
        oraz&nbsp;wykresów.
        <br />
        Użytkownik ma prawo usunąć konto w&nbsp;aplikacji{" "}
        <strong>fintrack.app</strong>, co spowoduje wyczyszczenie wszystkich
        danych wprowadzonych przez niego w&nbsp;aplikacji. Skutkiem będzie także
        usunięcie wszelkich informacji o&nbsp;tym, że Użytkownik o danym adresie
        e-mail kiedykolwiek korzystał z&nbsp;aplikacji{" "}
        <strong>fintrack.app</strong>.
      </Text>
      <Title>5. Odpowiedzialność</Title>
      <Text>
        Twórca nie ponosi odpowiedzialności za korzystanie z&nbsp;aplikacji{" "}
        <strong>fintrack.app</strong> niezgodnie z przeznaczeniem.
        <br />
        Twórca nie gwarantuje, że aplikacja <strong>fintrack.app</strong> będzie
        działała bez przerw, błędów i&nbsp;problemów.
      </Text>
      <Title>6. Reklamacje</Title>
      <Text>
        Użytkownikowi przysługuje prawo do złożenia zażalenia na działanie
        aplikacji. Wszelkie skargi oraz propozycje usprawnień należy zgłaszać na
        adres mailowy contact&nbsp;[at]&nbsp;fintrack.app.
      </Text>
      <Title>7. Własność intelektualna</Title>
      <Text>
        Zabrania się kopiowania kodu źródłowego, tekstu oraz grafik ze strony{" "}
        <strong>fintrack.app</strong> bez zgody Twórcy.
        <br />
        Aplikacja <strong>fintrack.app</strong> korzysta z fragmentów
        oprogramowania udostępnionych na podstawie odrębnych licencji.
      </Text>
      <Title>8. Prywatność</Title>
      <Text>
        Aplikacja <strong>fintrack.app</strong> nie udostępnia danych
        wprowadzonych do aplikacji osobom trzecim bez zgody Użytkownika.
        <br />
        Autoryzację Użytkowników w&nbsp;aplikacji <strong>
          fintrack.app
        </strong>{" "}
        zapewnia Google Firebase.
      </Text>
      <Title>9. Postanowienia końcowe</Title>
      <Text>
        Aplikacja <strong>fintrack.app</strong> zastrzega sobie prawo do edycji
        niniejszego regulaminu po&nbsp;wcześniejszym poinformowaniu Użytkowników
        drogą mailową.
        <br />
        Korzystanie z&nbsp;aplikacji <strong>fintrack.app</strong> po zmianie
        regulaminu jest równoznaczne z&nbsp;jego akceptacją.
      </Text>
    </HomeLayout>
  );
}
