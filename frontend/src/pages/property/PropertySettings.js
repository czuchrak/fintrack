import {useEffect, useState} from "react";
import {Grid} from "@mui/material";
import {useDispatch, useSelector} from "react-redux";
import {addError} from "src/redux/slices/errorSlice";
import PropertyCard from "../../components/property/PropertyCard";
import PropertySidebar from "../../components/property/PropertySidebar";
import InternalPage from "../../components/InternalPage";
import usePropertyServiceActions from "../../serviceActions/PropertyServiceActions";
import EmptyState from "../../components/utilities/EmptyState";
import HomeRoundedIcon from "@mui/icons-material/HomeRounded";

export default function PropertySettings() {
  const dispatch = useDispatch();
  const propertyServiceActions = usePropertyServiceActions();

  const { data, dataFetched, isLoading, error } = useSelector(
    (state) => state.propertySettings
  );

  dispatch(addError(error));

  const [openForm, setOpenForm] = useState(false);
  const [property, setProperty] = useState({});

  const handleOpenForm = () => {
    setProperty({});
    setOpenForm(true);
  };

  const handleCloseForm = () => {
    setOpenForm(false);
    setProperty({});
  };

  const handleChangeProperty = (property) => {
    setProperty(property);
    setOpenForm(true);
  };

  useEffect(() => {
    if (!dataFetched) {
      propertyServiceActions.getData().then(() => {});
    }
  }, [propertyServiceActions, dataFetched]);

  return (
    <InternalPage
      title="Nieruchomości"
      isLoading={isLoading}
      handleOpenForm={handleOpenForm}
      showEmptyState={!data || data.length === 0}
      emptyState={
        <EmptyState
          icon={HomeRoundedIcon}
          text="Nie masz dodanych żadnych nieruchomości."
          bodyText={
            <span>
              Tutaj możesz dodawać, edytować i&nbsp;usuwać nieruchomości,
              których finanse będziesz monitorować. <br />
              <br />
              Każdą swoją nieruchomość możesz zdezaktywować.
              <br />
              Jest to przydatne w sytuacji, np.&nbsp;gdy zmieniasz jej
              przeznaczenie z&nbsp;celów mieszkaniowych na wynajem lub ją
              sprzedajesz. <br />
              <br />
              Nieruchomości aktywne pojawiają się w&nbsp;menu rozwijanym.
            </span>
          }
          buttonText="Dodaj swoją pierwszą nieruchomość"
          buttonOnClick={handleOpenForm}
        />
      }
    >
      {dataFetched && (
        <>
          {isLoading || (
            <Grid container spacing={3}>
              {data
                .slice()
                .sort(
                  (a, b) =>
                    -a.isActive.toString().localeCompare(b.isActive.toString())
                )
                .map((property) => (
                  <Grid key={property.id} item xs={12} sm={6} md={3}>
                    <PropertyCard
                      property={property}
                      handleChangeProperty={handleChangeProperty}
                    />
                  </Grid>
                ))}
            </Grid>
          )}
          <PropertySidebar
            isOpenForm={openForm}
            onCloseForm={handleCloseForm}
            property={property}
          />
        </>
      )}
    </InternalPage>
  );
}
