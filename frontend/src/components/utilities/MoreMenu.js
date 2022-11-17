import {Icon} from "@iconify/react";
import {useRef, useState} from "react";
import editFill from "@iconify/icons-eva/edit-fill";
import trash2Outline from "@iconify/icons-eva/trash-2-outline";
import moreVerticalFill from "@iconify/icons-eva/more-vertical-fill";
// material
import {
    Button,
    Dialog,
    DialogActions,
    DialogContent,
    DialogContentText,
    DialogTitle,
    IconButton,
    ListItemIcon,
    ListItemText,
    Menu,
    MenuItem,
    Zoom,
} from "@mui/material";
import {appConfig} from "src/config/config";
import copyOutline from "@iconify/icons-eva/copy-outline";

const demo = appConfig.demo;

// ----------------------------------------------------------------------

export default function MoreMenu({ onEdit, onDelete, onDuplicate }) {
  const ref = useRef(null);
  const [isOpen, setIsOpen] = useState(false);
  const [openDeleteDialog, setOpenDeleteDialog] = useState(false);

  const handleDelete = () => {
    setIsOpen(false);
    onDelete();
  };

  const handleOpenDeleteDialog = () => {
    setOpenDeleteDialog(true);
    setIsOpen(false);
  };

  const handleCloseDeleteDialog = () => {
    setOpenDeleteDialog(false);
  };

  const CustomMenuItem = ({ onClick, disabled, icon, text }) => {
    return (
      <MenuItem
        onClick={() => {
          setIsOpen(false);
          onClick();
        }}
        sx={{ color: "text.secondary" }}
        disabled={disabled}
      >
        <ListItemIcon>
          <Icon icon={icon} width={24} height={24} />
        </ListItemIcon>
        <ListItemText
          primary={text}
          primaryTypographyProps={{ variant: "body2" }}
        />
      </MenuItem>
    );
  };

  return (
    <>
      <IconButton ref={ref} onClick={() => setIsOpen(true)} data-cy="moreMenu">
        <Icon icon={moreVerticalFill} width={16} height={16} />
      </IconButton>

      <Menu
        open={isOpen}
        anchorEl={ref.current}
        onClose={() => setIsOpen(false)}
        PaperProps={{
          sx: { width: 130, maxWidth: "100%" },
        }}
        anchorOrigin={{ vertical: "top", horizontal: "right" }}
        transformOrigin={{ vertical: "top", horizontal: "right" }}
        TransitionComponent={Zoom}
      >
        {onEdit && (
          <CustomMenuItem
            onClick={onEdit}
            disabled={demo}
            icon={editFill}
            text="Edytuj"
          />
        )}
        {onDuplicate && (
          <CustomMenuItem
            onClick={onDuplicate}
            icon={copyOutline}
            text="Duplikuj"
          />
        )}
        {onDelete && (
          <CustomMenuItem
            onClick={handleOpenDeleteDialog}
            disabled={demo}
            icon={trash2Outline}
            text="Usuń"
          />
        )}
      </Menu>

      <Dialog
        open={openDeleteDialog}
        onClose={handleCloseDeleteDialog}
        aria-labelledby="deleteFormDialog"
        aria-describedby="alert-dialog-description"
      >
        <DialogTitle id="deleteFormDialog">Usuwanie</DialogTitle>
        <DialogContent>
          <DialogContentText id="alert-dialog-description">
            Czy na pewno chcesz usunąć?
            <br />
            <strong>Tej akcji nie można cofnąć.</strong>
          </DialogContentText>
        </DialogContent>
        <DialogActions>
          <Button onClick={handleCloseDeleteDialog} data-cy="moreMenuCancel">
            Anuluj
          </Button>
          <Button onClick={handleDelete} autoFocus data-cy="moreMenuDelete">
            Usuń
          </Button>
        </DialogActions>
      </Dialog>
    </>
  );
}
