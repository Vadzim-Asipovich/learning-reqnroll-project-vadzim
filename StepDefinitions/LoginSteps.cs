using Reqnroll;
using learning_reqnroll_project_vadzim.Pages;

namespace learning_reqnroll_project_vadzim.StepDefinitions;

[Binding]
public class LoginSteps : BaseSteps
{
    public LoginSteps(ScenarioContext scenarioContext) : base(scenarioContext)
    {
    }

    private LoginPage GetLoginPage() => GetPage<LoginPage>();

    [Given("I am on the login page")]
    public void GivenIAmOnTheLoginPage()
    {
        GetLoginPage().Get();
    }

    [When("I enter valid credentials")]
    public void WhenIEnterValidCredentials()
    {
        GetLoginPage().Get().EnterCredentials(Config.Credentials.Username, Config.Credentials.Password);
    }

    [Then("I should see the dashboard")]
    public void ThenIShouldSeeTheDashboard()
    {
        // After login, we're on the inventory page, so don't reload the login page
        var loginPage = GetLoginPage();
        var header = loginPage.GetDashboardHeader();
        Assert.That(header.Displayed, Is.True, "Dashboard header is not visible after login");
    }

    [Given("I am logged in")]
    public void GivenIAmLoggedIn()
    {
        var loginPage = GetLoginPage().Get();
        loginPage.EnterCredentials(Config.Credentials.Username, Config.Credentials.Password);
        var header = loginPage.GetDashboardHeader();
        Assert.That(header.Displayed, Is.True, "Dashboard header is not visible after login");
    }
}
