Feature: Shopping Cart
  As a logged-in customer
  I want to manage items in my shopping cart
  So that I can complete my purchase

  Scenario: Add item to cart
    Given I am logged in
    When I add the first item to the cart
    Then I should see the item count in the cart icon is "1"

  Scenario: Remove item from cart
    Given I am logged in
    And I have added an item to the cart
    When I remove the item from the cart
    Then the cart should be empty

