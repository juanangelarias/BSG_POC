using Radzen;
using Radzen.Blazor;

namespace BSG.App.Common.Components;

public sealed class BSGDataGrid<T> : RadzenDataGrid<T>
    where T : class
{
    public BSGDataGrid()
    {
        AllowColumnPicking = true;
        AllowColumnResize = true;
        AllowFiltering = true;
        AllowGrouping = true;
        AllowSorting = true;
        Density = Density.Compact;
        EditMode = DataGridEditMode.Single;
        EmptyText = "No records to display.";
        FilterMode = FilterMode.Advanced;
        SelectionMode = DataGridSelectionMode.Single;
        Style = "border-radius: 10px; height: 400px;";
    }
}