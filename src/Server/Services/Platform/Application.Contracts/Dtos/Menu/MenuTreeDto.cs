namespace Hkust.Platform.Application.Contracts.Dtos;

[Serializable]
public class MenuTreeDto : IDto
{
    public IEnumerable<Node<long>> TreeData { get; set; }
    public IEnumerable<long> CheckedIds { get; set; }
}