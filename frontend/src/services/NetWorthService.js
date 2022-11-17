import {GOAL, NETWORTH, NETWORTHENTRY, NETWORTHPART} from "./CONSTANTS";
import {appConfig} from "src/config/config";
import {axiosDelete, axiosGet, axiosPost, axiosPut} from "./AxiosBase";
import {netWorthMock} from "../demo_mocks/NetWorthMock";

const demo = appConfig.demo;

export const getNetWorthData = async (token) => {
  return demo ? netWorthMock : axiosGet(NETWORTH(), token);
};

export const addNetWorthPart = async (model, token) => {
  model.id = null;
  return axiosPost(NETWORTHPART(), model, token);
};

export const updateNetWorthPart = async (model, token) => {
  return axiosPut(NETWORTHPART(), model, token);
};

export const deleteNetWorthPart = async (id, token) => {
  return axiosDelete(NETWORTHPART(), { id }, token);
};

export const changePartOrders = async (ids, token) => {
  return axiosPost(NETWORTHPART("order"), ids, token);
};

export const addNetWorthEntry = async (model, token) => {
  model.id = null;
  return axiosPost(NETWORTHENTRY(), model, token);
};

export const updateNetWorthEntry = async (model, token) => {
  return axiosPut(NETWORTHENTRY(), model, token);
};

export const deleteNetWorthEntry = async (id, token) => {
  return axiosDelete(NETWORTHENTRY(), { id }, token);
};

export const addNetWorthGoal = async (model, token) => {
  model.id = null;
  return axiosPost(GOAL(), model, token);
};

export const updateNetWorthGoal = async (model, token) => {
  return axiosPut(GOAL(), model, token);
};

export const deleteNetWorthGoal = async (id, token) => {
  return axiosDelete(GOAL(), { id }, token);
};
