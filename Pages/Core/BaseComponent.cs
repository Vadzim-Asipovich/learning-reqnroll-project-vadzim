using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using learning_reqnroll_project_vadzim.Configuration;
using learning_reqnroll_project_vadzim.Pages.Core;

namespace learning_reqnroll_project_vadzim.Pages.Core;

public abstract class BaseComponent<T> : LoadableComponent<T> where T : BaseComponent<T>
{
    protected readonly IWebDriver Driver;
    protected readonly TestConfiguration Config;
    protected readonly WebDriverWait Wait;

    protected BaseComponent(IWebDriver driver, TestConfiguration config) : base(driver)
    {
        Driver = driver ?? throw new ArgumentNullException(nameof(driver));
        Config = config ?? throw new ArgumentNullException(nameof(config));
        
        var explicitWaitSeconds = config.Browser.ExplicitWaitSeconds > 0 
            ? config.Browser.ExplicitWaitSeconds 
            : 10;
        Wait = new WebDriverWait(driver, TimeSpan.FromSeconds(explicitWaitSeconds));
    }

    protected IWebElement WaitForElement(By locator, int? timeoutSeconds = null)
    {
        return WaitForElement(locator, timeoutSeconds, e => e.Displayed && e.Enabled);
    }

    protected IWebElement WaitForElementClickable(By locator, int? timeoutSeconds = null)
    {
        return WaitForElement(locator, timeoutSeconds, e => e.Displayed && e.Enabled);
    }

    private IWebElement WaitForElement(By locator, int? timeoutSeconds, Func<IWebElement, bool> condition)
    {
        var wait = timeoutSeconds.HasValue 
            ? new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
            : Wait;
        
        return wait.Until(driver =>
        {
            var element = driver.FindElement(locator);
            return condition(element) ? element : null;
        });
    }

    protected void ClickElement(By locator)
    {
        var element = WaitForElementClickable(locator);
        element.Click();
    }

    protected void SendKeysToElement(By locator, string text, bool clearFirst = true)
    {
        var element = WaitForElement(locator);
        if (clearFirst)
        {
            element.Clear();
        }
        element.SendKeys(text);
    }

    protected bool IsElementVisible(By locator, int timeoutSeconds = 2)
    {
        try
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(driver => driver.FindElement(locator).Displayed);
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }
}

