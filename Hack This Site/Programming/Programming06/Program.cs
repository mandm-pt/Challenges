using System;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using Utils;
using System.Drawing;
using Tesseract;
using System.IO;
using System.Reflection;

namespace Programming06
{
    internal class Program
    {
        private const string siteUrl = "https://www.hackthissite.org/";
        private const string randomImageUrl = "https://www.hackthissite.org/missions/prog/6/image/";

        private static void Main(string[] args)
        {
            var authCookie = Http.GetAuthCookie();

            var driver = new FirefoxDriver(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            driver.Navigate().GoToUrl(siteUrl);
            driver.Manage().Cookies.AddCookie(new Cookie(authCookie.name, authCookie.value));
            driver.Navigate().GoToUrl(randomImageUrl);

            ConfirmAllAlerts(driver);

            var screenshot = ((ITakesScreenshot)driver).GetScreenshot();

            string imagePath = "./captcha.png";
            screenshot.SaveAsFile(imagePath, ScreenshotImageFormat.Png);

            string text = GetTextFromImageFile(imagePath);
        }

        private static void ConfirmAllAlerts(IWebDriver driver)
        {
            try
            {
                var alert = driver.SwitchTo().Alert();
                do
                {
                    alert.Accept();
                } while ((alert = driver.SwitchTo().Alert()) != null);
            }
            catch (NoAlertPresentException)
            {
                // do nothing
            }
        }

        private static string GetTextFromImageFile(string imagePath)
        {
            string text = string.Empty;

            using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
            using (var img = Pix.LoadFromFile(imagePath))
            using (var page = engine.Process(img))
            {
                text = page.GetText();
            }

            return text;
        }
    }
}