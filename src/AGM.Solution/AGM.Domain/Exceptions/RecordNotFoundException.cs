namespace AGM.Domain.Exceptions
{
    public class RecordNotFoundException<TEntity> : Exception where TEntity : class
    {
        private string EntityType { get; set; }
        private string Key { get; set; }
        public RecordNotFoundException(Guid Id) : base($"{typeof(TEntity).Name}'s record does not exist with Id = {Id.ToString()}")
        {

            EntityType = typeof(TEntity).Name;
            Key = Id.ToString();
        }
    }
}
