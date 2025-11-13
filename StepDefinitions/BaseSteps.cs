using System;
using System.Collections.Generic;
using Reqnroll;
using OpenQA.Selenium;
using learning_reqnroll_project_vadzim.Configuration;
using learning_reqnroll_project_vadzim.Pages;

namespace learning_reqnroll_project_vadzim.StepDefinitions;

public abstract class BaseSteps
{
    protected readonly ScenarioContext ScenarioContext;
    protected static readonly TestConfiguration Config = TestConfiguration.Load();

    protected BaseSteps(ScenarioContext scenarioContext)
    {
        ScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
    }

    protected IWebDriver GetDriver()
    {
        if (!ScenarioContext.ContainsKey("WebDriver"))
        {
            throw new InvalidOperationException(
                "WebDriver not found in ScenarioContext. Ensure [BeforeScenario] hook has created the driver.");
        }

        return ScenarioContext.Get<IWebDriver>("WebDriver");
    }

    protected T GetPage<T>() where T : LoadableComponent<T>
    {
        var driver = GetDriver();
        return (T)Activator.CreateInstance(typeof(T), driver, Config)!;
    }

    /// <summary>
    /// Gets a page instance and ensures it's loaded using the LoadableComponent pattern.
    /// </summary>
    protected T GetLoadedPage<T>() where T : LoadableComponent<T>
    {
        return GetPage<T>().Get();
    }

    protected void SetValue<T>(string key, T value)
    {
        ScenarioContext.Set(value, key);
    }

    protected T GetValue<T>(string key)
    {
        if (!ScenarioContext.ContainsKey(key))
        {
            throw new KeyNotFoundException($"Key '{key}' not found in ScenarioContext.");
        }

        return ScenarioContext.Get<T>(key);
    }

    protected bool HasValue(string key)
    {
        return ScenarioContext.ContainsKey(key);
    }
}
