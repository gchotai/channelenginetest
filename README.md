# C# .NET Assessment ChannelEngine

## 📖 Overview

- A .NET console application which can execute the business logic to the console output.
- An ASP.NET application, which can execute the business logic displays an HTML table with the results.

**Business Logic**
- Fetch all orders with status IN_PROGRESS from the API
- From these orders, compile a list of the top 5 products sold (product name, GTIN and total quantity), order these by the total quantity sold in descending order
- Pick one of the products from these orders and use the API to set the stock of this product to 25.
  
---

## 📂 Project Structure

This solution follows a **clean architecture** approach, separating concerns into different projects:

- ChannelEngine.Console *(Console App)*
- ChannelEngine.Core *(Shared Business Logic & Models)*
- ChannelEngine.Tests *(Unit Tests)*
- ChannelEngine.Web *(ASP.NET Web App)*

---

### ⚙️ Configure API Key in Web & Console App

Before you run project make sure to add Api Key.

Add your **API Key** to `appsettings.json`:

```json
{
  "ApiKey": "YOUR_API_KEY_HERE",
}
```

Or set it in `appsettings.Development.json` for local use.

---

## 🏗️ Future Improvements

- ✅ Add Logging with ILogger
- ✅ Implement Middleware-Based Exception Handling
- ✅ Improve UI/UX with a Modern Design
- ✅ Prepare Product Stock Update Page
