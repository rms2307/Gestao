using Gestao.Components;
using Gestao.Components.Account;
using Gestao.Data;
using Gestao.Data.Repositories.Interfaces;
using Gestao.Data.Repositories;
using Gestao.Infra.Mail;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using System.Net.Mail;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents()
    .AddInteractiveWebAssemblyComponents();

builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<IdentityUserAccessor>();
builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<AuthenticationStateProvider, PersistingRevalidatingAuthenticationStateProvider>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
    .AddIdentityCookies();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddSignInManager()
    .AddDefaultTokenProviders();

builder.Services.AddSingleton<SmtpClient>(opt =>
{
    string user = builder.Configuration.GetValue<string>("EmailSender:User")!;
    string password = builder.Configuration.GetValue<string>("EmailSender:Password")!;

    return new()
    {
        Host = builder.Configuration.GetValue<string>("EmailSender:Server")!,
        Port = builder.Configuration.GetValue<int>("EmailSender:Port")!,
        EnableSsl = builder.Configuration.GetValue<bool>("EmailSender:SSL")!,
        Credentials = new NetworkCredential(user, password)
    };
});

builder.Services.AddAuthentication()
   .AddGoogle(options =>
   {
       options.ClientId = builder.Configuration.GetValue<string>("OAuth:Google:ClientId")!;
       options.ClientSecret = builder.Configuration.GetValue<string>("OAuth:Google:ClientSecret")!;
   });

builder.Services.AddSingleton<IEmailSender<ApplicationUser>, EmailSender>();

AddRepositories(builder.Services);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode()
    .AddInteractiveWebAssemblyRenderMode()
    .AddAdditionalAssemblies(typeof(Gestao.Client._Imports).Assembly);

// Add additional endpoints required by the Identity /Account Razor components.
app.MapAdditionalIdentityEndpoints();

MapEndpoints(builder, app);

app.Run();

static void AddRepositories(IServiceCollection services)
{
    services.AddScoped<IAccountRepository, AccountRepository>();
    services.AddScoped<ICategoryRepository, CategoryRepository>();
    services.AddScoped<ICompanyRepository, CompanyRepository>();
    services.AddScoped<IDocumentRepository, DocumentRepository>();
    services.AddScoped<IFinanacialTransactionRepository, FinanacialTransactionRepository>();
}

static void MapEndpoints(WebApplicationBuilder builder, WebApplication app)
{
    int pageSize = builder.Configuration.GetValue<int>("Pagination:PageSize");

    app.MapGet("/api/categories", async (
        ICategoryRepository repository,
        [FromQuery] int companyId,
        [FromQuery] int pageIndex) =>
    {
        var data = await repository.GetAllAsync(companyId, pageIndex, pageSize);

        return Results.Ok(data);
    });
}