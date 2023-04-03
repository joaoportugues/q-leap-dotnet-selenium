using OpenQA.Selenium;

namespace QLeapSelenium.Specs.PageObjects
{
    internal class Contact
    {
        public static By GetElement(string element)
        {
            SortedDictionary<string, By> elementPath = new SortedDictionary<string, By>();
            elementPath.Add("firstName", By.XPath("//*[@name='fname']"));

            return elementPath[element];
        }
    }
}
