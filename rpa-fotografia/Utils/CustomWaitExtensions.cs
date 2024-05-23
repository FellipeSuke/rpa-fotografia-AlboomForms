using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace rpa_fotografia.Utils
{
    public static class CustomWaitExtensions
    {
        public static IWebElement WaitForElementToBeVisible(this IWebDriver driver, By locator, int timeoutInSeconds)
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
    }
}
