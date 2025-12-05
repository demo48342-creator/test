using System.Collections.Generic;

namespace RazorPagees.Models;

public class AppListing
{
    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public string Fit { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = new();

    public List<AlternativeOption> Alternatives { get; set; } = new();
}

public class AlternativeOption
{
    public string Name { get; set; } = string.Empty;

    public string Differentiator { get; set; } = string.Empty;

    public string Pricing { get; set; } = string.Empty;

    public string Link { get; set; } = string.Empty;
}
