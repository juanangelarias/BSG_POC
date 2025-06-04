namespace BSG.Common.Exceptions;

public class NotExistException : Exception
{
    public NotExistException(string message)
        : base(message)
    {
    }

    public NotExistException(string message, Exception inner)
        : base(message, inner)
    {
    }
}