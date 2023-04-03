using OpenQA.Selenium;

namespace QLeapSelenium.Specs.PageObjects
{
    internal class Training
    {
        public static By GetElement(string element)
        {
            SortedDictionary<string, By> elementPath = new SortedDictionary<string, By>();
            elementPath.Add("trainingCatalog", By.XPath("//*[@id=\"main\"]/div[5]/div/div/div[2]/h2"));

            return elementPath[element];
        }
    }
}
