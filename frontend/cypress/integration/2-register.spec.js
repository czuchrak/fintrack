describe("Profile", () => {
  beforeEach(() => {
    cy.visit("/");
  });

  it("register", () => {
    cy.clickByText("Utwórz konto");

    //empty fields
    cy.clickCheckUrl("Zarejestruj się", "/register");
    cy.contains("Adres e-mail jest wymagany");

    //wrong e-mail
    cy.typeInputByName("email", "kkkk");
    cy.clickCheckUrl("Zarejestruj się", "/register");
    cy.contains("Nieprawidłowy adres e-mail");

    //wrong password
    cy.typeInputByName("email", "contact@fintrack.app");
    cy.typeInputByName("password", "1");
    cy.clickCheckUrl("Zarejestruj się", "/register");
    cy.contains("Hasło jest za krótkie");

    //good register
    cy.typeInputByName("password", "password-very-hard123");
    cy.clickCheckUrl("Zarejestruj się", "/onboarding/mailverification");
    cy.get("[data-cy=verifyEmail]").click();
    cy.contains("Mail weryfikujący został wysłany");

    //logout
    cy.clickProfile();
    cy.logout();

    //account exists
    cy.clickByText("Utwórz je");
    cy.typeInputByName("email", "contact@fintrack.app");
    cy.typeInputByName("password", "password-very-hard123");
    cy.clickCheckUrl("Zarejestruj się", "/register");
    cy.contains("Konto z tym adresem email");
  });
});
