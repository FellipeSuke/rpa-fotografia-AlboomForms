using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace rpa_fotografia.Utils
{
    public static class CustomWaitExtensions
    {
        public static IWebElement WaitForElementToBeVisible(this IWebDriver driver, By locator, int timeoutInSeconds)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(timeoutInSeconds));
                return wait.Until(driver =>
                {
                    try
                    {
                        var element = driver.FindElement(locator);
                        if (element.Displayed && element.Enabled)
                        {
                            return element;
                        }
                        return null;
                    }
                    catch (NoSuchElementException)
                    {
                        return null;
                    }
                    catch (StaleElementReferenceException)
                    {
                        return null;
                    }
                });
            }
            catch (Exception)
            {

                driver.Navigate().Refresh();
                Thread.Sleep(5000);
                throw;

            }

        }
    }
}
