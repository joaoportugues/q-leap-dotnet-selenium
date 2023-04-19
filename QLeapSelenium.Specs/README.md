// replace the driver to run 
sed -i '/string browser/c\string browser = \"chrome\";' CalculatorSelenium.Specs\Drivers\BrowserDriver.cs & dotnet test --filter Category=run1

// to run tests 
dotnet test

// run by tags
dotnet test --filter Category=run

//run in parallel
edit AssemblyByInfo

// reports
dotnet tool install --global SpecFlow.Plus.LivingDoc.CLI

cd \bin\Debug\net6.0
livingdoc test-assembly QLeapSelenium.Specs.dll -t TestExecution.json
open : bin\Debug\net6.0\LivingDoc.html

0 - https://visualstudio.microsoft.com/downloads/ - community version 2022 should be fine
0 - https://www.selenium.dev/downloads/ - download selenium grid jar
1 - Install visual studio - default options should be ok
2 - DARK MODE!
3 - Continue without code
4 - Go to Extensions >> Install SpecFlow extension. Close all VS windows
5 - Open VS and select new project
6 - Select specflow project > TrainingQLeap > .NET6.0 > NUnit > FluentAssertions > Create (check if the features file resolved .feature. if not restart)
7 - Open TrainingQleap.sln file - check the dependencies
8 - Add Selenium dependencies and other browsers as needed > Projects > Manage NuGet packages > Browse > Selenium Support and ChromeDriver
9 - Create driver manager 

```BrowserDriver.cs
using OpenQA.Selenium.Remote;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TrainingQleap.Drivers
{
    /// Manages a browser instance using Selenium
    public class BrowserDriver : IDisposable
    {
        private readonly Lazy<IWebDriver> _currentWebDriverLazy;
        private bool _isDisposed;

        public BrowserDriver()
        {
            _currentWebDriverLazy = new Lazy<IWebDriver>(CreateWebDriver);
        }

        /// The Selenium IWebDriver instance
        public IWebDriver Current => _currentWebDriverLazy.Value;

        /// Creates the Selenium web driver (opens a browser)

        private IWebDriver CreateWebDriver()
        {       
            var gridOptions = new ChromeOptions();
            return new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), gridOptions);
        }

        /// Disposes the Selenium web driver (closing the browser)
        public void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (_currentWebDriverLazy.IsValueCreated)
            {
                Current.Quit();
            }

            _isDisposed = true;
        }
    }
}
```
10 - Create .feature file in Features folder
```Q-LeapLanding.feature
Feature: Q-Leap Landing

Scenario: Q-Leap Test
	Given I navigate to 'https://q-leap.eu/'

```
11 - Right click on the step and define step and allow it to create the file as suggested
12 - let call the string url and change the code to navigate to the browser using the driver
```Q_LeapLandingStepDefinitions.cs
using OpenQA.Selenium;
using TrainingQleap.Drivers;

namespace TrainingQleap.StepDefinitions
{
    [Binding]
    public class Q_LeapLandingStepDefinitions
    {
	    // webdriver instance
        private readonly IWebDriver _webDriver;
        private readonly WebDriverWait _wait;

        public Q_LeapLandingStepDefinitions(BrowserDriver browserDriver)
        {
		     // assign current thread to webdriver
            _webDriver = browserDriver.Current;
            _wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            _wait.PollingInterval = TimeSpan.FromMilliseconds(200);
        }


        [Given(@"I navigate to '([^']*)'")]
        public void GivenINavigateTo(string url)
        {
            _webDriver.Navigate().GoToUrl(url);
            _webDriver.Manage().Window.Maximize();
        }
    }
}

```
13 - change the step to a generic type using [StepDefinition(@"I navigate to '([^']*)'")]. This way it's not attached to one single definition
14 - Let's create a clicking and a reading step in the feature file 
```
	And I click training on Landing page
	Then I read Our Training Catalogue on element trainingCatalog on Training page
```
15 - right click define steps > don't overwrite, just copy the code a and place it on the step definition
16 - change it to generic wording and define the click using the same _webdriver
18 - for now let's define all actions locally
```cs
		[StepDefinition(@"I click training on Landing page")]
        public void GivenIClickTrainingOnLoginPage()
        {
            _wait.Until(_webdriver => _webDriver.FindElement(By.XPath("//a[contains(text(), 'Training')]"))).Click();
        }


        [StepDefinition(@"I read Our Training Catalogue on element trainingCatalog on Training page")]
        public void ThenIReadOurTrainingCatalogueOnElementTrainingCatalogOnTrainingPage()
        {
            string textFromElement = _wait.Until(_webdriver => _webDriver.FindElement(By.XPath("//*[@id=\"main\"]/div[5]/div/div/div[2]/h2"))).Text;
            Assert.AreEqual("Our Training Catalogue", textFromElement);

        }
```
19 - Let's create a  a PageObject model > Create Folder PageObject and file Landing.cs
```cs
using NUnit.Framework;
using OpenQA.Selenium;

namespace TrainingQleap.PageObjects
{
    public class Landing
    {
        // webdriver instance
        private readonly IWebDriver _webDriver;
        private readonly WebDriverWait _wait;

        public Landing(IWebDriver webDriver)
        {
            // assign current thread to webdriver
            _webDriver = webDriver;
            _wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            _wait.PollingInterval = TimeSpan.FromMilliseconds(200);
        }
   
        private IWebElement trainingTab => _wait.Until(_webDriver 
            => _webDriver.FindElement(By.XPath("//a[contains(text(), 'Training')]")));
        private IWebElement headerText => _wait.Until(_webDriver 
            =>_webDriver.FindElement(By.XPath("//*[@id=\"main\"]/div[5]/div/div/div[2]/h2")));

        public void clickTab() {
            trainingTab.Click();
        }

        public void readText()
        {
            Assert.AreEqual("Our Training Catalogue", headerText.Text);
        }

    }
}
```
20 - we can and should split the landing and training pages -> We can start to see some code being duplicated across the classes
21 - let's run tests in parallel > create file AssemblyInfo.cs
```cs
using NUnit.Framework;

[assembly: Parallelizable(ParallelScope.Fixtures)]
[assembly: LevelOfParallelism(2)]
```
22 - duplicate the feature file with a different feature and scenario name and run your tests
23 - lets reorganize a couple of things that will allow us to have less repetitive code. 
first let's generalize the clicking function and reading function
24 - let's start by change the feature file to pass a string that will be used to identify the element
25 - and change the click function to receive this input and pass it down to search the value in a dictionary
``` Feature with parameter
	And I click 'trainingTab' on Landing page

```
``` cs
step definition with paramter
        [StepDefinition(@"I click '([^']*)' on Landing page")]
        public void GivenIClickTrainingOnLoginPage(string element)
        {
            _landing.clickTab(element);
        }
```
```cs
implementation of the click with parameter and dictionary
public By GetElement(string element)
        {
            SortedDictionary<string, By> elementPath = new SortedDictionary<string, By>();

            elementPath.Add("trainingTab", By.XPath("//a[contains(text(), 'Training')]"));

            return elementPath[element];
        }

        public void clickTab(string element) {
            _wait.Until(_webDriver => _webDriver.FindElement(GetElement(element))).Click();
        }
```
26 - we actually now are able to map the entire landing page and use one function to reach all clickable elements
27 - let's do the same for read, but this time, we will pass 2 parameters, expected text and element.
```
	Then I read 'Our Training Catalogue' on element 'trainingCatalog' on Training page

```

