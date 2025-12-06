var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<RazorPagees.Services.AppCatalogService>();

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

app.MapPost("/api/apps/{slug}/vote", (string slug, VoteRequest request, RazorPagees.Services.AppCatalogService catalog) =>
{
    if (string.IsNullOrWhiteSpace(request.Direction))
    {
        return Results.BadRequest();
    }

    var result = catalog.Vote(slug, request.Direction);
    return result is null ? Results.NotFound() : Results.Ok(new { up = result.Value.Up, down = result.Value.Down, score = result.Value.Score });
});

app.MapRazorPages();

app.Run();

public record VoteRequest(string Direction);
