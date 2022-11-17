describe("Profile", () => {
  beforeEach(() => {
    cy.visit("/");
  });

  it("login", () => {
    cy.clickByText("Zaloguj się");

    //empty fields
    cy.clickCheckUrl("Zaloguj się", "/login");
    cy.contains("Adres e-mail jest wymagany");
    cy.contains("Hasło jest wymagane");

    //wrong e-mail
    cy.typeInputByName("email", "kkkk");
    cy.clickCheckUrl("Zaloguj się", "/login");
    cy.contains("Nieprawidłowy adres e-mail");

    //wrong password
    cy.typeInputByName("email", "contact@fintrack.app");
    cy.typeInputByName("password", "wrongpassword");
    cy.clickCheckUrl("Zaloguj się", "/login");
    cy.contains("Niepoprawne hasło");

    //account does not exist
    cy.typeInputByName("email", "k2222@k.pl");
    cy.typeInputByName("password", "wrongpassword");
    cy.clickCheckUrl("Zaloguj się", "/login");
    cy.contains("Użytkownik nie istnieje");

    //good login
    cy.login("contact@fintrack.app", "password-very-hard123");

    //logout
    cy.clickProfile();
    cy.logout();
  });

  it("reset password", () => {
    cy.clickByText("Zaloguj się");
    cy.clickCheckUrl("Zresetuj hasło", "/resetpassword");

    //empty field
    cy.clickCheckUrl("Zresetuj hasło", "/resetpassword");
    cy.contains("Adres e-mail jest wymagany");

    //wrong e-mail
    cy.typeInputByName("email", "kkkk");
    cy.clickCheckUrl("Zresetuj hasło", "/resetpassword");
    cy.contains("Nieprawidłowy adres e-mail");

    //account does not exist
    cy.typeInputByName("email", "c@fintrack.app");
    cy.clickCheckUrl("Zresetuj hasło", "/resetpassword");
    cy.contains("Użytkownik nie istnieje");

    //good reset
    cy.typeInputByName("email", "contact@fintrack.app");
    cy.clickCheckUrl("Zresetuj hasło", "/resetpassword");
    cy.contains("Link do zresetowania hasła");

    cy.clickCheckUrl("Wróć na stronę logowania", "/login");
  });

  it("change password", () => {
    cy.clickByText("Zaloguj się");
    cy.login("contact@fintrack.app", "password-very-hard123");
    cy.clickProfile();
    cy.clickCheckUrl("Ustawienia", "/settings");

    //empty fields
    cy.clickCheckUrl("Zapisz", "/settings");
    cy.contains("To pole jest wymagane");

    //wrong old password
    cy.typeInputByName("oldPassword", "wrong-password");
    cy.typeInputByName("newPassword", "wrong-password");
    cy.typeInputByName("newPassword2", "wrong-password");
    cy.clickCheckUrl("Zapisz", "/settings");
    cy.contains("Niepoprawne hasło");

    //wrong new password
    cy.typeInputByName("oldPassword", "password-very-hard123");
    cy.typeInputByName("newPassword", "w");
    cy.typeInputByName("newPassword2", "w");
    cy.clickCheckUrl("Zapisz", "/settings");
    cy.contains("Hasło jest za krótkie");

    //wrong new passwords
    cy.typeInputByName("newPassword", "wrongpassword");
    cy.typeInputByName("newPassword2", "wrongpassword2");
    cy.clickCheckUrl("Zapisz", "/settings");
    cy.contains("Hasła nie są identyczne");

    //change password
    cy.typeInputByName("newPassword", "password-very-hard123");
    cy.typeInputByName("newPassword2", "password-very-hard123");
    cy.clickCheckUrl("Zapisz", "/settings");
    cy.contains("Hasło zostało zmienione");

    //logout
    cy.clickProfile();
    cy.logout();
  });

  it("admin not visible in profile", () => {
    cy.clickByText("Zaloguj się");
    cy.login("contact@fintrack.app", "password-very-hard123");

    cy.clickProfile();
    cy.contains("Administracja").should("not.exist");

    //logout
    cy.logout();
  });

  it("admin not available", () => {
    cy.clickByText("Zaloguj się");
    cy.login("contact@fintrack.app", "password-very-hard123");

    cy.visit("/admin");
    cy.url().should("include", "/networth/dashboard");

    //logout
    cy.clickProfile();
    cy.logout();
  });

  it("check user mail", () => {
    cy.clickByText("Zaloguj się");
    cy.login("contact@fintrack.app", "password-very-hard123");

    cy.clickProfile();
    cy.contains("contact@fintrack.app");

    //logout
    cy.logout();
  });

  it("change email settings", () => {
    cy.clickByText("Zaloguj się");
    cy.login("contact@fintrack.app", "password-very-hard123");
    cy.clickProfile();
    cy.clickCheckUrl("Ustawienia", "/settings");

    cy.checkboxUncheck("NewMonthEmailEnabled");
    cy.checkboxUncheck("NewsEmailEnabled");

    cy.reload();

    cy.checkboxCheck("NewMonthEmailEnabled");
    cy.checkboxCheck("NewsEmailEnabled");

    //logout
    cy.clickProfile();
    cy.logout();
  });

  it("send message", () => {
    cy.clickByText("Zaloguj się");
    cy.login("contact@fintrack.app", "password-very-hard123");
    cy.clickCheckUrl("Kontakt", "/contact");

    //empty fields
    cy.clickCheckUrl("Wyślij", "/contact");
    cy.contains("To pole jest wymagane");

    //send message
    cy.typeInputByName("topic", "Test wiadomości");
    cy.typeTextAreaByName("message", "No elo elo, co tam? hejo, pzdr K.");
    cy.clickCheckUrl("Wyślij", "/contact");
    cy.contains("Wiadomość została wysłana.");

    //logout
    cy.clickProfile();
    cy.logout();
  });

  it("changelog", () => {
    cy.clickByText("Zaloguj się");
    cy.login("contact@fintrack.app", "password-very-hard123");
    cy.clickProfile();
    cy.clickCheckUrl("Lista zmian", "/changelog");

    //logout
    cy.clickProfile();
    cy.logout();
  });
});
