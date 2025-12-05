using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPagees.Models;

namespace RazorPagees.Pages;

public class IndexModel : PageModel
{
    public HeroModel Hero { get; private set; } = default!;

    public SectionHeadingModel ProductHeading { get; private set; } = default!;

    public SectionHeadingModel CuratedHeading { get; private set; } = default!;

    public List<AppListing> AppListings { get; private set; } = new();

    public List<ProductOption> ProductOptions { get; private set; } = new();

    public int TotalApps => AppListings.Count;

    public int TotalAlternatives => AppListings.Sum(a => a.Alternatives.Count);

    public void OnGet()
    {
        AppListings = new List<AppListing>
        {
            new AppListing
            {
                Name = "Adobe Photoshop",
                Category = "Creative Suite",
                Summary = "Pixel-perfect editing powerhouse for teams that need retouching, automation, and deep integration.",
                Fit = "Studios that demand advanced layer workflows and ecosystem plugins.",
                Tags = new List<string> { "Paid", "Pro", "Desktop" },
                Alternatives = new List<AlternativeOption>
                {
                    new AlternativeOption
                    {
                        Name = "Affinity Photo",
                        Differentiator = "One-time purchase with high-end tools tuned for Apple silicon.",
                        Pricing = "$69 once",
                        Link = "https://affinity.serif.com"
                    },
                    new AlternativeOption
                    {
                        Name = "GIMP",
                        Differentiator = "Open-source, scriptable, and friendly to custom pipelines.",
                        Pricing = "Free",
                        Link = "https://www.gimp.org"
                    },
                    new AlternativeOption
                    {
                        Name = "Pixelmator Pro",
                        Differentiator = "ML-powered adjustments and quick export presets for creatives on Mac.",
                        Pricing = "$49 once",
                        Link = "https://www.pixelmator.com/pro/"
                    }
                }
            },
            new AppListing
            {
                Name = "Slack",
                Category = "Team Communication",
                Summary = "Channel-based messaging with searchable history and a vast integration ecosystem.",
                Fit = "Distributed teams that rely on automations and app workflows.",
                Tags = new List<string> { "SaaS", "Collaboration", "Integrations" },
                Alternatives = new List<AlternativeOption>
                {
                    new AlternativeOption
                    {
                        Name = "Discord",
                        Differentiator = "Casual voice-first communication with flexible community roles.",
                        Pricing = "Free + Nitro",
                        Link = "https://discord.com"
                    },
                    new AlternativeOption
                    {
                        Name = "Microsoft Teams",
                        Differentiator = "Deep Office 365 integration and enterprise governance controls.",
                        Pricing = "Included with Microsoft 365",
                        Link = "https://www.microsoft.com/microsoft-teams"
                    },
                    new AlternativeOption
                    {
                        Name = "Twist",
                        Differentiator = "Threaded, asynchronous conversations that tame notification overload.",
                        Pricing = "Free tier + paid plans",
                        Link = "https://twist.com"
                    }
                }
            },
            new AppListing
            {
                Name = "Notion",
                Category = "Docs & Workspaces",
                Summary = "All-in-one docs, databases, and wiki with collaborative blocks and templates.",
                Fit = "Teams that need flexible knowledge bases and lightweight project tracking.",
                Tags = new List<string> { "SaaS", "Collaborative", "Flexible" },
                Alternatives = new List<AlternativeOption>
                {
                    new AlternativeOption
                    {
                        Name = "Coda",
                        Differentiator = "Docs that behave like apps with automations and Packs.",
                        Pricing = "Free tier + paid plans",
                        Link = "https://coda.io"
                    },
                    new AlternativeOption
                    {
                        Name = "Obsidian",
                        Differentiator = "Local-first markdown graph with extensible plugin ecosystem.",
                        Pricing = "Free + sync add-ons",
                        Link = "https://obsidian.md"
                    },
                    new AlternativeOption
                    {
                        Name = "Confluence",
                        Differentiator = "Structured docs with enterprise permissions and change tracking.",
                        Pricing = "Free tier + paid plans",
                        Link = "https://www.atlassian.com/software/confluence"
                    }
                }
            },
            new AppListing
            {
                Name = "Figma",
                Category = "Product Design",
                Summary = "Browser-based UI design with real-time collaboration and component systems.",
                Fit = "Cross-functional squads that iterate quickly with design tokens.",
                Tags = new List<string> { "SaaS", "Collaboration", "Design Systems" },
                Alternatives = new List<AlternativeOption>
                {
                    new AlternativeOption
                    {
                        Name = "Penpot",
                        Differentiator = "Open-source with self-hosting and CSS-friendly layout.",
                        Pricing = "Free",
                        Link = "https://penpot.app"
                    },
                    new AlternativeOption
                    {
                        Name = "Sketch",
                        Differentiator = "Mac-native performance with strong plugin support.",
                        Pricing = "$12/mo billed annually",
                        Link = "https://www.sketch.com"
                    },
                    new AlternativeOption
                    {
                        Name = "Framer",
                        Differentiator = "Design-to-site publishing with production-ready code export.",
                        Pricing = "Free tier + paid plans",
                        Link = "https://www.framer.com"
                    }
                }
            }
        };

        Hero = new HeroModel
        {
            Eyebrow = "Alternative Atlas",
            Title = "Find better apps before you commit.",
            Lede = "A curated, no-fluff directory that pairs popular tools with credible alternatives. Compare the trade-offs, pricing, and ideal fit without combing through endless reviews.",
            QuickFilters = new List<string> { "Privacy-first", "Self-hostable", "Budget friendly", "Enterprise ready" },
            PillBoard = new List<string> { "Design systems", "Docs & wikis", "Team chat", "Automation", "Data viz" },
            Stats = new List<StatBlock>
            {
                new StatBlock { Value = TotalApps.ToString(), Label = "apps reviewed", Note = "Every pick includes fit notes and clear trade-offs." },
                new StatBlock { Value = TotalAlternatives.ToString(), Label = "alternatives", Note = "SaaS, self-hosted, and open-source options in one view." }
            }
        };

        ProductHeading = new SectionHeadingModel
        {
            Eyebrow = "Product options",
            Title = "Choose how you work with AltAtlas.",
            Subtitle = "Pick a format that fits your team’s pace—do-it-yourself kits, guided sprints, or full-service research.",
            CtaText = "Talk with us",
            CtaHref = "#"
        };

        ProductOptions = new List<ProductOption>
        {
            new ProductOption
            {
                Label = "Teams",
                Title = "AltAtlas for Product Teams",
                Body = "Weekly drops of vetted alternatives plus decision briefs you can paste into Slack or PRDs.",
                Pill = "Best for: squads shipping every week",
                CtaText = "See a sample brief",
                CtaHref = "#"
            },
            new ProductOption
            {
                Label = "Advisory",
                Title = "Comparison Sprints",
                Body = "Two-week engagement to answer a head-to-head question with live demos, pricing snapshots, and risk notes.",
                Pill = "Best for: time-boxed evaluations",
                CtaText = "Book a sprint",
                CtaHref = "#"
            },
            new ProductOption
            {
                Label = "Self-serve",
                Title = "Playbooks & Templates",
                Body = "Decision kits, RFP starters, and scorecards that keep buying conversations on rails.",
                Pill = "Best for: smaller teams or pilots",
                CtaText = "Download a kit",
                CtaHref = "#"
            }
        };

        CuratedHeading = new SectionHeadingModel
        {
            Eyebrow = "Curated drops",
            Title = "Pick an app, see the strongest alternatives.",
            Subtitle = "Each card highlights when to choose it, who it fits, and what you gain.",
            CtaText = "Submit your stack",
            CtaHref = "#"
        };
    }
}
