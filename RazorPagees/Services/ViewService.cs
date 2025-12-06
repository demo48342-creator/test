using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RazorPagees.Data;

namespace RazorPagees.Services;

public class ViewService
{
    private readonly AppDbContext _db;

    public ViewService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<int?> RegisterAsync(string slug, CancellationToken cancellationToken = default)
    {
        var app = await _db.Apps.FirstOrDefaultAsync(a => a.Slug == slug, cancellationToken);
        if (app is null)
        {
            return null;
        }

        app.Views += 1;
        _db.Apps.Update(app);
        await _db.SaveChangesAsync(cancellationToken);
        return app.Views;
    }
}
