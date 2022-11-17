import {useEffect, useState} from "react";
import {useDispatch, useSelector} from "react-redux";
import {addError} from "src/redux/slices/errorSlice";
import InternalPage from "../../components/InternalPage";
import NetWorthPartSidebar from "../../components/netWorth/NetWorthPartSidebar";
import useNetWorthServiceActions from "../../serviceActions/NetWorthServiceActions";
import NetWorthPartCardsGrid from "../../components/netWorth/NetWorthPartCardsGrid";
import EmptyState from "../../components/utilities/EmptyState";
import AccountBalanceWalletRoundedIcon from "@mui/icons-material/AccountBalanceWalletRounded";

// ----------------------------------------------------------------------

export default function NetWorthParts() {
  const dispatch = useDispatch();
  const netWorthServiceActions = useNetWorthServiceActions();

  const { data, dataFetched, isLoading, error } = useSelector(
    (state) => state.netWorth
  );

  dispatch(addError(error));

  const [openForm, setOpenForm] = useState(false);
  const [part, setPart] = useState({});

  const handleOpenForm = () => {
    setPart({});
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setPart({});
  };

  const handleChangePart = (part) => {
    setPart(part);
    setOpenForm(true);
  };

  useEffect(() => {
    if (!dataFetched) {
      netWorthServiceActions.getData().then();
    }
  }, [dataFetched, netWorthServiceActions]);

  return (
    <InternalPage
      title="Składniki"
      isLoading={isLoading}
      handleOpenForm={handleOpenForm}
      showEmptyState={!data.parts || data.parts.length === 0}
      emptyState={
        <EmptyState
          icon={AccountBalanceWalletRoundedIcon}
          text="Nie masz dodanych żadnych składników majątku."
          bodyText={
            <span>
              <strong>Przykłady aktywów:</strong>
              <br />
              Mieszkanie, dom, działka, samochód, motocykl, konta i&nbsp;lokaty
              bankowe, konta emerytalne, inwestycje
              <br />
              <br />
              <strong>Przykłady zobowiązań:</strong>
              <br />
              Kredyty hipoteczne i&nbsp;gotówkowe, pożyczki od&nbsp;rodziny,
              karty kredytowe, chwilówki
              <br />
              <br />
              Aby przestać monitorować dany składnik, zmień jego widoczność na
              ukrytą.
              <br />
              Zmiana kolejności składników ma wpływ także na pozostałe zakładki.
            </span>
          }
          buttonText="Dodaj swój pierwszy składnik"
          buttonOnClick={handleOpenForm}
        />
      }
    >
      {dataFetched && (
        <>
          <NetWorthPartCardsGrid
            parts={data.parts}
            handleChangePart={handleChangePart}
          />
          <NetWorthPartSidebar
            isOpenForm={openForm}
            onCloseForm={handleCloseForm}
            part={part}
          />
        </>
      )}
    </InternalPage>
  );
}
