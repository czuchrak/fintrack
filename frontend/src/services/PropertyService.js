import {PROPERTY, PROPERTYTRANSACTION} from "./CONSTANTS";
import {appConfig} from "src/config/config";
import {propertiesMock} from "../demo_mocks/PropertiesMock";
import {propertyDetails} from "../demo_mocks/PropertyDetails";
import {axiosDelete, axiosGet, axiosPost, axiosPut} from "./AxiosBase";

const demo = appConfig.demo;

export const getPropertyDetails = async (id, token) => {
  return demo
    ? propertyDetails(id)
    : await axiosGet(PROPERTY("details"), token, { id });
};

export const getProperties = async (token) => {
  return demo ? propertiesMock : await axiosGet(PROPERTY(), token);
};

export const addProperty = async (model, token) => {
  model.id = null;
  return await axiosPost(PROPERTY(), model, token);
};

export const updateProperty = async (model, token) => {
  return await axiosPut(PROPERTY(), model, token);
};

export const deleteProperty = async (id, token) => {
  return await axiosDelete(PROPERTY(), { id }, token);
};

export const addPropertyTransaction = async (model, token) => {
  model.id = null;
  return await axiosPost(PROPERTYTRANSACTION(), model, token);
};

export const updatePropertyTransaction = async (model, token) => {
  return await axiosPut(PROPERTYTRANSACTION(), model, token);
};

export const deletePropertyTransaction = async (
  propertyId,
  transactionId,
  token
) => {
  return await axiosDelete(
    PROPERTYTRANSACTION(),
    { propertyId, transactionId },
    token
  );
};
