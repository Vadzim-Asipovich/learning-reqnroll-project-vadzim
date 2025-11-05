using Reqnroll;
using OpenQA.Selenium;
using NUnit.Framework;
using learning_reqnroll_project_vadzim.Configuration;
using learning_reqnroll_project_vadzim.Drivers;

namespace learning_reqnroll_project_vadzim.Hooks;

[Binding]
public class Hooks
{
    private readonly ScenarioContext _scenarioContext;
    private static readonly TestConfiguration Config = TestConfiguration.Load();

    public Hooks(ScenarioContext scenarioContext)
    {
        _scenarioContext = scenarioContext;
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        IWebDriver? driver = null;
        try
        {
            var driverFactory = new DriverFactory(Config);
            driver = driverFactory.CreateDriver();
            _scenarioContext.Set(driver, "WebDriver");
            _scenarioContext.Set(Config, "TestConfiguration");
        }
        catch (Exception ex)
        {
            TestContext.Out.WriteLine($"Failed to create WebDriver: {ex.Message}");
            driver?.Dispose();
            throw;
        }
    }

    [AfterScenario]
    public void AfterScenario()
    {
        if (!_scenarioContext.ContainsKey("WebDriver"))
            return;

        IWebDriver? driver = null;
        try
        {
            driver = _scenarioContext.Get<IWebDriver>("WebDriver");
            
            // Take screenshot on failure
            var testResult = TestContext.CurrentContext.Result;
            var status = testResult.Outcome.Status.ToString();
            if (status != "Passed")
            {
                TakeScreenshot(driver, _scenarioContext.ScenarioInfo.Title);
            }
        }
        catch (Exception ex)
        {
            TestContext.Out.WriteLine($"Error during test cleanup: {ex.Message}");
        }
        finally
        {
            // Proper cleanup - Dispose() calls Quit() internally
            try
            {
                driver?.Dispose();
            }
            catch (Exception ex)
            {
                TestContext.Out.WriteLine($"Error disposing WebDriver: {ex.Message}");
            }
        }
    }

    private void TakeScreenshot(IWebDriver driver, string scenarioTitle)
    {
        try
        {
            if (driver is ITakesScreenshot takesScreenshot)
            {
                var screenshot = takesScreenshot.GetScreenshot();
                var fileName = $"Screenshot_{scenarioTitle}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
                
                // More robust path handling
                var screenshotsDir = GetScreenshotsDirectory();
                Directory.CreateDirectory(screenshotsDir);
                
                var filePath = Path.Combine(screenshotsDir, fileName);
                screenshot.SaveAsFile(filePath);
                
                TestContext.AddTestAttachment(filePath);
                TestContext.Out.WriteLine($"Screenshot saved: {filePath}");
            }
        }
        catch (Exception ex)
        {
            TestContext.Out.WriteLine($"Failed to take screenshot: {ex.Message}");
        }
    }

    private static string GetScreenshotsDirectory()
    {
        // Try test directory first
        try
        {
            var testDir = TestContext.CurrentContext.TestDirectory;
            if (!string.IsNullOrEmpty(testDir) && Directory.Exists(testDir))
            {
                return Path.Combine(testDir, "Screenshots");
            }
        }
        catch
        {
            // Fall through to next option
        }

        // Fallback to current directory
        var currentDir = Directory.GetCurrentDirectory();
        return Path.Combine(currentDir, "Screenshots");
    }
}