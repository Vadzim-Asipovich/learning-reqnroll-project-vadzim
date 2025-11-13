using System;
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
    
    protected T GetLoadedPage<T>() where T : LoadableComponent<T>
    {
        return GetPage<T>().Get();
    }
}
