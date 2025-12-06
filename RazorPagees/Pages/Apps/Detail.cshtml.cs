using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagees.Models;
using RazorPagees.Services;

namespace RazorPagees.Pages.Apps;

public class DetailModel : PageModel
{
    private readonly AppCatalogService _catalog;
    private readonly ViewService _views;

    public DetailModel(AppCatalogService catalog, ViewService views)
    {
        _catalog = catalog;
        _views = views;
    }

    public AppListing? App { get; private set; }

    public async Task<IActionResult> OnGet(string slug)
    {
        App = _catalog.GetBySlug(slug);
        if (App is null)
        {
            return NotFound();
        }

        _catalog.RegisterView(slug);
        await _views.RegisterAsync(slug);

        return Page();
    }
}
