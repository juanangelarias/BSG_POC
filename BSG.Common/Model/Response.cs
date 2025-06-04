namespace BSG.Common.Model;

public class Response<T>
{
    public T? Content { get; set; }
    public Error? Error { get; set; }
    public bool Success => Error == null;
    public DateTime ExecutionTime { get; set; } = DateTime.UtcNow;
}