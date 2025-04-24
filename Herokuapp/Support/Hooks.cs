using AventStack.ExtentReports;
using AventStack.ExtentReports.Gherkin.Model;
using AventStack.ExtentReports.Reporter;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using TechTalk.SpecFlow;
using WebDriverManager;
using WebDriverManager.DriverConfigs.Impl;

namespace Herokuapp.Support
{
    [Binding]
    public class Hooks
    {
        private static IWebDriver _sharedDriver;
        private static ExtentReports _extent;
        private static ExtentTest _feature;
        private ExtentTest _scenario;
        private static ExtentSparkReporter _htmlReporter;
        private readonly ScenarioContext _scenarioContext;
        private static readonly object _lock = new();

        public Hooks(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
        }

        [BeforeTestRun]
        public static void BeforeTestRun()
        {
            // Set up ExtentReport
            string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
            string reportPath = Path.Combine(projectRoot, "Reports", "ExtentReport.html");

            new DriverManager().SetUpDriver(new ChromeConfig());

            _sharedDriver = new ChromeDriver();
            _sharedDriver.Manage().Window.Maximize();

            _htmlReporter = new ExtentSparkReporter(reportPath);
            _extent = new ExtentReports();
            _extent.AttachReporter(_htmlReporter);

            _extent.AddSystemInfo("Tester", "Jaideep Naidu");
            _extent.AddSystemInfo("Environment", "QA");
            _extent.AddSystemInfo("Browser", "Chrome");
            _extent.AddSystemInfo("Platform", Environment.OSVersion.Platform.ToString());
            _extent.AddSystemInfo(".NET Version", Environment.Version.ToString());
        }

        [BeforeFeature]
        public static void BeforeFeature(FeatureContext featureContext)
        {
            lock (_lock)
            {
                _feature = _extent.CreateTest<Feature>(featureContext.FeatureInfo.Title);
            }
        }

        [BeforeScenario]
        public void BeforeScenario()
        {
            _scenarioContext["WebDriver"] = _sharedDriver;

            lock (_lock)
            {
                _scenario = _feature.CreateNode<Scenario>(_scenarioContext.ScenarioInfo.Title);
            }
        }

        [AfterScenario]
        public void AfterScenario()
        {
            try
            {
                if (_scenarioContext.TestError != null)
                {
                    string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                    string screenshotFolder = Path.Combine(projectRoot, "Screenshots");
                    Directory.CreateDirectory(screenshotFolder);

                    string fileName = $"screenshot_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";
                    string filePath = Path.Combine(screenshotFolder, fileName);

                    Screenshot screenshot = ((ITakesScreenshot)_sharedDriver).GetScreenshot();
                    File.WriteAllBytes(filePath, screenshot.AsByteArray);

                    lock (_lock)
                    {
                        _scenario.Fail("Scenario failed. Screenshot below:").AddScreenCaptureFromPath(filePath);
                    }
                }
                else
                {
                    lock (_lock)
                    {
                        _scenario.Pass("Scenario passed");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error capturing screenshot: " + ex.Message);
            }
        }

        [AfterTestRun]
        public static void AfterTestRun()
        {
            _extent?.Flush();

            try
            {
                string projectRoot = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
                string reportPath = Path.Combine(projectRoot, "Reports", "ExtentReport.html");

                if (File.Exists(reportPath))
                {
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo()
                    {
                        FileName = reportPath,
                        UseShellExecute = true
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not open the ExtentReport automatically: " + ex.Message);
            }

            _sharedDriver?.Quit();
        }
    }
}
