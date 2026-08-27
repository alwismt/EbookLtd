
# EBooK LTD

Ebook ltd ASP.NET Core MVC application targeting .NET 10, powered by Microsoft, demonstrating a single-process (monolithic) application architecture and deployment mode. Uses Entity Framework Core with PostgreSQL 16 (Npgsql).

## Installation

The first time you run the application, it will seed both databases with data such that you should see products in the store, and you should be able to log in using the admin@ebook.lk with Password: Ebook@1234
Admin password and email seed from "AppDBInitializer" "Data/AppDBInitializer.cs"


1. Configure PostgreSQL 16 and set `ConnectionStrings:DefaultConnection` in `ecom/appsettings.json` (or override via the `ConnectionStrings__DefaultConnection` environment variable / user secrets). Do not commit real passwords.

```json
"ConnectionStrings": {
  "DefaultConnection": "Host=127.0.0.1;Port=5432;Database=ad_book;Username=ad_book;Password=YOUR_PASSWORD;Timeout=5"
}
```

`Program.cs` registers the DbContext with:

```csharp
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        npgsql => npgsql.SetPostgresVersion(16, 0)));
```

 2. Ensure the tool EF was already installed
```bash
dotnet tool update --global dotnet-ef
```
3. Open a command prompt in the Web folder and execute the following commands:

```bash
dotnet restore
dotnet tool restore
```
4. Apply migrations to PostgreSQL, then run the application
## Demo Visit

https://ebook-dotnet.alwis.dev

Admin Username: admin@ebook.lk

Password: Ebook@1234


## Screenshots

![App Screenshot](https://imgs.cirp.xyz/ebook/homepage.png)
