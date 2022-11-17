import {addError, removeError} from "../redux/slices/errorSlice";
import {fetchData, setLoading} from "../redux/netWorth/netWorthSlice";
import {useDispatch} from "react-redux";
import {useAuth} from "../navigation/PrivateRoute";
import {
    addNetWorthEntry,
    addNetWorthGoal,
    addNetWorthPart,
    changePartOrders,
    deleteNetWorthEntry,
    deleteNetWorthGoal,
    deleteNetWorthPart,
    updateNetWorthEntry,
    updateNetWorthGoal,
    updateNetWorthPart,
} from "../services";
import {appConfig} from "../config/config";
import {getFullDate} from "../utils/helpers";

const demo = appConfig.demo;

export default function useNetWorthServiceActions() {
  const dispatch = useDispatch();
  let auth = useAuth();

  const wrapper = async (method) => {
    dispatch(setLoading(true));
    try {
      await method();
      dispatch(fetchData(await auth.getToken()));
    } catch (error) {
      dispatch(setLoading(false));
      dispatch(addError(error.message));
    }
  };

  const getData = async () => {
    dispatch(removeError());
    dispatch(fetchData(demo ? "" : await auth.getToken()));
  };

  const savePart = async (model) => {
    await wrapper(async () => {
      if (model.id === undefined) {
        await addNetWorthPart(model, await auth.getToken());
      } else {
        await updateNetWorthPart(model, await auth.getToken());
      }
    });
  };

  const deletePart = async (id) => {
    await wrapper(async () => {
      await deleteNetWorthPart(id, await auth.getToken());
    });
  };

  const changeOrders = async (ids) => {
    await wrapper(async () => {
      await changePartOrders(ids, await auth.getToken());
    });
  };

  const saveEntry = async (parts, values, showAll) => {
    await wrapper(async () => {
      const model = {};
      model.partValues = {};
      model.date = values.date;
      parts
        .filter((x) => showAll || x.isVisible)
        .forEach((x) => {
          model.partValues[x.id] = values[x.id];
        });
      if (values.id === undefined) {
        await addNetWorthEntry(model, await auth.getToken());
        auth.refreshBckUser();
      } else {
        model.id = values.id;
        await updateNetWorthEntry(model, await auth.getToken());
      }
    });
  };

  const deleteEntry = async (id) => {
    await wrapper(async () => {
      await deleteNetWorthEntry(id, await auth.getToken());
      auth.refreshBckUser();
    });
  };

  const saveGoal = async (model) => {
    await wrapper(async () => {
      model.parts = model.parts.map((x) => x.id);
      model.deadline = getFullDate(model.deadline);
      if (model.id === undefined) {
        await addNetWorthGoal(model, await auth.getToken());
      } else {
        await updateNetWorthGoal(model, await auth.getToken());
      }
    });
  };

  const deleteGoal = async (id) => {
    await wrapper(async () => {
      await deleteNetWorthGoal(id, await auth.getToken());
    });
  };

  return {
    getData,
    savePart,
    deletePart,
    changeOrders,
    saveEntry,
    deleteEntry,
    saveGoal,
    deleteGoal,
  };
}
