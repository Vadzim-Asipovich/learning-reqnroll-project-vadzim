using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Pages;

public abstract class BasePage
{
    protected readonly IWebDriver Driver;
    protected readonly TestConfiguration Config;
    protected readonly WebDriverWait Wait;

    protected BasePage(IWebDriver driver, TestConfiguration config)
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

    protected void WaitForUrlContains(string partialUrl, int? timeoutSeconds = null)
    {
        var wait = timeoutSeconds.HasValue 
            ? new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
            : Wait;
        
        wait.Until(driver => driver.Url.Contains(partialUrl));
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

    protected void NavigateToUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be null or empty", nameof(url));
        
        Driver.Navigate().GoToUrl(url);
    }
}
