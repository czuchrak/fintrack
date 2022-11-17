import moment from "moment";
import "moment/locale/pl";
import pl from "date-fns/locale/pl";

export function getFnsLocale() {
  return pl;
}

export function sortByOrder() {
  return function (a, b) {
    if (a.order < b.order) return -1;
    if (a.order > b.order) return 1;
    return 0;
  };
}

export function sortByDate(sign = 1) {
  return function (a, b) {
    if (a.date < b.date) return sign;
    else if (a.date > b.date) return -sign;
    else return 0;
  };
}

export function getFullDate(date) {
  return moment(date).format("YYYY-MM-DDTHH:mm:ssZ");
}

export function getDate(date) {
  return moment(date).format("yyyy-MM-DD");
}

export function getMonthAndYear(date) {
  const m = moment(date);
  m.locale("pl");
  return m.format("MMMM yyyy");
}

export function getMonthName(month) {
  return moment(month, "M").format("MMM");
}

export function getFullMonthNameFromMonth3(month3) {
  return moment(month3, "MMM").format("MMMM");
}

export function getMonth3AndYear(date) {
  const m = moment(date);
  m.locale("pl");
  return m.format("MMM yyyy");
}

export function getFullDateInText(date) {
  const m = moment(date);
  m.locale("pl");
  return m.format("D MMM yyyy");
}

export function getCurrencyFormatter(currency, maximumFractionDigits = 2) {
  return new Intl.NumberFormat(getCurrencyLocale(currency), {
    style: "currency",
    currency: currency,
    minimumFractionDigits: 2,
    maximumFractionDigits: maximumFractionDigits,
  });
}

export function getCurrencySymbol(currency, style = "currency") {
  return (0)
    .toLocaleString(getCurrencyLocale(currency), {
      style: style,
      currency: currency,
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    })
    .replace(/\d/g, "")
    .trim();
}

export function checkPreviousMonthFromCurrentEntry(
  date,
  currentEntryDate,
  month
) {
  const startDate = new Date(currentEntryDate);
  startDate.setMonth(new Date(currentEntryDate).getMonth() - month, 1);
  startDate.setHours(0, 0, 0, 0);

  const endDate = new Date(currentEntryDate);
  endDate.setMonth(new Date(currentEntryDate).getMonth() - month + 1, 1);
  endDate.setHours(0, 0, 0, 0);

  return date >= startDate && date < endDate;
}

export function handleChartRange(x, chartRange, currentDate) {
  switch (chartRange) {
    case "last12months":
      const last12Date = new Date(currentDate);
      return (
        x.date >= getDate(last12Date.setYear(last12Date.getFullYear() - 1))
      );
    case "thisyear":
      const thisYearDate = new Date();
      thisYearDate.setMonth(0, 1);
      return x.date >= getDate(thisYearDate);
    case "lastyear":
      const startDate = new Date();
      startDate.setYear(startDate.getFullYear() - 1);
      startDate.setMonth(0, 1);
      const endDate = new Date();
      endDate.setMonth(0, 1);
      return x.date >= getDate(startDate) && x.date < getDate(endDate);
    case "max":
    default:
      return true;
  }
}

export function onlyUnique(value, index, self) {
  return self.indexOf(value) === index;
}

function getCurrencyLocale(currency) {
  switch (currency) {
    case "PLN":
      return "pl-pl";
    case "USD":
      return "en-US";
    case "EUR":
      return "de-DE";
    case "GBP":
      return "en-GB";
    case "CHF":
      return "de-CH";
  }
}

export function getGoalSummary(parts, goal, partValues) {
  const goalParts = parts.filter((x) => goal.parts.includes(x.id));
  const isMix =
    goalParts.map((x) => x.type).includes("asset") &&
    goalParts.map((x) => x.type).includes("liability");
  const dateFrom = new Date();
  const dateTo = new Date(goal.deadline);
  const months =
    dateTo.getMonth() -
    dateFrom.getMonth() +
    12 * (dateTo.getFullYear() - dateFrom.getFullYear());
  const currentValue = goalParts
    .map((x) => {
      return !isMix || x.type === "asset"
        ? partValues[x.id].valueRate
        : -partValues[x.id].valueRate;
    })
    .reduce((a, b) => a + b, 0);

  const i = goal.returnRate / 12 / 100;
  const monthValue =
    i > 0
      ? ((1 + i) * goal.value - currentValue * Math.pow(1 + i, months)) /
        (((1 + i) * (Math.pow(1 + i, months) - 1)) / i)
      : (goal.value - currentValue) / months;

  return {
    months,
    currentValue,
    avgMonthValue: monthValue,
  };
}
