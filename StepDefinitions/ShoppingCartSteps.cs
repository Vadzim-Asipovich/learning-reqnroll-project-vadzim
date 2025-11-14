using Reqnroll;
using learning_reqnroll_project_vadzim.Pages;

namespace learning_reqnroll_project_vadzim.StepDefinitions;

[Binding]
public class ShoppingCartSteps : BaseSteps
{
    public ShoppingCartSteps(ScenarioContext scenarioContext) : base(scenarioContext)
    {
    }

    private ShoppingCartPage GetLoadedShoppingCartPage() => GetLoadedPage<ShoppingCartPage>();

    [When("I add the first item to the cart")]
    public void WhenIAddTheFirstItemToTheCart()
    {
        GetLoadedShoppingCartPage().AddFirstItemToCart();
    }

    [Then("I should see the item count in the cart icon is {string}")]
    public void ThenIShouldSeeTheItemCountInTheCartIconIs(string expectedCount)
    {
        var actualCount = GetLoadedShoppingCartPage().GetCartItemCount();
        Assert.That(actualCount, Is.EqualTo(expectedCount), 
            $"Expected cart count '{expectedCount}' but found '{actualCount}'");
    }

    [Given("I have added an item to the cart")]
    public void GivenIHaveAddedAnItemToTheCart()
    {
        GetLoadedShoppingCartPage().AddFirstItemToCart();
    }

    [When("I remove the item from the cart")]
    public void WhenIRemoveTheItemFromTheCart()
    {
        GetLoadedShoppingCartPage().RemoveItemFromCart();
    }

    [Then("the cart should be empty")]
    public void ThenTheCartShouldBeEmpty()
    {
        var isCartEmpty = GetLoadedShoppingCartPage().IsCartEmpty();
        Assert.That(isCartEmpty, Is.True, "Cart is not empty - badge is still visible");
    }
}

