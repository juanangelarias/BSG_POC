namespace BSG.BackEnd.Common.Model;

public class EmailMessage
{
    public IEnumerable<string>? To { get; set; }
    public string? Subject { get; set; }
    public string? Body { get; set; }
    public bool IsHtml { get; set; }
}