import {Box, Card, TableCell, TableRow} from "@mui/material";
import SimpleTable from "../SimpleTable";
import {DecimalLabel, PercentLabel} from "../Label";

export default function PropertyYearSummary({ data }) {
  return (
    <Card sx={{ p: 3, height: "100%" }}>
      <Box>
        <SimpleTable
          height={300}
          minWidth={0}
          pagination={false}
          card={false}
          headers={
            <>
              <TableCell align="center">Rok</TableCell>
              <TableCell align="center">Przychody</TableCell>
              <TableCell align="center">Koszty</TableCell>
              <TableCell align="center">Bilans</TableCell>
              <TableCell align="center">Zwrot</TableCell>
            </>
          }
          data={data.slice().sort((a, b) => {
            if (a.year < b.year) return 1;
            else if (a.year > b.year) return -1;
            else return 0;
          })}
          mapping={(cell) => {
            const { year, costs, incomes, balance, rate } = cell;

            return (
              <TableRow hover key={year} tabIndex={-1}>
                <TableCell align="center">{year}</TableCell>
                <TableCell align="center">
                  <DecimalLabel value={incomes} currency="PLN" />
                </TableCell>
                <TableCell align="center">
                  <DecimalLabel value={costs} invert currency="PLN" />
                </TableCell>
                <TableCell align="center">
                  <DecimalLabel value={balance} currency="PLN" />
                </TableCell>
                <TableCell align="center">
                  <PercentLabel value={rate} currency="PLN" />
                </TableCell>
              </TableRow>
            );
          }}
          paging={[10, 50, 100]}
        />
      </Box>
    </Card>
  );
}
