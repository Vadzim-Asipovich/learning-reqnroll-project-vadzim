using System;
using System.Collections.Generic;
using System.Linq;
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
        ConfigureChromePreferences(options);
        var driver = new ChromeDriver(options);
        ConfigureDriver(driver);
        return driver;
    }

    private void ConfigureChromePreferences(ChromeOptions options)
    {
        // Disable password manager and password change popups
        options.AddUserProfilePreference("credentials_enable_service", false);
        options.AddUserProfilePreference("password_manager_enabled", false);
        options.AddUserProfilePreference("profile.password_manager_enabled", false);
        
        // Disable Chrome's password leak detection and security warnings
        options.AddUserProfilePreference("profile.default_content_setting_values.notifications", 2);
        
        // Additional arguments to suppress password-related dialogs
        options.AddArgument("--disable-password-manager-reauthentication");
        options.AddArgument("--disable-features=PasswordChange");
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
            AddArgument(options, "--headless=new");
        }
        
        if (_config.Browser.BrowserArguments != null && _config.Browser.BrowserArguments.Any())
        {
            foreach (var argument in _config.Browser.BrowserArguments)
            {
                if (!string.IsNullOrWhiteSpace(argument))
                {
                    AddArgument(options, argument);
                }
            }
        }
        
        return options;
    }

    private void AddArgument(DriverOptions options, string argument)
    {
        if (options is ChromeOptions chromeOptions)
            chromeOptions.AddArgument(argument);
        else if (options is EdgeOptions edgeOptions)
            edgeOptions.AddArgument(argument);
    }

    private void ConfigureDriver(IWebDriver driver)
    {
        driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(_config.Browser.PageLoadTimeoutSeconds);
        driver.Manage().Timeouts().ImplicitWait = TimeSpan.Zero;
    }
}
