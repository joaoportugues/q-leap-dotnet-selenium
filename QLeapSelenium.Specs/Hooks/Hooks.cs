using QLeapSelenium.Specs.Drivers;
using TechTalk.SpecFlow;

namespace QLeapSelenium.Specs.Hooks
{
    /// <summary>
    /// Calculator related hooks
    /// </summary>
    [Binding]
    public class Hooks
    {
        ///<summary>
        ///  Reset the calculator before each scenario tagged with "Calculator"
        /// </summary>
        [BeforeScenario()]
        public static void BeforeScenario(BrowserDriver browserDriver)
        {
            // add any before scenario tasks
        }

        [AfterScenario]
        public static void AfterScenario(BrowserDriver browserDriver) { 
        }
    }
}