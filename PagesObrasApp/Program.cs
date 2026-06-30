using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// ==================== SERVICIOS ====================

builder.Services.AddRazorPages(options =>
{
    // Carpetas protegidas — Razor las bloquea automáticamente
    options.Conventions.AuthorizeFolder("/Admin", "SoloAdmin");
    options.Conventions.AuthorizeFolder("/Empleado", "Personal");

    // Páginas públicas — no requieren login
    options.Conventions.AllowAnonymousToFolder("/Auth");
    options.Conventions.AllowAnonymousToFolder("/Cliente");
    options.Conventions.AllowAnonymousToPage("/Index");
    options.Conventions.AllowAnonymousToPage("/Privacy");
    options.Conventions.AllowAnonymousToPage("/Error");
});

// ── Cookie Authentication ──────────────────────────────────────────────
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";
        options.AccessDeniedPath = "/Auth/AccesDenied";

        // La cookie dura 8 horas (una jornada laboral)
        // y se renueva si el usuario sigue activo (SlidingExpiration)
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;

        options.Cookie.Name = "ConstructoraAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;

        // En producción cambiar a Always para forzar HTTPS
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

// ── Políticas de autorización por rol ─────────────────────────────────
builder.Services.AddAuthorization(options =>
{
    // Solo administradores
    options.AddPolicy("SoloAdmin", policy =>
        policy.RequireRole("Administrador"));

    // Cualquier persona del equipo
    options.AddPolicy("Personal", policy =>
        policy.RequireRole("Administrador", "Capataz", "Empleado"));
});

// ── HttpClient hacia la API ────────────────────────────────────────────
builder.Services.AddHttpClient("API", client =>
{
    var apiUrl = builder.Configuration["ApiBaseUrl"];
    client.BaseAddress = new Uri(apiUrl ?? "https://localhost:7000/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// ── Sesión (para datos temporales tipo carrito, mensajes flash, etc.) ─
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();


// ==================== PIPELINE ====================

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();

// ⚠️ Orden obligatorio: Authentication SIEMPRE antes de Authorization
app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();