# 🛍️ ShopQA Master Framework

> A production-grade, end-to-end test automation framework covering Desktop, Web, API, Security, Performance, and Accessibility — built with C# and modern QA tooling.

[![Desktop Tests](https://img.shields.io/github/actions/workflow/status/your-username/ShopQA/desktop-tests.yml?label=Desktop%20Tests&logo=windows&logoColor=white)](https://github.com/your-username/ShopQA/actions)
[![Web Tests](https://img.shields.io/github/actions/workflow/status/your-username/ShopQA/web-tests.yml?label=Web%20Tests&logo=playwright&logoColor=white)](https://github.com/your-username/ShopQA/actions)
[![API Tests](https://img.shields.io/github/actions/workflow/status/your-username/ShopQA/api-tests.yml?label=API%20Tests&logo=postman&logoColor=white)](https://github.com/your-username/ShopQA/actions)
[![Security Scan](https://img.shields.io/github/actions/workflow/status/your-username/ShopQA/security-tests.yml?label=Security%20Scan&logo=owasp&logoColor=white)](https://github.com/your-username/ShopQA/actions)
[![Allure Report](https://img.shields.io/badge/Allure-Report-orange?logo=testng&logoColor=white)](https://your-username.github.io/ShopQA)
[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](LICENSE)

---

## 📖 Overview

**ShopQA** is a comprehensive test automation framework built around a custom **WPF Shop Desktop Application** and the public [AutomationExercise](https://automationexercise.com) web/API target. It demonstrates a full modern QA stack — from BDD scenarios and contract testing to security scanning and cloud-native performance testing — all orchestrated via GitHub Actions CI/CD.

### Why this project?
- Demonstrates **end-to-end ownership**: from building the application under test to writing and running every layer of tests
- Covers **all major testing disciplines** in a single cohesive codebase
- Integrates **AI-assisted testing** with Microsoft Copilot throughout the workflow
- Reflects **2025–2026 industry trends**: TestOps, DevSecOps, API-first, accessibility, cloud-native performance

---

## 🗺️ Architecture

```
┌─────────────────────────────────────────────────────────────────┐
│                      ShopQA Framework                           │
├─────────────────┬───────────────────┬───────────────────────────┤
│  🖥️ Desktop     │  🌐 Web            │  🔌 API                   │
│  FlaUI + WPF    │  Playwright        │  MSTest + HttpClient      │
│  SQLite checks  │  Accessibility     │  Postman / Newman         │
├─────────────────┴───────────────────┴───────────────────────────┤
│                   🥒 BDD Layer — SpecFlow + Gherkin              │
│          Desktop features │ Web features │ API features          │
├──────────────────────────┬──────────────────────────────────────┤
│  🔐 Security              │  📦 Contract Testing                 │
│  OWASP ZAP                │  PactNet (Consumer / Provider)       │
├──────────────────────────┴──────────────────────────────────────┤
│  📊 Performance                                                  │
│  JMeter — load / endurance / stress + k6 — cloud-native         │
├─────────────────────────────────────────────────────────────────┤
│  ⚙️ CI/CD — GitHub Actions                                      │
│  PR → fast suite (UI + API)  │  Nightly → full suite + perf     │
├─────────────────────────────────────────────────────────────────┤
│  📈 Reporting — Allure TestOps → GitHub Pages                   │
├─────────────────────────────────────────────────────────────────┤
│  🤖 AI — Microsoft Copilot                                      │
│  Test generation │ Code review │ Documentation │ Data seeding    │
└─────────────────────────────────────────────────────────────────┘
```

---

## 🧰 Tech Stack

### Languages & Frameworks
| Area | Technology |
|------|-----------|
| Primary language | C# (.NET 8) |
| BDD | SpecFlow, Gherkin, Cucumber Reports |
| Unit / Integration | MSTest |
| Desktop UI Automation | FlaUI |
| Web UI Automation | Playwright |
| API Testing | HttpClient, RestSharp, Postman / Newman |
| Contract Testing | PactNet |
| Security Testing | OWASP ZAP |
| Performance | Apache JMeter, k6 |
| Accessibility | Playwright Accessibility API (WCAG 2.1) |

### Infrastructure & Tools
| Area | Technology |
|------|-----------|
| CI/CD | GitHub Actions |
| Reporting | Allure TestOps, Allure Report |
| Version Control | Git |
| Scripting | PowerShell |
| Database | SQLite + SQL |
| API Client | Postman |
| Network simulation | Clumsy |
| Virtualization | VM VirtualBox |
| AI Assistant | Microsoft Copilot |
| Config formats | JSON, XML |

---

## 📁 Project Structure

```
ShopQA/
├── src/
│   ├── ShopQA.App/                    # 🖥️ WPF application (system under test)
│   │   ├── Models/                    # Product, Order, User domain models
│   │   ├── Views/                     # LoginWindow, ProductListView, CartView
│   │   ├── ViewModels/                # MVVM: LoginVM, ProductListVM, CartVM
│   │   └── Data/                      # SQLite context + migrations
│   │
│   ├── ShopQA.Core/                   # 🔧 Shared framework foundation
│   │   ├── Config/                    # appsettings.json, XML config reader
│   │   ├── Helpers/                   # DataGenerator, DateHelper, FileHelper
│   │   ├── Models/                    # Shared DTO / response models
│   │   └── Database/                  # SQL query helpers, DB assertions
│   │
│   ├── ShopQA.Desktop.Tests/          # 🖥️ FlaUI desktop automation
│   │   ├── PageObjects/               # LoginPage, ProductListPage, CartPage
│   │   ├── Tests/                     # MSTest classes
│   │   └── Fixtures/                  # App launch / teardown base
│   │
│   ├── ShopQA.Web.Tests/              # 🌐 Playwright web automation
│   │   ├── PageObjects/               # POM for web UI
│   │   ├── Tests/                     # Functional web tests
│   │   └── Accessibility/             # ♿ WCAG 2.1 a11y tests
│   │
│   ├── ShopQA.API.Tests/              # 🔌 REST API test suite
│   │   ├── Clients/                   # Typed HttpClient wrappers
│   │   ├── Tests/                     # Endpoint tests (MSTest)
│   │   ├── Schemas/                   # JSON Schema validation files
│   │   └── Contracts/                 # 📦 PactNet consumer pacts
│   │
│   ├── ShopQA.BDD/                    # 🥒 SpecFlow BDD layer
│   │   ├── Features/
│   │   │   ├── Desktop/               # Login.feature, Cart.feature
│   │   │   ├── Web/                   # Search.feature, Checkout.feature
│   │   │   ├── API/                   # ProductAPI.feature, OrderAPI.feature
│   │   │   └── Security/              # AuthSecurity.feature
│   │   └── StepDefinitions/
│   │
│   └── ShopQA.Security.Tests/         # 🔐 OWASP ZAP security scans
│       ├── Scans/                     # ZAP scan configurations
│       └── Reports/                   # Generated security reports
│
├── performance/
│   ├── jmeter/                        # 📊 JMeter test plans (.jmx)
│   │   ├── load-test.jmx
│   │   ├── stress-test.jmx
│   │   └── endurance-test.jmx
│   └── k6/                            # ☁️ k6 cloud-native scripts
│       ├── load-test.js
│       ├── stress-test.js
│       └── spike-test.js
│
├── postman/                           # 📬 Postman collections & environments
│   ├── ShopQA.postman_collection.json
│   └── ShopQA.postman_environment.json
│
├── scripts/                           # ⚙️ PowerShell automation scripts
│   ├── setup-env.ps1                  # Install dependencies, configure environment
│   ├── run-all-tests.ps1              # Run full test suite locally
│   ├── seed-database.ps1              # Populate SQLite with test data
│   ├── run-zap-scan.ps1               # Trigger OWASP ZAP security scan
│   └── generate-report.ps1            # Build and publish Allure report
│
├── .github/
│   └── workflows/
│       ├── desktop-tests.yml          # FlaUI tests on PR
│       ├── web-tests.yml              # Playwright tests on PR
│       ├── api-tests.yml              # API + Contract tests on PR
│       ├── security-tests.yml         # OWASP ZAP on PR
│       ├── contract-tests.yml         # PactNet provider verification
│       └── nightly-performance.yml    # JMeter + k6 nightly
│
├── docs/
│   ├── architecture.md                # Detailed architecture decisions
│   ├── ai-copilot-usage.md            # 🤖 How Copilot was used
│   └── test-strategy.md               # Testing strategy & coverage map
│
└── README.md
```

---

## 🚀 Getting Started

### Prerequisites
- .NET 8 SDK
- Node.js 18+ (for k6 and Newman)
- Java 11+ (for JMeter)
- PowerShell 7+
- Docker (optional, for OWASP ZAP)

### Setup

```powershell
# Clone the repository
git clone https://github.com/your-username/ShopQA.git
cd ShopQA

# Run setup script (installs dependencies, configures environment)
./scripts/setup-env.ps1

# Seed the test database
./scripts/seed-database.ps1

# Run all tests
./scripts/run-all-tests.ps1
```

### Run specific test suites

```powershell
# Desktop tests only
dotnet test src/ShopQA.Desktop.Tests

# Web tests only
dotnet test src/ShopQA.Web.Tests

# BDD scenarios
dotnet test src/ShopQA.BDD

# API + Contract tests
dotnet test src/ShopQA.API.Tests

# Security scan
./scripts/run-zap-scan.ps1

# Performance — JMeter
jmeter -n -t performance/jmeter/load-test.jmx -l results/load.jtl

# Performance — k6
k6 run performance/k6/load-test.js
```

---

## 🥒 BDD Example

```gherkin
Feature: Shopping Cart — Desktop Application

  Background:
    Given the Shop Desktop application is launched
    And I am logged in as "testuser@shop.com"

  Scenario: Add product to cart and verify total
    Given I am on the Product List screen
    When I select product "Laptop" with price "999.99"
    And I click "Add to Cart"
    Then the cart should contain 1 item
    And the total price should display "999.99"
    And the database record should reflect the pending order

  Scenario Outline: Validate cart with multiple products
    Given I add "<product>" costing "<price>" to the cart
    Then the cart total should equal "<expected_total>"

    Examples:
      | product  | price  | expected_total |
      | Laptop   | 999.99 | 999.99         |
      | Mouse    | 29.99  | 1029.98        |
      | Keyboard | 79.99  | 1109.97        |
```

---

## 🔐 Security Testing

OWASP ZAP is integrated into the CI pipeline to perform automated security scanning on every PR targeting `main`.

**Checks performed:**
- SQL Injection
- Cross-Site Scripting (XSS)
- Broken authentication
- Sensitive data exposure
- Security misconfigurations

```powershell
# Run ZAP baseline scan locally
./scripts/run-zap-scan.ps1 -Target "http://localhost:5000" -ReportPath "./reports/zap-report.html"
```

---

## 📦 Contract Testing

Consumer-driven contract testing with **PactNet** ensures API contracts between the web frontend and backend API are never broken.

```
Consumer (Web Tests) → generates pact file → Provider (API) verifies
```

Pact files are stored in `/src/ShopQA.API.Tests/Contracts/` and verified on every CI run.

---

## ♿ Accessibility Testing

All key user journeys are validated against **WCAG 2.1 AA** standards using the Playwright Accessibility API.

```csharp
var accessibilityReport = await page.Accessibility.SnapshotAsync();
// Assertions on roles, labels, keyboard navigation
```

Violations are reported in Allure with severity levels and WCAG rule references.

---

## 📊 Performance Testing

### JMeter (Classic)
| Test Type | Users | Duration | Pass Threshold |
|-----------|-------|----------|----------------|
| Load | 100 | 10 min | p95 < 2s |
| Stress | 500 | 5 min | No errors < 1% |
| Endurance | 50 | 60 min | Memory stable |

### k6 (Cloud-Native)
```javascript
// performance/k6/load-test.js
export const options = {
  stages: [
    { duration: '2m', target: 100 },   // ramp up
    { duration: '5m', target: 100 },   // steady state
    { duration: '2m', target: 0 },     // ramp down
  ],
  thresholds: {
    http_req_duration: ['p(95)<2000'],
    http_req_failed: ['rate<0.01'],
  },
};
```

---

## 🤖 AI-Assisted Testing with Microsoft Copilot

Copilot was used throughout this project to accelerate and improve quality:

| Area | How Copilot helped |
|------|--------------------|
| Test case generation | Generated initial BDD .feature files from user stories |
| Code review | Identified edge cases and missing assertions |
| SQL data seeding | Generated seed scripts for 500+ test records |
| Documentation | Drafted architecture.md and test-strategy.md |
| Refactoring | Suggested Page Object structure improvements |

See [`docs/ai-copilot-usage.md`](docs/ai-copilot-usage.md) for detailed examples and prompts used.

---

## ⚙️ CI/CD Pipeline

```
On every PR:
  ✅ API Tests          (~2 min)
  ✅ Contract Tests     (~1 min)
  ✅ Web Tests          (~5 min)
  ✅ Desktop Tests      (~5 min)
  ✅ Security Scan      (~3 min)

Nightly (00:00 UTC):
  ✅ Full BDD Suite
  ✅ JMeter Performance
  ✅ k6 Performance
  ✅ Allure Report → published to GitHub Pages
```

---

## 📈 Test Reports

Live Allure TestOps report: **[View Report →](https://your-username.github.io/ShopQA)**

Reports include:
- Test execution history and trends
- Pass/fail rate per module
- Performance trend graphs
- Security scan summaries
- Accessibility violation breakdown

---

## 👩‍💻 Author

**Rosina Poloka** — Senior Test Automation Engineer  
10+ years experience in C# automation, desktop and web testing, CI/CD integration.

[![LinkedIn](https://img.shields.io/badge/LinkedIn-rosina--poloka-blue?logo=linkedin)](https://linkedin.com/in/rosina-poloka-8259207)
[![Email](https://img.shields.io/badge/Email-rosina.poloka%40gmail.com-red?logo=gmail)](mailto:rosina.poloka@gmail.com)

---

## 📄 License

This project is licensed under the MIT License — see [LICENSE](LICENSE) for details.
