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

    public string GetCartItemCount()
    {
        var badge = WaitForElement(CartBadge);
        return badge.Text;
    }

    public void RemoveItemFromCart()
    {
        ClickElement(RemoveButton);
    }

    public bool IsCartEmpty()
    {
        return !IsElementVisible(CartBadge, timeoutSeconds: 2);
    }
}

