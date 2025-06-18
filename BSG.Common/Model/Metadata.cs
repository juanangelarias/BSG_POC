using BSG.Common.DTO;

namespace BSG.Common.Model;

public class Metadata
{
  public ComponentDto Component { get; set; } = null!;

  public List<MetadataDetail> Details { get; set; } = [];
}