import {Box, Card, TableCell, TableRow} from "@mui/material";
import SimpleTable from "../SimpleTable";
import {DecimalLabel, PercentLabel} from "../Label";
import {useSelector} from "react-redux";
// ----------------------------------------------------------------------

export default function NetWorthYearSummary({ years }) {
  let { currency } = useSelector((state) => state.profile);

  return (
    <Card sx={{ p: 1, height: "100%" }}>
      <Box>
        <SimpleTable
          height={300}
          minWidth={0}
          pagination={false}
          card={false}
          headers={
            <>
              <TableCell align="center">Rok</TableCell>
              <TableCell align="center">Wartość netto</TableCell>
              <TableCell align="center">%</TableCell>
              <TableCell align="center">Śr. miesięczna</TableCell>
            </>
          }
          data={years.slice().sort((a, b) => {
            if (a.year < b.year) return 1;
            else if (a.year > b.year) return -1;
            else return 0;
          })}
          mapping={(entry) => {
            const { year, value, valuePercent, valueAvg } = entry;

            return (
              <TableRow hover key={year} tabIndex={-1}>
                <TableCell align="center">{year}</TableCell>
                <TableCell align="center">
                  <DecimalLabel value={value} plus={true} currency={currency} />
                </TableCell>
                <TableCell align="center">
                  <PercentLabel value={valuePercent} plus={true} />
                </TableCell>
                <TableCell align="center">
                  <DecimalLabel
                    value={valueAvg}
                    plus={true}
                    currency={currency}
                  />
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
