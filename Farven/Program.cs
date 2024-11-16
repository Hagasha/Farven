using Farven.Data;
using Farven.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Farven.Pages;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os servi�os ao cont�iner.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // Certifique-se de que os controladores est�o habilitados

// Adiciona o DbContext � aplica��o.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona o servi�o de autentica��o.
builder.Services.AddSingleton<AuthService>();

// Registra IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Registra o ConversionService
builder.Services.AddScoped<ConversionService>();

builder.Services.AddScoped<EmailService>(); 

// Adiciona suporte � autentica��o baseada em cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";      // Rota para login
        options.LogoutPath = "/Logout";    // Rota para logout
        options.AccessDeniedPath = "/AccessDenied"; // P�gina para acesso negado, se necess�rio
    });

// Adiciona o HttpClient ao cont�iner de servi�os
builder.Services.AddHttpClient<ConversorModel>();

// Configura��o de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", corsBuilder =>
    {
        corsBuilder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
    });
});

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

// Habilita o CORS antes de outros middlewares
app.UseCors("AllowAll");

// Habilita a autentica��o e autoriza��o.
app.UseAuthentication(); // Deve vir antes de UseAuthorization
app.UseAuthorization();

// Mapeia as rotas dos controladores e p�ginas
app.MapControllers();
app.MapRazorPages();

// Adiciona uma rota para logout
app.MapGet("/Logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/Index"); // Redireciona ap�s logout
});

app.Run();