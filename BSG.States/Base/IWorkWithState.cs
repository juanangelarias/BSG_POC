using BSG.Common.DTO.Base;
using BSG.Common.Model;

namespace BSG.States.Base;

public interface IWorkWithState<T, TEnum> 
    where T : DtoBase, new() 
    where TEnum : Enum
{
    List<T> List { get; set; }
    List<T>? Selected { get; set; }
    //TableData<T>? Table { get; set; }
    int Count { get; set; }
    TEnum ActiveFunction { get; set; }
    Task Get(QueryParams parameters);
    Task Create();
    Task Update();
    Task Delete();
    void SetSelected(long id);
}