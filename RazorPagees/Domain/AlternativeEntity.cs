namespace RazorPagees.Domain;

public class AlternativeEntity
{
    public int Id { get; set; }

    public int AppId { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Differentiator { get; set; } = string.Empty;

    public string Pricing { get; set; } = string.Empty;

    public string Link { get; set; } = string.Empty;

    public string LogoUrl { get; set; } = string.Empty;

    public AppEntity? App { get; set; }
}
