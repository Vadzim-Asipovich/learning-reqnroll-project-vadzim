using OpenQA.Selenium;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Pages;

public class LoginPage : BasePage
{
    // Locators
    private By UsernameField => By.Id("user-name");
    private By PasswordField => By.Id("password");
    private By LoginButton => By.Id("login-button");
    private By DashboardHeader => By.ClassName("app_logo");

    public LoginPage(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
    }

    public void NavigateToLoginPage()
    {
        NavigateToUrl(Config.BaseUrl);
        WaitForElement(UsernameField);
    }

    public void EnterCredentials(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or empty", nameof(username));
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        SendKeysToElement(UsernameField, username);
        SendKeysToElement(PasswordField, password);
        ClickElement(LoginButton);
    }

    public void VerifyDashboardIsVisible()
    {
        WaitForUrlContains("inventory.html");
        
        var header = WaitForElement(DashboardHeader);
        if (!header.Displayed)
        {
            throw new InvalidOperationException("Dashboard header is not visible after login");
        }
    }
}
