using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System.Threading;
using System.Windows.Forms;

namespace AdsRenewalAutomation
{
    public class Scraper
    {

        ChromeOptions options = new ChromeOptions();
        private ChromeDriver driver;
        public bool IsBusy = false;
        public bool ShouldStop = false;
        public String Username;
        public String Password;
        public Scraper()
        {

        }

        public void Start(string username, string password)
        {
            ShouldStop = false;
            Username = username;
            Password = password;
            Task taskA = new Task(() => StartScraping());
            taskA.ContinueWith((encryptTask) => {
                this.IsBusy = false;
                Destroy();
            });
            if (ShouldStop!=true)
            {
                taskA.Start();
            }
        }

        public void StartScraping()
        {
            IsBusy = true;
            try {
                if (ShouldStop) { return; }
                driver = new ChromeDriver();
                driver.Manage().Window.Maximize();
                if (ShouldStop) { return; }
                driver.Navigate().GoToUrl("https://www.gumtree.com.au/");
                if (ShouldStop) { return; }
                driver.FindElementByXPath("//*[@id=\"header-new\"]/div/div[1]/div[1]/div[1]/ul/li[5]/a").Click();
                WebDriverWait wait = new WebDriverWait(driver, System.TimeSpan.FromSeconds(3));
                var ready = wait.Until(ExpectedConditions.ElementExists(By.XPath("//*[@id=\"login-email\"]")));
                driver.FindElement(By.XPath("//*[@id=\"login-email\"]")).SendKeys(Username);
                driver.FindElement(By.XPath("//*[@id=\"login-password\"]")).SendKeys(Password);
                driver.FindElement(By.XPath("//*[@id=\"btn-submit-login\"]")).Click();
                if (ShouldStop) { return; }
                Thread.Sleep(3000);
                if (ShouldStop) { return; }
                driver.Navigate().GoToUrl("https://www.gumtree.com.au/m-my-ads.html?show=EXPIRED");
                while (driver.FindElements(By.XPath("//*[@id=\"my-adlisting\"]/ul/li")).ToList().FirstOrDefault()!=null) {
                    if (ShouldStop!=true)
                    {
                        handlePageListings();
                    }else
                    {
                        break;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                DialogResult res = MessageBox.Show(e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (res == DialogResult.OK)
                {
                    this.IsBusy = false;
                    ShouldStop = true;
                }
            }
        }

        public void handlePageListings()
        {
            IWebElement firstListing = driver.FindElements(By.XPath("//*[@id=\"my-adlisting\"]/ul/li")).ToList().FirstOrDefault();
            if (firstListing!=null)
            {
                firstListing.FindElement(By.CssSelector("div.c-padding-bottom1 > a.repost-ad-free")).Click();
            }
            Thread.Sleep(300);
            driver.Navigate().GoToUrl("https://www.gumtree.com.au/m-my-ads.html?show=EXPIRED");
        }
        
        public void Destroy()
        {
            if (this.driver != null)
            {
                this.driver.Quit();
            }
        }
    }
}
