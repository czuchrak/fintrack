import {createSlice} from "@reduxjs/toolkit";
import {appConfig} from "src/config/config";
import {notificationsMock} from "src/demo_mocks/NotificationsMock";
import {propertiesMock} from "../../demo_mocks/PropertiesMock";

const demo = appConfig.demo;

export const profileSlice = createSlice({
  name: "profile",
  initialState: {
    notifications: demo ? notificationsMock : [],
    userSettings: {},
    properties: demo ? propertiesMock : [],
    currency: "PLN",
    currencies: ["CHF", "EUR", "GBP", "PLN", "USD"],
  },
  reducers: {
    addProfile: (state, action) => {
      state.notifications = action.payload.notifications;

      state.notifications = state.notifications.sort((a, b) => {
        if (a.date < b.date) return 1;
        if (a.date > b.date) return -1;
        return 0;
      });

      state.userSettings = action.payload.userSettings;
      state.properties = action.payload.properties;
      state.currencies = action.payload.currencies;

      state.mailVerificationSent = action.payload.mailVerificationSent;
      state.currency = action.payload.currency;
    },
    reset: (state) => {
      state.notifications = [];
      state.properties = [];
      state.userSettings = {};
      state.currencies = ["CHF", "EUR", "GBP", "PLN", "USD"];
      state.currency = "PLN";
    },
  },
});

export const { reset, addProfile } = profileSlice.actions;

export default profileSlice.reducer;
