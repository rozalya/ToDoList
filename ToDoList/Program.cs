using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Constants;
using ToDoList.Extensions.DependencyInjection;
using ToDoList.Infrastructure.Data;
using ToDoList.ModelBinders;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<IdentityUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddControllersWithViews()
     .AddMvcOptions(options =>
     {
         options.ModelBinderProviders.Insert(1, new DateTimeModelBinderProvider(FormatingConstant.NormalDateFormat));
     })
    .AddMvcLocalization(LanguageViewLocationExpanderFormat.Suffix);

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("DeleteRolePolicy",
        policy => policy.RequireClaim("Delete Role"));

    options.AddPolicy("EditRolePolicy",
         policy => policy.RequireClaim("Edit Role"));

    options.AddPolicy("InactiveTaskRolePolicy",
       policy => policy.RequireClaim("Inactive Task Role"));

    options.AddPolicy("AdminRolePolicy",
        policy => policy.RequireRole("Admin"));

    options.AddPolicy("StepsUserRolePolicy",
       policy => policy.RequireRole("StepsUser"));

    options.AddPolicy("StatementsUserRolePolicy",
       policy => policy.RequireRole("StatementsUser"));
});

builder.Services.AddApplicationServices();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    /*  app.UseExceptionHandler("/Home/Error");*/
    app.UseExceptionHandler("/Error");
    app.UseStatusCodePagesWithReExecute("/Error/{0}");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.Run();
