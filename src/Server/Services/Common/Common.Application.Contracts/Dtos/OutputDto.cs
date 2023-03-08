namespace Hkust.Common.Application.Contracts.Dtos;

[Serializable]
public abstract class OutputDto : IDto
{
    public virtual long Id { get; set; }
}