var builder = WebApplication.CreateBuilder(args);

// ==================== AGREGAR SERVICIOS ====================
builder.Services.AddRazorPages();

// Configurar HttpClient para la API
builder.Services.AddHttpClient("API", client =>
{
    var apiUrl = builder.Configuration["ApiBaseUrl"];
    client.BaseAddress = new Uri(apiUrl ?? "https://localhost:7000/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Configurar sesiones
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// ==================== CONSTRUIR APP ====================
var app = builder.Build();

// ==================== CONFIGURAR PIPELINE ====================
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();      // ¡Importante! Debe ir después de UseRouting()
app.UseAuthorization();

app.MapRazorPages();

// ==================== EJECUTAR ====================
app.Run();