import {Card, Stack, Typography} from "@mui/material";
import MoreMenu from "../utilities/MoreMenu";
import useNetWorthServiceActions from "../../serviceActions/NetWorthServiceActions";
import {appConfig} from "../../config/config";

const demo = appConfig.demo;

export default function NetWorthPartCard({
  part,
  handleChangePart,
  dragHandle: DragHandle,
}) {
  const { name, type, isVisible, order, currency } = part;
  const netWorthServiceActions = useNetWorthServiceActions();

  return (
    <>
      <Card>
        <Stack spacing={2} sx={{ p: 3 }}>
          <Stack
            direction="row"
            alignItems="center"
            justifyContent="space-between"
          >
            <Typography variant="caption">#{order}</Typography>
            <span>
              {!demo && <DragHandle />}
              <MoreMenu
                onEdit={() => handleChangePart(part)}
                onDelete={() => netWorthServiceActions.deletePart(part.id)}
              />
            </span>
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
              <span style={{ color: type === "asset" ? "#00AB55" : "#FF4842" }}>
                {type === "asset" ? "Aktywo" : "Zobowiązanie"}
              </span>{" "}
              ({isVisible ? "widoczne" : "ukryte"})
            </Typography>
            <Typography variant="caption" noWrap>
              {currency}
            </Typography>
          </Stack>
        </Stack>
      </Card>
    </>
  );
}
