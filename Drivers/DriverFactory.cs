using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Drivers;

public class DriverFactory
{
    private readonly TestConfiguration _config;

    public DriverFactory(TestConfiguration config)
    {
        _config = config ?? throw new ArgumentNullException(nameof(config));
    }

    public IWebDriver CreateDriver()
    {
        try
        {
            var browserType = _config.Browser.BrowserType?.Trim() ?? "Chrome";
            
            return browserType.ToLowerInvariant() switch
            {
                "edge" => CreateEdgeDriver(),
                "chrome" => CreateChromeDriver(),
                _ => throw new NotSupportedException(
                    $"Browser type '{browserType}' is not supported. Supported browsers: Chrome, Edge")
            };
        }
        catch (Exception ex) when (!(ex is NotSupportedException))
        {
            throw new InvalidOperationException(
                $"Failed to create WebDriver: {ex.Message}", ex);
        }
    }

    private IWebDriver CreateChromeDriver()
    {
        var options = CreateBrowserOptions<ChromeOptions>();
        var driver = new ChromeDriver(options);
        ConfigureDriver(driver);
        return driver;
    }

    private IWebDriver CreateEdgeDriver()
    {
        var options = CreateBrowserOptions<EdgeOptions>();
        var driver = new EdgeDriver(options);
        ConfigureDriver(driver);
        return driver;
    }

    private T CreateBrowserOptions<T>() where T : DriverOptions, new()
    {
        var options = new T();
        
        if (_config.Browser.Headless)
        {
            if (options is ChromeOptions chromeOptions)
                chromeOptions.AddArgument("--headless=new");
            else if (options is EdgeOptions edgeOptions)
                edgeOptions.AddArgument("--headless=new");
        }
        
        // Add configurable browser arguments
        if (_config.Browser.BrowserArguments != null && _config.Browser.BrowserArguments.Any())
        {
            foreach (var argument in _config.Browser.BrowserArguments)
            {
                if (!string.IsNullOrWhiteSpace(argument))
                {
                    if (options is ChromeOptions chromeOpts)
                        chromeOpts.AddArgument(argument);
                    else if (options is EdgeOptions edgeOpts)
                        edgeOpts.AddArgument(argument);
                }
            }
        }
        
        return options;
    }

    private void ConfigureDriver(IWebDriver driver)
    {
        // Set page load timeout only - NO implicit waits
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(_config.Browser.PageLoadTimeoutSeconds);
        
        // Explicitly set implicit wait to 0 to avoid conflicts with explicit waits
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
    }
}