```cs
   [StepDefinition(@"I read '([^']*)' on element '([^']*)' on Training page")]
        public void ThenIReadOurTrainingCatalogueOnElementTrainingCatalogOnTrainingPage(string expectedText, string element)
        {
            _training.readText(expectedText, element);

        }
```

```cs
 public By GetElement(string element)
        {
            SortedDictionary<string, By> elementPath = new SortedDictionary<string, By>();

            elementPath.Add("trainingCatalog", By.XPath("//*[@id=\"main\"]/div[5]/div/div/div[2]/h2"));

            return elementPath[element];
        }
   
        public void readText(string expectedText, string element)
        {
            Assert.AreEqual(expectedText, _wait.Until(_webDriver
            => _webDriver.FindElement(GetElement(element))).Text);
        }
```
28 - now while we did generalize the use of these functions, we can rename them a bit better
29 - we did not manage to reduce amount of code, but we did avoid it. we can now move on to restructure a bit more and finally get rid of the duplicated driver and wait calls
30 - let's split the page objects from the function that performs the action
31 - on the page objects, let's keep only the dictionary and move the click and read functions to a new class called DefaultSteps
```cs
namespace TrainingQleap.PageObjects
{
    public class Landing
    {
        public By GetElement(string element)
        {
            SortedDictionary<string, By> elementPath = new SortedDictionary<string, By>();

            elementPath.Add("trainingTab", By.XPath("//a[contains(text(), 'Training')]"));

            return elementPath[element];
        }
    }
}

```

