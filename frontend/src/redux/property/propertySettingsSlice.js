import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {getProperties} from "src/services";

export const fetchData = createAsyncThunk(
  "propertySettings/fetchData",
  async (token) => {
    return await getProperties(token);
  }
);

export const propertySettingsSlice = createSlice({
  name: "propertySettings",
  initialState: {
    data: [],
    dataFetched: false,
    isLoading: false,
    error: null,
  },
  reducers: {
    setLoading: (state, action) => {
      state.isLoading = action.payload;
    },
    reset: (state) => {
      state.data = [];
      state.dataFetched = false;
      state.isLoading = false;
      state.error = null;
    },
  },
  extraReducers: {
    [fetchData.pending]: (state) => {
      state.error = null;
      state.isLoading = true;
    },
    [fetchData.fulfilled]: (state, action) => {
      state.data = action.payload;
      state.dataFetched = true;
      state.error = null;
      state.isLoading = false;
    },
    [fetchData.rejected]: (state, action) => {
      state.data = [];
      state.dataFetched = false;
      state.error = action.error.message;
      state.isLoading = false;
    },
  },
});

export const { reset, setLoading } = propertySettingsSlice.actions;

export default propertySettingsSlice.reducer;
