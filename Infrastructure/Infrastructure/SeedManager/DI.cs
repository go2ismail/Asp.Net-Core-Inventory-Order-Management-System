using Infrastructure.DataAccessManager.EFCore.Contexts;
using Infrastructure.SeedManager.Demos;
using Infrastructure.SeedManager.Systems;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.SeedManager;

public static class DI
{
    //>>> System Seed

    public static IServiceCollection RegisterSystemSeedManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<RoleSeeder>();
        services.AddScoped<UserAdminSeeder>();
        services.AddScoped<CompanySeeder>();
        services.AddScoped<SystemWarehouseSeeder>();

        return services;
    }


    public static IHost SeedSystemData(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var context = serviceProvider.GetRequiredService<DataContext>();
        if (!context.Roles.Any()) //if empty, thats mean never been seeded before
        {
            var roleSeeder = serviceProvider.GetRequiredService<RoleSeeder>();
            roleSeeder.GenerateDataAsync().Wait();

            var userAdminSeeder = serviceProvider.GetRequiredService<UserAdminSeeder>();
            userAdminSeeder.GenerateDataAsync().Wait();

            var companySeeder = serviceProvider.GetRequiredService<CompanySeeder>();
            companySeeder.GenerateDataAsync().Wait();

            var systemWarehouseSeeder = serviceProvider.GetRequiredService<SystemWarehouseSeeder>();
            systemWarehouseSeeder.GenerateDataAsync().Wait();

        }

        return host;
    }



    //>>> Demo Seed

    public static IServiceCollection RegisterDemoSeedManager(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<TaxSeeder>();
        services.AddScoped<UserSeeder>();
        services.AddScoped<CustomerCategorySeeder>();
        services.AddScoped<CustomerGroupSeeder>();
        services.AddScoped<CustomerSeeder>();
        services.AddScoped<CustomerContactSeeder>();
        services.AddScoped<VendorCategorySeeder>();
        services.AddScoped<VendorGroupSeeder>();
        services.AddScoped<VendorSeeder>();
        services.AddScoped<VendorContactSeeder>();
        services.AddScoped<UnitMeasureSeeder>();
        services.AddScoped<ProductGroupSeeder>();
        services.AddScoped<ProductSeeder>();
        services.AddScoped<WarehouseSeeder>();
        services.AddScoped<SalesOrderSeeder>();
        services.AddScoped<PurchaseOrderSeeder>();
        services.AddScoped<DeliveryOrderSeeder>();
        services.AddScoped<SalesReturnSeeder>();
        services.AddScoped<GoodsReceiveSeeder>();
        services.AddScoped<PurchaseReturnSeeder>();
        services.AddScoped<TransferOutSeeder>();
        services.AddScoped<TransferInSeeder>();
        services.AddScoped<PositiveAdjustmentSeeder>();
        services.AddScoped<NegativeAdjustmentSeeder>();
        services.AddScoped<ScrappingSeeder>();
        services.AddScoped<StockCountSeeder>();
        return services;
    }
    public static IHost SeedDemoData(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var serviceProvider = scope.ServiceProvider;

        var context = serviceProvider.GetRequiredService<DataContext>();
        if (!context.Tax.Any()) //if empty, thats mean never been seeded before
        {
            var taxSeeder = serviceProvider.GetRequiredService<TaxSeeder>();
            taxSeeder.GenerateDataAsync().Wait();

            var userSeeder = serviceProvider.GetRequiredService<UserSeeder>();
            userSeeder.GenerateDataAsync().Wait();

            var customerCategorySeeder = serviceProvider.GetRequiredService<CustomerCategorySeeder>();
            customerCategorySeeder.GenerateDataAsync().Wait();

            var customerGroupSeeder = serviceProvider.GetRequiredService<CustomerGroupSeeder>();
            customerGroupSeeder.GenerateDataAsync().Wait();

            var customerSeeder = serviceProvider.GetRequiredService<CustomerSeeder>();
            customerSeeder.GenerateDataAsync().Wait();

            var customerContactSeeder = serviceProvider.GetRequiredService<CustomerContactSeeder>();
            customerContactSeeder.GenerateDataAsync().Wait();

            var vendorCategorySeeder = serviceProvider.GetRequiredService<VendorCategorySeeder>();
            vendorCategorySeeder.GenerateDataAsync().Wait();

            var vendorGroupSeeder = serviceProvider.GetRequiredService<VendorGroupSeeder>();
            vendorGroupSeeder.GenerateDataAsync().Wait();

            var vendorSeeder = serviceProvider.GetRequiredService<VendorSeeder>();
            vendorSeeder.GenerateDataAsync().Wait();

            var vendorContactSeeder = serviceProvider.GetRequiredService<VendorContactSeeder>();
            vendorContactSeeder.GenerateDataAsync().Wait();

            var unitMeasureSeeder = serviceProvider.GetRequiredService<UnitMeasureSeeder>();
            unitMeasureSeeder.GenerateDataAsync().Wait();

            var productGroupSeeder = serviceProvider.GetRequiredService<ProductGroupSeeder>();
            productGroupSeeder.GenerateDataAsync().Wait();

            var productSeeder = serviceProvider.GetRequiredService<ProductSeeder>();
            productSeeder.GenerateDataAsync().Wait();

            var warehouseSeeder = serviceProvider.GetRequiredService<WarehouseSeeder>();
            warehouseSeeder.GenerateDataAsync().Wait();

            var salesOrderSeeder = serviceProvider.GetRequiredService<SalesOrderSeeder>();
            salesOrderSeeder.GenerateDataAsync().Wait();

            var purchaseOrderSeeder = serviceProvider.GetRequiredService<PurchaseOrderSeeder>();
            purchaseOrderSeeder.GenerateDataAsync().Wait();

            var deliveryOrderSeeder = serviceProvider.GetRequiredService<DeliveryOrderSeeder>();
            deliveryOrderSeeder.GenerateDataAsync().Wait();

            var salesReturnSeeder = serviceProvider.GetRequiredService<SalesReturnSeeder>();
            salesReturnSeeder.GenerateDataAsync().Wait();

            var goodsReceiveSeeder = serviceProvider.GetRequiredService<GoodsReceiveSeeder>();
            goodsReceiveSeeder.GenerateDataAsync().Wait();

            var purchaseReturnSeeder = serviceProvider.GetRequiredService<PurchaseReturnSeeder>();
            purchaseReturnSeeder.GenerateDataAsync().Wait();

            var transferOutSeeder = serviceProvider.GetRequiredService<TransferOutSeeder>();
            transferOutSeeder.GenerateDataAsync().Wait();

            var transferInSeeder = serviceProvider.GetRequiredService<TransferInSeeder>();
            transferInSeeder.GenerateDataAsync().Wait();

            var positiveAdjustmentSeeder = serviceProvider.GetRequiredService<PositiveAdjustmentSeeder>();
            positiveAdjustmentSeeder.GenerateDataAsync().Wait();

            var negativeAdjustmentSeeder = serviceProvider.GetRequiredService<NegativeAdjustmentSeeder>();
            negativeAdjustmentSeeder.GenerateDataAsync().Wait();

            var scrappingSeeder = serviceProvider.GetRequiredService<ScrappingSeeder>();
            scrappingSeeder.GenerateDataAsync().Wait();

            var stockCountSeeder = serviceProvider.GetRequiredService<StockCountSeeder>();
            stockCountSeeder.GenerateDataAsync().Wait();

        }
        return host;
    }
}

