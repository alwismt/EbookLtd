# EBooK LTD

Ebook ltd ASP.NET Core MVC application targeting .NET 10, powered by Microsoft, demonstrating a single-process (monolithic) application architecture and deployment mode. Uses Entity Framework Core with PostgreSQL 16 (Npgsql).

## Installation

The first time you run the application, it seeds admin users and roles so you can log in as **admin@ebook.lk** with password **Ebook@1234**. Seeding is defined in `ecom/Data/AppDBInitializer.cs`.

### 1. Install .NET 10.x

Install the .NET 10 SDK from https://dotnet.microsoft.com/download/dotnet/10.0.

Confirm the version:

```bash
dotnet --version
```

It should report `10.x`.

### 2. Create the PostgreSQL database

Create a PostgreSQL 16 user and database matching the default connection:

```sql
CREATE USER ad_book WITH PASSWORD 'password';
CREATE DATABASE ad_book OWNER ad_book;
```

### 3. Set the connection string

Choose one of the following.

**Option A — `appsettings.json`**

Edit `ecom/appsettings.json` and replace `YOUR_PASSWORD` with your database password.

```
"ConnectionStrings": {
  "DefaultConnection": "Host=127.0.0.1;Port=5432;Database=ad_book;Username=ad_book;Password=YOUR_PASSWORD;Timeout=5"
}
```

**Option B — User secrets (preferred for local development)**

```bash
dotnet user-secrets init --project ecom/ecom.csproj
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
"Host=127.0.0.1;Port=5432;Database=ad_book;Username=ad_book;Password=password" \
--project ecom/ecom.csproj
```

### 4. Install EF Core tools

```bash
dotnet tool install --global dotnet-ef --version 10.0.11
```

If the tool is already installed at a different version, update it:

```bash
dotnet tool update --global dotnet-ef --version 10.0.11
```

### 5. Restore, migrate, and run

From the repository root:

```bash
dotnet restore ecom/ecom.csproj
dotnet ef database update --project ecom/ecom.csproj
dotnet run --project ecom/ecom.csproj
```

Then open http://localhost:5000 or http://localhost:7290.

## Demo Visit

https://ebook-dotnet.alwis.dev

Admin Username: admin@ebook.lk

Password: Ebook@1234


## Screenshots

![App Screenshot](https://imgs.cirp.xyz/ebook/homepage.png)
