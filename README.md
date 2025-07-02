# 🏨 ReservationApp

A web-based reservation system built using **Clean Architecture** with ASP.NET Core.

---

## ✨ Features

- 🔐 **Authentication & Authorization**  
  Secure login and role-based access with **ASP.NET Identity**.

- 💳 **VNPay Integration**  
  Seamless payment processing via **VNPay** gateway.

- 📁 **File Download**  
  Allow users to download important reservation-related files.

- 🔧 **CRUD Operations**  
  Full Create, Read, Update, Delete functionality for core entities.

- 🚀 **Easy to Deploy & Test**  
  Ready-to-run with simple setup for local or production environments.

---

## 🚀 Getting Started

Follow these steps to run the project locally:

```bash
# Step 1: Navigate to the web project folder
cd ReservationApp.Web

# Step 2: Apply database migrations
dotnet ef database update

# Step 3: Run the application
dotnet run
