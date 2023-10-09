using ContactApp;
using ContactApp.Contacts;
using ContactApp.Utils;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<ContactDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ConnectionDb");
    options.UseSqlite(connectionString);
    options.LogTo(Console.WriteLine);
});

builder.Services.AddScoped<ContactRepository>();
builder.Services.AddSingleton<Archiver>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddRazorPages();

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

app.MapGet("/", () => Results.Redirect("/Contacts"));

app.MapContactsApiEndpoints();
app.MapArchiveApiEndpoints();
app.MapContactsDataApiEndpoints();

app.MapRazorPages();

app.Run();
