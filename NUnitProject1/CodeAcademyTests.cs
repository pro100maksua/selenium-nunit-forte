using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NUnitProject1
{
    [TestFixture]
    public class CodeAcademyTests
    {
        private IWebDriver _driver;

        [SetUp]
        public void Setup()
        {
            var options = new ChromeOptions();
            options.AddArguments
            (
                "--start-maximized",
                "--disable-extensions",
                "--disable-notifications",
                "--disable-application-cache"
            );
            _driver = new ChromeDriver(options);
            _driver.Navigate().GoToUrl("https://www.codeacademy.com/");
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }

        [Test]
        [TestCase("catalog", "Catalog | Codecademy")]
        [TestCase("pricing", "The Easiest Way to Learn to Code | Codecademy")]
        public void CheckTitle(string elementId, string result)
        {
            var catalogLink = _driver.FindElement(By.Id(elementId));
            catalogLink.Click();

            Assert.AreEqual(result, _driver.Title);
        }
    }
}