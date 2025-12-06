using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagees.Models;
using RazorPagees.Services;

namespace RazorPagees.Pages.Explore;

public class CategoryModel : PageModel
{
    private readonly AppCatalogService _catalog;

    public CategoryModel(AppCatalogService catalog)
    {
        _catalog = catalog;
    }

    public string CategoryName { get; private set; } = string.Empty;

    public List<AppListing> Apps { get; private set; } = new();

    public void OnGet(string category)
    {
        CategoryName = category.Replace("-", " ");
        Apps = _catalog.GetByCategorySlug(category);

        if (Apps.Count == 0)
        {
            // Try case-insensitive category match without slug conversion
            Apps = _catalog.GetTopApps().Concat(_catalog.GetNewLaunches())
                .Where(a => a.Category.Equals(category, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    }
}
