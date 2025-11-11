using System;
using System.IO;
using Reqnroll;
using OpenQA.Selenium;
using NUnit.Framework;
using AventStack.ExtentReports;
using learning_reqnroll_project_vadzim.Configuration;
using learning_reqnroll_project_vadzim.Drivers;
using learning_reqnroll_project_vadzim.Reports;

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

    [BeforeTestRun]
    public static void BeforeTestRun()
    {
        var config = TestConfiguration.Load();
        ExtentReportsManager.GetInstance(config.Report);
    }

    [AfterTestRun]
    public static void AfterTestRun()
    {
        ExtentReportsManager.Flush();
    }

    [BeforeScenario]
    public void BeforeScenario()
    {
        var scenarioTitle = _scenarioContext.ScenarioInfo.Title;
        var scenarioDescription = _scenarioContext.ScenarioInfo.Description;
        var extentTest = ExtentReportsManager.CreateTest(scenarioTitle, scenarioDescription);
        _scenarioContext.Set(extentTest, "ExtentTest");

        IWebDriver? driver = null;
        try
        {
            var driverFactory = new DriverFactory(Config);
            driver = driverFactory.CreateDriver();
            _scenarioContext.Set(driver, "WebDriver");
            _scenarioContext.Set(Config, "TestConfiguration");

            var test = ExtentReportsManager.GetTest();
            test.Log(Status.Info, $"Starting scenario: {scenarioTitle}");
        }
        catch (Exception ex)
        {
            var test = ExtentReportsManager.GetTest();
            test.Log(Status.Fatal, $"Failed to create WebDriver: {ex.Message}");
            TestContext.Out.WriteLine($"Failed to create WebDriver: {ex.Message}");
            driver?.Dispose();
            throw;
        }
    }

    [AfterScenario]
    public void AfterScenario()
    {
        var testResult = TestContext.CurrentContext.Result;
        var status = testResult.Outcome.Status.ToString();
        var errorMessage = testResult.Message;
        var stackTrace = testResult.StackTrace;

        ExtentTest? extentTest = null;
        try
        {
            if (_scenarioContext.ContainsKey("ExtentTest"))
            {
                extentTest = _scenarioContext.Get<ExtentTest>("ExtentTest");
            }
        }
        catch
        {
        }

        IWebDriver? driver = null;
        try
        {
            if (_scenarioContext.ContainsKey("WebDriver"))
            {
                driver = _scenarioContext.Get<IWebDriver>("WebDriver");
                
                if (status != "Passed")
                {
                    var screenshotPath = TakeScreenshot(driver, _scenarioContext.ScenarioInfo.Title);
                    if (extentTest != null && !string.IsNullOrEmpty(screenshotPath))
                    {
                        try
                        {
                            extentTest.Fail("Screenshot captured: " + 
                                extentTest.AddScreenCaptureFromPath(screenshotPath));
                        }
                        catch
                        {
                            extentTest.Log(Status.Fail, $"Screenshot captured: {screenshotPath}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            TestContext.Out.WriteLine($"Error during test cleanup: {ex.Message}");
            if (extentTest != null)
            {
                extentTest.Log(Status.Warning, $"Error during cleanup: {ex.Message}");
            }
        }
        finally
        {
            if (extentTest != null)
            {
                var extentStatus = status switch
                {
                    "Passed" => Status.Pass,
                    "Failed" => Status.Fail,
                    "Skipped" => Status.Skip,
                    _ => Status.Warning
                };

                extentTest.Log(extentStatus, $"Test {status}");

                if (!string.IsNullOrEmpty(errorMessage))
                    extentTest.Log(Status.Error, $"Error: {errorMessage}");

                if (!string.IsNullOrEmpty(stackTrace))
                    extentTest.Log(Status.Error, $"Stack Trace: {stackTrace}");
            }

            try
            {
                driver?.Dispose();
            }
            catch (Exception ex)
            {
                TestContext.Out.WriteLine($"Error disposing WebDriver: {ex.Message}");
            }

            ExtentReportsManager.EndTest();
        }
    }

    private string? TakeScreenshot(IWebDriver driver, string scenarioTitle)
    {
        try
        {
            if (driver is ITakesScreenshot takesScreenshot)
            {
                var screenshot = takesScreenshot.GetScreenshot();
                var fileName = $"Screenshot_{scenarioTitle}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                fileName = string.Join("_", fileName.Split(Path.GetInvalidFileNameChars()));
                
                var screenshotsDir = GetScreenshotsDirectory();
                Directory.CreateDirectory(screenshotsDir);
                
                var filePath = Path.Combine(screenshotsDir, fileName);
                screenshot.SaveAsFile(filePath);
                
                TestContext.AddTestAttachment(filePath);
                TestContext.Out.WriteLine($"Screenshot saved: {filePath}");
                return filePath;
            }
        }
        catch (Exception ex)
        {
            TestContext.Out.WriteLine($"Failed to take screenshot: {ex.Message}");
        }
        return null;
    }

    private static string GetScreenshotsDirectory()
    {
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
        }

        var currentDir = Directory.GetCurrentDirectory();
        return Path.Combine(currentDir, "Screenshots");
    }
}