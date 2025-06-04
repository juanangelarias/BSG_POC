namespace BSG.Common.Exceptions;

public class DataServiceException : Exception
{
    public DataServiceException(string message)
        : base(message)
    {
    }

    public DataServiceException(string message, Exception inner)
        : base(message, inner)
    {
    }
}