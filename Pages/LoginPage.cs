using OpenQA.Selenium;
using learning_reqnroll_project_vadzim.Configuration;
using learning_reqnroll_project_vadzim.Pages.Components;

namespace learning_reqnroll_project_vadzim.Pages;

public class LoginPage : BasePage
{
    private readonly LoginFormComponent _loginForm;
    private readonly DashboardHeaderComponent _dashboardHeader;

    public LoginPage(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
        _loginForm = new LoginFormComponent(driver, config);
        _dashboardHeader = new DashboardHeaderComponent(driver, config);
    }

    public void NavigateToLoginPage()
    {
        NavigateToUrl(Config.BaseUrl);
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
}
