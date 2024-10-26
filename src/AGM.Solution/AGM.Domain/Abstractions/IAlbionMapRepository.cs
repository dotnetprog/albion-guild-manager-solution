using AGM.Domain.Entities;
using AGM.Domain.Enums;

namespace AGM.Domain.Abstractions
{
    public interface IAlbionMapRepository
    {
        public Task<AlbionMap> GetByName(string name);
        public Task<AlbionMap> GetById(AlbionMapId Id);
        public Task<IEnumerable<AlbionMap>> GetAllByType(AlbionMapType type);
        public Task<IEnumerable<AlbionMap>> GetAll();
    }
}
