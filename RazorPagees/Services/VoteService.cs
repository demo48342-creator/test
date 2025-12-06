using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPagees.Data;
using RazorPagees.Domain;

namespace RazorPagees.Services;

public class VoteService
{
    private readonly AppDbContext _dbContext;

    public VoteService(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<VoteTally> RecordAsync(string slug, VoteDirection direction, string? actor = null, CancellationToken cancellationToken = default)
    {
        var record = new VoteRecord
        {
            AppSlug = slug,
            Direction = direction,
            Actor = actor,
            CreatedAt = DateTimeOffset.UtcNow
        };

        await _dbContext.Votes.AddAsync(record, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var up = await _dbContext.Votes.CountAsync(v => v.AppSlug == slug && v.Direction == VoteDirection.Up, cancellationToken);
        var down = await _dbContext.Votes.CountAsync(v => v.AppSlug == slug && v.Direction == VoteDirection.Down, cancellationToken);

        var app = await _dbContext.Apps.FirstOrDefaultAsync(a => a.Slug == slug, cancellationToken);
        if (app is not null)
        {
            app.Upvotes = up;
            app.Downvotes = down;
            _dbContext.Apps.Update(app);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        var score = (up * 3) - down;
        return new VoteTally(slug, up, down, score);
    }
}
