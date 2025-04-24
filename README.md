
# Herokuapp Test Automation Framework

This project is a C#-based automated testing framework using **SpecFlow**, **Selenium WebDriver**, **xUnit**, and **ExtentReports** for testing interactive features on the [Herokuapp demo site](https://the-internet.herokuapp.com/). It follows the BDD (Behavior Driven Development) approach using Gherkin syntax.

## 🛠 Technologies Used

- **.NET 6**
- **SpecFlow** (BDD framework)
- **Selenium WebDriver** (Web automation)
- **xUnit** (Test runner)
- **ExtentReports** (For rich test reports, auto-opens after test run)
- **WebDriverManager** (Driver management)
- **Hooks and Support Files** (For driver setup, report generation, screenshots)

## 📁 Project Structure

```
Herokuapp/
│
├── Features/               # Gherkin feature files
│   └── Herokuapp.feature
│
├── StepDefinitions/        # Step definitions for feature steps
│   └── HerokuappStepDefinitions.cs
│
├── Pages/                  # Page Object Model (POM) classes
│   └── (LoginPage.cs, AlertPage.cs, etc.)
│
├── Support/                # Hooks and utilities
│   ├── Hooks.cs
│   └── ExtentReportManager.cs
│
├── Reports/                # Output folder for Extent HTML reports
├── screenshots/            # Captured screenshots on failure
├── TestFiles/              # Files used for upload or download tests
└── Herokuapp.sln           # Solution file
```

## ✅ Features Covered

The test suite includes scenarios for:

- File upload
- iFrames interaction
- Alerts handling
- Dynamic controls
- Infinite scrolling
- Screenshot at the botton of the page

## 🧪 How to Run Tests

### 1. Clone the Repository
```bash
git clone https://github.com/your-username/Herokuapp-TestAutomation.git
cd Herokuapp
```

### 2. Restore Dependencies
```bash
dotnet restore
```

### 3. Run the Tests
```bash
dotnet test
```

> ✅ **Note**: The ExtentReport (`ExtentReport.html`) will automatically open after the test run.

## 🧩 Further Enhancements

- **Allure Reporting Integration** with tags, severity levels, and step annotations
- Cross-browser testing support
- Integration with CI/CD tools like GitHub Actions or Azure DevOps
- Dockerized test execution
- Enhanced PageFactory or FluentPage pattern
- Logging framework (e.g., Serilog, NLog)
- Data-driven testing using external files (CSV, JSON, Excel)

