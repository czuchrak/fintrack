import {
  Alert,
  AlertTitle,
  Box,
  Button,
  Card,
  Divider,
  Grid,
  Link,
  Stack,
  Typography,
} from "@mui/material";
import Logo from "src/components/Logo";
import Typewriter from "typewriter-effect";
import { useTheme } from "@mui/material/styles";
import HomeLayout from "src/components/home/HomeLayout";
import { Link as RouterLink } from "react-router-dom";
import { appConfig } from "../config/config";
const testApp = appConfig.testApp;
// ----------------------------------------------------------------------

export default function Home() {
  const theme = useTheme();

  return (
    <HomeLayout>
      <Grid container textAlign="center">
        {testApp && (
          <Stack sx={{ width: "100%" }}>
            <Alert severity="info">
              <AlertTitle>
                To jest środowisko testowe, które może być niestabilne.{" "}
                <Link
                  underline="hover"
                  sx={{ cursor: "pointer" }}
                  href="https://fintrack.app"
                >
                  Kliknij tutaj
                </Link>
                , aby przejść na produkcyjną wersję aplikacji fintrack.app.
              </AlertTitle>
            </Alert>
          </Stack>
        )}
        <Grid item xs={12} md={6} sx={{ height: "100%" }} data-aos="fade-right">
          <Box sx={{ py: 4, textAlign: "-webkit-center" }}>
            <Logo sx={{ height: 100 }} />
          </Box>
          <Typography variant="h3" sx={{ opacity: "0.9" }}>
            Monitoruj zmiany wartości
            <Typewriter
              options={{
                cursor: ".",
                loop: true,
              }}
              onInit={(typewriter) => {
                typewriter
                  .pauseFor(1000)
                  .typeString(
                    "<strong style='color: " +
                      theme.palette.primary.main +
                      "'>aktywów</strong>"
                  )
                  .pauseFor(1000)
                  .deleteChars(7)
                  .typeString(
                    "<strong style='color: " +
                      theme.palette.error.main +
                      "'>zobowiązań</strong>"
                  )
                  .pauseFor(1000)
                  .deleteChars(10)
                  .typeString(
                    "<strong style='color: " +
                      theme.palette.info.main +
                      "'>netto</strong>"
                  )
                  .pauseFor(2000)
                  .deleteChars(5)
                  .start();
              }}
            />
          </Typography>
          <Typography variant="h5" sx={{ opacity: "0.8", mt: 1 }}>
            i miej pod kontrolą cały swój majątek!
          </Typography>
          <Stack
            direction="row"
            justifyContent="center"
            spacing={{ xs: 1.5 }}
            sx={{ mt: 2 }}
          >
            <Button
              target="_blank"
              variant="outlined"
              href="https://github.com/czuchrak/fintrack"
            >
              Zobacz na GitHub
            </Button>
            <Button
              variant="contained"
              component={RouterLink}
              to={process.env.PUBLIC_URL + "/register"}
            >
              Utwórz konto
            </Button>
          </Stack>
        </Grid>
        <Grid
          item
          xs={12}
          md={6}
          sx={{ padding: 5, position: "relative" }}
          data-aos="fade-left"
        >
          <Card sx={{ padding: "2%" }}>
            <Box
              component="img"
              src={process.env.PUBLIC_URL + "/static/images/desktop_screen.png"}
              sx={{ border: "1px solid #eaeaeb" }}
            />
          </Card>
          <Card
            sx={{
              width: "22%",
              padding: "2%",
              position: "absolute",
              right: 30,
              bottom: 0,
            }}
          >
            <Box
              component="img"
              src={process.env.PUBLIC_URL + "/static/images/mobile_screen.png"}
              sx={{ border: "1px solid #eaeaeb" }}
            />
          </Card>
        </Grid>
        <Grid item xs={12} sx={{ mt: 10, mb: 5 }} data-aos="zoom-in">
          <Divider variant="middle" />
          <Typography variant="h3" sx={{ opacity: "0.9", mt: 2 }}>
            Wartość netto = Aktywa&nbsp;-&nbsp;Zobowiązania
          </Typography>
        </Grid>
        <Grid item xs={12}>
          <Grid container justifyContent="center" spacing={3}>
            <Grid item xs={12} sm={4}>
              <Card sx={{ p: 3, height: "100%" }} data-aos="flip-right">
                <Typography
                  variant="h4"
                  sx={{ color: theme.palette.info.main }}
                >
                  Wartość netto
                </Typography>
                <br />
                <Typography variant="body2">
                  Wartość netto to kwota, która zostałaby Ci po sprzedaniu
                  wszystkich Twoich aktywów oraz spłacie wszelkich zobowiązań.
                </Typography>
              </Card>
            </Grid>
            <Grid item xs={12} sm={4}>
              <Card
                sx={{ p: 3, height: "100%" }}
                data-aos="flip-right"
                data-aos-delay="400"
              >
                <Typography
                  variant="h4"
                  sx={{ color: theme.palette.primary.main }}
                >
                  Aktywa
                </Typography>
                <br />
                <Typography variant="body2">
                  Aktywa to np. mieszkanie, dom, działka, samochód, motocykl,
                  konta i&nbsp;lokaty bankowe, konta emerytalne, inwestycje
                  (akcje i&nbsp;obligacje).
                </Typography>
              </Card>
            </Grid>
            <Grid item xs={12} sm={4}>
              <Card
                sx={{ p: 3, height: "100%" }}
                data-aos="flip-right"
                data-aos-delay="800"
              >
                <Typography
                  variant="h4"
                  sx={{ color: theme.palette.error.main }}
                >
                  Zobowiązania
                </Typography>
                <br />
                <Typography variant="body2">
                  Zobowiązania to np. kredyty hipoteczne i&nbsp;gotówkowe,
                  pożyczki od rodziny, karty kredytowe, chwilówki.
                </Typography>
              </Card>
            </Grid>
          </Grid>
        </Grid>

        <Grid item xs={12} sx={{ mt: 10, mb: 5 }} data-aos="zoom-in">
          <Divider variant="middle" />
          <Typography variant="h3" sx={{ opacity: "0.9", mt: 2 }}>
            Jak to działa?
          </Typography>
        </Grid>
        <Grid item xs={12}>
          <Grid container justifyContent="center" spacing={3}>
            <Grid
              item
              xs={12}
              md={6}
              sx={{
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
                alignItems: "center",
              }}
              data-aos="fade-right"
            >
              <Typography variant="h4">Składniki</Typography>
              <br />
              <Typography variant="body2" sx={{ pl: 5, pr: 5 }}>
                Zacznij od zdefiniowania części swojego majątku. Dodaj aktywa
                i&nbsp;zobowiązania w&nbsp;zakładce <strong>Składniki</strong>.
                W&nbsp;tym miejscu ustawisz, edytujesz lub&nbsp;usuniesz
                istniejące składniki. Wprowadzane tutaj zmiany mają wpływ na
                wykresy oraz zakładkę <strong>Dane</strong>.
              </Typography>
            </Grid>
            <Grid item xs={12} md={6}>
              <Card sx={{ padding: "2%" }} data-aos="fade-left">
                <Box
                  component="img"
                  src={
                    process.env.PUBLIC_URL + "/static/images/desktop_parts.png"
                  }
                  sx={{ border: "1px solid #eaeaeb" }}
                />
              </Card>
            </Grid>
            <Grid
              item
              xs={12}
              md={6}
              sx={{
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
                alignItems: "center",
              }}
              data-aos="fade-right"
            >
              <Typography variant="h4">Dane</Typography>
              <br />
              <Typography variant="body2" sx={{ pl: 5, pr: 5 }}>
                Następnie wybierz jeden dzień miesiąca, w&nbsp;którym będziesz
                uzupełniać aktualną wartość swoich aktywów oraz zobowiązań
                w&nbsp;zakładce <strong>Dane</strong>. Może to być zarówno dzień
                otrzymywania wypłaty, jak i&nbsp;pierwszy lub ostatni dzień
                miesiąca. I&nbsp;to wszystko! 5&nbsp;minut miesięcznie pozwoli
                Ci trzymać cały Twój majątek pod kontrolą.
              </Typography>
              <br />
              <Typography variant="body2" sx={{ pl: 5, pr: 5 }}>
                Możesz w każdej chwili wyeksportować lub zaimportować swoje dane
                w ustawieniach. Dzięki temu łatwo przeniesiesz dane między
                kontami lub wykonasz kopię zapasową.
              </Typography>
            </Grid>
            <Grid item xs={12} md={6}>
              <Card sx={{ padding: "2%" }} data-aos="fade-left">
                <Box
                  component="img"
                  src={
                    process.env.PUBLIC_URL + "/static/images/desktop_data.png"
                  }
                  sx={{ border: "1px solid #eaeaeb" }}
                />
              </Card>
            </Grid>
            <Grid
              item
              xs={12}
              md={6}
              sx={{
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
                alignItems: "center",
              }}
              data-aos="fade-right"
            >
              <Typography variant="h4">Cele</Typography>
              <br />
              <Typography variant="body2" sx={{ pl: 5, pr: 5 }}>
                Możesz także zdefiniować cele dotyczące Twojego majątku.
                Całkowita spłata kredytu hipotecznego do&nbsp;2030 roku?
                Odłożenie 1&nbsp;mln&nbsp;zł na&nbsp;emeryturę? Zebranie
                50&nbsp;tys.&nbsp;zł na&nbsp;samochód? Na&nbsp;bieżąco zobaczysz
                status swoich celów finansowych oraz dowiesz się,
                ile&nbsp;miesięcznie powinieneś odkładać/inwestować/nadpłacać.
              </Typography>
            </Grid>
            <Grid item xs={12} md={6}>
              <Card sx={{ padding: "2%" }} data-aos="fade-left">
                <Box
                  component="img"
                  src={
                    process.env.PUBLIC_URL + "/static/images/desktop_goals.png"
                  }
                  sx={{ border: "1px solid #eaeaeb" }}
                />
              </Card>
            </Grid>
            <Grid
              item
              xs={12}
              md={6}
              sx={{
                display: "flex",
                flexDirection: "column",
                justifyContent: "center",
                alignItems: "center",
              }}
              data-aos="fade-right"
            >
              <Typography variant="h4">Przegląd</Typography>
              <br />
              <Typography variant="body2" sx={{ pl: 5, pr: 5 }}>
                W&nbsp;każdej chwili w&nbsp;zakładce <strong>Pulpit</strong>{" "}
                możesz sprawdzić jak zmieniała się Twoja wartość netto
                w&nbsp;ostatnich miesiącach i&nbsp;latach oraz analizować
                wartości swojego majątku na podstawie statystyk i&nbsp;wykresów.
                Zyskaj świadomość i&nbsp;korzystaj z&nbsp;życia!
              </Typography>
            </Grid>
            <Grid item xs={12} md={6}>
              <Card sx={{ padding: "2%" }} data-aos="fade-left">
                <Box
                  component="img"
                  src={
                    process.env.PUBLIC_URL + "/static/images/desktop_chart.png"
                  }
                  sx={{ border: "1px solid #eaeaeb" }}
                />
              </Card>
            </Grid>
          </Grid>
        </Grid>
        <Grid item xs={12} sx={{ mt: 10, mb: 5 }} data-aos="zoom-in">
          <Divider variant="middle" />
          <Typography variant="h3" sx={{ opacity: "0.9", mt: 2 }}>
            Dlaczego warto monitorować swój majątek?
          </Typography>
        </Grid>
        <Grid item xs={12}>
          <Grid container justifyContent="center" spacing={3}>
            <Grid item xs={12} sm={4} data-aos="flip-up">
              <Typography variant="h4">Wiedza</Typography>
              <br />
              <Typography variant="body2">
                Dzięki regularnemu spisywaniu wartości Twoich aktywów
                i&nbsp;zobowiązań <strong>zyskasz wiedzę</strong>, co aktualnie
                posiadasz oraz ile jesteś jeszcze winny wierzycielom.
              </Typography>
            </Grid>
            <Grid item xs={12} sm={4} data-aos="flip-up" data-aos-delay="400">
              <Typography variant="h4">Świadomość</Typography>
              <br />
              <Typography variant="body2">
                Porównywanie zmian w&nbsp;ostatnich miesiącach do historycznych
                danych sprawi, że <strong>będziesz świadomy</strong>, czy
                wszystko zmierza w&nbsp;dobrym kierunku.
              </Typography>
            </Grid>
            <Grid item xs={12} sm={4} data-aos="flip-up" data-aos-delay="800">
              <Typography variant="h4">Reakcja</Typography>
              <br />
              <Typography variant="body2">
                Gdy Twoja wartość netto od kilku miesięcy sukcesywnie się
                zmniejsza, <strong>szybko zareagujesz</strong> wprowadzając
                odpowiedni plan, aby uratować wartość Twojego majątku.
              </Typography>
            </Grid>
          </Grid>
        </Grid>
        <Grid container justifyContent="center">
          <Stack direction="row" spacing={{ xs: 1.5 }} sx={{ mt: 6 }}>
            <Button variant="outlined" href="https://fintrack.app/demo">
              Demo
            </Button>
            <Button
              variant="contained"
              component={RouterLink}
              to={process.env.PUBLIC_URL + "/register"}
            >
              Utwórz konto
            </Button>
          </Stack>
        </Grid>
      </Grid>
    </HomeLayout>
  );
}