```cs
        public void clickTab(string element)
        {
            _wait.Until(_webDriver => _webDriver.FindElement(_landing.GetElement(element))).Click();
        }

        public void readText(string expectedText, string element)
        {
            Assert.AreEqual(expectedText, _wait.Until(_webDriver
            => _webDriver.FindElement(_training.GetElement(element))).Text);
        }
```
```cs
public class Q_LeapLandingStepDefinitions
    {
        // webdriver instance
        private readonly IWebDriver _webDriver;
        private readonly WebDriverWait _wait;

        private readonly DefaultSteps _defaultSteps;


        public Q_LeapLandingStepDefinitions(BrowserDriver browserDriver)
        {
            // assign current thread to webdriver
            _webDriver = browserDriver.Current;
            _wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            _wait.PollingInterval = TimeSpan.FromMilliseconds(200);

            _defaultSteps = new DefaultSteps(_webDriver);
        }

        [StepDefinition(@"I navigate to '([^']*)'")]
        public void GivenINavigateTo(string url)
        {
            _webDriver.Navigate().GoToUrl(url);
            _webDriver.Manage().Window.Maximize();
        }

        [StepDefinition(@"I click '([^']*)' on Landing page")]
        public void GivenIClickTrainingOnLoginPage(string element)
        {
            _defaultSteps.clickTab(element);
        }

        [StepDefinition(@"I read '([^']*)' on element '([^']*)' on Training page")]
        public void ThenIReadOurTrainingCatalogueOnElementTrainingCatalogOnTrainingPage(string expectedText, string element)
        {
            _defaultSteps.readText(expectedText, element);

        }
    }
```
32 - Let's move on to some weird stuff by creating a generic interface to be able to call which PageObject model to check the element 
33 - change the GetElement function to static, since we are accessing it through reflection
```PageObjects.PageFactory.cs
using OpenQA.Selenium;

namespace TrainingQleap.PageObjects
{
    public class PageFactory
    {
    #pragma warning disable 8600, 8602, 8603
        internal By ElementPath(string element, string page)
        {
            Type type = Type.GetType("TrainingQleap.PageObjects." + page);
            return (By)type!.GetMethod("GetElement").Invoke(type, new object[] { element });
        }
    }
}

```

``` DefaultSteps.cs

using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using NUnit.Framework;
using TrainingQleap.PageObjects;

namespace TrainingQleap.StepDefinitions
{
    internal class DefaultSteps
    {
        // webdriver instance
        private readonly IWebDriver _webDriver;
        private readonly WebDriverWait _wait;
        private readonly PageFactory _pageFactory = new PageFactory();

        public DefaultSteps(IWebDriver webDriver)
        {
            // assign current thread to webdriver
            _webDriver = webDriver;
            _wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            _wait.PollingInterval = TimeSpan.FromMilliseconds(200);

        }
        public void click(string element, string page)
        {
            _wait.Until(_webDriver => _webDriver.FindElement(_pageFactory.ElementPath(element, page))).Click();
        }

        public void readText(string expectedText, string element, string page)
        {
            Assert.AreEqual(expectedText, _wait.Until(_webDriver
            => _webDriver.FindElement(_pageFactory.ElementPath(element, page))).Text);
        }
    }
}
```
33 - and then by creating a BaseActions class, that will contain all the driver and wait functionalities + the ability to call all the elements
34 - the next changes will move all implementations away from the step definitions (only one left anyway), keeping the code cleaner and leaving the base class accessible to other functions with similar functionalities
```cs
[StepDefinition(@"I navigate to '([^']*)'")]
        public void GivenINavigateTo(string url)
        {
            _defaultSteps.naviagetToUrl(url);
        }
```

```cs
        public void naviagetToUrl(string url)
        {
            _webDriver.Navigate().GoToUrl(url);
            _webDriver.Manage().Window.Maximize();
        }
```

And we can remove the driver related stuff from the StepDefinitions
```cs
    public class Q_LeapLandingStepDefinitions
    {
        private readonly DefaultSteps _defaultSteps;

        public Q_LeapLandingStepDefinitions(BrowserDriver browserDriver)
        {
            _defaultSteps = new DefaultSteps(browserDriver.Current);
        }
```

``` BaseActions.cs
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium;
using TrainingQleap.PageObjects;

namespace TrainingQleap.StepDefinitions
{
    public class BaseActions
    {
        private readonly IWebDriver _webDriver;
        private readonly WebDriverWait _wait;


        private readonly PageFactory pageFactory = new PageFactory();

        public BaseActions(IWebDriver webDriver)
        {
            _webDriver = webDriver;
            _wait = new WebDriverWait(_webDriver, TimeSpan.FromSeconds(10));
            _wait.PollingInterval = TimeSpan.FromMilliseconds(200);
        }

        internal IWebElement findElement(string element, string page)
        {
            return _wait.Until(_webDriver => _webDriver.FindElement(pageFactory.ElementPath(element, page)));
        }
    }
}

```

