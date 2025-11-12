using OpenQA.Selenium;
using learning_reqnroll_project_vadzim.Configuration;
using learning_reqnroll_project_vadzim.Pages.Components;

namespace learning_reqnroll_project_vadzim.Pages;

public class ShoppingCartPage : BasePage
{
    private By FirstAddToCartButton => By.Id("add-to-cart-sauce-labs-backpack");
    private By RemoveButton => By.Id("remove-sauce-labs-backpack");
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
        return _shoppingCart.GetCartItemCount();
    }

    public void RemoveItemFromCart()
    {
        ClickElement(RemoveButton);
    }

    public bool IsCartEmpty()
    {
        return _shoppingCart.IsCartEmpty();
    }
}

