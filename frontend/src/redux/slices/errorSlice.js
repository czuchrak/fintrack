import {createSlice} from "@reduxjs/toolkit";

export const errorSlice = createSlice({
  name: "error",
  initialState: {
    error: null,
  },
  reducers: {
    addError: (state, action) => {
      state.error = action.payload;
    },
    removeError: (state) => {
      state.error = null;
    },
  },
});

export const { addError, removeError } = errorSlice.actions;

export default errorSlice.reducer;
