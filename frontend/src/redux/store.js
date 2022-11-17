import {configureStore} from "@reduxjs/toolkit";
import profileReducer, {reset as resetProfile,} from "src/redux/slices/profileSlice";
import netWorthReducer, {reset as resetNetWorth,} from "src/redux/netWorth/netWorthSlice";
import errorReducer, {removeError} from "src/redux/slices/errorSlice";
import propertySettingsReducer, {reset as resetPropertySettings,} from "src/redux/property/propertySettingsSlice";
import propertyReducer, {reset as resetProperty,} from "src/redux/property/propertySlice";
import {appConfig} from "src/config/config";
import {useDispatch} from "react-redux";

export default configureStore({
  reducer: {
    profile: profileReducer,
    netWorth: netWorthReducer,
    error: errorReducer,
    propertySettings: propertySettingsReducer,
    property: propertyReducer,
  },
  devTools: !appConfig.prodMode,
});

export function useReduxActions() {
  const dispatch = useDispatch();

  const reset = () => {
    dispatch(resetNetWorth());
    dispatch(resetProfile());
    dispatch(resetPropertySettings());
    dispatch(resetProperty());
    dispatch(removeError());
  };

  return { reset };
}
