namespace Hkust.Infras.Entities
{
    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }
}