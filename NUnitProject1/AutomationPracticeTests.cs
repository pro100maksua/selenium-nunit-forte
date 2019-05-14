using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace NUnitProject1
{
    [TestFixture]
    public class AutomationPracticeTests
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
            _driver.Navigate().GoToUrl("http://automationpractice.com/");

            Login();
        }
        private void Login()
        {
            _driver.FindElement(By.ClassName("login")).Click();
            _driver.FindElement(By.Id("email")).SendKeys("maksim741@ukr.net");
            _driver.FindElement(By.Id("passwd")).SendKeys("pass123");
            _driver.FindElement(By.Id("SubmitLogin")).Click();
        }

        [TearDown]
        public void TearDown()
        {
            _driver.Quit();
        }

        [Test]
        [TestCase("contact-link", "Contact us - My Store")]
        public void CheckTitle(string elementId, string result)
        {
            var catalogLink = _driver.FindElement(By.Id(elementId));
            catalogLink.Click();

            Assert.AreEqual(result, _driver.Title);
        }

        [Test]
        public void CheckChangeName()
        {
            _driver.FindElement(By.ClassName("account")).Click();
            _driver.FindElement(By.ClassName("icon-user")).Click();

            var firstNameElement = _driver.FindElement(By.Id("firstname"));
            var lastNameElement = _driver.FindElement(By.Id("lastname"));
            var oldFirstName = firstNameElement.GetAttribute("value");
            var oldLastName = lastNameElement.GetAttribute("value");

            firstNameElement.Clear();
            firstNameElement.SendKeys(oldLastName);
            lastNameElement.Clear();
            lastNameElement.SendKeys(oldFirstName);

            _driver.FindElement(By.Id("old_passwd")).SendKeys("pass123");
            _driver.FindElement(By.Name("submitIdentity")).Click();

            var fullName = _driver.FindElement(By.ClassName("account")).Text.Split(' ');
            Assert.AreEqual(fullName[0], oldLastName);
            Assert.AreEqual(fullName[1], oldFirstName);
        }
    }
}