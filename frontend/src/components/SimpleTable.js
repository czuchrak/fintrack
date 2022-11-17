import Scrollbar from "./Scrollbar";
import {Card, Table, TableBody, TableContainer, TableHead, TablePagination, TableRow,} from "@mui/material";
import {useEffect, useState} from "react";

export default function SimpleTable({
  headers,
  data,
  mapping,
  paging,
  minWidth = 800,
  height = "auto",
  pagination = true,
  card = true,
}) {
  const [page, setPage] = useState(0);
  const [rowsPerPage, setRowsPerPage] = useState(parseInt(paging[0], 10));

  const handleChangePage = (_event, newPage) => {
    setPage(newPage);
  };

  const handleChangeRowsPerPage = (event) => {
    setRowsPerPage(parseInt(event.target.value, 10));
    setPage(0);
  };

  useEffect(() => {
    setPage(0);
  }, [data]);

  const Inside = () => {
    return (
      <>
        <Scrollbar>
          <TableContainer sx={{ minWidth, height }}>
            <Table size="small" stickyHeader>
              <TableHead>
                <TableRow>{headers}</TableRow>
              </TableHead>
              <TableBody>
                {data
                  .slice(page * rowsPerPage, page * rowsPerPage + rowsPerPage)
                  .map((row) => mapping(row))}
              </TableBody>
            </Table>
          </TableContainer>
        </Scrollbar>

        {pagination && (
          <TablePagination
            rowsPerPageOptions={paging}
            component="div"
            count={data.length}
            rowsPerPage={rowsPerPage}
            page={page}
            onPageChange={handleChangePage}
            onRowsPerPageChange={handleChangeRowsPerPage}
            labelRowsPerPage={"Wiersze:"}
            labelDisplayedRows={({ from, to, count }) => {
              return from + "-" + to + " z " + count;
            }}
          />
        )}
      </>
    );
  };

  return card ? (
    <Card>
      <Inside />
    </Card>
  ) : (
    <Inside />
  );
}
