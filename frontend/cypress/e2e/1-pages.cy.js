describe("Pages", () => {
  beforeEach(() => {
    cy.visit("/");
  });

  it("open app", () => {
    cy.visit("/");
  });

  it("open terms", () => {
    cy.clickReloadCheckUrl("Regulamin", "/terms");
  });

  it("open privacy", () => {
    cy.clickReloadCheckUrl("Polityka prywatności", "/privacy");
  });

  it("open login page", () => {
    cy.clickReloadCheckUrl("Zaloguj się", "/login");
  });

  it("open register page", () => {
    cy.clickReloadCheckUrl("Utwórz konto", "/register");
  });

  it("open terms page from register", () => {
    cy.clickReloadCheckUrl("Utwórz konto", "/register");
    cy.clickReloadCheckUrl("Regulamin", "/terms");
  });

  it("open privacy page from register", () => {
    cy.clickReloadCheckUrl("Utwórz konto", "/register");
    cy.clickReloadCheckUrl("Politykę", "/privacy");
  });

  it("networth", () => {
    cy.visitPrivatePage("/networth");
  });

  it("networth dashboard", () => {
    cy.visitPrivatePage("/networth/dashboard");
  });

  it("networth data", () => {
    cy.visitPrivatePage("/networth/data");
  });

  it("networth parts", () => {
    cy.visitPrivatePage("/networth/parts");
  });

  it("contact", () => {
    cy.visitPrivatePage("/contact");
  });

  it("settings", () => {
    cy.visitPrivatePage("/settings");
  });

  it("admin", () => {
    cy.visitPrivatePage("/admin");
  });

  it("property settings", () => {
    cy.visitPrivatePage("/property/settings");
  });

  it("property details", () => {
    cy.visitPrivatePage("/property/fddfac6d-912f-4e8f-b3ca-5292ae905adf");
  });

  it("incorrect route", () => {
    cy.visit("/fddfac6d-912f-4e8f-b3ca-5292ae905adf");
    cy.url().should("include", "/");
  });
});
