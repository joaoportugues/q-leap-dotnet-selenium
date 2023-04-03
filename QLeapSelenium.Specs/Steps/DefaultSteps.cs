using OpenQA.Selenium;
using NUnit.Framework;

namespace QLeapSelenium.Specs.Steps
{
    public class DefaultSteps
    {
        private readonly IWebDriver _webDriver;
        private readonly BaseActions _baseActions;


        public DefaultSteps(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _baseActions = new BaseActions(_webDriver);
        }

        internal void NavigateToUrl(string url)
        {
            _webDriver.Navigate().GoToUrl(url);
            _webDriver.Manage().Window.Maximize();
        }

        internal void DefaultType(string text, string element, string page)
        {
            _baseActions.findElement(element, page).SendKeys(text);
        }  
        internal void DefaultClick(string element, string page)
        {
            _baseActions.findElement(element, page).Click();
        }
        
        internal void DefaultRead(string expectedText, string element, string page)
        {
            string? actualText = null;
            string[] attributes = { "text", "textContent", "innerHTML", "innerText", "outerText" };

            // attempt to get the text using multiple attributes
            foreach (string attribute in attributes)
            {
                actualText = _baseActions.findElement(element, page).GetAttribute(attribute);
                if (!string.IsNullOrEmpty(actualText))
                {
                    break;
                }
            }
            Assert.AreEqual(expectedText, actualText);
        }
    }
}
