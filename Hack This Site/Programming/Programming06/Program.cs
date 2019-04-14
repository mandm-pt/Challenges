using System;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Utils;
using System.Drawing;
using Tesseract;

namespace Programming06
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var authCookie = Http.GetAuthCookie();

            var driver = new ChromeDriver("./");
            driver.Navigate().GoToUrl("https://www.hackthissite.org/");
            driver.Manage().Cookies.AddCookie(new Cookie(authCookie.name, authCookie.value));
            driver.Navigate().GoToUrl("https://www.hackthissite.org/missions/prog/6/image/");

            ConfirmAllAlerts(driver);

            var ss = ((ITakesScreenshot)driver).GetScreenshot();

            string imagePath = "./captcha.png";
            ss.SaveAsFile(imagePath, ScreenshotImageFormat.Png);

            string text = GetTextFromImageFile(imagePath);
        }

        private static void ConfirmAllAlerts(ChromeDriver driver)
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
