using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace WebDriverPaste.Test
{
    public class PastebinTest
    {
        private IWebDriver webDriver;

        private WebDriverWait wait;

        [SetUp]
        public void SetUp()
        {
            webDriver = new ChromeDriver();
            wait = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));
        }

        [Test]
        public void CreateNewPasteTest()
        {
            webDriver.Navigate().GoToUrl("https://pastebin.com/");

            webDriver.FindElement(By.Id("postform-text")).SendKeys("Hello from WebDriver");

            webDriver.FindElement(By.Id("select2-postform-expiration-container")).Click();

            By dropdownOptionSelector = By.CssSelector(".select2-results__option[id*='select2-postform-expiration-result']");
            new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(driver => driver.FindElement(dropdownOptionSelector).Displayed);

            IWebElement option10Minutes = webDriver.FindElement(dropdownOptionSelector);
            option10Minutes.Click();

            webDriver.FindElement(By.Id("postform-name")).SendKeys("helloweb");

            By submitButtonSelector = By.CssSelector("button[class='btn -big']");
            new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(driver =>
            {
                try
                {
                    var submitButton = driver.FindElement(submitButtonSelector);
                    return submitButton.Enabled && submitButton.Displayed;
                }
                catch (NoSuchElementException)
                {
                    return false;
                }
            
            });

            webDriver.FindElement(submitButtonSelector).Click();

            wait.Until(d => d.Title.StartsWith("helloweb"));

            Assert.AreEqual("Hello from WebDriver", webDriver.FindElement(By.ClassName("de1")).Text.Trim());
        }

        [TearDown]
        public void TearDown()
        {
            webDriver.Quit();
        }
    }
}