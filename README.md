Banking System MVC Project
An ASP.NET Core MVC banking application developed during my internship at the National Bank of Egypt (NBE).
It simulates real-world banking workflows with a focus on clean backend architecture and a modern, responsive UI.

💡 Features
User Login / Session Management

Account Request workflow with Admin Approval

Post-approval Account Setup (type + currency)

Multi-Currency Transactions with automatic exchange-rate conversion

Validation rules: balance check, status check, duplicate/invalid accounts

Modern user Dashboard with transaction summary

Split Sent / Received Transactions views

Admin tool for Transaction Extraction via Stored Procedure

🔧 Tech Stack
Layer	Technology
Backend	ASP.NET Core MVC + C#
ORM	Entity Framework Core
Database	SQL Server
Front-End UI	Bootstrap 5 + Razor Pages
Extras	LINQ · Sessions · DTOs

🚀 Getting Started
Prerequisites
.NET 7 SDK

SQL Server

Visual Studio 2022+ (or any code editor)

Setup
Clone the repository

bash
Copy
Edit
git clone https://github.com/your-username/BankingSystemProject.git
Configure the database connection string
In appsettings.json, replace with your local SQL Server name:

json
Copy
Edit
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=BankingSystem;Trusted_Connection=True;Encrypt=False;"
}
⚠️ Replace YOUR_SERVER_NAME with your actual SQL Server instance name
For example: localhost, .\SQLEXPRESS, or DESKTOP-XYZ\SQLEXPRESS01

Apply migrations

bash
Copy
Edit
dotnet ef database update
Run the application

bash
Copy
Edit
dotnet run
📁 Project Structure
arduino
Copy
Edit
│
├── Models/             → Entity classes
├── ViewModels/         → Data transfer objects
├── Views/              → MVC Razor views
├── Controllers/        → Business logic
├── Migrations/         → EF Core migrations
└── wwwroot/            → Static assets (CSS, JS)


👤 Author
Rana Ali
Software Engineering Intern – National Bank of Egypt

🔐 Admin Login (for testing)
Use these credentials to access the admin approval dashboard: 
Email: admin@nbe.com
Password: Admin123!
