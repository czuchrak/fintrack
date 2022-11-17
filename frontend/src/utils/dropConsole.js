/**
 * This file is to handle unwanted console logs.
 * It is important to call this file at the beginning of the app.
 */
const console = (function (oldCons) {
  // eslint-disable-next-line no-undef
  let drop = "true" === process.env.REACT_APP_PROD_MODE;

  let newCons = { ...oldCons };
  window.consol = oldCons; // A quick handle to enable all console logs again. In browser > developer tools > console tab: {window.console = window.consol}
  if (drop) {
    oldCons.log(
      "%c%s",
      "color: red; font-size: 14px; padding: 6px;",
      "UWAGA: To miejsce przeznaczone jest tylko dla programistów. Nie wklejaj tutaj żadnych skryptów. To może być niebezpieczne!"
    );
    newCons.log = function () {};
    newCons.warn = function () {};
    newCons.error = oldCons.error; // You can retain error logs if required.
  }
  return newCons;
})(window.console);
window.console = console;
