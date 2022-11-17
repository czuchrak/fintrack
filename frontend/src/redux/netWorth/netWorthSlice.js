import {createAsyncThunk, createSlice} from "@reduxjs/toolkit";
import {getNetWorthData} from "../../services";

export const fetchData = createAsyncThunk(
  "netWorth/fetchData",
  async (token) => {
    return await getNetWorthData(token);
  }
);

export const netWorthSlice = createSlice({
  name: "netWorth",
  initialState: {
    data: {},
    dataFetched: false,
    isLoading: false,
    error: null,
  },
  reducers: {
    setLoading: (state, action) => {
      state.isLoading = action.payload;
    },
    reset: (state) => {
      state.data = {};
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
      let entries = [];
      let parts = action.payload.parts;
      let rates = action.payload.rates;
      let goals = action.payload.goals;

      action.payload.entries.forEach((entryTmp) => {
        let entry = {
          id: entryTmp.id,
          date: entryTmp.date,
          partValues: [],
          exchangeRateDate: entryTmp.exchangeRateDate,
        };
        let assets = 0;
        let liabilities = 0;

        parts.forEach((part) => {
          const rate =
            part.currency === "PLN"
              ? 1
              : rates.find(
                  (x) =>
                    x.currency === part.currency &&
                    x.date === entryTmp.exchangeRateDate
                ).rate;

          const value =
            entryTmp.partValues[part.id] === undefined
              ? 0
              : entryTmp.partValues[part.id];
          const valueRate = value * rate;

          entry.partValues[part.id] = { value, valueRate, rate };

          if (part.type === "asset") assets += valueRate;
          else liabilities += valueRate;
        });

        entry.assets = assets.toFixed(2);
        entry.liabilities = liabilities.toFixed(2);
        entry.value = (assets - liabilities).toFixed(2);

        entries.push(entry);
      });

      state.data = { entries, parts, rates, goals };
      state.dataFetched = true;
      state.error = null;
      state.isLoading = false;
    },
    [fetchData.rejected]: (state, action) => {
      state.error = action.error.message;
      state.isLoading = false;
    },
  },
});

export const { reset, setLoading } = netWorthSlice.actions;

export default netWorthSlice.reducer;
