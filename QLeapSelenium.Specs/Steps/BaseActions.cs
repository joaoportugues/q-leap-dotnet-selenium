using QLeapSelenium.Specs.PageObjects;
using OpenQA.Selenium;

namespace QLeapSelenium.Specs.Steps
{
    public class BaseActions
    {
        private readonly IWebDriver _webDriver;

        private readonly PageFactory pageFactory = new PageFactory();

        public BaseActions(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        internal IWebElement findElement(string element, string page)
        {
            return _webDriver.FindElement(pageFactory.ElementPath(element, page));
        }
    }
}
