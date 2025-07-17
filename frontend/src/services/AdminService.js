import { ADMIN } from "./CONSTANTS";
import { axiosDelete, axiosGet, axiosPost, axiosPut } from "./AxiosBase";

export const getLogs = async (token) => {
  return await axiosGet(ADMIN("logs"), token);
};

export const deleteLogs = async (token) => {
  return await axiosDelete(ADMIN("logs"), null, token);
};

export const getUsers = async (token) => {
  return await axiosGet(ADMIN("users"), token);
};

export const getNotifications = async (token) => {
  return await axiosGet(ADMIN("notifications"), token);
};

export const addNotification = async (model, token) => {
  model.id = null;
  return await axiosPost(ADMIN("notifications"), model, token);
};

export const updateNotification = async (model, token) => {
  return await axiosPut(ADMIN("notifications"), model, token);
};

export const duplicateNotification = async (id, token) => {
  return await axiosPost(ADMIN("notifications/duplicate"), null, token, {
    id,
  });
};

export const deleteNotification = async (id, token) => {
  return await axiosDelete(ADMIN("notifications"), { id }, token);
};

export const getPropertyCategories = async (token) => {
  return await axiosGet(ADMIN("PropertyCategory"), token);
};

export const addPropertyCategory = async (model, token) => {
  model.id = null;
  return await axiosPost(ADMIN("PropertyCategory"), model, token);
};

export const updatePropertyCategory = async (model, token) => {
  return await axiosPut(ADMIN("PropertyCategory"), model, token);
};

export const getExchangeRates = async (token) => {
  return await axiosGet(ADMIN("rates"), token);
};

export const fillExchangeRates = async (token) => {
  return await axiosPost(ADMIN("rates"), null, token);
};
