using Microsoft.AspNetCore.Authentication.Cookies;
using PagesObrasApp.Services;

var builder = WebApplication.CreateBuilder(args);

// ==================== SERVICIOS ====================

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "SoloAdmin");
    options.Conventions.AuthorizeFolder("/Empleado", "Personal");

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
        options.AccessDeniedPath = "/Account/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromHours(8);
        options.SlidingExpiration = true;
        options.Cookie.Name = "ConstructoraAuth";
        options.Cookie.HttpOnly = true;
        options.Cookie.IsEssential = true;
        options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

// ── Políticas de autorización por rol ─────────────────────────────────
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("SoloAdmin", policy =>
        policy.RequireRole("Administrador"));

    options.AddPolicy("Personal", policy =>
        policy.RequireRole("Administrador", "Supervisor", "Empleado"));
});

// ── HttpClient hacia la API ────────────────────────────────────────────
builder.Services.AddHttpClient("API", client =>
{
    var apiUrl = builder.Configuration["ApiBaseUrl"];
    client.BaseAddress = new Uri(apiUrl ?? "https://left-groom-whomever.ngrok-free.dev/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// ── Servicios de la API ────────────────────────────────────────────────
builder.Services.AddScoped<IApiService, ApiService>();
builder.Services.AddScoped<IAuthHttpService, AuthHttpService>();
builder.Services.AddScoped<IClienteHttpService, ClienteHttpService>();
builder.Services.AddScoped<IEmpleadoHttpService, EmpleadoHttpService>();
builder.Services.AddScoped<IGastoHttpService, GastoHttpService>();
builder.Services.AddScoped<IHerramientaHttpService, HerramientaHttpService>();
builder.Services.AddScoped<IMaterialHttpService, MaterialHttpService>();
builder.Services.AddScoped<INovedadHttpService, NovedadHttpService>();
builder.Services.AddScoped<IObraHttpService, ObraHttpService>();
builder.Services.AddScoped<IOrdenCompraHttpService, OrdenCompraHttpService>();
builder.Services.AddScoped<IPagoProveedorHttpService, PagoProveedorHttpService>();
builder.Services.AddScoped<IPresupuestoHttpService, PresupuestoHttpService>();
builder.Services.AddScoped<IProveedorHttpService, ProveedorHttpService>();
builder.Services.AddScoped<ISeguimientoHttpService, SeguimientoHttpService>();
builder.Services.AddScoped<IUsuarioHttpService, UsuarioHttpService>();
builder.Services.AddScoped<IPresupuestoHttpService, PresupuestoHttpService>();


// ── Sesión ──────────────────────────────────────────────────────────
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

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();