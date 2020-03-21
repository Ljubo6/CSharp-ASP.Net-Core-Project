namespace DealerShip.Web.Tests
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.Remote;

    using Xunit;

    public class SeleniumTests : IClassFixture<SeleniumServerFactory<Startup>>
    {
        private readonly SeleniumServerFactory<Startup> server;
        private readonly IWebDriver browser;

        // Be sure that selenium-server-standalone-3.141.59.jar is running
        public SeleniumTests(SeleniumServerFactory<Startup> server)
        {
            this.server = server;
            server.CreateClient();
            var opts = new ChromeOptions();
<<<<<<< HEAD
            opts.AddArguments("--headless", "--ignore-certificate-errors");
=======
            opts.AddArgument("--headless"); // Optional, comment this out if you want to SEE the browser window
            opts.AddArgument("no-sandbox");
>>>>>>> 0556923ec7b4f1d4a099784c8d86103e1d13a6d5
            this.browser = new RemoteWebDriver(opts);
        }

        [Fact(Skip = "Example test. Disabled for CI.")]
        public void FooterOfThePageContainsPrivacyLink()
        {
            this.browser.Navigate().GoToUrl(this.server.RootUri);
            Assert.Contains(
                this.browser.FindElements(By.CssSelector("footer a")),
                x => x.GetAttribute("href").EndsWith("/Home/Privacy"));
        }
    }
}
