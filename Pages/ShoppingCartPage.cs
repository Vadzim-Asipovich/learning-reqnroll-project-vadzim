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

    }

    public bool IsCartEmpty()
    {
        return _shoppingCart.IsCartEmpty();
    }

    protected override void Load()
    {
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

