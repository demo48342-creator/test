using Microsoft.EntityFrameworkCore;
using RazorPagees.Data;
using RazorPagees.Domain;
using RazorPagees.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("AltAtlas"));
builder.Services.AddSingleton<AppCatalogService>();
builder.Services.AddScoped<VoteService>();
builder.Services.AddScoped<ViewService>();

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

app.MapPost("/api/apps/{slug}/vote", async (string slug, VoteRequest request, AppCatalogService catalog, VoteService votes) =>
{
    if (string.IsNullOrWhiteSpace(request.Direction))
    {
        return Results.BadRequest();
    }

    var direction = request.Direction.Equals("up", StringComparison.OrdinalIgnoreCase)
        ? VoteDirection.Up
        : request.Direction.Equals("down", StringComparison.OrdinalIgnoreCase)
            ? VoteDirection.Down
            : null;

    if (direction is null)
    {
        return Results.BadRequest();
    }

    var result = catalog.Vote(slug, request.Direction);
    if (result is null)
    {
        return Results.NotFound();
    }

    await votes.RecordAsync(slug, direction.Value);
    return Results.Ok(new { up = result.Value.Up, down = result.Value.Down, score = result.Value.Score });
});

app.MapPost("/api/apps/{slug}/view", async (string slug, AppCatalogService catalog, ViewService views) =>
{
    var updated = catalog.RegisterView(slug);
    if (updated is null)
    {
        return Results.NotFound();
    }

    await views.RegisterAsync(slug);
    return Results.Ok(new { views = updated.Value });
});

app.MapRazorPages();

app.Run();

public record VoteRequest(string Direction);
