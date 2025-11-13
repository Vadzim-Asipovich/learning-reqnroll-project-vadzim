using OpenQA.Selenium;

namespace learning_reqnroll_project_vadzim.Pages;

public abstract class LoadableComponent<T> where T : LoadableComponent<T>
{
    protected IWebDriver driver;

    public LoadableComponent(IWebDriver driver)
    {
        this.driver = driver;
    }

    protected abstract void Load();

    protected abstract void IsLoaded();

    public T Get()
    {
        Load();
        IsLoaded();
        return (T)this;
    }
}
