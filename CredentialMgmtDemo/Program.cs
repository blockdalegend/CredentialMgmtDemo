using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CredentialMgmtDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Configuration.SetBasePath(Directory.GetCurrentDirectory());
            builder.Configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);
            builder.Services.AddTransient<AzureKeyVaultService>();

            // Add DbContext
            builder.Services.AddDbContext<ApplicationDbContext>((serviceProvider, options) =>
            {
                var sqlConnection = new SqlConnection();
                sqlConnection.ConnectionString = builder.Configuration.GetSection("AppSettings").GetSection("SQL_DATABASE_CONNECTION_STRING").Value;

                var defaultAzureCredentialOptions = new Azure.Identity.DefaultAzureCredentialOptions
                {
                    ManagedIdentityClientId = builder.Configuration.GetSection("AppSettings").GetSection("AZURE_CLIENT_ID").Value
                };

                sqlConnection.AccessToken = new Azure.Identity.DefaultAzureCredential(defaultAzureCredentialOptions).GetToken(
                    new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/.default" })).Token;

                options.UseSqlServer(sqlConnection);
            });

            // ...

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            app.Run();
        }
    }
}
