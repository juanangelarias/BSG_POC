namespace BSG.Common.Model;

public class PagedResult<T>: Response<List<T>>   
    where T : class
{
    public PagedResult(List<T> items, int count)
    {
        Content = items;
        Count = count;
    }

    public int Count { get; set; }   
}