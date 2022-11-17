import Sidebar from "../utilities/Sidebar";
import {DecimalInput, TextInput} from "../form/Inputs";
import {useForm} from "../form/FormikHelper";
import {
    ArrayValidation,
    DateValidation,
    DecimalValidation,
    PercentDecimalValidation,
    TextValidation,
} from "../form/YupHelper";
import useNetWorthServiceActions from "../../serviceActions/NetWorthServiceActions";
import {useEffect, useMemo, useState} from "react";
import {DateInput} from "../form/Dates";
import {getCurrencyFormatter, getGoalSummary, sortByDate,} from "../../utils/helpers";
import {Typography} from "@mui/material";
import {AutoCompleteMultiInput} from "../form/AutoCompleteInput";

export default function NetWorthGoalSidebar({
  isOpenForm,
  onCloseForm,
  goal,
  parts,
  entries,
}) {
  const netWorthServiceActions = useNetWorthServiceActions();
  const [obj, setObj] = useState(null);

  const lastEntry = useMemo(() => {
    return entries.length > 0 ? entries.slice().sort(sortByDate())[0] : null;
  }, [entries]);

  const minDate = useMemo(() => {
    let date = new Date();
    date.setDate(1);
    date.setMonth(date.getMonth() + 1);
    return date;
  }, []);

  const defaultDate = useMemo(() => {
    let date = new Date();
    date.setFullYear(date.getFullYear() + 2, 0, 1);
    return date;
  }, []);

  const formik = useForm(onCloseForm, netWorthServiceActions.saveGoal, goal, [
    { name: "id" },
    { name: "name", default: "", validation: TextValidation(40, true) },
    {
      name: "parts",
      value: parts.filter((x) => goal.parts && goal.parts.includes(x.id)),
      default: [],
      validation: ArrayValidation(true),
    },
    {
      name: "deadline",
      default: defaultDate,
      validation: DateValidation(true),
    },
    {
      name: "value",
      default: 0,
      validation: DecimalValidation(10000000, true),
    },
    {
      name: "returnRate",
      default: 0,
      validation: PercentDecimalValidation(99.99, true),
    },
  ]);

  const categoryOptions = useMemo(() => {
    return parts
      .slice()
      .sort((a, b) => -b.name.localeCompare(a.name))
      .map((part) => {
        return {
          groupBy: part.type === "asset" ? "Aktywa" : "Zobowiązania",
          ...part,
        };
      })
      .sort((a, b) => -b.groupBy.localeCompare(a.groupBy));
  }, [parts]);

  useEffect(() => {
    const ps = formik.values.parts;
    if (ps.length > 0) {
      const obj = getGoalSummary(
        parts,
        {
          parts: ps.map((x) => x.id),
          deadline: formik.values.deadline,
          value: formik.values.value,
          returnRate: formik.values.returnRate,
        },
        lastEntry.partValues
      );
      setObj(obj);
    } else {
      setObj(null);
    }
  }, [
    formik.values.deadline,
    formik.values.parts,
    formik.values.value,
    formik.values.returnRate,
    lastEntry,
    parts,
  ]);

  return (
    <Sidebar
      isOpenForm={isOpenForm}
      onCloseForm={onCloseForm}
      formik={formik}
      isAdding={!goal.id}
    >
      <TextInput name="name" label="Nazwa" formik={formik} autoFocus />
      <AutoCompleteMultiInput
        name="parts"
        label="Składniki"
        formik={formik}
        options={categoryOptions}
        disabled={Boolean(goal.id)}
      />
      <DateInput
        name="deadline"
        label="Termin"
        formik={formik}
        views={["month", "year"]}
        minDate={minDate}
        maxDate={new Date("2099-01-01")}
      />
      {obj && (
        <>
          <DecimalInput
            name="value"
            label="Docelowa wartość"
            formik={formik}
            currency={"PLN"}
          />
          <DecimalInput
            name="returnRate"
            label="Roczna stopa zwrotu"
            formik={formik}
            currency={"PLN"}
            percent
          />
          <Typography variant="body2" align="center">
            Aktualna wartość:
            <br />
            <strong>
              {getCurrencyFormatter("PLN").format(obj.currentValue)}
            </strong>
          </Typography>
          <Typography variant="body2" align="center">
            Pozostało miesięcy:
            <br />
            <strong>{obj.months}</strong>
          </Typography>
          <Typography variant="body2" align="center">
            {obj.avgMonthValue > 0
              ? "Zwiększaj miesięcznie o:"
              : "Zmniejszaj miesięcznie o:"}
            <br />
            <strong>
              {getCurrencyFormatter("PLN").format(Math.abs(obj.avgMonthValue))}
            </strong>
          </Typography>
        </>
      )}
    </Sidebar>
  );
}
