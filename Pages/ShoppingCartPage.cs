using System;
using OpenQA.Selenium;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Pages;

public class ShoppingCartPage : BasePage
{
    private By FirstAddToCartButton => By.Id("add-to-cart-sauce-labs-backpack");
    private By CartIcon => By.ClassName("shopping_cart_link");
    private By CartBadge => By.ClassName("shopping_cart_badge");
    private By RemoveButton => By.Id("remove-sauce-labs-backpack");

    public ShoppingCartPage(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
    }

    public void AddFirstItemToCart()
    {
        ClickElement(FirstAddToCartButton);
    }

    public void VerifyCartItemCount(string expectedCount)
    {
        var badge = WaitForElement(CartBadge);
        var actualCount = badge.Text;
        if (actualCount != expectedCount)
        {
            throw new InvalidOperationException(
                $"Expected cart count '{expectedCount}' but found '{actualCount}'");
        }
    }

    public void RemoveItemFromCart()
    {
        ClickElement(RemoveButton);
    }

    public void VerifyCartIsEmpty()
    {
        try
        {
            var badge = Driver.FindElement(CartBadge);
            throw new InvalidOperationException("Cart is not empty - badge is still visible");
        }
        catch (NoSuchElementException)
        {
        }
    }
}

