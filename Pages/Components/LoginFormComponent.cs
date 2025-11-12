using System;
using OpenQA.Selenium;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Pages.Components;

public class LoginFormComponent : BaseComponent
{
    private By UsernameField => By.Id("user-name");
    private By PasswordField => By.Id("password");
    private By LoginButton => By.Id("login-button");

    public LoginFormComponent(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
    }

    public void EnterUsername(string username)
    {
        if (string.IsNullOrWhiteSpace(username))
            throw new ArgumentException("Username cannot be null or empty", nameof(username));

        SendKeysToElement(UsernameField, username);
    }

    public void EnterPassword(string password)
    {
        if (string.IsNullOrWhiteSpace(password))
            throw new ArgumentException("Password cannot be null or empty", nameof(password));

        SendKeysToElement(PasswordField, password);
    }

    public void ClickLoginButton()
    {
        ClickElement(LoginButton);
    }

    public void EnterCredentials(string username, string password)
    {
        EnterUsername(username);
        EnterPassword(password);
        ClickLoginButton();
    }

    public void WaitForLoginForm()
    {
        WaitForElement(UsernameField);
    }
}

