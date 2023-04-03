using OpenQA.Selenium;

namespace QLeapSelenium.Specs.PageObjects
{
    public class PageFactory
    {
#pragma warning disable 8600, 8602, 8603
        internal By ElementPath(string element, string page)
        {
            Type type = Type.GetType("QLeapSelenium.Specs.PageObjects." + page);
            return (By)type!.GetMethod("GetElement").Invoke(type, new object[] { element });
        }
    }
}
