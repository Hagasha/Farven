using Farven.Data;
using Farven.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Farven.Pages;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços ao contêiner.
builder.Services.AddRazorPages();

// Adiciona o DbContext à aplicação.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona o serviço de autenticação.
builder.Services.AddSingleton<AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout"; // Rota para logout
        options.AccessDeniedPath = "/AccessDenied"; // Opcional: página para acesso negado
    });

// Adiciona o HttpClient ao contêiner de serviços
builder.Services.AddHttpClient<ConversorModel>();

var app = builder.Build();

// Configura o pipeline de requisições HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilita a autenticação e autorização.
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Adiciona uma rota para logout
app.MapGet("/Logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/Index"); // Redireciona após logout
});

app.Run();
