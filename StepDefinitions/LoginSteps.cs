using Reqnroll;
using NUnit.Framework;
using WebTestProject.Pages;

namespace WebTestProject.StepDefinitions;

[Binding]
public class LoginSteps
{
    private readonly LoginPage _loginPage;

    public LoginSteps()
    {
        _loginPage = new LoginPage();
    }

    [Given("I am on the login page")]
    public void GivenIAmOnTheLoginPage()
    {
        _loginPage.NavigateToLoginPage();
    }

    [When("I enter valid credentials")]
    public void WhenIEnterValidCredentials()
    {
        _loginPage.EnterCredentials("standard_user", "secret_sauce");
    }

    [Then("I should see the dashboard")]
    public void ThenIShouldSeeTheDashboard()
    {
        Assert.That(_loginPage.IsDashboardVisible());
    }
}
