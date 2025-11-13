using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using NUnit.Framework;
using learning_reqnroll_project_vadzim.Configuration;
using learning_reqnroll_project_vadzim.Pages.Core;
using learning_reqnroll_project_vadzim.Pages.Components;

namespace learning_reqnroll_project_vadzim.Pages;

public class ShoppingCartPage : BasePage<ShoppingCartPage>
{
    private By FirstAddToCartButton => By.Id("add-to-cart-sauce-labs-backpack");
    private By RemoveButton => By.Id("remove-sauce-labs-backpack");
    private By InventoryContainer => By.Id("inventory_container");
    private readonly ShoppingCartComponent _shoppingCart;

    public ShoppingCartPage(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
        _shoppingCart = new ShoppingCartComponent(driver, config);
    }

    public void AddFirstItemToCart()
    {
        ClickElement(FirstAddToCartButton);
    }

    public string GetCartItemCount()
    {
        return _shoppingCart.Get().GetCartItemCount();
    }

    public void RemoveItemFromCart()
    {
        ClickElement(RemoveButton);
        // Wait for the badge to be removed from DOM after removing the item
        var badgeLocator = By.ClassName("shopping_cart_badge");
        var shortWait = new WebDriverWait(Driver, TimeSpan.FromSeconds(5));
        shortWait.Until(driver =>
        {
            try
            {
                driver.FindElement(badgeLocator);
                return false; // Badge still exists
            }
            catch (OpenQA.Selenium.NoSuchElementException)
            {
                return true; // Badge removed from DOM, cart is empty
            }
        });
    }

    public bool IsCartEmpty()
    {
        // Component should already be loaded, just check if badge is visible
        return _shoppingCart.IsCartEmpty();
    }

    protected override void Load()
    {
        // Only navigate if we're not already on the inventory page
        if (!Driver.Url.Contains("inventory"))
        {
            Driver.Navigate().GoToUrl(Config.BaseUrl + "inventory.html");
        }
        WaitForElement(InventoryContainer);
        _shoppingCart.Get();
    }

    protected override void IsLoaded()
    {
        Assert.That(Driver.Url.Contains("inventory"), Is.True, "Shopping cart page not loaded!");
        Assert.That(IsElementVisible(InventoryContainer, 2), Is.True, "Inventory container not visible!");
    }
}

