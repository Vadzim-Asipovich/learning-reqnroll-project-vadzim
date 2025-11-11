Feature: Login
  As a user
  I want to log into the website
  So that I can see my dashboard

  Scenario: Successful login
    Given I am on the login page
    When I enter valid credentials
    Then I should see the dashboard