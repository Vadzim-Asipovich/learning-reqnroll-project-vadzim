using OpenQA.Selenium;
using NUnit.Framework;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Pages.Components;

public class ShoppingCartComponent : BaseComponent<ShoppingCartComponent>
{
    private By CartIcon => By.ClassName("shopping_cart_link");
    private By CartBadge => By.ClassName("shopping_cart_badge");

    public ShoppingCartComponent(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
    }

    protected override void Load()
    {
        // Component is loaded when the page is loaded, so we just wait for the cart icon
        WaitForElement(CartIcon);
    }

    protected override void IsLoaded()
    {
        Assert.That(IsElementVisible(CartIcon, 2), Is.True, "Cart icon not visible!");
    }

    public string GetCartItemCount()
    {
        var badge = WaitForElement(CartBadge);
        return badge.Text;
    }

    public bool IsCartEmpty()
    {
        // Check if badge exists and is visible, or if it doesn't exist at all
        try
        {
            var badge = Driver.FindElement(CartBadge);
            return !badge.Displayed;
        }
        catch (OpenQA.Selenium.NoSuchElementException)
        {
            // Badge doesn't exist, cart is empty
            return true;
        }
    }

    public IWebElement GetCartIcon()
    {
        return WaitForElement(CartIcon);
    }
}

