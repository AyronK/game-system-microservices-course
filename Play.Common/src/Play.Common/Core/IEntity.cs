namespace Play.Common.Core
{
    public interface IEntity<TId>
    {
        public TId Id { get; set;  }
    }
}