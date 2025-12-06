using System;
using System.Collections.Generic;

namespace RazorPagees.Domain;

public class AppEntity
{
    public int Id { get; set; }

    public string Slug { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public string Fit { get; set; } = string.Empty;

    public string Signal { get; set; } = string.Empty;

    public string Website { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;

    public string? HtmlDescription { get; set; }

    public string? DiscussionLink { get; set; }

    public bool IsNewLaunch { get; set; }

    public bool GoldenKittyEligible { get; set; }

    public DateTime LaunchDate { get; set; } = DateTime.UtcNow;

    public int CommentsCount { get; set; }

    public int Upvotes { get; set; }

    public int Downvotes { get; set; }

    public int Views { get; set; }

    public double? TrustpilotScore { get; set; }

    public int? TrustpilotReviews { get; set; }

    public string? TrustpilotUrl { get; set; }

    public List<AppTag> Tags { get; set; } = new();

    public List<AlternativeEntity> Alternatives { get; set; } = new();

    public List<VoteRecord> Votes { get; set; } = new();
}
