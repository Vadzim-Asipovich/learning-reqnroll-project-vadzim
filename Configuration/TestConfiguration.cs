using System.Reflection;
using Microsoft.Extensions.Configuration;

namespace learning_reqnroll_project_vadzim.Configuration;

public class TestConfiguration
{
    public string BaseUrl { get; set; } = string.Empty;
    public Credentials Credentials { get; set; } = new();
    public BrowserSettings Browser { get; set; } = new();

    private static TestConfiguration? _instance;
    private static readonly object _lock = new object();

    public static TestConfiguration Load()
    {
        if (_instance != null)
            return _instance;

        lock (_lock)
        {
            if (_instance != null)
                return _instance;

            var basePath = GetBasePath();
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            var configuration = builder.Build();

            var testConfig = new TestConfiguration();
            configuration.Bind(testConfig);
            
            // Validate configuration
            testConfig.Validate();
            
            _instance = testConfig;
            return _instance;
        }
    }

    private static string GetBasePath()
    {
        // Try AppContext.BaseDirectory first (works in most scenarios)
        var baseDirectory = AppContext.BaseDirectory;
        if (!string.IsNullOrEmpty(baseDirectory) && Directory.Exists(baseDirectory))
        {
            return baseDirectory;
        }

        // Fallback to executing assembly location
        var assemblyLocation = Assembly.GetExecutingAssembly().Location;
        if (!string.IsNullOrEmpty(assemblyLocation))
        {
            return Path.GetDirectoryName(assemblyLocation) ?? Directory.GetCurrentDirectory();
        }

        // Final fallback
        return Directory.GetCurrentDirectory();
    }

    private void Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(BaseUrl))
        {
            errors.Add("BaseUrl is required and cannot be empty");
        }
        else if (!Uri.TryCreate(BaseUrl, UriKind.Absolute, out _))
        {
            errors.Add($"BaseUrl '{BaseUrl}' is not a valid URL");
        }

        if (Credentials == null)
        {
            errors.Add("Credentials configuration is required");
        }
        else
        {
            if (string.IsNullOrWhiteSpace(Credentials.Username))
                errors.Add("Credentials.Username is required");
            
            if (string.IsNullOrWhiteSpace(Credentials.Password))
                errors.Add("Credentials.Password is required");
        }

        if (Browser == null)
        {
            errors.Add("Browser configuration is required");
        }
        else
        {
            if (Browser.ExplicitWaitSeconds <= 0)
                errors.Add("Browser.ExplicitWaitSeconds must be greater than 0");
            
            if (Browser.PageLoadTimeoutSeconds <= 0)
                errors.Add("Browser.PageLoadTimeoutSeconds must be greater than 0");
            
            var validBrowsers = new[] { "Chrome", "Edge" };
            if (!validBrowsers.Contains(Browser.BrowserType, StringComparer.OrdinalIgnoreCase))
                errors.Add($"Browser.BrowserType must be one of: {string.Join(", ", validBrowsers)}");
        }

        if (errors.Any())
        {
            throw new InvalidOperationException(
                "Configuration validation failed:\n" + string.Join("\n", errors));
        }
    }
}

public class Credentials
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class BrowserSettings
{
    public string BrowserType { get; set; } = "Chrome";
    public bool Headless { get; set; }
    public int ExplicitWaitSeconds { get; set; } = 10;
    public int PageLoadTimeoutSeconds { get; set; } = 30;
    public List<string> BrowserArguments { get; set; } = new();
}
