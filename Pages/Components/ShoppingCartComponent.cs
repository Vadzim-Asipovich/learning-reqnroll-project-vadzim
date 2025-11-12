using OpenQA.Selenium;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Pages.Components;

public class ShoppingCartComponent : BaseComponent
{
    private By CartIcon => By.ClassName("shopping_cart_link");
    private By CartBadge => By.ClassName("shopping_cart_badge");

    public ShoppingCartComponent(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
    }

    public string GetCartItemCount()
    {
        var badge = WaitForElement(CartBadge);
        return badge.Text;
    }

    public bool IsCartEmpty()
    {
        return !IsElementVisible(CartBadge, timeoutSeconds: 2);
    }

    public IWebElement GetCartIcon()
    {
        return WaitForElement(CartIcon);
    }
}

