import {Container, Typography} from "@mui/material";
import Page from "src/components/Page";
import ContactForm from "src/components/profile/ContactForm";

// ----------------------------------------------------------------------

export default function Contact() {
  return (
    <>
      <Page title="Kontakt">
        <Container maxWidth="xl">
          <Typography variant="h4" gutterBottom>
            Kontakt
          </Typography>
          <ContactForm />
        </Container>
      </Page>
    </>
  );
}
