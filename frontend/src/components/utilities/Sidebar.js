import {Icon} from "@iconify/react";
import closeFill from "@iconify/icons-eva/close-fill";
import saveOutline from "@iconify/icons-eva/save-outline";
import {Box, Button, Divider, Drawer, IconButton, Stack, Typography,} from "@mui/material";
import Scrollbar from "../Scrollbar";
import {appConfig} from "src/config/config";

const demo = appConfig.demo;

export default function Sidebar({
  children,
  isOpenForm,
  isAdding,
  onCloseForm,
  formik,
  showAll,
}) {
  return (
    <Drawer
      anchor="right"
      open={isOpenForm}
      onClose={onCloseForm}
      PaperProps={{
        sx: { width: 280, border: "none", overflow: "hidden" },
      }}
    >
      <Stack
        direction="row"
        alignItems="center"
        justifyContent="space-between"
        sx={{ px: 1, py: 2 }}
      >
        <Typography variant="subtitle1" sx={{ ml: 1 }}>
          {isAdding ? "Dodawanie" : "Edytowanie"}
        </Typography>
        <Stack direction="row">
          {showAll}
          <IconButton onClick={onCloseForm}>
            <Icon icon={closeFill} width={20} height={20} />
          </IconButton>
        </Stack>
      </Stack>

      <Divider />

      <Scrollbar>
        <Stack spacing={3} sx={{ p: 3 }}>
          {children}
        </Stack>
      </Scrollbar>

      <Box sx={{ p: 3 }}>
        <Button
          fullWidth
          size="large"
          type="submit"
          color="inherit"
          variant="outlined"
          onClick={() => {
            const err = Object.keys(formik.errors);
            if (err.length) {
              const input = document.querySelector(`input[id="${err[0]}"]`);

              input.scrollIntoView({
                behavior: "smooth",
                block: "end",
              });
            }
            formik.handleSubmit();
          }}
          startIcon={<Icon icon={saveOutline} />}
          disabled={demo}
        >
          Zapisz
        </Button>
      </Box>
    </Drawer>
  );
}
