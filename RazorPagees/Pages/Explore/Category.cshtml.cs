using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagees.Models;
using RazorPagees.Services;

namespace RazorPagees.Pages.Explore;

public class CategoryModel : PageModel
{
    private readonly AppCatalogService _catalog;
    private const int PageSize = 9;

    public CategoryModel(AppCatalogService catalog)
    {
        _catalog = catalog;
    }

    public string CategoryName { get; private set; } = string.Empty;

    public string CategorySlug { get; private set; } = string.Empty;

    public List<AppListing> Apps { get; private set; } = new();

    public int PageNumber { get; private set; }

    public int TotalPages { get; private set; }

    public int TotalCount { get; private set; }

    public void OnGet(string category, int page = 1)
    {
        CategorySlug = category;
        CategoryName = category.Replace("-", " ");
        var all = _catalog.GetByCategorySlug(category);

        if (all.Count == 0)
        {
            all = _catalog.GetTopApps().Concat(_catalog.GetNewLaunches())
                .Where(a => a.Category.Equals(category, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        TotalCount = all.Count;
        PageNumber = Math.Max(1, page);
        TotalPages = Math.Max(1, (int)Math.Ceiling(TotalCount / (double)PageSize));
        if (PageNumber > TotalPages) PageNumber = TotalPages;

        Apps = all
            .OrderByDescending(a => a.Score)
            .Skip((PageNumber - 1) * PageSize)
            .Take(PageSize)
            .ToList();
    }
}
