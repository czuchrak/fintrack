import { defineConfig } from "cypress";

export default defineConfig({
  viewportWidth: 1400,
  viewportHeight: 1000,
  e2e: {
    setupNodeEvents(on, config) {
      return require("./cypress/plugins/index.js")(on, config);
    },
    baseUrl: "https://czuchra.com/fintrack-test",
  },
});
