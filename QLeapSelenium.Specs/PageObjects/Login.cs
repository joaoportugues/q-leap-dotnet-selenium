using OpenQA.Selenium;

namespace QLeapSelenium.Specs.PageObjects
{
    internal class Login
    {
        public static By GetElement(string element)
        {
            SortedDictionary<string, By> elementPath = new SortedDictionary<string, By>();
            elementPath.Add("aboutUs", By.XPath("//a[contains(text(), 'About us')]"));
            elementPath.Add("testing", By.XPath("//a[contains(text(), 'Testing')]"));
            elementPath.Add("training", By.XPath("//a[contains(text(), 'Training')]"));
            elementPath.Add("joinUs", By.XPath("//a[contains(text(), 'Join us')]"));
            elementPath.Add("contact", By.XPath("//a[contains(text(), 'Contact')]"));

            return elementPath[element];
        }
    }
}
