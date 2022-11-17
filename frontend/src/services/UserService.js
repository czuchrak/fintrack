import {USER} from "./CONSTANTS";
import {axiosDelete, axiosGet, axiosPost} from "./AxiosBase";

export const getUser = async (token) => {
  return await axiosGet(USER(), token);
};

export const deleteUser = async (token) => {
  return await axiosDelete(USER(), null, token);
};

export const sendMessage = async (model, token) => {
  return await axiosPost(USER("message"), model, token);
};

export const markNotificationAsRead = async (id, token) => {
  return await axiosPost(USER("notifications/markAsRead"), null, token, {
    notificationId: id,
  });
};

export const setUserSetting = async (name, value, token) => {
  return await axiosPost(USER("settings"), null, token, {
    name,
    value,
  });
};

export const setMailVerificationSent = async (token) => {
  return await axiosPost(USER("mailVerificationSent"), null, token);
};
