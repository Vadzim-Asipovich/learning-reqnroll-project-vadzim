using System;
using OpenQA.Selenium;
using NUnit.Framework;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Pages.Components;

public class LoginFormComponent : BaseComponent<LoginFormComponent>
{
    private By UsernameField => By.Id("user-name");
    private By PasswordField => By.Id("password");
    private By LoginButton => By.Id("login-button");

    public LoginFormComponent(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
    }

    protected override void Load()
    {
        // Component is loaded when the page is loaded, so we just wait for the form elements
        WaitForElement(UsernameField);
    }

    protected override void IsLoaded()
    {
        Assert.That(IsElementVisible(UsernameField, 2), Is.True, "Username field not visible!");
        Assert.That(IsElementVisible(PasswordField, 2), Is.True, "Password field not visible!");
        Assert.That(IsElementVisible(LoginButton, 2), Is.True, "Login button not visible!");
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
}

