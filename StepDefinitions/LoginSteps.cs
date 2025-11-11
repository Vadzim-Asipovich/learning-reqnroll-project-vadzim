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
        GetLoginPage().NavigateToLoginPage();
    }

    [When("I enter valid credentials")]
    public void WhenIEnterValidCredentials()
    {
        GetLoginPage().EnterCredentials(Config.Credentials.Username, Config.Credentials.Password);
    }

    [Then("I should see the dashboard")]
    public void ThenIShouldSeeTheDashboard()
    {
        GetLoginPage().VerifyDashboardIsVisible();
    }

    [Given("I am logged in")]
    public void GivenIAmLoggedIn()
    {
        GetLoginPage().NavigateToLoginPage();
        GetLoginPage().EnterCredentials(Config.Credentials.Username, Config.Credentials.Password);
        GetLoginPage().VerifyDashboardIsVisible();
    }
}
