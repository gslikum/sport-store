# SportsStore Blazor Web App (.NET 10)

A modern, high-performance e-commerce storefront adapted from the classic **SportsStore** application in *"Pro ASP.NET Core MVC 2" by Adam Freeman*, built using **Blazor Web App** architecture on **.NET 10** with Entity Framework Core and SQLite.

This project features Clean Architecture separation of concerns, In-Memory caching, real-time reactive SignalR bindings, and complete ASP.NET Core Identity authentication.

---

## 🚀 Quick Start

### 📋 Prerequisites
*   [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) or higher.

### 🏃 Running the Application
1.  Navigate to the solution directory:
    ```bash
    cd SportsStore
    ```
2.  Restore dependencies and build the solution:
    ```bash
    dotnet build
    ```
3.  Run the server host project:
    ```bash
    dotnet run --project SportsStore
    ```
4.  Open your browser and navigate to `https://localhost:5001` (or the port specified in the console).

### 🔑 Seeding and Test Accounts
The database is pre-seeded with catalog items and identity security roles on startup:
*   **Customer Storefront:** Accessible immediately at `/` (catalog, categories, shopping cart, checkout).
*   **Admin Dashboard:** Accessible at `/admin` (CRUD management for products and order dispatch status).
*   **Default Admin Account:**
    *   **Email/Username:** `admin@sportsstore.com`
    *   **Password:** `Password123!`

### 🧪 Running Tests
The solution includes a xUnit and bUnit testing suite verifying the domain logic and component rendering:
```bash
dotnet test
```

---

## 🛠️ Tech Stack & Layered Design

*   **UI Framework:** Blazor Web App (.NET 10) utilizing **InteractiveServer** WebSocket rendering (OSI Layer 7 over TCP).
*   **Styling:** Custom-designed dark-mode glassmorphic theme with **Outfit** typography.
*   **Data Access:** Entity Framework Core utilizing **SQLite** for development database storage.
*   **Caching:** In-Memory Caching (`IMemoryCache`) implemented via the **Decorator Pattern** to optimize page loading throughput.
*   **Security:** Role-Based Access Control (RBAC) via **ASP.NET Core Identity** to secure administrative endpoints.



## 📂 Directory Structure

```text
├── SportsStore/                  # Application Code
│   ├── SportsStore/              # Server-Side Host (Prerendering, Database, DI)
│   │   ├── Components/           # Blazor Pages, Layouts, & Custom Components
│   │   │   ├── Pages/            # Home (Catalog), Cart, Checkout, Admin Panels
│   │   │   └── Layout/           # Main Layout & Headers
│   │   ├── Data/                 # ApplicationDbContext, Migrations, & Seed Utilities
│   │   └── Program.cs            # Server Middleware & Service DI Registry
│   ├── SportsStore.Client/       # WebAssembly Client-Side Support
│   │   ├── Models/               # Normalized Domain Models (Product, Category, Order)
│   │   └── Services/             # Cart State Service
│   └── SportsStore.Tests/        # Test Suite (xUnit Domain and bUnit Component tests)
├── InterviewPrep/                # Architecture and System Design Guides
├── design.md                     # Project Migration Design Document
└── README.md                     # Project Overview (this file)
```
