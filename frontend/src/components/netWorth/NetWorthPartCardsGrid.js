import {Grid, IconButton, Tooltip} from "@mui/material";
import NetWorthPartCard from "./NetWorthPartCard";
import {SortableContainer, SortableElement, SortableHandle,} from "react-sortable-hoc";
import {arrayMoveImmutable} from "array-move";
import moveOutline from "@iconify/icons-eva/move-outline";
import {Icon} from "@iconify/react/dist/iconify";
import useNetWorthServiceActions from "../../serviceActions/NetWorthServiceActions";

export default function NetWorthPartCardsGrid({ parts, handleChangePart }) {
  const netWorthServiceActions = useNetWorthServiceActions();

  const DragHandle = SortableHandle(() => (
    <span tabIndex={0}>
      <Tooltip title="Zmień kolejność" placement="bottom">
        <IconButton>
          <Icon icon={moveOutline} width={16} height={16} />
        </IconButton>
      </Tooltip>
    </span>
  ));

  const SortableItem = SortableElement(({ part }) => (
    <Grid key={part.id} item xs={12} sm={6} md={3}>
      <NetWorthPartCard
        part={part}
        dragHandle={DragHandle}
        handleChangePart={handleChangePart}
      />
    </Grid>
  ));

  const SortableList = SortableContainer(({ items }) => {
    return (
      <Grid container spacing={3}>
        {items.map((part, index) => (
          <SortableItem key={`item-${part.id}`} index={index} part={part} />
        ))}
      </Grid>
    );
  });

  const onSortEnd = async ({ oldIndex, newIndex }) => {
    if (oldIndex === newIndex) return;
    let newItems = arrayMoveImmutable(parts, oldIndex, newIndex);
    await netWorthServiceActions.changeOrders(newItems.map((x) => x.id));
  };

  return (
    <SortableList items={parts} onSortEnd={onSortEnd} axis="xy" useDragHandle />
  );
}
