using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagees.Models;
using RazorPagees.Services;

namespace RazorPagees.Pages;

public class IndexModel : PageModel
{
    private readonly AppCatalogService _catalog;

    public IndexModel(AppCatalogService catalog)
    {
        _catalog = catalog;
    }

    public HeroModel Hero { get; private set; } = default!;

    public SectionHeadingModel TopHeading { get; private set; } = default!;

    public SectionHeadingModel NewHeading { get; private set; } = default!;

    public List<AppListing> TopApps { get; private set; } = new();

    public List<AppListing> NewLaunches { get; private set; } = new();

    public int TotalApps => AllApps.Count();

    public int TotalAlternatives => AllApps.Sum(a => a.Alternatives.Count);

    private IEnumerable<AppListing> AllApps => TopApps.Concat(NewLaunches);

    public void OnGet()
    {
        TopApps = _catalog.GetTopApps().ToList();
        NewLaunches = _catalog.GetNewLaunches().ToList();

        var allApps = AllApps.ToList();
        var sortedApps = allApps
            .OrderByDescending(a => a.Score)
            .ThenBy(a => a.Name)
            .ToList();

        var hotApps = NewLaunches.Take(3).ToList();

        var categories = allApps
            .Select(a => a.Category)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .OrderBy(c => c)
            .ToList();

        var popularTags = allApps
            .SelectMany(a => a.Tags)
            .GroupBy(t => t, StringComparer.OrdinalIgnoreCase)
            .OrderByDescending(g => g.Count())
            .ThenBy(g => g.Key)
            .Take(7)
            .Select(g => g.Key)
            .ToList();

        var bucketNames = new[] { "Productivity", "Engineering & Development", "Design & Creative" };

        var bucketedCategories = categories
            .Select((cat, index) => new { cat, index })
            .GroupBy(x => x.index % bucketNames.Length)
            .Select(g => g.Select(x => x.cat).ToList())
            .ToList();

        static string Anchorize(string text) => text.Replace(" ", "-").ToLowerInvariant();

        var quickFilters = categories.Take(4).ToList();
        if (!quickFilters.Any())
        {
            quickFilters = new List<string> { "Productivity", "AI", "Design", "Security" };
        }

        var categorySets = new List<SearchCategory>();
        for (var i = 0; i < bucketNames.Length; i++)
        {
            var items = bucketedCategories.ElementAtOrDefault(i) ?? new List<string>();
            if (!items.Any())
            {
                items = categories.Take(3).ToList();
            }

            categorySets.Add(new SearchCategory
            {
                Title = bucketNames[i],
                Items = items,
                CtaText = "View all",
                CtaHref = items.Any() ? $"/Explore#{Anchorize(items.First())}" : "/Explore"
            });
        }

        var spotlight = sortedApps
            .Take(4)
            .ToList();

        Hero = new HeroModel
        {
            Eyebrow = "Accessible app atlas",
            Title = "Find better tools without the noise.",
            Lede = "Human-reviewed launches, pricing notes, and accessibility-first alternatives in one place.",
            QuickFilters = quickFilters,
            PillBoard = new List<string> { "Human reviewed", "Clear pricing", "Privacy-respectful" },
            CategoryTags = categories.Take(12).ToList(),
            HotApps = hotApps,
            SearchPalette = new SearchPalette
            {
                PopularTags = popularTags,
                CategorySets = categorySets,
                SpotlightResults = spotlight,
                SearchableApps = sortedApps
            },
            Stats = new List<StatBlock>
            {
                new StatBlock { Value = TotalApps.ToString(), Label = "live launches", Note = "Ranked by community voting and engagement." },
                new StatBlock { Value = TotalAlternatives.ToString(), Label = "alternatives", Note = "Explore options and discussion on every launch." }
            }
        };

        TopHeading = new SectionHeadingModel
        {
            Eyebrow = "Leaderboards",
            Title = "Top scoring products",
            Subtitle = "Ranked by community votes, engagement, and freshness.",
            CtaText = "Browse all launches",
            CtaHref = "/Explore"
        };

        NewHeading = new SectionHeadingModel
        {
            Eyebrow = "Fresh launches",
            Title = "Today’s contenders",
            Subtitle = "Vote, discuss, and follow along with new drops.",
            CtaText = "See full launch board",
            CtaHref = "/Explore#launches"
        };
    }
}
