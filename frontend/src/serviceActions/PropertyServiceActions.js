import {addError, removeError} from "../redux/slices/errorSlice";
import {fetchData, setLoading} from "../redux/property/propertySettingsSlice";
import {useDispatch} from "react-redux";
import {useAuth} from "../navigation/PrivateRoute";
import {addProperty, deleteProperty, updateProperty} from "../services";
import {appConfig} from "../config/config";
import {resetProperty} from "../redux/property/propertySlice";

const demo = appConfig.demo;

export default function usePropertyServiceActions() {
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

  const saveProperty = async (model) => {
    await wrapper(async () => {
      if (model.id === undefined) {
        await addProperty(model, await auth.getToken());
      } else {
        await updateProperty(model, await auth.getToken());
        dispatch(resetProperty(model.id));
      }
      auth.refreshBckUser();
    });
  };

  const deleteProp = async (id) => {
    await wrapper(async () => {
      await deleteProperty(id, await auth.getToken());
      auth.refreshBckUser();
    });
  };

  return {
    getData,
    saveProperty,
    deleteProp,
  };
}
