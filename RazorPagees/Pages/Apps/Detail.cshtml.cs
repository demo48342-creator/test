using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagees.Models;
using RazorPagees.Services;

namespace RazorPagees.Pages.Apps;

public class DetailModel : PageModel
{
    private readonly AppCatalogService _catalog;

    public DetailModel(AppCatalogService catalog)
    {
        _catalog = catalog;
    }

    public AppListing? App { get; private set; }

    public IActionResult OnGet(string slug)
    {
        App = _catalog.GetBySlug(slug);
        if (App is null)
        {
            return NotFound();
        }

        return Page();
    }
}
