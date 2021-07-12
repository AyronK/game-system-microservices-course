namespace Play.Catalog.Service.Core
{
    public interface IEntity<TId>
    {
        public TId Id { get; set;  }
    }
}