import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {getPropertyDetails} from "src/services";

export const fetchData = createAsyncThunk(
  "property/fetchData",
  async (data) => {
    return await getPropertyDetails(data.id, data.token);
  }
);

export const propertySlice = createSlice({
  name: "property",
  initialState: {
    properties: {},
    dataFetched: {},
    isLoading: false,
    error: null,
  },
  reducers: {
    setLoading: (state, action) => {
      state.isLoading = action.payload;
    },
    reset: (state) => {
      state.properties = {};
      state.dataFetched = {};
      state.isLoading = false;
      state.error = null;
    },
    resetProperty: (state, action) => {
      state.dataFetched[action.payload] = false;
    },
  },
  extraReducers: {
    [fetchData.pending]: (state) => {
      state.error = null;
      state.isLoading = true;
    },
    [fetchData.fulfilled]: (state, action) => {
      let id = action.meta.arg.id;
      state.properties[id] = action.payload;
      state.dataFetched[id] = true;
      state.error = null;
      state.isLoading = false;
    },
    [fetchData.rejected]: (state, action) => {
      let id = action.meta.arg.id;
      state.properties[id] = undefined;
      state.dataFetched[id] = false;
      state.error = action.error.message;
      state.isLoading = false;
    },
  },
});

export const { reset, setLoading, resetProperty } = propertySlice.actions;

export default propertySlice.reducer;
