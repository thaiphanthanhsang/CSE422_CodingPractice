
using DeviceManagement.Data;
using DeviceManagement.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. Register MVC services
builder.Services.AddControllersWithViews();

// 2. Connect to SQL Server
builder.Services.AddDbContext<DeviceManagementDbContext>(
    options => options.UseSqlServer(
        builder.Configuration.GetConnectionString(
            "DBConnection")    
    )
);

// 3. Register Repository
builder.Services.AddScoped<IDeviceRepository, DeviceRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

var app = builder.Build();

// 4. Configure error handling
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/DeviceCategories/Error");
    app.UseHsts();
}

// 5. Configure middleware
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

// 6. Configure MVC routing
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=DeviceCategories}/{action=Index}/{id?}"
);

// 7. Run application
app.Run();


