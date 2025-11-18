using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using learning_reqnroll_project_vadzim.Configuration;
using learning_reqnroll_project_vadzim.Pages.Core;

namespace learning_reqnroll_project_vadzim.Pages.Core;

public abstract class BasePage<T> : BaseWebElementHandler<T> where T : LoadableComponent<T>
{
    protected BasePage(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
    }

    protected void WaitForUrlContains(string partialUrl, int? timeoutSeconds = null)
    {
        var wait = timeoutSeconds.HasValue 
            ? new WebDriverWait(Driver, TimeSpan.FromSeconds(timeoutSeconds.Value))
            : Wait;
        
        wait.Until(driver => driver.Url.Contains(partialUrl));
    }

    protected void NavigateToUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            throw new ArgumentException("URL cannot be null or empty", nameof(url));
        
        Driver.Navigate().GoToUrl(url);
    }
}

