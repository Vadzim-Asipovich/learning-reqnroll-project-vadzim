using OpenQA.Selenium;
using NUnit.Framework;
using learning_reqnroll_project_vadzim.Configuration;
using learning_reqnroll_project_vadzim.Pages.Components;

namespace learning_reqnroll_project_vadzim.Pages;

public class LoginPage : BasePage<LoginPage>
{
    private readonly LoginFormComponent _loginForm;
    private readonly DashboardHeaderComponent _dashboardHeader;
    private By UsernameField => By.Id("user-name");

    public LoginPage(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
        _loginForm = new LoginFormComponent(driver, config);
        _dashboardHeader = new DashboardHeaderComponent(driver, config);
    }

    protected override void Load()
    {
        Driver.Navigate().GoToUrl(Config.BaseUrl);
        _loginForm.WaitForLoginForm();
    }

    public void EnterCredentials(string username, string password)
    {
        _loginForm.EnterCredentials(username, password);
    }

    public IWebElement GetDashboardHeader()
    {
        WaitForUrlContains("inventory.html");
        return _dashboardHeader.GetHeader();
    }

    protected override void IsLoaded()
    {
        Assert.That(Driver.Url.Contains("saucedemo.com"), Is.True, "Login page not loaded!");
        Assert.That(IsElementVisible(UsernameField, 2), Is.True, "Username field not visible on login page!");
    }
}
