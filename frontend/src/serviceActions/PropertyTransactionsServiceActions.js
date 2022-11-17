import {addError, removeError} from "../redux/slices/errorSlice";
import {fetchData, setLoading} from "../redux/property/propertySlice";
import {useDispatch} from "react-redux";
import {useAuth} from "../navigation/PrivateRoute";
import {addPropertyTransaction, deletePropertyTransaction, updatePropertyTransaction,} from "../services";
import {appConfig} from "../config/config";
import {getFullDate} from "../utils/helpers";

const demo = appConfig.demo;

export default function usePropertyTransactionsServiceActions() {
  const dispatch = useDispatch();
  let auth = useAuth();

  const wrapper = async (method, id) => {
    dispatch(setLoading(true));
    try {
      await method();
      dispatch(fetchData({ id, token: demo ? "" : await auth.getToken() }));
    } catch (error) {
      dispatch(setLoading(false));
      dispatch(addError(error.message));
    }
  };

  const getData = async (id) => {
    dispatch(removeError());
    dispatch(fetchData({ id, token: demo ? "" : await auth.getToken() }));
  };

  const savePropertyTransaction = async (model, propertyId) => {
    await wrapper(async () => {
      if (!model.details) model.details = null;
      model.categoryId = model.categoryId.id;
      model.propertyId = propertyId;
      model.date = getFullDate(model.date);
      if (model.id === undefined) {
        await addPropertyTransaction(model, await auth.getToken());
      } else {
        await updatePropertyTransaction(model, await auth.getToken());
      }
    }, propertyId);
  };

  const deletePropertyTrans = async (propertyId, transactionId) => {
    await wrapper(async () => {
      await deletePropertyTransaction(
        propertyId,
        transactionId,
        await auth.getToken()
      );
    }, propertyId);
  };

  return {
    getData,
    savePropertyTransaction,
    deletePropertyTrans,
  };
}
