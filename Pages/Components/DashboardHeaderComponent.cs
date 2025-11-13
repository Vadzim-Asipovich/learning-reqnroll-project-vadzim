using OpenQA.Selenium;
using NUnit.Framework;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Pages.Components;

public class DashboardHeaderComponent : BaseComponent<DashboardHeaderComponent>
{
    private By DashboardHeader => By.ClassName("app_logo");

    public DashboardHeaderComponent(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
    }

    protected override void Load()
    {
        // Component is loaded when the page is loaded, so we just wait for the header
        WaitForElement(DashboardHeader);
    }

    protected override void IsLoaded()
    {
        Assert.That(IsElementVisible(DashboardHeader, 2), Is.True, "Dashboard header not visible!");
    }

    public IWebElement GetHeader()
    {
        return WaitForElement(DashboardHeader);
    }
}

