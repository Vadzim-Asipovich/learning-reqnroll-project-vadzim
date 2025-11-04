using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace WebTestProject.Pages;

public class LoginPage
{
    private static IWebDriver? _driver;

    public static IWebDriver Driver
    {
        get
        {
            _driver ??= new ChromeDriver();
            return _driver;
        }
    }

    public void NavigateToLoginPage()
    {
        Driver.Navigate().GoToUrl("https://www.saucedemo.com/");
    }

    public void EnterCredentials(string username, string password)
    {
        Driver.FindElement(By.Id("user-name")).SendKeys(username);
        Driver.FindElement(By.Id("password")).SendKeys(password);
        Driver.FindElement(By.Id("login-button")).Click();
    }

    public bool IsDashboardVisible()
    {
        return Driver.Url.Contains("inventory");
    }

    public static void QuitBrowser()
    {
        _driver?.Quit();
        _driver = null;
    }
}
