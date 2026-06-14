# SportsStore Blazor Web App (.NET 10)

A modern, high-performance e-commerce storefront adapted from the classic **SportsStore** application in *"Pro ASP.NET Core MVC 2" by Adam Freeman*, built using **Blazor Web App** architecture on **.NET 10** with Entity Framework Core and SQLite.

This project is enhanced with advanced academic theories, design patterns, and engineering constraints derived from 12 MSIS graduate courses.

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

---

## 🎓 Academic Concepts & Interview Prep Guides

This application integrates core theories and design frameworks from our MSIS graduate program. Detailed, topic-specific explanations are located in the [InterviewPrep](file:///Users/gerrell/Documents/Sports%20Store/InterviewPrep) directory:

1.  **[Software Architecture (CIS 518/510/512)](file:///Users/gerrell/Documents/Sports%20Store/InterviewPrep/design_architecture.md):** Clean Architecture layers, SOLID principles, Dependency Inversion, and Decorator/Observer design patterns.
2.  **[Database Strategic Planning & ACID (CIS 515)](file:///Users/gerrell/Documents/Sports%20Store/InterviewPrep/database_strategy.md):** 3rd Normal Form schema, index optimization, and database transaction scopes guaranteeing ACID properties.
3.  **[Performance Engineering (CIS 555)](file:///Users/gerrell/Documents/Sports%20Store/InterviewPrep/performance_requirements.md):** Resolution of the ORM N+1 query problem, read-caching, and queue capacity planning ($M/M/1$ queues).
4.  **[Security Management & RBAC (CIS 502)](file:///Users/gerrell/Documents/Sports%20Store/InterviewPrep/security_management.md):** Identity AAA integration, RBAC boundaries, the CIA Triad, and PCI-DSS payment compliance.
5.  **[Network Protocols & Communication (CIS 505)](file:///Users/gerrell/Documents/Sports%20Store/InterviewPrep/network_communication.md):** Stateful WebSockets/SignalR connection circuits and UDP-multiplexed HTTP/3 QUIC protocol.
6.  **[Usability & HCI (CIS 524)](file:///Users/gerrell/Documents/Sports%20Store/InterviewPrep/usability_hci.md):** Shneiderman's Eight Golden Rules of interface design and WCAG POUR accessibility.
7.  **[Project Governance (BUS 517/CIS 554/599)](file:///Users/gerrell/Documents/Sports%20Store/InterviewPrep/project_governance.md):** WBS scope controls, RACI matrices, Earned Value Management, and Requirements Traceability.
8.  **[Layered System Architecture Diagram](file:///Users/gerrell/Documents/Sports%20Store/InterviewPrep/system_architecture.md):** Technical Mermaid diagram mapping request flows across all boundaries.

---

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
├── InterviewPrep/                # MSIS Graduate Course Alignment Guides
├── design.md                     # Initial Project Migration Design Document
└── README.md                     # Project Overview & Entry point (this file)
```
