using Reqnroll;
using WebTestProject.Pages;

namespace WebTestProject.Hooks;

[Binding]
public class Hooks
{
    [AfterScenario]
    public void AfterScenario()
    {
        LoginPage.QuitBrowser();
    }
}