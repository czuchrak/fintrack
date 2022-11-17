import {
    Box,
    Card,
    Checkbox,
    Grid,
    ListItemIcon,
    ListItemText,
    MenuItem,
    Stack,
    TextField,
    Typography,
} from "@mui/material";
import {useEffect, useMemo, useState} from "react";
import {getMonthAndYear} from "src/utils/helpers";
import PieChart from "../charts/PieChart";
import {MultipleSelect} from "../form/Inputs";

export default function NetWorthALPieChart({
  entry,
  type,
  parts,
  dates,
  handlePieDateChange,
}) {
  const [selected, setSelected] = useState([]);
  const [ps, setPs] = useState([]);
  const isAllSelected = ps.length > 0 && selected.length === ps.length;

  const handleChange = (event) => {
    const value = event.target.value;
    if (value[value.length - 1] === "all") {
      setSelected(selected.length === ps.length ? [] : ps.map((x) => x.name));
      return;
    }
    setSelected(value);
  };

  const sum = useMemo(() => {
    return ps
      .filter((x) => selected.includes(x.name))
      .map((x) => entry.partValues[x.id].valueRate)
      .reduce((a, b) => a + b, 0);
  }, [entry, ps, selected]);

  const pieChartData = useMemo(() => {
    return ps
      .filter((x) => selected.includes(x.name))
      .map((x) => {
        return {
          id: x.name,
          label: x.name,
          value: entry.partValues[x.id].valueRate,
          valueNoRate: entry.partValues[x.id].value,
          share: entry.partValues[x.id].valueRate / sum,
          currency: x.currency,
        };
      })
      .filter((x) => x.value > 0);
  }, [entry, ps, selected, sum]);

  useEffect(() => {
    let ps = parts.filter(
      (x) => x.type === type && entry.partValues[x.id].value > 0
    );
    setPs(ps);
    setSelected(ps.map((x) => x.name));
  }, [entry, type, parts]);

  return (
    <Card sx={{ p: 1 }}>
      <Stack
        direction="row"
        alignItems="center"
        justifyContent="center"
        textAlign="center"
      >
        <Grid container>
          <Grid item xs={12} sm={12}>
            <Typography variant="h6" gutterBottom>
              {type === "asset" ? "Aktywa" : "Zobowiązania"}
            </Typography>
          </Grid>
          <Grid item xs={12} sm={12}>
            <TextField
              select
              value={getMonthAndYear(entry.date)}
              onChange={handlePieDateChange}
              variant="standard"
              SelectProps={{
                MenuProps: {
                  PaperProps: {
                    style: {
                      maxHeight: 200,
                    },
                  },
                },
              }}
            >
              {dates.map((x) => {
                return (
                  <MenuItem key={x} value={getMonthAndYear(x)}>
                    {getMonthAndYear(x)}
                  </MenuItem>
                );
              })}
            </TextField>
          </Grid>
          <Grid item xs={12} sm={12}>
            <MultipleSelect
              selected={selected}
              handleChangeSelected={handleChange}
              render={(selected) => {
                const count = selected.length;
                if (count === 0 || count > 4) {
                  return `${selected.length} składników`;
                } else if (count === 1) {
                  return `${selected.length} składnik`;
                }
                return `${selected.length} składniki`;
              }}
            >
              {ps.length > 0 && (
                <MenuItem value="all">
                  <ListItemIcon>
                    <Checkbox
                      checked={isAllSelected}
                      indeterminate={
                        selected.length > 0 && selected.length < ps.length
                      }
                    />
                  </ListItemIcon>
                  <ListItemText primary="Zaznacz wszystko" />
                </MenuItem>
              )}
              {ps.map((part) => (
                <MenuItem key={part.id} value={part.name}>
                  <ListItemIcon>
                    <Checkbox checked={selected.indexOf(part.name) > -1} />
                  </ListItemIcon>
                  <ListItemText primary={part.name} />
                </MenuItem>
              ))}
            </MultipleSelect>
          </Grid>
        </Grid>
      </Stack>
      <Box sx={{ height: "376px" }}>
        {pieChartData.length === 0 ? (
          <Typography variant="h6" textAlign="center" sx={{ pt: 10, px: 2 }}>
            Brak danych.
          </Typography>
        ) : (
          <PieChart data={pieChartData} all={sum} />
        )}
      </Box>
    </Card>
  );
}
