
# EBooK LTD

Ebook ltd ASP.NET MVC 6 application, powered by Microsoft, demonstrating a single-process (monolithic) application architecture and deployment mode

## Installation

The first time you run the application, it will seed both databases with data such that you should see products in the store, and you should be able to log in using the admin@ebook.lk with Password: Ebook@1234
Admin password and email seed from "AppDBInitializer" "Data/AppDBInitializer.cs"


1. Ensure your connection strings in Program.cs point to a local SQL Server instance.

```bash
//DbContext configuration
builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(
    "server=127.0.0.1;user=ad_book;password=<password goes here>;port=3306;database=ad_book;Connect Timeout=5;",
    new MariaDbServerVersion(new Version(10, 4, 19))
));
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
4. Run the application
## Demo Visit

https://ebook.cirp.xyz

Admin Username: admin@ebook.lk

Password: Ebook@1234


## Screenshots

![App Screenshot](https://imgs.cirp.xyz/ebook/homepage.png)

