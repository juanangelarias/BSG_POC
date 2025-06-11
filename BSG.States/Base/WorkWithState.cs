using BSG.Common.DTO.Base;
using BSG.Common.Model;

namespace BSG.States.Base;

public abstract class WorkWithState<T, TEnum>: StateBase, IWorkWithState<T, TEnum>
    where T : DtoBase, new()
    where TEnum : Enum
{
    #region Fields & Properties

    #region List

    private List<T> _list = [];

    public List<T> List
    {
        get => _list;
        set
        {
            _list = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Selected

    private List<T>? _selected;

    public List<T>? Selected
    {
        get => _selected;
        set
        {
            _selected = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region Table

    /*private TableData<T>? _table;

    public TableData<T>? Table
    {
        get => _table;
        set
        {
            _table = value;
            OnPropertyChanged();
        }
    }*/

    #endregion

    #region Count

    private int _count;

    public int Count
    {
        get => _count;
        set
        {
            _count = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #region ActiveFunction

    private TEnum _activeFunction = default!;

    public TEnum ActiveFunction
    {
        get => _activeFunction;
        set
        {
            _activeFunction = value;
            OnPropertyChanged();
        }
    }

    #endregion

    #endregion

    public virtual async Task Get(QueryParams parameters)
    {
        await Task.Delay(5);
    }

    public virtual async Task Create()
    {
        await Task.Delay(5);
    }

    public virtual async Task Update()
    {
        await Task.Delay(5);
    }

    public virtual async Task Delete()
    {
        await Task.Delay(5);
    }

    public virtual void SetSelected(long id)
    {
        Selected = id == 0
            ? [GetNew()]
            : List.Where(f => f.Id == id).ToList();
    }
    
    protected virtual T GetNew()
    {
        return new T { Id = 0 };
    }
}