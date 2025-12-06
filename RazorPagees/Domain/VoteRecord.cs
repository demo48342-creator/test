using System;

namespace RazorPagees.Domain;

public class VoteRecord
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string AppSlug { get; set; } = string.Empty;

    public VoteDirection Direction { get; set; }

    public string? Actor { get; set; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}

public enum VoteDirection
{
    Up = 1,
    Down = -1
}

public record VoteTally(string AppSlug, int Up, int Down, int Score);
