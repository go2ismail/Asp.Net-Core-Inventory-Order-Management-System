![indotalent-whms](https://cdn.shopify.com/s/files/1/0097/8422/9945/files/product_hunt_inventory_wms_2.png?v=1738303996)
<a href="https://www.producthunt.com/posts/indotalent-wms?embed=true&utm_source=badge-featured&utm_medium=badge&utm_souce=badge-indotalent&#0045;wms" target="_blank"><img src="https://api.producthunt.com/widgets/embed-image/v1/featured.svg?post_id=836427&theme=light&t=1738304283591" alt="INDOTALENT&#0032;WMS - Free&#0032;and&#0032;open&#0045;source&#0032;powerful&#0032;inventory&#0032;management&#0032;system | Product Hunt" style="width: 250px; height: 54px;" width="250" height="54" /></a>
# 🔥 Introduction  
Welcome to our **Warehouse Inventory Management System (WHMS)**, an innovative and scalable solution meticulously crafted to streamline and optimize your inventory operations.  

Now powered by **ASP.NET Core 10.0**, the latest cutting-edge technology from Microsoft, WHMS is faster than ever and showcases the future of modern web development. With its fully decoupled **headless API** architecture, WHMS enables seamless integration between the back end and front end, offering unparalleled flexibility and performance.  

The back end is built using **Clean Architecture**, **CQRS**, **MediatR**, and the **Repository Pattern**, ensuring maintainability and scalability for enterprise-grade applications. On the front end, **ASP.NET Core Razor Pages** and **Vue.js** come together to create a dynamic and user-friendly interface.  

## Key Features  
WHMS provides a comprehensive suite of capabilities:  
- **Sales, Purchase, Delivery, and Goods Receive**  
- **Transfer, Adjustment, Return, and Scrapping**  
- **Stock Count and Detailed Reporting Functionalities**  

🎯 **Challenge Yourself!**  
Experience the blazing speed and efficiency of **.NET 10**, the latest breakthrough in performance from Microsoft. Explore our **live online demo** today and see firsthand how WHMS can revolutionize your inventory management operations.  



# Monolithic Clean Architecture  

WHMS is built using a **Monolithic Clean Architecture** approach, ensuring a structured and simplified development process. By keeping all components within a single codebase, dependency management is streamlined, eliminating the risk of a **dependency nightmare**. This approach consolidates all dependencies in one place, ensuring compatibility and coherence across the entire system.  

Additionally, it simplifies **deployment**, as all code resides in a single repository with a well-optimized pipeline, reducing complexity. The **cohesive project structure** provides a clear and consistent source code pattern, making it easier for developers to understand and maintain the system. With the combination of Clean Architecture, CQRS, and MediatR, WHMS delivers a **scalable, maintainable, and enterprise-ready solution**.



# Technical Features
- **ASP.NET Core 10.0 Headless API** (Back End)
  - Clean Architecture
  - CQRS with MediatR
  - Repository Pattern
  - Entity Framework Core (EF Core) for data access
  - AutoMapper for object mapping
  - FluentValidation for input validation
  - Serilog for logging
  - Support for file uploads and downloads (images/documents)
  - Secure authentication and authorization with ASP.NET Identity + JWT
- **ASP.NET Core Razor Pages with a Simple & Modern UI** (Front End)  
  - **Effortless** dynamic interactivity using Vue.js **without any build system**  
  - **Ready-to-use** industry-leading Syncfusion UI components (free community edition)  
  - **Lightweight and straightforward** API communication with Axios  
  - **Easy-to-customize** responsive UI powered by the AdminLTE template  




# Functional Features
- **Customer Management**
  - Customer Group, Category, Details, and Contacts
- **Sales Management**
  - Sales Order, Sales Return, Sales Reports
- **Vendor Management**
  - Vendor Group, Category, Details, and Contacts
- **Purchase Management**
  - Purchase Order, Purchase Return, Purchase Reports
- **Warehouse Operations**
  - Unit Measure, Product Group, Products
  - Delivery Order, Goods Receive
  - Transfers, Adjustments, Scrapping, Stock Counts
- **Reporting**
  - Transaction Report, Stock Report, Movement Report
- **System Settings**
  - Company Settings, Tax Configuration, User Management
  - Number Sequence for systematic tracking
- **Analytics and Logs**
  - Error Logs, Analytic Logs
- **Authentication & Membership**
  - Secure user authentication and role-based access control



# Demo Mode and Production Configuration

The application has two explicit modes, controlled by the `IsDemoVersion` setting:

| Mode | `IsDemoVersion` | Credentials | Demo data |
|---|---|---|---|
| Demo (default) | `true` | the documented demo key and `admin@root.com` / `123456` from `appsettings.json` | seeded |
| Production | `false` | `Jwt__Key` and `AspNetIdentity__DefaultAdmin__Password` from the host | not seeded |

**A fresh clone runs in demo mode.** It starts without a `.env` file and without any environment variables, creates the database, seeds the demo data, and accepts the documented demo credentials. The application logs a prominent warning at start up while demo mode is enabled.

> **Never use demo mode with real data.** If `IsDemoVersion=true` is left enabled in production, the published `123456` administrator password and the demo signing key stay valid. A production deployment must set `IsDemoVersion=false` and provide the required secrets.

## Production secrets

`appsettings.json` keeps only settings that are safe to publish. The two deployment secrets are read from the first source that provides them, highest precedence first:

| Setting | Secret | Why |
|---|---|---|
| `Jwt:Key` | signing key of the access tokens | anybody who knows it can mint a token with any role |
| `AspNetIdentity:DefaultAdmin:Password` | password of the administrator account that the first start up creates | every installation would otherwise share the same known password |

| Source | How to set it | Typical use |
|---|---|---|
| Environment variable | `Jwt__Key=<secret>` (`__` is the section separator) | containers, CI/CD, production |
| `.env` file | `Jwt__Key=<secret>` in `<repo>/.env` (or `Presentation/ASPNET/.env`) | local development, Docker Compose |
| `dotnet user-secrets` | `dotnet user-secrets set "Jwt:Key" "<secret>" --project Presentation/ASPNET` | local development without a file |

Generate the secrets and switch the application to production mode with one command (creates or updates `<repo>/.env`):

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/new-secrets.ps1
```

The script writes `IsDemoVersion=false`, a random `Jwt__Key` of at least 32 bytes, a random `AspNetIdentity__DefaultAdmin__Password` of at least 12 characters, and a random `MSSQL_SA_PASSWORD` for Docker Compose. `docker-compose.yml` passes the SQL Server password to both containers, so it is never stored in `appsettings.Docker.json`. The individual helpers `scripts/new-jwt-key.ps1` and `scripts/new-admin-password.ps1` remain available when only one value needs to change.

Or, on Linux and macOS:

```bash
{
  echo "IsDemoVersion=false"
  echo "Jwt__Key=$(openssl rand -base64 48 | tr '+/' '-_' | tr -d '=')"
  echo "AspNetIdentity__DefaultAdmin__Password=$(openssl rand -base64 24 | tr -d '/+=' | cut -c1-20)"
  echo "MSSQL_SA_PASSWORD=$(openssl rand -base64 24 | tr -d '/+=' | cut -c1-20)"
} > .env
```

Facts that matter when you operate the application:

- **Demo mode is the default and is intentionally public.** `appsettings.json` contains the documented demo key and `admin@root.com` / `123456`. Demo mode accepts them, keeps the password unchanged, and seeds the demo data.
- **Production mode rejects the published values.** With `IsDemoVersion=false`, the demo JWT key, an empty key, and a key shorter than 32 bytes stop the start up. The published administrator password, an empty password, and a password shorter than 12 characters are rejected too.
- **Environment variables always win.** The `.env` file only fills the gaps, so a container, a CI job, or a production host can override it without editing files.
- **`.env` is never committed.** It is listed in `.gitignore` and in `.dockerignore`; only `.env.example` (documentation with placeholder values) is in the repository.
- **Rotating the key signs everybody out.** Access tokens that were issued before the change no longer validate, which is exactly how a leaked key is taken out of circulation.
- **Authorization is checked against the database.** Every authenticated request resolves the account and its current roles, so a deleted, blocked, or demoted user loses access immediately instead of keeping the roles that were stored in the token.
- **Existing production installations rotate the published password automatically.** When the stored administrator account still accepts `123456`, the application replaces it with `AspNetIdentity__DefaultAdmin__Password` at the next start up and logs a warning. An installation in that state refuses to start when no replacement password is configured.
- **The public live demo keeps `123456`.** Demo mode does not rotate the documented credential. Set `IsDemoVersion=false` before storing any real data.

> **Upgrading an existing production installation (JWT key):** the demo key that earlier releases shipped is rejected when `IsDemoVersion=false`, the application stops at start up and asks for a real one. Generate a key, set it on the server, and reload the site. Every user has to sign in again because tokens that were signed with the old key stop validating.

> **Upgrading an existing production installation (administrator password):** set `IsDemoVersion=false`, `AspNetIdentity__DefaultAdmin__Password` (and `AspNetIdentity__DefaultAdmin__Email` when the account uses another address) before starting the new version. The account keeps working, but the password that the repository published is replaced on the next start up.

### Docker demo stack

To try the application without installing SQL Server or creating any secret, run the self-contained demo stack:

```bash
docker compose -f docker-compose.demo.yml up --build
```

Open `http://localhost:5000` and sign in with `admin@root.com` / `123456`. The stack runs with `IsDemoVersion=true`, the demo key, and demo data. Its SQL Server password defaults to a published demo value; set `MSSQL_SA_PASSWORD` to override it. Do not store real data in it, and do not use it as a production deployment.

### Uploaded files

The document and image endpoints only serve file names that were generated on upload. A name with a directory part, a drive, or a parent directory segment (`../../appsettings.json`, `C:\Windows\win.ini`, `/etc/passwd`) is rejected with `400 Bad Request`, and the storage service additionally verifies that the canonical path is a child of the configured `PathFolder` before it reads a byte. Keep `FileImageManager:PathFolder` and `FileDocumentManager:PathFolder` outside the folders that hold configuration files.

### Known follow-ups

- **Static uploaded files:** uploads still live under `wwwroot/app_data`, so a file is reachable without authentication when its generated GUID name is known. The API endpoints enforce authentication; moving the storage folders outside the web root is a separate migration.
- **Regular user password policy:** production only enforces the 12-character minimum for the administrator. The global Identity policy in `appsettings.json` still allows 6-character user passwords; raising it is a separate change.
- **AutoMapper license:** `AutoMapper` is on 16.3.0 and is distributed under RPL 1.5 or a commercial license; this project uses the RPL 1.5 option. The denial-of-service advisory GHSA-rvv3-g6hj-g44x is fixed in this version.
- The `MailKit`/`MimeKit` (4.18.2) and `System.Security.Cryptography.Pkcs` (10.0.14) references contain the current security fixes.

---

# Run The Project: Visual Studio  

Getting started is **easy**! Thanks to **Monolithic Clean Architecture**, everything is structured and streamlined. Plus, even though this project uses a **modern JavaScript framework like Vue.js**, it **does not require a build system**—just use Visual Studio to run and build effortlessly.  

1. Open the project using Visual Studio.  
2. Update the connection string in `appsettings.json` to match your SQL Server database, if needed.
3. **Demo mode needs no additional setup.** `IsDemoVersion` is `true` by default, so the documented demo key and `admin@root.com` / `123456` are used and the demo data is seeded. Do not create a `.env` file for the demo.
4. **For a production database, run the one-command setup** (see [Demo Mode and Production Configuration](#demo-mode-and-production-configuration)):
   - `powershell -NoProfile -ExecutionPolicy Bypass -File scripts/new-secrets.ps1`
   - It writes `IsDemoVersion=false`, `Jwt__Key`, and `AspNetIdentity__DefaultAdmin__Password` to `<repo>/.env`.
   - The application refuses to start with demo values when `IsDemoVersion=false`, and the start up error explains how to set them.
5. Clean and build the solution:
   - Right-click the solution > Clean
   - Right-click the solution > Build
6. Run the project:
   - Click the green "play" button in the Visual Studio toolbar.  

> **Note**: The database will be created automatically if it does not exist.  





# Run The Project: IIS Web Server  

Deployment is **easy**! Thanks to **Monolithic Clean Architecture**, both the front end and back end are in a single deployable unit, making the process straightforward.  

For deployment to IIS:  

1. **Publish the project**:  
   - Right-click the project > Publish.  
   - Choose a folder to output the files.  

2. **Deploy to IIS**:  
   - Copy the published files to your IIS directory.  
   - Configure IIS to point to the folder and ensure database connectivity.  

3. **Disable demo mode and set the secrets on the server** (see [Demo Mode and Production Configuration](#demo-mode-and-production-configuration)). The published `appsettings.json` contains the public demo values, so a production deployment must provide `IsDemoVersion=false`, a real `Jwt__Key`, and a real `AspNetIdentity:DefaultAdmin:Password` through the environment of the application pool or through the `web.config` that is generated by the publish:

   ```xml
   <aspNetCore processPath="dotnet" arguments=".\ASPNET.dll" stdoutLogEnabled="false" hostingModel="inprocess">
     <environmentVariables>
       <environmentVariable name="IsDemoVersion" value="false" />
       <environmentVariable name="Jwt__Key" value="PASTE_A_RANDOM_SECRET_OF_AT_LEAST_32_BYTES" />
       <environmentVariable name="AspNetIdentity__DefaultAdmin__Password" value="PASTE_A_PASSWORD_OF_AT_LEAST_12_CHARACTERS" />
     </environmentVariables>
   </aspNetCore>
   ```

   Keep `web.config` out of version control when it contains the real secrets, and give every deployment its own values.

> For detailed instructions, refer to [Microsoft's official IIS deployment documentation](https://learn.microsoft.com/en-us/aspnet/core/tutorials/publish-to-iis).  




# Unit Tests

Fast regression tests for `TokenSettingsValidation`, `DefaultAdminCredentialPolicy`, and `StoragePathGuard` (empty, demo, production, traversal, and valid values):

```powershell
dotnet test Tests/Unit/Unit.csproj -c Debug
```

# E2E Tests (Playwright)

End-to-end tests use **Playwright for .NET** with **NUnit** (`Tests/E2E`). They exercise login, warehouse/customer/vendor/product CRUD, and UI smoke flows against a locally started application.

Test fixtures:
- `LoginTests` - valid and invalid credentials.
- `FileDownloadSecurityTests` - the document and image endpoints reject names that try to leave the storage folder.
- `UiSmokeTests` - home, login, profile, warehouse list, and dashboard pages.
- `WarehouseCrudTests`, `CustomerCrudTests`, `VendorCrudTests`, `ProductCrudTests` - required-name validation plus create, update, and delete through the list page modal.

Prerequisites:
- .NET 10 SDK
- SQL Server Express running as `localhost\SQLEXPRESS` with a `dev`/`dev` login (the tests create/use the `WHMS-LTE-FS-E2E` database; if `dev` lacks `dbcreator`, the existing `WHMS-LTE-FS` database is used)
- Demo seeding enabled (`IsDemoVersion=true`)
- No secret is needed: the fixture starts the application with a random `Jwt__Key` and a test-only `AspNetIdentity__DefaultAdmin__Password` for the local test database, so the suite never depends on a committed or local secret. An existing `WHMS-LTE-FS-E2E` database whose administrator still uses the password of an earlier release is rotated by the application at start up.

Run everything with one command:

```powershell
powershell -NoProfile -ExecutionPolicy Bypass -File scripts/run-e2e.ps1
```

The script installs the Chromium browser, builds the solution in `Debug`, starts the app on `http://127.0.0.1:5187`, and runs all tests. Set `E2E_BASE_URL` to test an already running instance, or `E2E_DB_NAME` to use a different database.

# Live Demo Link
- **URL**: [https://whms-lte-fs.csharpasp.net/](https://whms-lte-fs.csharpasp.net/)
- ✅ **Username**: admin@root.com  
- ✅ **Password**: 123456  

> **Demo credentials only.** They work while `IsDemoVersion=true` and must never be used for a production deployment. Production requires `IsDemoVersion=false` plus a deployment specific admin password and JWT key; see [Demo Mode and Production Configuration](#demo-mode-and-production-configuration).




# Acknowledgments

This project uses the following open-source and community resources:

- [Syncfusion Community Edition](https://www.syncfusion.com/products/communitylicense) - For advanced UI components.  
- [AdminLTE](https://github.com/ColorlibHQ/AdminLTE) - Licensed under the MIT License.  

We thank these projects for their contributions to the developer community.



# License

This work is licensed under a [Creative Commons Attribution 4.0 International License (CC BY 4.0)](http://creativecommons.org/licenses/by/4.0/).  
To view a copy of this license, visit [http://creativecommons.org/licenses/by/4.0/](http://creativecommons.org/licenses/by/4.0/).

Users are required to retain the footer link in all instances of this software as part of the attribution requirement under CC BY 4.0.



# Starred  
If you find this project useful, please consider supporting us by giving a star on GitHub! ⭐  
Your support keeps us motivated and helps us continue developing and improving this project to deliver even greater value to the community.  


# 🌱 Help Us Grow!  
Please help us grow by sharing it with your friends, family, and colleagues!  
Every share helps us reach more people, grow the community, and continue improving this open-source project. ❤️  
 


# One Last Kiss ❤️  

Thanks for stopping by and reading this far! This project is supported by **INDOTALENT**

🌐 Feel free to visit: [https://indotalent.com](https://indotalent.com)  

[![store-indotalent](https://indotalent.com/img/indotalent-hero.png)](https://indotalent.com)







