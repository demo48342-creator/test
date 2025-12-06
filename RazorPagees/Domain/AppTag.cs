namespace RazorPagees.Domain;

public class AppTag
{
    public int Id { get; set; }

    public int AppId { get; set; }

    public string Value { get; set; } = string.Empty;

    public AppEntity? App { get; set; }
}
