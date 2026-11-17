
************************************************************************************************
Transactions & Dispute Management Portal
************************************************************************************************

This is a full-stack banking portal for viewing transaction history and submitting dispute claims, built with Angulr 21 and .NET 8 Web API also backed by SQL Server.



*************************************************************************************************
Architecture Overview Summary
*************************************************************************************************

Frontend - transactions.portal.web : Angular 21 with standalne components, reactive signals, with custom SCSS styling, and centralized HTTP error handling.

Backend - transactions.portal.services : .NET 8 Web API with EF Core 8, repository pattern, and global exception and error - handling middleware.

Database: Microsoft SQL Server - database migrations and seed data are applied automatically on startup.

*************************************************************************************************
Running the project option.
*************************************************************************************************

Option 1: Running with Docker(I recommend this option)

Run with Docker Compose - Run the entire stack (SQL Server, Backend API, and Frontend Web) with just a single command.

Pre-requisites: Docker Desktop must be running.

Step 1 (This is Optional): Copy the environment configuration:
run cmd => cp .env.example .env


Step 2: Start the entire stack:
run cmd => docker compose up --build -d


Once running, access the services:

- Frontend Portal: http://localhost:4200
- Backend API / Swagger UI: http://localhost:8080/swagger



Option 2: Running Without Docker/ Running the Frontend & Backend Locally.



Step 1. Database - Configure your SQL server connection

The backend will automatically apply your db migrations and seed all sample transactions and disputes on startup. You just need to supply a connection string.

You can set the connection string using any of the methods below:

Via Environment Variable:

Apple macOS/Linux:

bash
export Database__SqlServer="Server=localhost,1433;Database=DisputePortal;User Id=sa;Password=YourPassword!;TrustServerCertificate=True;MultipleActiveResultSets=true;"

Windows Machine:

powershell
$env:Database__SqlServer="Server=localhost\SQLEXPRESS;Database=DisputePortal;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;"


Connection String Examples:

SQL Server Express - Windows Authentication
Server=localhost\SQLEXPRESS;Database=DisputePortal;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true;

Local or Docker SQL Server ("sa" account):
Server=localhost,1433;Database=DisputePortal;User Id=sa;Password=[PassHere];TrustServerCertificate=True;MultipleActiveResultSets=true;


Step 2: Start the Backend api

1. Navigate to the backend directory: 
   cd transactions.portal.services
2. Run the service: 
   dotnet run
3. The Services/API will start on: 
   http://localhost:5068

Step 3: Start the Frontend/Angular app.

1. Open a new terminal and navigate to the web directory: 
   cd transactions.portal.web
2. Install dependencies: 
   npm install
3. Start the angular dev server: 
   ng serve or npm start
4. Open the app in your browser: 
   http://localhost:4200


*************************************************************************************************
Running Tests
*************************************************************************************************

Backend Unit Tests:
1. Navigate to tests directory:
   cd transactions.portal.services/tests
2. Run:
   dotnet test

Frontend Unit Tests:
1. Navigate to web directory:
   cd transactions.portal.web
2. Run:
   npm test


*************************************************************************************************
Running Tests
*************************************************************************************************

Backend Unit Tests:
1. Navigate to tests directory:
   cd transactions.portal.services/tests
2. Run:
   dotnet test

Frontend Unit Tests:
1. Navigate to web directory:
   cd transactions.portal.web
2. Run:
   npm test
