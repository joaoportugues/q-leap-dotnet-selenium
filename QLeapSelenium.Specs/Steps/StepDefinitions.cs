using QLeapSelenium.Specs.Drivers;
using TechTalk.SpecFlow;

namespace QLeapSelenium.Specs.Steps
{
    [Binding]
    public sealed class StepDefinitions
    {
        private readonly DefaultSteps _defaultSteps;

        public StepDefinitions(BrowserDriver browserDriver)
        {
            _defaultSteps = new DefaultSteps(browserDriver.Current);
        }

        [StepDefinition(@"I navigate to '([^']*)'")]
        public void INavigate(string url)
        {
            _defaultSteps.NavigateToUrl(url);
        }

        [StepDefinition(@"I click '([^']*)' on '([^']*)'")]
        public void IClick(string element, string page)
        {
            _defaultSteps.DefaultClick(element, page);
        }

        [StepDefinition(@"I type '([^']*)' on element '([^']*)' on '([^']*)'")]
        public void ThenITypeOnElementOn(string text, string element, string page)
        {
            _defaultSteps.DefaultType(text, element, page);
        }

        [StepDefinition(@"I read '([^']*)' on element '([^']*)' on '([^']*)'")]
        public void IRead(string expectedText, string element, string page)
        {
            _defaultSteps.DefaultRead(expectedText, element, page);
        }
    }
}