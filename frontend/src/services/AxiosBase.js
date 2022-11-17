import axios from "axios";

const timeout = 10000;

const getConfig = (token, params) => {
  return {
    timeout: timeout,
    params,
    headers: { Authorization: `Bearer ${token}` },
  };
};

export const axiosGet = async (url, token, params) => {
  return (await axios.get(url, getConfig(token, params))).data;
};

export const axiosPost = async (url, model, token, params) => {
  return await axios.post(url, model, getConfig(token, params));
};

export const axiosPut = async (url, model, token) => {
  return await axios.put(url, model, getConfig(token));
};

export const axiosDelete = async (url, params, token) => {
  return await axios.delete(url, getConfig(token, params));
};
