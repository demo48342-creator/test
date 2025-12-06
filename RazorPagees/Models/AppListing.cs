using System;
using System.Collections.Generic;

namespace RazorPagees.Models;

public class AppListing
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public string Fit { get; set; } = string.Empty;

    public List<string> Tags { get; set; } = new();

    public string Signal { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;

    public string HtmlDescription { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public DateTime LaunchDate { get; set; }

    public int CommentsCount { get; set; }

    public bool GoldenKittyEligible { get; set; }

    public string? DiscussionLink { get; set; }

    public int Score { get; set; }

    public double? TrustpilotScore { get; set; }

    public int? TrustpilotReviews { get; set; }

    public string? TrustpilotUrl { get; set; }

    public bool IsNewLaunch { get; set; }

    public int Upvotes { get; set; }

    public int Downvotes { get; set; }

    public List<AlternativeOption> Alternatives { get; set; } = new();
}

public class AlternativeOption
{
    public string Name { get; set; } = string.Empty;

    public string Differentiator { get; set; } = string.Empty;

    public string Pricing { get; set; } = string.Empty;

    public string Link { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;
}
