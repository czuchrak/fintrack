import {useEffect, useMemo, useState} from "react";
import Sidebar from "../utilities/Sidebar";
import {DecimalInput, TextInput} from "../form/Inputs";
import {DateInput} from "../form/Dates";
import {AutoCompleteInput} from "../form/AutoCompleteInput";
import {useForm} from "../form/FormikHelper";
import {DateValidation, DecimalValidation, ObjectValidation, TextValidation,} from "../form/YupHelper";
import usePropertyTransactionsServiceActions from "../../serviceActions/PropertyTransactionsServiceActions";
import {sortByDate} from "../../utils/helpers";

export default function PropertyTransactionSidebar({
  isOpenForm,
  onCloseForm,
  propertyTransaction,
  property,
  propertyId,
}) {
  const [lastValue, setLastValue] = useState(null);
  const propertyTransactionsServiceActions =
    usePropertyTransactionsServiceActions();

  const onSave = async (values) => {
    await propertyTransactionsServiceActions.savePropertyTransaction(
      values,
      propertyId
    );
  };

  const formik = useForm(onCloseForm, onSave, propertyTransaction, [
    { name: "id" },
    { name: "propertyId" },
    {
      name: "date",
      default: new Date(),
      validation: DateValidation(true),
    },
    {
      name: "categoryId",
      value: property.categories.find(
        (x) => x.id === propertyTransaction.categoryId
      ),
      default: null,
      validation: ObjectValidation(true),
    },
    {
      name: "value",
      default: "",
      validation: DecimalValidation(10000000, true),
    },
    { name: "details", default: "", validation: TextValidation(40) },
  ]);

  const categoryOptions = useMemo(() => {
    return property.categories
      .slice()
      .sort((a, b) => -b.name.localeCompare(a.name))
      .map((category) => {
        return {
          groupBy: category.isCost ? "Koszt" : "Przychód",
          ...category,
        };
      })
      .sort((a, b) => -b.groupBy.localeCompare(a.groupBy));
  }, [property.categories]);

  useEffect(() => {
    if (!propertyTransaction.id) {
      const categoryId = formik.values.categoryId?.id;
      const trans = property.transactions
        .slice()
        .sort(sortByDate())
        .find((x) => x.categoryId === categoryId);

      setLastValue(trans ? trans.value : null);
    }
  }, [formik.values.categoryId, property.transactions, propertyTransaction.id]);

  return (
    <Sidebar
      isOpenForm={isOpenForm}
      onCloseForm={onCloseForm}
      formik={formik}
      isAdding={!propertyTransaction.id}
    >
      <DateInput
        name="date"
        label="Data"
        formik={formik}
        inputFormat="d MMMM yyyy"
      />
      <AutoCompleteInput
        name="categoryId"
        label="Kategoria"
        formik={formik}
        options={categoryOptions}
      />
      <DecimalInput
        name="value"
        label="Kwota"
        formik={formik}
        currency="PLN"
        lastValue={lastValue}
      />
      <TextInput name="details" label="Uwagi" formik={formik} />
    </Sidebar>
  );
}
