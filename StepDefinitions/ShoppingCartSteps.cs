using Reqnroll;
using learning_reqnroll_project_vadzim.Pages;

namespace learning_reqnroll_project_vadzim.StepDefinitions;

[Binding]
public class ShoppingCartSteps : BaseSteps
{
    public ShoppingCartSteps(ScenarioContext scenarioContext) : base(scenarioContext)
    {
    }

    private ShoppingCartPage GetShoppingCartPage() => GetPage<ShoppingCartPage>();

    [When("I add the first item to the cart")]
    public void WhenIAddTheFirstItemToTheCart()
    {
        GetShoppingCartPage().AddFirstItemToCart();
    }

    [Then("I should see the item count in the cart icon is {string}")]
    public void ThenIShouldSeeTheItemCountInTheCartIconIs(string expectedCount)
    {
        var actualCount = GetShoppingCartPage().GetCartItemCount();
        Assert.That(actualCount, Is.EqualTo(expectedCount), 
            $"Expected cart count '{expectedCount}' but found '{actualCount}'");
    }

    [Given("I have added an item to the cart")]
    public void GivenIHaveAddedAnItemToTheCart()
    {
        GetShoppingCartPage().AddFirstItemToCart();
    }

    [When("I remove the item from the cart")]
    public void WhenIRemoveTheItemFromTheCart()
    {
        GetShoppingCartPage().RemoveItemFromCart();
    }

    [Then("the cart should be empty")]
    public void ThenTheCartShouldBeEmpty()
    {
        var isCartEmpty = GetShoppingCartPage().IsCartEmpty();
        Assert.That(isCartEmpty, Is.True, "Cart is not empty - badge is still visible");
    }
}

