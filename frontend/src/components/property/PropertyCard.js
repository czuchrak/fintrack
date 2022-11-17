import {Card, IconButton, Stack, Typography} from "@mui/material";
import {Icon} from "@iconify/react/dist/iconify";
import {Link as RouterLink} from "react-router-dom";
import homeOutline from "@iconify/icons-eva/home-outline";
import MoreMenu from "../utilities/MoreMenu";
import usePropertyServiceActions from "../../serviceActions/PropertyServiceActions";

export default function PropertyCard({ property, handleChangeProperty }) {
  const propertyServiceActions = usePropertyServiceActions();
  const { name, isActive } = property;

  return (
    <>
      <Card>
        <Stack spacing={2} sx={{ p: 3 }}>
          <Stack direction="row" justifyContent="space-between">
            <IconButton
              component={RouterLink}
              to={`${process.env.PUBLIC_URL}/property/${property.id}`}
            >
              <Icon icon={homeOutline} width={16} height={16} />
            </IconButton>
            <MoreMenu
              onEdit={() => handleChangeProperty(property)}
              onDelete={() => propertyServiceActions.deleteProp(property.id)}
            />
          </Stack>
          <Stack
            direction="row"
            alignItems="center"
            justifyContent="space-between"
            style={{ marginTop: 0 }}
          >
            <Typography variant="h6" noWrap>
              {name}
            </Typography>
          </Stack>
          <Stack
            direction="row"
            alignItems="center"
            justifyContent="space-between"
          >
            <Typography variant="body2">
              <span style={{ color: isActive ? "#00AB55" : "#FF4842" }}>
                {isActive ? "Aktywne" : "Nieaktywne"}
              </span>
            </Typography>
          </Stack>
        </Stack>
      </Card>
    </>
  );
}
