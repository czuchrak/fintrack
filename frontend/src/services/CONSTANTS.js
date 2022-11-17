export const NETWORTH = () => `api/networth`;
export const NETWORTHENTRY = () => `api/networthentry`;
export const NETWORTHPART = (action) => wrapper("api/networthpart", action);
export const GOAL = (action) => wrapper("api/networthgoal", action);
export const USER = (action) => wrapper("api/user", action);
export const ADMIN = (action) => wrapper("api/admin", action);
export const PROPERTY = (action) => wrapper("api/property", action);
export const PROPERTYTRANSACTION = (action) =>
  wrapper("api/propertytransaction", action);

const wrapper = (url, action) => {
  return action === undefined ? url : `${url}/${action}`;
};