```DefaultSteps.cs
using OpenQA.Selenium;
using NUnit.Framework;

namespace TrainingQleap.StepDefinitions
{
    internal class DefaultSteps
    {
        private readonly IWebDriver _webDriver;
        private readonly BaseActions _baseActions;

        public DefaultSteps(IWebDriver webDriver)
        {
            // assign current thread to webdriver
            _webDriver = webDriver;
            _baseActions = new BaseActions(webDriver);

        }
        public void naviagetToUrl(string url)
        {
            _webDriver.Navigate().GoToUrl(url);
            _webDriver.Manage().Window.Maximize();
        }
        public void click(string element, string page)
        {
            _baseActions.findElement(element, page).Click();
        }

        public void readText(string expectedText, string element, string page)
        {
            Assert.AreEqual(expectedText, _baseActions.findElement(element, page).Text);
        }
    }
}
```
35 - Additional Step definition files, implementations, etc
```
@runThis
Scenario: Grouped Steps
	Given I navigate to 'https://q-leap.eu/'
	Then I see 'Our Training Catalogue' in the Training Catalog
```

```GroupedStepDefinitions.cs
using TrainingQleap.Drivers;

namespace TrainingQleap.StepDefinitions
{
    [Binding]
    public class GroupedStepDefinitions
    {
        private readonly GroupedSteps _groupedSteps;

        public GroupedStepDefinitions(BrowserDriver browserDriver)
        {
            _groupedSteps = new GroupedSteps(browserDriver.Current);
        }

        [Then(@"I see '([^']*)' in the Training Catalog")]
        public void ThenISeeInTheTrainingCatalog(string expectedText)
        {
            _groupedSteps.clickAndRead(expectedText);
        }

    }
}

```

```cs 
GroupedSteps impl
using OpenQA.Selenium;
using NUnit.Framework;
using System.Xml.Linq;

namespace TrainingQleap.StepDefinitions.StepImplementation
{
    internal class GroupedSteps
    {
        private readonly IWebDriver _webDriver;
        private readonly BaseActions _baseActions;
        private readonly DefaultSteps _defaultSteps;

        public GroupedSteps(IWebDriver webDriver)
        {
            // assign current thread to webdriver
            _webDriver = webDriver;
            _defaultSteps = new DefaultSteps(webDriver);
        }
        public void clickAndRead(string expectedText)
        {
            _defaultSteps.click("trainingTab", "Landing");
            _defaultSteps.readText(expectedText, "trainingCatalog", "Training");
        }
    }
}

```
36 - run from terminal 
```
dotnet clean
dotnet build
dotnet test
```
37 - run with tags
```
dotnet test --filter Category=runThis
```
38 - reports > living documentation 
```
cd \bin\Debug\net6.0
livingdoc test-assembly TrainingQleap.dll -t TestExecution.json
open : bin\Debug\net6.0\LivingDoc.html
```
39 - Create folder hooks and file Hooks.cs
```cs
using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;
using TrainingQleap.Drivers;

namespace TrainingQleap.Hooks
{
    // https://docs.specflow.org/projects/specflow/en/latest/Bindings/Hooks.html
    [Binding]
    public class Hooks
    {
        private readonly BrowserDriver _browserDriver;
        private readonly ISpecFlowOutputHelper _specFlowOutputHelper;
        private ScenarioContext _scenarioContext;

        public Hooks(BrowserDriver browserDriver, ISpecFlowOutputHelper specFlowOutputHelper, ScenarioContext scenarioContext)
        {
            _browserDriver = browserDriver;
            _specFlowOutputHelper = specFlowOutputHelper;
            _scenarioContext = scenarioContext;
        }

        [BeforeScenario()]
        public static void BeforeScenario(BrowserDriver browserDriver)
        {
            // add any before scenario tasks
        }

        [AfterScenario]
        public void AfterScenario()
        {
            if (_scenarioContext.TestError != null)
            {
                if (_browserDriver.Current is ITakesScreenshot screnshotTaker)
                {

                    var filename = Path.ChangeExtension(Path.GetRandomFileName(), "png");
                    screnshotTaker.GetScreenshot().SaveAsFile(filename);
                    _specFlowOutputHelper.AddAttachment(filename);
                }

            }
        }

    }
}

```
