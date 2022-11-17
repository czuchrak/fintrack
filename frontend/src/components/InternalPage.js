import {Button, Card, Container, Grid, Stack, Typography,} from "@mui/material";
import {Icon} from "@iconify/react/dist/iconify";
import plusFill from "@iconify/icons-eva/plus-fill";
import Loader from "./Loader";
import Page from "./Page";
import {appConfig} from "../config/config";
import {styled} from "@mui/material/styles";
import {useEffect, useState} from "react";

const demo = appConfig.demo;

const RootStyle = styled(Card)(({ theme }) => ({
  textAlign: "center",
  padding: theme.spacing(3, 0),
}));

export default function InternalPage({
  children,
  title,
  isLoading,
  handleOpenForm,
  showEmptyState,
  emptyState,
  actionPrefix,
}) {
  const [hideEmptyState, setHideEmptyState] = useState(true);

  useEffect(() => {
    setHideEmptyState(!showEmptyState);
  }, [showEmptyState]);

  return (
    <Page title={demo ? "Demo" : title}>
      <Container>
        <>
          <Grid container sx={{ mb: 2 }}>
            <Grid item xs={6}>
              <Typography variant="h4" gutterBottom>
                {title}
              </Typography>
            </Grid>
            <Grid item xs={6}>
              <Stack direction="row" justifyContent="flex-end">
                {actionPrefix}
                {handleOpenForm && !showEmptyState && (
                  <Button
                    variant="contained"
                    onClick={handleOpenForm}
                    startIcon={<Icon icon={plusFill} />}
                  >
                    Dodaj
                  </Button>
                )}
              </Stack>
            </Grid>
          </Grid>
          {!isLoading && (
            <>
              {!hideEmptyState && <RootStyle>{emptyState}</RootStyle>}
              {children}
            </>
          )}
        </>
      </Container>
      <Loader open={isLoading} />
    </Page>
  );
}
