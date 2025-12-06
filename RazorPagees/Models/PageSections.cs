using System.Collections.Generic;

namespace RazorPagees.Models;

public class HeroModel
{
    public string Eyebrow { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Lede { get; set; } = string.Empty;

    public List<string> QuickFilters { get; set; } = new();

    public List<string> PillBoard { get; set; } = new();

    public List<StatBlock> Stats { get; set; } = new();

    public List<string> HotApps { get; set; } = new();
}

public class StatBlock
{
    public string Label { get; set; } = string.Empty;

    public string Value { get; set; } = string.Empty;

    public string Note { get; set; } = string.Empty;
}

public class SectionHeadingModel
{
    public string Eyebrow { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Subtitle { get; set; } = string.Empty;

    public string CtaText { get; set; } = string.Empty;

    public string? CtaHref { get; set; }
}

public class ProductOption
{
    public string Label { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public string Pill { get; set; } = string.Empty;

    public string CtaText { get; set; } = string.Empty;

    public string CtaHref { get; set; } = "#";
}
