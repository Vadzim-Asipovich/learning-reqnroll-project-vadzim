using OpenQA.Selenium;
using learning_reqnroll_project_vadzim.Configuration;

namespace learning_reqnroll_project_vadzim.Pages.Components;

public class DashboardHeaderComponent : BaseComponent
{
    private By DashboardHeader => By.ClassName("app_logo");

    public DashboardHeaderComponent(IWebDriver driver, TestConfiguration config) : base(driver, config)
    {
    }

    public IWebElement GetHeader()
    {
        return WaitForElement(DashboardHeader);
    }
}

