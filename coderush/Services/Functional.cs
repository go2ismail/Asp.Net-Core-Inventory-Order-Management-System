using coderush.Data;
using coderush.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace coderush.Services
{
    public class Functional : IFunctional
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IRoles _roles;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;

        public Functional(UserManager<ApplicationUser> userManager,
           RoleManager<IdentityRole> roleManager,
           ApplicationDbContext context,
           SignInManager<ApplicationUser> signInManager,
           IRoles roles,
           IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
            _roles = roles;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
        }

      

        public async Task InitAppData()
        {
            try
            {
               
                await _context.BillType.AddAsync(new BillType { BillTypeName = "Default" });
                await _context.SaveChangesAsync();

                await _context.Branch.AddAsync(new Branch { BranchName = "Default" });
                await _context.SaveChangesAsync();

                await _context.Warehouse.AddAsync(new Warehouse { WarehouseName = "Default" });
                await _context.SaveChangesAsync();

                await _context.CashBank.AddAsync(new CashBank { CashBankName = "Default" });
                await _context.SaveChangesAsync();

                await _context.Currency.AddAsync(new Currency { CurrencyName = "Default", CurrencyCode = "USD" });
                await _context.SaveChangesAsync();

                await _context.InvoiceType.AddAsync(new InvoiceType { InvoiceTypeName = "Default" });
                await _context.SaveChangesAsync();

                await _context.PaymentType.AddAsync(new PaymentType { PaymentTypeName = "Default" });
                await _context.SaveChangesAsync();

                await _context.PurchaseType.AddAsync(new PurchaseType { PurchaseTypeName = "Default" });
                await _context.SaveChangesAsync();

                await _context.SalesType.AddAsync(new SalesType { SalesTypeName = "Default" });
                await _context.SaveChangesAsync();

                await _context.ShipmentType.AddAsync(new ShipmentType { ShipmentTypeName = "Default" });
                await _context.SaveChangesAsync();

                await _context.UnitOfMeasure.AddAsync(new UnitOfMeasure { UnitOfMeasureName = "PCS" });
                await _context.SaveChangesAsync();

                await _context.ProductType.AddAsync(new ProductType { ProductTypeName = "Default" });
                await _context.SaveChangesAsync();

                List<Product> products = new List<Product>() {
                    new Product{ProductName = "Chai"},
                    new Product{ProductName = "Chang"},
                    new Product{ProductName = "Aniseed Syrup"},
                    new Product{ProductName = "Chef Anton's Cajun Seasoning"},
                    new Product{ProductName = "Chef Anton's Gumbo Mix"},
                    new Product{ProductName = "Grandma's Boysenberry Spread"},
                    new Product{ProductName = "Uncle Bob's Organic Dried Pears"},
                    new Product{ProductName = "Northwoods Cranberry Sauce"},
                    new Product{ProductName = "Mishi Kobe Niku"},
                    new Product{ProductName = "Ikura"},
                    new Product{ProductName = "Queso Cabrales"},
                    new Product{ProductName = "Queso Manchego La Pastora"},
                    new Product{ProductName = "Konbu"},
                    new Product{ProductName = "Tofu"},
                    new Product{ProductName = "Genen Shouyu"},
                    new Product{ProductName = "Pavlova"},
                    new Product{ProductName = "Alice Mutton"},
                    new Product{ProductName = "Carnarvon Tigers"},
                    new Product{ProductName = "Teatime Chocolate Biscuits"},
                    new Product{ProductName = "Sir Rodney's Marmalade"}

                };
                await _context.Product.AddRangeAsync(products);
                await _context.SaveChangesAsync();

                await _context.CustomerType.AddAsync(new CustomerType { CustomerTypeName = "Default" });
                await _context.SaveChangesAsync();

                List<Customer> customers = new List<Customer>() {
                    new Customer{CustomerName = "Hanari Carnes", Address = "Rua do Paço, 67"},
                    new Customer{CustomerName = "HILARION-Abastos", Address = "Carrera 22 con Ave. Carlos Soublette #8-35"},
                    new Customer{CustomerName = "Hungry Coyote Import Store", Address = "City Center Plaza 516 Main St."},
                    new Customer{CustomerName = "Hungry Owl All-Night Grocers", Address = "8 Johnstown Road"},
                    new Customer{CustomerName = "Island Trading", Address = "Garden House Crowther Way"},
                    new Customer{CustomerName = "Königlich Essen", Address = "Maubelstr. 90"},
                    new Customer{CustomerName = "La corne d'abondance", Address = "67, avenue de l'Europe"},
                    new Customer{CustomerName = "La maison d'Asie", Address = "1 rue Alsace-Lorraine"},
                    new Customer{CustomerName = "Laughing Bacchus Wine Cellars", Address = "1900 Oak St."},
                    new Customer{CustomerName = "Lazy K Kountry Store", Address = "12 Orchestra Terrace"},
                    new Customer{CustomerName = "Lehmanns Marktstand", Address = "Magazinweg 7"},
                    new Customer{CustomerName = "Let's Stop N Shop", Address = "87 Polk St. Suite 5"},
                    new Customer{CustomerName = "LILA-Supermercado", Address = "Carrera 52 con Ave. Bolívar #65-98 Llano Largo"},
                    new Customer{CustomerName = "LINO-Delicateses", Address = "Ave. 5 de Mayo Porlamar"},
                    new Customer{CustomerName = "Lonesome Pine Restaurant", Address = "89 Chiaroscuro Rd."},
                    new Customer{CustomerName = "Magazzini Alimentari Riuniti", Address = "Via Ludovico il Moro 22"},
                    new Customer{CustomerName = "Maison Dewey", Address = "Rue Joseph-Bens 532"},
                    new Customer{CustomerName = "Mère Paillarde", Address = "43 rue St. Laurent"},
                    new Customer{CustomerName = "Morgenstern Gesundkost", Address = "Heerstr. 22"},
                    new Customer{CustomerName = "Old World Delicatessen", Address = "2743 Bering St."}
                };
                await _context.Customer.AddRangeAsync(customers);
                await _context.SaveChangesAsync();

                await _context.VendorType.AddAsync(new VendorType { VendorTypeName = "Default" });
                await _context.SaveChangesAsync();

                List<Vendor> vendors = new List<Vendor>() {
                    new Vendor{VendorName = "Exotic Liquids", Address = "49 Gilbert St."},
                    new Vendor{VendorName = "New Orleans Cajun Delights", Address = "P.O. Box 78934"},
                    new Vendor{VendorName = "Grandma Kelly's Homestead", Address = "707 Oxford Rd."},
                    new Vendor{VendorName = "Tokyo Traders", Address = "9-8 Sekimai Musashino-shi"},
                    new Vendor{VendorName = "Cooperativa de Quesos 'Las Cabras'", Address = "Calle del Rosal 4"},
                    new Vendor{VendorName = "Mayumi's", Address = "92 Setsuko Chuo-ku"},
                    new Vendor{VendorName = "Pavlova, Ltd.", Address = "74 Rose St. Moonie Ponds"},
                    new Vendor{VendorName = "Specialty Biscuits, Ltd.", Address = "29 King's Way"},
                    new Vendor{VendorName = "PB Knäckebröd AB", Address = "Kaloadagatan 13"},
                    new Vendor{VendorName = "Refrescos Americanas LTDA", Address = "Av. das Americanas 12.890"},
                    new Vendor{VendorName = "Heli Süßwaren GmbH & Co. KG", Address = "Tiergartenstraße 5"},
                    new Vendor{VendorName = "Plutzer Lebensmittelgroßmärkte AG", Address = "Bogenallee 51"},
                    new Vendor{VendorName = "Nord-Ost-Fisch Handelsgesellschaft mbH", Address = "Frahmredder 112a"},
                    new Vendor{VendorName = "Formaggi Fortini s.r.l.", Address = "Viale Dante, 75"},
                    new Vendor{VendorName = "Norske Meierier", Address = "Hatlevegen 5"},
                    new Vendor{VendorName = "Bigfoot Breweries", Address = "3400 - 8th Avenue Suite 210"},
                    new Vendor{VendorName = "Svensk Sjöföda AB", Address = "Brovallavägen 231"},
                    new Vendor{VendorName = "Aux joyeux ecclésiastiques", Address = "203, Rue des Francs-Bourgeois"},
                    new Vendor{VendorName = "New England Seafood Cannery", Address = "Order Processing Dept. 2100 Paul Revere Blvd."}
                };
                await _context.Vendor.AddRangeAsync(vendors);
                await _context.SaveChangesAsync();

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task SendEmailBySendGridAsync(string apiKey, 
            string fromEmail, 
            string fromFullName, 
            string subject, 
            string message, 
            string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(fromEmail, fromFullName),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email, email));
            await client.SendEmailAsync(msg);

        }

        public async Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL)
        {
            var body = messageBody;
            var message = new MailMessage();
            message.To.Add(new MailAddress(toEmail, toFullName));
            message.From = new MailAddress(fromEmail, fromFullName);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = smtpUser,
                    Password = smtpPassword
                };
                smtp.Credentials = credential;
                smtp.Host = smtpHost;
                smtp.Port = smtpPort;
                smtp.EnableSsl = smtpSSL;
                await smtp.SendMailAsync(message);

            }

        }

        public async Task CreateDefaultSuperAdmin()
        {
            try
            {
                await _roles.GenerateRolesFromPagesAsync();

                ApplicationUser superAdmin = new ApplicationUser();
                superAdmin.Email = _superAdminDefaultOptions.Email;
                superAdmin.UserName = superAdmin.Email;
                superAdmin.EmailConfirmed = true;

                var result = await _userManager.CreateAsync(superAdmin, _superAdminDefaultOptions.Password);

                if (result.Succeeded)
                {
                    //add to user profile
                    UserProfile profile = new UserProfile();
                    profile.FirstName = "Super";
                    profile.LastName = "Admin";
                    profile.Email = superAdmin.Email;
                    profile.ApplicationUserId = superAdmin.Id;
                    await _context.UserProfile.AddAsync(profile);
                    await _context.SaveChangesAsync();

                    await _roles.AddToRoles(superAdmin.Id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        public async Task<string> UploadFile(List<IFormFile> files, IHostingEnvironment env, string uploadFolder)
        {
            var result = "";

            var webRoot = env.WebRootPath;
            var uploads = System.IO.Path.Combine(webRoot, uploadFolder);
            var extension = "";
            var filePath = "";
            var fileName = "";


            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    extension = System.IO.Path.GetExtension(formFile.FileName);
                    fileName = Guid.NewGuid().ToString() + extension;
                    filePath = System.IO.Path.Combine(uploads, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await formFile.CopyToAsync(stream);
                    }

                    result = fileName;

                }
            }

            return result;
        }

    }
}
