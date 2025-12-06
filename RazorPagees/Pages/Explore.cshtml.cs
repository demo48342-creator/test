using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagees.Models;
using RazorPagees.Services;

namespace RazorPagees.Pages;

public class ExploreModel : PageModel
{
    private readonly AppCatalogService _catalog;
    private const int PageSize = 9;

    public ExploreModel(AppCatalogService catalog)
    {
        _catalog = catalog;
    }

    public List<CategoryGroup> Categories { get; private set; } = new();

    public List<CategoryGroup> CategoryGroups { get; private set; } = new();

    public List<AppListing> PagedApps { get; private set; } = new();

    public int PageNumber { get; private set; }

    public int TotalPages { get; private set; }

    public int TotalCount { get; private set; }

    public void OnGet(int page = 1)
    {
        var apps = _catalog.GetTopApps().Concat(_catalog.GetNewLaunches());

        Categories = apps
            .GroupBy(a => a.Category)
            .Select(g => new CategoryGroup(g.Key, g.Count()))
            .OrderBy(g => g.Name)
            .ToList();

        CategoryGroups = apps
            .GroupBy(a => a.Category)
            .Select(g => new CategoryGroup(g.Key, g.Count(), g.OrderByDescending(a => a.Score).Take(6).ToList()))
            .OrderBy(g => g.Name)
            .ToList();

        TotalCount = apps.Count();
        PageNumber = Math.Max(1, page);
        TotalPages = Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
        if (PageNumber > TotalPages) PageNumber = TotalPages;

        PagedApps = apps
            .OrderByDescending(a => a.Score)
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }
}

public record CategoryGroup(string Name, int Count, List<AppListing>? Apps = null)
{
    public string Anchor => Name.Replace(" ", "-").ToLower();

    public List<AppListing> AppsOrEmpty => Apps ?? new List<AppListing>();
}
