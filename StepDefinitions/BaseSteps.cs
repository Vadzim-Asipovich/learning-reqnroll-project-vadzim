using Reqnroll;
using OpenQA.Selenium;
using learning_reqnroll_project_vadzim.Configuration;
using learning_reqnroll_project_vadzim.Pages;

namespace learning_reqnroll_project_vadzim.StepDefinitions;

/// <summary>
/// Base class for all step definitions.
/// Provides common functionality for accessing WebDriver, Configuration, and creating page objects.
/// </summary>
public abstract class BaseSteps
{
    protected readonly ScenarioContext ScenarioContext;
    protected static readonly TestConfiguration Config = TestConfiguration.Load();

    protected BaseSteps(ScenarioContext scenarioContext)
    {
        ScenarioContext = scenarioContext ?? throw new ArgumentNullException(nameof(scenarioContext));
    }

    /// <summary>
    /// Gets the WebDriver instance from ScenarioContext.
    /// </summary>
    /// <returns>The IWebDriver instance</returns>
    /// <exception cref="InvalidOperationException">Thrown if WebDriver is not found in ScenarioContext</exception>
    protected IWebDriver GetDriver()
    {
        if (!ScenarioContext.ContainsKey("WebDriver"))
        {
            throw new InvalidOperationException(
                "WebDriver not found in ScenarioContext. Ensure [BeforeScenario] hook has created the driver.");
        }

        return ScenarioContext.Get<IWebDriver>("WebDriver");
    }

    /// <summary>
    /// Creates a page object instance. Each call creates a new instance to ensure fresh state.
    /// </summary>
    /// <typeparam name="T">The page object type (must inherit from BasePage)</typeparam>
    /// <returns>A new instance of the page object</returns>
    protected T GetPage<T>() where T : BasePage
    {
        var driver = GetDriver();
        return (T)Activator.CreateInstance(typeof(T), driver, Config)!;
    }

    /// <summary>
    /// Stores a value in ScenarioContext for sharing data between steps.
    /// </summary>
    /// <typeparam name="T">The type of value to store</typeparam>
    /// <param name="key">The key to store the value under</param>
    /// <param name="value">The value to store</param>
    protected void SetValue<T>(string key, T value)
    {
        ScenarioContext.Set(value, key);
    }

    /// <summary>
    /// Gets a value from ScenarioContext.
    /// </summary>
    /// <typeparam name="T">The type of value to retrieve</typeparam>
    /// <param name="key">The key to retrieve the value for</param>
    /// <returns>The stored value</returns>
    /// <exception cref="KeyNotFoundException">Thrown if the key is not found</exception>
    protected T GetValue<T>(string key)
    {
        if (!ScenarioContext.ContainsKey(key))
        {
            throw new KeyNotFoundException($"Key '{key}' not found in ScenarioContext.");
        }

        return ScenarioContext.Get<T>(key);
    }

    /// <summary>
    /// Checks if a key exists in ScenarioContext.
    /// </summary>
    /// <param name="key">The key to check</param>
    /// <returns>True if the key exists, false otherwise</returns>
    protected bool HasValue(string key)
    {
        return ScenarioContext.ContainsKey(key);
    }
}
