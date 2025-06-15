using BSG.Common.DTO;

namespace BSG.Common.Model;

public class Metadata
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Tooltip { get; set; } = "";
    public string Help { get; set; } = "";
    public bool IsEnabled { get; set; }
    public bool IsVisible { get; set; }
    
    public ComponentDto Component { get; set; } = null!;
}