using Farven.Data;
using Farven.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Farven.Pages;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os servi�os ao cont�iner.
builder.Services.AddRazorPages();

// Adiciona o DbContext � aplica��o.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona o servi�o de autentica��o.
builder.Services.AddSingleton<AuthService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";
        options.LogoutPath = "/Logout"; // Rota para logout
        options.AccessDeniedPath = "/AccessDenied"; // Opcional: p�gina para acesso negado
    });

// Adiciona o HttpClient ao cont�iner de servi�os
builder.Services.AddHttpClient<ConversorModel>();

var app = builder.Build();

// Configura o pipeline de requisi��es HTTP.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Habilita a autentica��o e autoriza��o.
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

// Adiciona uma rota para logout
app.MapGet("/Logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/Index"); // Redireciona ap�s logout
});

app.Run();
