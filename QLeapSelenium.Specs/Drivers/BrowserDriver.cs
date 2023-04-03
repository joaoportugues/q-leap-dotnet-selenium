using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;

namespace QLeapSelenium.Specs.Drivers
{
    /// <summary>
    /// Manages a browser instance using Selenium
    /// </summary>
    public class BrowserDriver : IDisposable
    {
        private readonly Lazy<IWebDriver> _currentWebDriverLazy;
        private bool _isDisposed;

        public BrowserDriver()
        {
            _currentWebDriverLazy = new Lazy<IWebDriver>(CreateWebDriver);
        }

        /// <summary>
        /// The Selenium IWebDriver instance
        /// </summary>
        public IWebDriver Current => _currentWebDriverLazy.Value;

        /// <summary>
        /// Creates the Selenium web driver (opens a browser)
        /// </summary>
        /// <returns></returns>
        private IWebDriver CreateWebDriver()
        {
            string browser = "grid";
            switch (browser.ToLower()) {
                case "chrome":
                    var chromeDriverService = ChromeDriverService.CreateDefaultService();
                    var chromeOptions = new ChromeOptions();
                    //var chromeDriver = new ChromeDriver("C:\\Users\\matjo\\automation\\webDrivers\\");
                    return new ChromeDriver(chromeDriverService, chromeOptions);
                default : //grid
                    var gridOptions = new ChromeOptions();
                    // var gridOptions = new edgeOptions();
                    // edgeOptions.AddArguments("user-data-dir=C:\\Users\\matjo\\AppData\\Local\\Microsoft\\Edge\\automationUser");
                    return new RemoteWebDriver(new Uri("http://localhost:4444/wd/hub"), gridOptions);
            }
        }

        /// <summary>
        /// Disposes the Selenium web driver (closing the browser)
        /// </summary>
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