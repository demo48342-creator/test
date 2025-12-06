using System;
using System.Collections.Generic;
using System.Linq;
using RazorPagees.Models;

namespace RazorPagees.Services;

public class AppCatalogService
{
    private readonly List<AppListing> _topApps;
    private readonly List<AppListing> _newLaunches;
    private readonly object _lock = new();

    public AppCatalogService()
    {
        (_topApps, _newLaunches) = Seed();
        RecalculateScores();
    }

    public IReadOnlyList<AppListing> GetTopApps() => _topApps;

    public IReadOnlyList<AppListing> GetNewLaunches() => _newLaunches;

    public AppListing? GetBySlug(string slug) =>
        _topApps.Concat(_newLaunches).FirstOrDefault(a => a.Slug.Equals(slug, System.StringComparison.OrdinalIgnoreCase));

    public List<AppListing> GetByCategorySlug(string categorySlug)
    {
        var anchor = Slugify(categorySlug);
        return _topApps.Concat(_newLaunches)
            .Where(a => Slugify(a.Category).Equals(anchor, System.StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private static string Slugify(string text) => text.Replace(" ", "-").ToLowerInvariant();

    public (int Up, int Down, int Score)? Vote(string slug, string direction)
    {
        lock (_lock)
        {
            var app = GetBySlug(slug);
            if (app is null)
            {
                return null;
            }

            if (string.Equals(direction, "up", System.StringComparison.OrdinalIgnoreCase))
            {
                app.Upvotes += 1;
            }
            else if (string.Equals(direction, "down", System.StringComparison.OrdinalIgnoreCase))
            {
                app.Downvotes += 1;
            }
            else
            {
                return null;
            }

            RecalculateScores();
            return (app.Upvotes, app.Downvotes, app.Score);
        }
    }

    private void RecalculateScores()
    {
        foreach (var app in _topApps.Concat(_newLaunches))
        {
            // Simple leaderboard score: heavier weight on upvotes, some on comments, freshness bump for new launches.
            var freshness = app.IsNewLaunch ? 25 : 10;
            var kitty = app.GoldenKittyEligible ? 5 : 0;
            app.Score = (app.Upvotes * 3) + (app.CommentsCount * 2) + freshness + kitty;
        }

        _newLaunches.Sort((a, b) => b.Score.CompareTo(a.Score));
        _topApps.Sort((a, b) => b.Score.CompareTo(a.Score));
    }

    private static (List<AppListing> Top, List<AppListing> NewLaunches) Seed()
    {
        var top = new List<AppListing>
        {
            new AppListing
            {
                Slug = "adobe-photoshop",
                Name = "Adobe Photoshop",
                Category = "Creative Suite",
                Summary = "Pixel-perfect editing powerhouse for teams that need retouching, automation, and deep integration.",
                Fit = "Studios that demand advanced layer workflows and ecosystem plugins.",
                Tags = new List<string> { "Paid", "Pro", "Desktop" },
                Signal = "Top pick",
                LogoUrl = "https://logo.clearbit.com/adobe.com",
                Website = "https://www.adobe.com/products/photoshop",
                HtmlDescription = "<p>Trusted by studios for heavy-duty editing, automations, and plugin depth. Ideal when you need layered workflows and ecosystem integrations.</p>",
                Upvotes = 1840,
                Downvotes = 63,
                CommentsCount = 412,
                LaunchDate = DateTime.UtcNow.AddDays(-16),
                GoldenKittyEligible = true,
                DiscussionLink = "#",
                Alternatives = new List<AlternativeOption>
                {
                    new AlternativeOption
                    {
                        Name = "Affinity Photo",
                        Differentiator = "One-time purchase with high-end tools tuned for Apple silicon.",
                        Pricing = "$69 once",
                        Link = "https://affinity.serif.com",
                        LogoUrl = "https://logo.clearbit.com/serif.com"
                    },
                    new AlternativeOption
                    {
                        Name = "GIMP",
                        Differentiator = "Open-source, scriptable, and friendly to custom pipelines.",
                        Pricing = "Free",
                        Link = "https://www.gimp.org",
                        LogoUrl = "https://logo.clearbit.com/gimp.org"
                    },
                    new AlternativeOption
                    {
                        Name = "Pixelmator Pro",
                        Differentiator = "ML-powered adjustments and quick export presets for creatives on Mac.",
                        Pricing = "$49 once",
                        Link = "https://www.pixelmator.com/pro/",
                        LogoUrl = "https://logo.clearbit.com/pixelmator.com"
                    }
                }
            },
            new AppListing
            {
                Slug = "slack",
                Name = "Slack",
                Category = "Team Communication",
                Summary = "Channel-based messaging with searchable history and a vast integration ecosystem.",
                Fit = "Distributed teams that rely on automations and app workflows.",
                Tags = new List<string> { "SaaS", "Collaboration", "Integrations" },
                Signal = "Top pick",
                LogoUrl = "https://logo.clearbit.com/slack.com",
                Website = "https://slack.com",
                HtmlDescription = "<p>Channel-first messaging with search, bots, and a massive integration ecosystem. Great for teams that automate workflows.</p>",
                Upvotes = 1620,
                Downvotes = 120,
                CommentsCount = 365,
                LaunchDate = DateTime.UtcNow.AddDays(-5),
                GoldenKittyEligible = true,
                DiscussionLink = "#",
                Alternatives = new List<AlternativeOption>
                {
                    new AlternativeOption
                    {
                        Name = "Discord",
                        Differentiator = "Casual voice-first communication with flexible community roles.",
                        Pricing = "Free + Nitro",
                        Link = "https://discord.com",
                        LogoUrl = "https://logo.clearbit.com/discord.com"
                    },
                    new AlternativeOption
                    {
                        Name = "Microsoft Teams",
                        Differentiator = "Deep Office 365 integration and enterprise governance controls.",
                        Pricing = "Included with Microsoft 365",
                        Link = "https://www.microsoft.com/microsoft-teams",
                        LogoUrl = "https://logo.clearbit.com/microsoft.com"
                    },
                    new AlternativeOption
                    {
                        Name = "Twist",
                        Differentiator = "Threaded, asynchronous conversations that tame notification overload.",
                        Pricing = "Free tier + paid plans",
                        Link = "https://twist.com",
                        LogoUrl = "https://logo.clearbit.com/twist.com"
                    }
                }
            },
            new AppListing
            {
                Slug = "notion",
                Name = "Notion",
                Category = "Docs & Workspaces",
                Summary = "All-in-one docs, databases, and wiki with collaborative blocks and templates.",
                Fit = "Teams that need flexible knowledge bases and lightweight project tracking.",
                Tags = new List<string> { "SaaS", "Collaborative", "Flexible" },
                Signal = "Top pick",
                LogoUrl = "https://logo.clearbit.com/notion.so",
                Website = "https://www.notion.so",
                HtmlDescription = "<p>Modular docs, databases, and wiki blocks. Solid for flexible knowledge bases and lightweight projects.</p>",
                Upvotes = 1510,
                Downvotes = 45,
                CommentsCount = 298,
                LaunchDate = DateTime.UtcNow.AddDays(-22),
                GoldenKittyEligible = true,
                DiscussionLink = "#",
                Alternatives = new List<AlternativeOption>
                {
                    new AlternativeOption
                    {
                        Name = "Coda",
                        Differentiator = "Docs that behave like apps with automations and Packs.",
                        Pricing = "Free tier + paid plans",
                        Link = "https://coda.io",
                        LogoUrl = "https://logo.clearbit.com/coda.io"
                    },
                    new AlternativeOption
                    {
                        Name = "Obsidian",
                        Differentiator = "Local-first markdown graph with extensible plugin ecosystem.",
                        Pricing = "Free + sync add-ons",
                        Link = "https://obsidian.md",
                        LogoUrl = "https://logo.clearbit.com/obsidian.md"
                    },
                    new AlternativeOption
                    {
                        Name = "Confluence",
                        Differentiator = "Structured docs with enterprise permissions and change tracking.",
                        Pricing = "Free tier + paid plans",
                        Link = "https://www.atlassian.com/software/confluence",
                        LogoUrl = "https://logo.clearbit.com/atlassian.com"
                    }
                }
            },
            new AppListing
            {
                Slug = "figma",
                Name = "Figma",
                Category = "Product Design",
                Summary = "Browser-based UI design with real-time collaboration and component systems.",
                Fit = "Cross-functional squads that iterate quickly with design tokens.",
                Tags = new List<string> { "SaaS", "Collaboration", "Design Systems" },
                Signal = "Top pick",
                LogoUrl = "https://logo.clearbit.com/figma.com",
                Website = "https://www.figma.com",
                HtmlDescription = "<p>Real-time design with components, tokens, and dev-friendly handoff. Built for fast, cross-functional squads.</p>",
                Upvotes = 1760,
                Downvotes = 51,
                CommentsCount = 502,
                LaunchDate = DateTime.UtcNow.AddDays(-9),
                GoldenKittyEligible = true,
                DiscussionLink = "#",
                Alternatives = new List<AlternativeOption>
                {
                    new AlternativeOption
                    {
                        Name = "Penpot",
                        Differentiator = "Open-source with self-hosting and CSS-friendly layout.",
                        Pricing = "Free",
                        Link = "https://penpot.app",
                        LogoUrl = "https://logo.clearbit.com/penpot.app"
                    },
                    new AlternativeOption
                    {
                        Name = "Sketch",
                        Differentiator = "Mac-native performance with strong plugin support.",
                        Pricing = "$12/mo billed annually",
                        Link = "https://www.sketch.com",
                        LogoUrl = "https://logo.clearbit.com/sketch.com"
                    },
                    new AlternativeOption
                    {
                        Name = "Framer",
                        Differentiator = "Design-to-site publishing with production-ready code export.",
                        Pricing = "Free tier + paid plans",
                        Link = "https://www.framer.com",
                        LogoUrl = "https://logo.clearbit.com/framer.com"
                    }
                }
            }
        };

        var launches = new List<AppListing>
        {
            new AppListing
            {
                Slug = "arc-browser",
                Name = "Arc",
                Category = "Browser",
                Summary = "A rethink of the browser with spaces, tidy tabs, and built-in easels for quick mocks.",
                Fit = "Product teams that context-switch across projects and want less tab sprawl.",
                Tags = new List<string> { "Mac/Windows", "Fast UI", "Spaces" },
                Signal = "New launch",
                LogoUrl = "https://logo.clearbit.com/arc.net",
                Website = "https://arc.net",
                HtmlDescription = "<p>Spaces, split views, and tidy tabs plus a built-in canvas for quick mocks. Designed for context-switchers.</p>",
                IsNewLaunch = true,
                Upvotes = 1240,
                Downvotes = 42,
                CommentsCount = 410,
                LaunchDate = DateTime.UtcNow.AddDays(-1),
                GoldenKittyEligible = true,
                DiscussionLink = "#",
                Alternatives = new List<AlternativeOption>
                {
                    new AlternativeOption
                    {
                        Name = "Sidekick",
                        Differentiator = "Workspace-aware browser that groups apps by project and blocks attention drains.",
                        Pricing = "Free + Pro",
                        Link = "https://www.meetsidekick.com",
                        LogoUrl = "https://logo.clearbit.com/meetsidekick.com"
                    },
                    new AlternativeOption
                    {
                        Name = "Vivaldi",
                        Differentiator = "Advanced tab stacks, tiling, and keyboard-first workflows.",
                        Pricing = "Free",
                        Link = "https://vivaldi.com",
                        LogoUrl = "https://logo.clearbit.com/vivaldi.com"
                    },
                    new AlternativeOption
                    {
                        Name = "Brave",
                        Differentiator = "Privacy-first browsing with built-in shields and fast defaults.",
                        Pricing = "Free",
                        Link = "https://brave.com",
                        LogoUrl = "https://logo.clearbit.com/brave.com"
                    }
                }
            },
            new AppListing
            {
                Slug = "perplexity-ai",
                Name = "Perplexity",
                Category = "AI & ML",
                Summary = "Fast AI answers with live search grounding and citations.",
                Fit = "Teams that need quick research with source links.",
                Tags = new List<string> { "AI", "Search", "Productivity" },
                Signal = "New launch",
                LogoUrl = "https://logo.clearbit.com/perplexity.ai",
                Website = "https://www.perplexity.ai",
                HtmlDescription = "<p>Live, cited answers powered by multiple models. Great for drafting, research, and quick summaries.</p>",
                IsNewLaunch = true,
                Upvotes = 860,
                Downvotes = 22,
                CommentsCount = 180,
                LaunchDate = DateTime.UtcNow.AddDays(-1),
                GoldenKittyEligible = true,
                DiscussionLink = "#",
                Alternatives = new List<AlternativeOption>
                {
                    new AlternativeOption
                    {
                        Name = "ChatGPT",
                        Differentiator = "Conversational AI with GPT-4 options and plugins.",
                        Pricing = "Free + Plus",
                        Link = "https://chat.openai.com",
                        LogoUrl = "https://logo.clearbit.com/openai.com"
                    },
                    new AlternativeOption
                    {
                        Name = "Claude",
                        Differentiator = "Long-context assistant great for documents and analysis.",
                        Pricing = "Free + Pro",
                        Link = "https://claude.ai",
                        LogoUrl = "https://logo.clearbit.com/anthropic.com"
                    },
                    new AlternativeOption
                    {
                        Name = "You.com",
                        Differentiator = "AI search assistant with plugins and multi-modal answers.",
                        Pricing = "Free + Pro",
                        Link = "https://you.com",
                        LogoUrl = "https://logo.clearbit.com/you.com"
                    }
                }
            },
            new AppListing
            {
                Slug = "linear",
                Name = "Linear",
                Category = "Issue Tracking",
                Summary = "Blazing-fast issue tracking with opinionated workflows and keyboard-first commands.",
                Fit = "Product squads that want speed, clean API access, and predictable sprints.",
                Tags = new List<string> { "SaaS", "Product", "Speed" },
                Signal = "New drop",
                LogoUrl = "https://logo.clearbit.com/linear.app",
                Website = "https://linear.app",
                HtmlDescription = "<p>Lightning-fast issue tracking with opinionated flows, keyboard commands, and clean API hooks.</p>",
                IsNewLaunch = true,
                Upvotes = 980,
                Downvotes = 28,
                CommentsCount = 210,
                LaunchDate = DateTime.UtcNow.AddDays(-2),
                GoldenKittyEligible = true,
                DiscussionLink = "#",
                Alternatives = new List<AlternativeOption>
                {
                    new AlternativeOption
                    {
                        Name = "Height",
                        Differentiator = "Powerful automations, docs, and chat woven into tasks.",
                        Pricing = "Free tier + paid plans",
                        Link = "https://height.app",
                        LogoUrl = "https://logo.clearbit.com/height.app"
                    },
                    new AlternativeOption
                    {
                        Name = "Shortcut",
                        Differentiator = "Lightweight sprints with clubhouse-style flows and strong Slack/ GitHub links.",
                        Pricing = "Free tier + paid plans",
                        Link = "https://www.shortcut.com",
                        LogoUrl = "https://logo.clearbit.com/shortcut.com"
                    },
                    new AlternativeOption
                    {
                        Name = "Jira",
                        Differentiator = "Deep enterprise controls, workflows, and reporting for large teams.",
                        Pricing = "Free tier + paid plans",
                        Link = "https://www.atlassian.com/software/jira",
                        LogoUrl = "https://logo.clearbit.com/atlassian.com"
                    }
                }
            }
        };

        return (top, launches);
    }
}
