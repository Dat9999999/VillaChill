# 🏨 ReservationApp (VillaChill)

A web-based villa reservation system built with **ASP.NET Core MVC** following **Clean Architecture**. The system allows customers to book villas, make payments, and check in via QR code, while villa owners can track revenue and feedback in real-time through an interactive dashboard.

---

## ✨ Key Features

- 📊 **Real-time Dashboard**  
  Live tracking of revenue, bookings, and customer feedback through interactive charts.

- 💳 **Integrated Payment Gateway (VNPay)**  
  Secure and seamless online payment processing via VNPay.

- 🧠 **Sentiment Analysis for Reviews**  
  Automatically classifies customer feedback into positive/negative sentiments.

- 📲 **QR Code Check-in System**  
  Generates and scans QR codes for smooth guest check-in at the villa.

- 📧 **Automated Email Reports**  
  Sends weekly/monthly booking and revenue summaries to villa owners.

- 📄 **Dynamic Invoice Generation**  
  Exports invoices with booking data and charts in `.docx` format.

- 🔐 **Authentication & Authorization**  
  Role-based access control for Guest, Villa Owner, and Admin using ASP.NET Identity.

- 🛠️ **Full CRUD Operations**  
  Complete Create/Read/Update/Delete for key entities: User, Villa, Booking, Review, Invoice...

---

## 🧰 Tech Stack

- **Backend:** ASP.NET Core MVC, C#, Entity Framework Core, LINQ  
- **Frontend:** Razor View, HTML/CSS/JavaScript, Bootstrap  
- **Database:** Microsoft SQL Server  
- **Others:** ASP.NET Identity, VNPay API, QRCoder, Chart.js, Xceed.Words.NET, SMTP

---

 Notes

Admin, Villa Owner, and Guest roles are seeded by default
You need to configure settings before run this project (e.g., connection strings, SMTP, VNPay) are located in appsettings.json

## 🚀 Getting Started

To run the project locally:

```bash
# Step 1: Navigate to the web project folder
cd ReservationApp.Web

# Step 2: Apply database migrations
dotnet ef database update

# Step 3: Run the application
dotnet run
