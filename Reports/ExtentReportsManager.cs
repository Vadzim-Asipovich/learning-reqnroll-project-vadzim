using System;
using System.IO;
using System.Threading;
using AventStack.ExtentReports;
using AventStack.ExtentReports.Reporter;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Reports;

public class ExtentReportsManager
{
    private static ExtentReports? _extent;
    private static readonly object _lock = new object();
    private static ThreadLocal<ExtentTest?> _test = new ThreadLocal<ExtentTest?>();

    public static ExtentReports GetInstance(ReportSettings reportSettings)
    {
        if (_extent != null)
            return _extent;

        lock (_lock)
        {
            if (_extent != null)
                return _extent;

            _extent = new ExtentReports();

            var reportPath = GetReportPath(reportSettings.ReportPath);
            Directory.CreateDirectory(Path.GetDirectoryName(reportPath) ?? "Reports");

            var htmlReporter = new ExtentHtmlReporter(reportPath);

            htmlReporter.Config.Theme = reportSettings.Theme == "Dark"
                ? AventStack.ExtentReports.Reporter.Configuration.Theme.Dark
                : AventStack.ExtentReports.Reporter.Configuration.Theme.Standard;
            htmlReporter.Config.DocumentTitle = reportSettings.DocumentTitle;
            htmlReporter.Config.ReportName = reportSettings.ReportName;
            htmlReporter.Config.EnableTimeline = reportSettings.EnableTimeline;

            _extent.AttachReporter(htmlReporter);

            _extent.AddSystemInfo("OS", Environment.OSVersion.ToString());
            _extent.AddSystemInfo("Machine Name", Environment.MachineName);
            _extent.AddSystemInfo("User Name", Environment.UserName);
            _extent.AddSystemInfo(".NET Version", Environment.Version.ToString());

            return _extent;
        }
    }

    public static ExtentTest CreateTest(string testName, string? description = null)
    {
        if (_extent == null)
            throw new InvalidOperationException("ExtentReports instance not initialized. Call GetInstance() first.");

        var test = _extent.CreateTest(testName, description);
        _test.Value = test;
        return test;
    }

    public static ExtentTest GetTest()
    {
        return _test.Value ?? throw new InvalidOperationException("No active test found. Call CreateTest() first.");
    }

    public static void Flush()
    {
        _extent?.Flush();
    }

    public static void EndTest()
    {
        _test.Value = null;
    }

    private static string GetReportPath(string? configuredPath)
    {
        if (configuredPath is not null && configuredPath.Trim().Length == 0)
        {
            throw new ArgumentException("Configured report path cannot be an empty or whitespace string.", nameof(configuredPath));
        }
        if (!string.IsNullOrWhiteSpace(configuredPath))
        {
            return configuredPath!;
        }

        var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
        var reportsDir = Path.Combine(Directory.GetCurrentDirectory(), "Reports");
        return Path.Combine(reportsDir, $"TestReport_{timestamp}.html");
    }
}

