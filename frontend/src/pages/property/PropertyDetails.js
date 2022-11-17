import {useEffect, useState} from "react";
import {Grid} from "@mui/material";
import {useDispatch, useSelector} from "react-redux";
import {addError} from "src/redux/slices/errorSlice";
import {useNavigate, useParams} from "react-router-dom";
import PropertySummary from "../../components/property/PropertySummary";
import PropertyTransactions from "../../components/property/PropertyTransactions";
import {PropertyHeatMap} from "../../components/property/PropertyHeatMap";
import PropertyYears from "../../components/property/PropertyYears";
import usePropertyTransactionsServiceActions from "../../serviceActions/PropertyTransactionsServiceActions";
import InternalPage from "../../components/InternalPage";
import PropertyTransactionSidebar from "../../components/property/PropertyTransactionSidebar";
import EmptyState from "../../components/utilities/EmptyState";
import ShoppingCartRoundedIcon from "@mui/icons-material/ShoppingCartRounded";

export default function PropertyDetails() {
  const propertyTransactionsServiceActions =
    usePropertyTransactionsServiceActions();
  const dispatch = useDispatch();
  const { id } = useParams();
  let navigate = useNavigate();

  const [propertyTransaction, setPropertyTransaction] = useState({});
  const [openForm, setOpenForm] = useState(false);

  const userProperties = useSelector((state) => state.profile.properties);

  const { properties, dataFetched, isLoading, error } = useSelector(
    (state) => state.property
  );
  const property = properties[id];
  dispatch(addError(error));

  const handleOpenForm = () => {
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setPropertyTransaction({});
  };

  const handleChangePropertyTransaction = (propertyTransaction) => {
    setPropertyTransaction(propertyTransaction);
    setOpenForm(true);
  };

  useEffect(() => {
    if (userProperties.find((x) => x.id === id) === undefined) {
      navigate(process.env.PUBLIC_URL + "/property/settings");
      return;
    }

    if (!dataFetched[id]) {
      propertyTransactionsServiceActions.getData(id).then(() => {});
    }
  }, [
    propertyTransactionsServiceActions,
    id,
    dataFetched,
    userProperties,
    navigate,
  ]);

  return (
    <InternalPage
      title={property !== undefined ? property.name : ""}
      isLoading={isLoading}
      handleOpenForm={handleOpenForm}
      showEmptyState={
        !property ||
        !property.transactions ||
        property.transactions.length === 0
      }
      emptyState={
        <EmptyState
          icon={ShoppingCartRoundedIcon}
          text="Nie masz dodanych żadnych transakcji."
          bodyText={
            <span>
              Tutaj możesz dodawać, edytować i&nbsp;usuwać transakcje dotyczące
              Twojej nieruchomości.
              <br />
              <br />
              Jeśli masz kredyt hipoteczny na nieruchomość, w&nbsp;której
              mieszkasz,
              <br />
              to&nbsp;część kapitałową raty zapisuj co&nbsp;miesiąc jako 'Zakup
              nieruch.', a&nbsp;odsetki jako 'Koszty kredytu'.
              <br />
              <br />
              Natomiast w&nbsp;sytuacji, gdy masz kredyt hipoteczny na
              mieszkanie inwestycyjne,
              <br />
              to jako kwotę zakupu wpisz całość kwoty nieruchomości (+ opłaty)
              i&nbsp;co&nbsp;miesiąc uzupełniaj tylko odsetki jako
              'Koszty&nbsp;kredytu'.
            </span>
          }
          buttonText="Dodaj swoją pierwszą transakcję"
          buttonOnClick={handleOpenForm}
        />
      }
    >
      {dataFetched && property && property.transactions && (
        <>
          <Grid container spacing={3}>
            {property.transactions && property.transactions.length > 0 && (
              <>
                <Grid item xs={12} md={6}>
                  <PropertySummary property={property} />
                </Grid>
                <Grid item xs={12} md={6}>
                  <PropertyTransactions
                    property={property}
                    handleChangePropertyTransaction={
                      handleChangePropertyTransaction
                    }
                  />
                </Grid>
                <PropertyYears property={property} />
                <Grid item xs={12}>
                  <PropertyHeatMap property={property} />
                </Grid>
              </>
            )}
          </Grid>
          <PropertyTransactionSidebar
            isOpenForm={openForm}
            onCloseForm={handleCloseForm}
            property={property}
            propertyTransaction={propertyTransaction}
            propertyId={id}
          />
        </>
      )}
    </InternalPage>
  );
}
