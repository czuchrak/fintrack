import {Card, Container, Divider, List, ListItem, ListItemText, Stack, Typography,} from "@mui/material";
import Label from "src/components/Label";
import Page from "src/components/Page";

// ----------------------------------------------------------------------

const changes = [
  {
    title: "14 października 2022 r.",
    list: [
      {
        type: "New",
        text: "Cele finansowe",
      },
      {
        type: "New",
        text: 'Zmiana kolejności składników majątku za pomocą "przeciągnij i upuść"',
      },
    ],
  },
  {
    title: "26 sierpnia 2022 r.",
    list: [
      {
        type: "New",
        text: "Obsługa walut CHF, EUR, GBP, USD w składnikach wartości netto",
      },
    ],
  },
  {
    title: "23 marca 2022 r.",
    list: [
      {
        type: "New",
        text: "Podsumowanie lat w wartości netto",
      },
      {
        type: "New",
        text: "Filtrowanie transakcji nieruchomości",
      },
      {
        type: "New",
        text: "Bilans miesięczny nieruchomości",
      },
      {
        type: "New",
        text: "Podsumowanie lat przy wynajmie nieruchomości",
      },
    ],
  },
  {
    title: "24 lutego 2022 r.",
    list: [
      {
        type: "New",
        text: "Wybór składników w wykresach kołowych",
      },
      {
        type: "New",
        text: "Zarządzanie nieruchomościami",
      },
    ],
  },
  {
    title: "28 stycznia 2022 r.",
    list: [
      {
        type: "New",
        text: "Ustawienia powiadomień e-mail",
      },
    ],
  },
  {
    title: "15 grudnia 2021 r.",
    list: [
      {
        type: "New",
        text: "System powiadomień",
      },
      {
        type: "New",
        text: "Kopiowanie wartości składnika z ostatniego miesiąca",
      },
    ],
  },
  {
    title: "26 listopada 2021 r.",
    list: [
      {
        type: "New",
        text: "Nowy wykres na pulpicie - tzw. mapa ciepła (heatmap)",
      },
      {
        type: "New",
        text: "Niniejsza lista zmian w aplikacji",
      },
    ],
  },
  {
    title: "18 listopada 2021 r.",
    list: [
      {
        type: "New",
        text: "Zakładka kontakt",
      },
    ],
  },
  {
    title: "1 listopada 2021 r.",
    list: [
      {
        type: "New",
        text: "Zarządzanie składnikami majątku",
      },
      {
        type: "New",
        text: "Zarządzanie wartościami majątku",
      },
      {
        type: "New",
        text: "Przegląd majątku",
      },
    ],
    last: true,
  },
];

export default function Changelog() {
  return (
    <>
      <Page title="Lista zmian">
        <Container maxWidth="xl">
          <Typography variant="h4" gutterBottom>
            Lista zmian
          </Typography>

          <Card sx={{ p: 3, textAlign: "left", mb: 3 }}>
            {changes.map(({ title, list, last }) => {
              return (
                <Stack key={title}>
                  <Typography variant="h6">{title}</Typography>
                  <List>
                    {list.map((x) => {
                      return (
                        <ListItem key={x.text}>
                          <ListItemText>
                            <Label
                              variant="ghost"
                              color="success"
                              sx={{ fontSize: "0.875rem", mb: 1 }}
                            >
                              {x.type === "New" ? "Nowość" : ""}
                            </Label>
                            &nbsp;
                            {x.text}
                          </ListItemText>
                        </ListItem>
                      );
                    })}
                  </List>
                  {!last && (
                    <>
                      <Divider />
                      <br />
                    </>
                  )}
                </Stack>
              );
            })}
          </Card>
        </Container>
      </Page>
    </>
  );
}
