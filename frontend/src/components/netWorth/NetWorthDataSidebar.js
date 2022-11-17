import {useMemo, useState} from "react";
import {Icon} from "@iconify/react";
import {IconButton, Tooltip} from "@mui/material";
import {getDate, sortByOrder} from "src/utils/helpers";
import eyeFill from "@iconify/icons-eva/eye-fill";
import eyeOffFill from "@iconify/icons-eva/eye-off-fill";
import {DateInput} from "../form/Dates";
import {DecimalInput} from "../form/Inputs";
import Sidebar from "../utilities/Sidebar";
import {useForm} from "../form/FormikHelper";
import {DateValidation, DecimalValidation} from "../form/YupHelper";
import useNetWorthServiceActions from "../../serviceActions/NetWorthServiceActions";

export default function NetWorthDataSidebar({
  isOpenForm,
  onCloseForm,
  entry,
  parts,
  dates,
  lastEntry,
}) {
  const [showAll, setShowAll] = useState(false);
  const netWorthServiceActions = useNetWorthServiceActions();

  const handleShowAll = () => {
    setShowAll(!showAll);
  };

  const handleCloseForm = () => {
    onCloseForm();
    setShowAll(false);
  };

  const fields = useMemo(() => {
    const date = new Date();
    date.setDate(1);

    const result = [
      { name: "id" },
      {
        name: "date",
        default: date,
        validation: DateValidation(true, {
          error: "Wpis dla tego miesiąca już istnieje",
          test: (item) => {
            return !dates.find(
              (x) =>
                x === getDate(item) && getDate(item) !== getDate(entry.date)
            );
          },
        }),
      },
    ];

    parts.forEach((part) => {
      result.push({
        name: part.id,
        value:
          entry === undefined || entry.partValues === undefined
            ? ""
            : entry.partValues[part.id].value,
        validation: DecimalValidation(100000000, part.isVisible || showAll),
      });
    });
    return result;
  }, [entry, parts, showAll, dates]);

  const onSave = async (values) => {
    await netWorthServiceActions.saveEntry(parts, values, showAll);
  };

  const formik = useForm(handleCloseForm, onSave, entry, fields);

  return (
    <Sidebar
      isOpenForm={isOpenForm}
      onCloseForm={handleCloseForm}
      formik={formik}
      isAdding={!entry.id}
      showAll={
        parts.some((x) => !x.isVisible) && (
          <Tooltip
            title={showAll ? "Schowaj ukryte" : "Pokaż ukryte"}
            placement="bottom"
          >
            <IconButton onClick={handleShowAll}>
              <Icon icon={showAll ? eyeFill : eyeOffFill} />
            </IconButton>
          </Tooltip>
        )
      }
    >
      <DateInput
        name="date"
        label="Data"
        formik={formik}
        views={["month", "year"]}
        disabled={entry.id}
      />
      {parts
        .filter((x) => showAll || x.isVisible)
        .sort(sortByOrder())
        .map((part) => (
          <DecimalInput
            key={part.id}
            name={part.id}
            label={part.name}
            currency={part.currency}
            formik={formik}
            lastValue={
              Object.keys(entry).length === 0 &&
              lastEntry &&
              lastEntry.partValues[part.id].value !== 0 &&
              lastEntry.partValues[part.id].value
            }
          />
        ))}
    </Sidebar>
  );
}
