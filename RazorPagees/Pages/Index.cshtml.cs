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

        var hotApps = NewLaunches.Take(3)
            .Select(a => $"{a.Name} — {a.Upvotes} votes")
            .ToList();

        var categories = allApps
            .Select(a => a.Category)
            .Concat(new[] { "AI", "Services", "Productivity" })
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
            Eyebrow = "Community launchboard",
            Title = "Discover, vote, and discuss the best new products in tech.",
            Lede = "Submit or hunt products you love. Daily launches compete for the top of the leaderboard, Product of the Day, and Golden Kitty glory.",
            QuickFilters = new List<string> { "New today", "Trending", "Maker launch", "Golden Kitty hopefuls" },
            PillBoard = new List<string> { "Dev tools", "AI & ML", "Productivity", "Design", "Community picks" },
            CategoryTags = categories,
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
            Title = "Top products competing for Product of the Day.",
            Subtitle = "Ranked by score: votes, engagement, and freshness. Golden Kitty eligible.",
            CtaText = "Submit a launch",
            CtaHref = "#"
        };

        NewHeading = new SectionHeadingModel
        {
            Eyebrow = "Fresh launches",
            Title = "Today’s contenders.",
            Subtitle = "Vote, discuss, and help decide Product of the Day.",
            CtaText = "Hunt a product",
            CtaHref = "#"
        };
    }
}
