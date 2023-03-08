namespace Hkust.Infras.Entities
{
    public interface IEntity<TKey>
    {
        public TKey Id { get; set; }
    }
}