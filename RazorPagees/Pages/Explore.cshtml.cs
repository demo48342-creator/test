using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagees.Models;
using RazorPagees.Services;

namespace RazorPagees.Pages;

public class ExploreModel : PageModel
{
    private readonly AppCatalogService _catalog;

    public ExploreModel(AppCatalogService catalog)
    {
        _catalog = catalog;
    }

    public List<CategoryGroup> Categories { get; private set; } = new();

    public List<CategoryGroup> CategoryGroups { get; private set; } = new();

    public void OnGet()
    {
        var apps = _catalog.GetTopApps().Concat(_catalog.GetNewLaunches());

        Categories = apps
            .GroupBy(a => a.Category)
            .Select(g => new CategoryGroup(g.Key, g.Count()))
            .OrderBy(g => g.Name)
            .ToList();

        CategoryGroups = apps
            .GroupBy(a => a.Category)
            .Select(g => new CategoryGroup(g.Key, g.Count(), g.ToList()))
            .OrderBy(g => g.Name)
            .ToList();
    }
}

public record CategoryGroup(string Name, int Count, List<AppListing>? Apps = null)
{
    public string Anchor => Name.Replace(" ", "-").ToLower();

    public List<AppListing> AppsOrEmpty => Apps ?? new List<AppListing>();
}
