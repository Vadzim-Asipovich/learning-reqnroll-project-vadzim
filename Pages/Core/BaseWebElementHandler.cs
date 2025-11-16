using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Pages.Core;

/// <summary>
/// Base class providing common WebDriver element interaction methods with stale element handling.
/// This class contains shared functionality used by both BasePage and BaseComponent.
/// </summary>
public abstract class BaseWebElementHandler<T> : LoadableComponent<T> where T : LoadableComponent<T>
{
    protected readonly IWebDriver Driver;
    protected readonly TestConfiguration Config;
    protected readonly WebDriverWait Wait;

    protected BaseWebElementHandler(IWebDriver driver, TestConfiguration config) : base(driver)
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
        RetryStaleElement(() =>
        {
            var element = WaitForElementClickable(locator);
            element.Click();
        });
    }

    protected void SendKeysToElement(By locator, string text, bool clearFirst = true)
    {
        RetryStaleElement(() =>
        {
            var element = WaitForElement(locator);
            if (clearFirst)
            {
                element.Clear();
            }
            element.SendKeys(text);
        });
    }

    protected bool IsElementVisible(By locator, int timeoutSeconds = 2)
    {
        try
        {
            var wait = new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds));
            wait.Until(driver =>
            {
                try
                {
                    var element = driver.FindElement(locator);
                    return element.Displayed;
                }
                catch (StaleElementReferenceException)
                {
                    // Retry by returning false, which will cause the wait to retry
                    return false;
                }
            });
            return true;
        }
        catch (WebDriverTimeoutException)
        {
            return false;
        }
    }

    /// <summary>
    /// Retries an action when StaleElementReferenceException occurs.
    /// This handles cases where the DOM changes between finding and using an element.
    /// </summary>
    /// <param name="action">The action to perform</param>
    /// <param name="maxRetries">Maximum number of retry attempts (default: 3)</param>
    protected void RetryStaleElement(Action action, int maxRetries = 3)
    {
        var attempts = 0;
        while (attempts < maxRetries)
        {
            try
            {
                action();
                return;
            }
            catch (StaleElementReferenceException)
            {
                attempts++;
                if (attempts >= maxRetries)
                {
                    throw;
                }
                // Small delay before retry to allow DOM to stabilize
                System.Threading.Thread.Sleep(100);
            }
        }
    }

    /// <summary>
    /// Retries a function when StaleElementReferenceException occurs.
    /// This handles cases where the DOM changes between finding and using an element.
    /// </summary>
    /// <typeparam name="TResult">The return type of the function</typeparam>
    /// <param name="func">The function to execute</param>
    /// <param name="maxRetries">Maximum number of retry attempts (default: 3)</param>
    /// <returns>The result of the function</returns>
    protected TResult RetryStaleElement<TResult>(Func<TResult> func, int maxRetries = 3)
    {
        var attempts = 0;
        while (attempts < maxRetries)
        {
            try
            {
                return func();
            }
            catch (StaleElementReferenceException)
            {
                attempts++;
                if (attempts >= maxRetries)
                {
                    throw;
                }
                // Small delay before retry to allow DOM to stabilize
                System.Threading.Thread.Sleep(100);
            }
        }
        throw new InvalidOperationException("Retry logic failed unexpectedly");
    }
}

