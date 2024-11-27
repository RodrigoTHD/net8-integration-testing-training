namespace MyApp.MockingDependencies.Model;

public class Notification
{
    public string To { get; set; } = string.Empty;

    public string Subject { get; set; } = string.Empty;

    public string Body { get; set; } = string.Empty;

    public bool IsEmpty
        => string.IsNullOrWhiteSpace(To) || string.IsNullOrWhiteSpace(Subject) || string.IsNullOrWhiteSpace(Body);
}
