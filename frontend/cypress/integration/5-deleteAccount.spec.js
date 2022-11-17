describe("Delete account", () => {
  beforeEach(() => {
    cy.visit("/");
  });

  it("delete account", () => {
    cy.clickByText("Zaloguj się");
    cy.login("contact@fintrack.app", "password-very-hard123");
    cy.clickProfile();
    cy.clickCheckUrl("Ustawienia", "/settings");

    //checkbox is not checked
    cy.contains("Usuń konto").should("be.disabled");

    //empty password
    cy.clickByText("Chcę usunąć konto");
    cy.clickCheckUrl("Usuń konto", "/settings");
    cy.contains("To pole jest wymagane");

    //wrong password
    cy.typeInputByName("password", "wrong-password");
    cy.clickCheckUrl("Usuń konto", "/settings");
    cy.contains("Niepoprawne hasło");

    //delete account
    cy.typeInputByName("password", "password-very-hard123");
    cy.clickCheckUrl("Usuń konto", "/settings");
    cy.contains("Logowanie");
  });
});
