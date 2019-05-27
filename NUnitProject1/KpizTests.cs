using System;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace NUnitProject1
{
    [TestFixture]
    public class KpizTests
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
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            _driver.Navigate().GoToUrl("https://www.goldtoe.com/");

            ClosePopup();
            Login();
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }

        private void ClosePopup()
        {
            var popup = _driver.FindElement(By.Id("popup-subcription-closes-icon-85d6dd11-dc2e-4949-bd75-0a9ec85091bf"));
            popup.Click();

            var wait = new DefaultWait<IWebElement>(popup) { Timeout = TimeSpan.FromSeconds(10) };
            wait.Until(NotDisplayed);
        }

        private bool NotDisplayed(IWebElement element)
        {
            try
            {
                return !element.Displayed;
            }
            catch (StaleElementReferenceException)
            {
                return true;
            }
            catch (NoSuchElementException)
            {
                return true;
            }
        }

        private void Login()
        {
            _driver.FindElement(By.XPath("//*[@id=\"top-tools-div\"]/ul/li[1]")).Click();

            var email = "qopofpain1@gmail.com";
            var password = "Pass123!";
            try
            {
                var emailInput = _driver.FindElement(By.XPath("//*[@id=\"main\"]/div[1]/form/div[1]/input"));
                emailInput.SendKeys(email);

                var passwordInput = _driver.FindElement(By.XPath("//*[@id=\"main\"]/div[1]/form/div[2]/input"));
                passwordInput.SendKeys(password);
            }
            catch (NoSuchElementException)
            {
                var emailInput = _driver.FindElement(By.XPath("//*[@id=\"main\"]/div[1]/form/div[1]/div/input"));
                emailInput.SendKeys(email);

                var passwordInput = _driver.FindElement(By.XPath("//*[@id=\"main\"]/div[1]/form/div[2]/div/input"));
                passwordInput.SendKeys(password);
            }

            _driver.FindElement(By.XPath("//*[@id=\"main\"]/div[1]/form/div[3]/div[2]/button")).Click();
        }

        [Test]
        public void LoginTest()
        {
            var name = _driver.FindElement(By.XPath("//*[@id=\"top-tools-div\"]/ul/li[2]/a/span/span"));
            var wait = new DefaultWait<IWebElement>(name) { Timeout = TimeSpan.FromSeconds(10) };
            wait.Until(e => !string.IsNullOrWhiteSpace(e.Text));

            Assert.AreEqual("Maksym", name.Text);
        }

        [Test]
        public void LogoutTest()
        {
            var myAccount = _driver.FindElement(By.XPath("//*[@id=\"top-tools-div\"]/ul/li[2]"));
            myAccount.Click();
            myAccount.FindElement(By.XPath(".//ul/li[9]")).Click();

            var greeting = _driver.FindElement(By.XPath("//*[@id=\"top-tools-div\"]/ul/li[1]/a"));
            var wait = new DefaultWait<IWebElement>(greeting) { Timeout = TimeSpan.FromSeconds(10) };
            wait.Until(e => !string.IsNullOrWhiteSpace(e.Text));

            Assert.AreEqual("Sign In/Register", greeting.Text);
        }

        [Test]
        public void CreateWishListTest()
        {
            var myAccount = _driver.FindElement(By.XPath("//*[@id=\"top-tools-div\"]/ul/li[2]"));
            myAccount.Click();
            myAccount.FindElement(By.XPath(".//ul/li[2]")).Click();

            _driver.FindElement(By.XPath("//*[@id=\"subnav\"]/li[4]")).Click();
            _driver.FindElement(By.XPath("//*[@id=\"content\"]/div[3]/div[1]/button")).Click();

            var expectedName = "My List";
            _driver.FindElement(By.XPath("//*[@id=\"82\"]")).SendKeys(expectedName);
            var createButton = _driver.FindElement(By.XPath("//*[@id=\"modal-wishlist\"]/div/div/div[3]/button[2]"));
            createButton.Click();
            var wait = new DefaultWait<IWebElement>(createButton) { Timeout = TimeSpan.FromSeconds(10) };
            wait.Until(NotDisplayed);

            var actualName = _driver.FindElement(By.XPath("//*[@id=\"content\"]/div[3]/ul/li/div/a/strong/span[1]")).Text;

            Assert.AreEqual(expectedName, actualName);

            _driver.FindElement(By.XPath("//*[@id=\"content\"]/div[3]/ul/li/ul/li[3]/a")).Click();
            _driver.FindElement(By.XPath("//*[@id=\"modal-confirm\"]/div/div/div[3]/button[2]")).Click();
        }
    }
}