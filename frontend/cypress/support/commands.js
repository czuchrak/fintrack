Cypress.Commands.add("clickByText", (text) => {
  cy.contains(text).click();
});

Cypress.Commands.add("clickReloadCheckUrl", (label, url) => {
  cy.clickByText(label);
  cy.reload();
  cy.url().should("include", url);
});

Cypress.Commands.add("clickCheckUrl", (label, url) => {
  cy.clickByText(label);
  cy.url().should("include", url);
});

Cypress.Commands.add("visitPrivatePage", (url) => {
  cy.visit(url);
  cy.url().should("include", "/login");
});

Cypress.Commands.add("typeInputByName", (name, value) => {
  cy.get(`input[name="${name}"]`).focus().clear();
  cy.get(`input[name="${name}"]`).focus().type(value);
});

Cypress.Commands.add("typeTextAreaByName", (name, value) => {
  cy.get(`textarea[name="${name}"]`).focus().clear();
  cy.get(`textarea[name="${name}"]`).focus().type(value);
});

Cypress.Commands.add("checkboxCheck", (name) => {
  cy.get(`input[name="${name}"]`).should("not.be.checked");
  cy.get(`input[name="${name}"]`).check();
});

Cypress.Commands.add("checkboxUncheck", (name) => {
  cy.get(`input[name="${name}"]`).should("be.checked");
  cy.get(`input[name="${name}"]`).uncheck();
});

Cypress.Commands.add("clickProfile", () => {
  cy.get("[data-cy=profile]").click();
});

Cypress.Commands.add("login", (email, password) => {
  cy.typeInputByName("email", email);
  cy.typeInputByName("password", password);
  cy.clickCheckUrl("Zaloguj się", "/networth/dashboard");
});

Cypress.Commands.add("logout", () => {
  cy.clickCheckUrl("Wyloguj", "/login");
  cy.contains("Logowanie");
});
