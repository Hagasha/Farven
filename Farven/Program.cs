using Farven.Data;
using Farven.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.DependencyInjection;
using Farven.Pages;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Adiciona os serviços ao contêiner.
builder.Services.AddRazorPages();
builder.Services.AddControllers(); // Certifique-se de que os controladores estão habilitados

// Adiciona o DbContext à aplicação.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adiciona o serviço de autenticação.
builder.Services.AddSingleton<AuthService>();

// Registra IHttpContextAccessor
builder.Services.AddHttpContextAccessor();

// Registra o ConversionService
builder.Services.AddScoped<ConversionService>();

builder.Services.AddScoped<EmailService>(); 

// Adiciona suporte à autenticação baseada em cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Login";      // Rota para login
        options.LogoutPath = "/Logout";    // Rota para logout
        options.AccessDeniedPath = "/AccessDenied"; // Página para acesso negado, se necessário
    });

// Adiciona o HttpClient ao contêiner de serviços
builder.Services.AddHttpClient<ConversorModel>();

// Configuração de CORS
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

// Configura o pipeline de requisições HTTP.
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

// Habilita a autenticação e autorização.
app.UseAuthentication(); // Deve vir antes de UseAuthorization
app.UseAuthorization();

// Mapeia as rotas dos controladores e páginas
app.MapControllers();
app.MapRazorPages();

// Adiciona uma rota para logout
app.MapGet("/Logout", async context =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    context.Response.Redirect("/Index"); // Redireciona após logout
});

app.Run();