// describe("NetWorth", () => {
//   beforeEach(() => {
//     cy.visit("/");
//   });
//
//   it("networthparts", () => {
//     cy.clickByText("Zaloguj się");
//     cy.login("contact@fintrack.app", "password-very-hard123");
//
//     cy.clickCheckUrl("Składniki", "/networth/parts");
//
//     cy.clickByText("Dodaj");
//     cy.contains("Dodawanie");
//
//     cy.typeInputByName("name", "Nowy part 123");
//     cy.clickByText("Zapisz");
//
//     cy.contains("Nowy part 123");
//     cy.contains("Aktywo");
//     cy.contains("widoczne");
//
//     cy.get("[data-cy=showInfo]").click();
//     cy.contains("Sprawi to");
//     cy.get("[data-cy=showInfo]").click();
//     cy.contains("Sprawi to").should("not.exist");
//
//     cy.get("[data-cy=moreMenu]").click();
//     cy.clickByText("Edytuj");
//     cy.contains("Edytowanie");
//
//     cy.typeInputByName("name", "Edytowane 999");
//
//     cy.get('input[name="type"]').parent().click();
//     cy.clickByText("Zobowiązanie");
//     cy.get('input[name="isVisible"]').parent().click();
//     cy.clickByText("Ukryte");
//
//     cy.clickByText("Zapisz");
//
//     cy.contains("Edytowane 999");
//     cy.contains("Zobowiązanie");
//     cy.contains("ukryte");
//
//     cy.get("[data-cy=moreMenu]").click();
//     cy.clickByText("Usuń");
//
//     cy.get("[data-cy=moreMenuCancel]").click();
//     cy.contains("Edytowane 999");
//
//     cy.get("[data-cy=moreMenu]").click();
//     cy.clickByText("Usuń");
//     cy.get("[data-cy=moreMenuDelete]").click();
//
//     cy.contains("Edytowane 999").should("not.exist");
//
//     //logout
//     cy.clickProfile();
//     cy.logout();
//   });
//
//   it("networthentries", () => {
//     cy.clickByText("Zaloguj się");
//     cy.login("contact@fintrack.app", "password-very-hard123");
//
//     cy.clickCheckUrl("Składniki", "/networth/parts");
//
//     cy.clickByText("Dodaj");
//     cy.typeInputByName("name", "Gotówka");
//     cy.clickByText("Zapisz");
//
//     cy.clickByText("Dodaj");
//     cy.typeInputByName("name", "Poduszka");
//     cy.get('input[name="isVisible"]').parent().click();
//     cy.clickByText("Ukryte");
//     cy.clickByText("Zapisz");
//
//     cy.clickByText("Dodaj");
//     cy.typeInputByName("name", "Kredyt");
//     cy.get('input[name="type"]').parent().click();
//     cy.clickByText("Zobowiązanie");
//     cy.clickByText("Zapisz");
//
//     cy.clickCheckUrl("Dane", "/networth/data");
//
//     cy.contains("Ukryte kolumny");
//     cy.get("[data-cy=showInfo]").click();
//     cy.contains("Ukryte kolumny").should("not.exist");
//     cy.get("[data-cy=showInfo]").click();
//
//     cy.clickByText("Dodaj");
//     cy.clickByText("Zapisz");
//     cy.contains("To pole jest wymagane");
//
//     cy.get('[data-cy="decimalInput"]').eq(0).type(123);
//     cy.get('[data-cy="decimalInput"]').eq(1).type(666);
//     cy.clickByText("Zapisz");
//
//     cy.contains("123");
//     cy.contains("666");
//     cy.contains("Gotówka");
//     cy.contains("Kredyt");
//     cy.contains("Poduszka").should("not.exist");
//     cy.get("[data-cy=showAll]").click();
//     cy.contains("Poduszka");
//     cy.contains(0);
//     cy.get("[data-cy=showAll]").click();
//     cy.contains("Poduszka").should("not.exist");
//
//     cy.get("[data-cy=moreMenu]").click();
//     cy.clickByText("Edytuj");
//     cy.contains("Edytowanie");
//     cy.clickByText("Zapisz");
//
//     cy.clickCheckUrl("Przegląd", "/networth/dashboard");
//     cy.clickCheckUrl("Dane", "/networth/data");
//
//     cy.get("[data-cy=moreMenu]").click();
//     cy.clickByText("Usuń");
//
//     cy.get("[data-cy=moreMenuCancel]").click();
//     cy.contains("Gotówka");
//
//     cy.get("[data-cy=moreMenu]").click();
//     cy.clickByText("Usuń");
//     cy.get("[data-cy=moreMenuDelete]").click();
//
//     cy.contains("Gotówka").should("not.exist");
//
//     //logout
//     cy.clickProfile();
//     cy.logout();
//   });
// });
