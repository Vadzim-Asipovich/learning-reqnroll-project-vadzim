using System;
using OpenQA.Selenium;
using learning_reqnroll_project_vadzim.Configuration;
using learning_reqnroll_project_vadzim.Pages.Core;

namespace learning_reqnroll_project_vadzim.Pages.Core;

public abstract class BaseComponent<T> : BaseWebElementHandler<T> where T : BaseComponent<T>
{
    protected BaseComponent(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
    }

    protected new void SendKeysToElement(By locator, string text, bool clearFirst = true)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("Key cannot be null or empty", nameof(text));

        base.SendKeysToElement(locator, text, clearFirst);
    }
}

