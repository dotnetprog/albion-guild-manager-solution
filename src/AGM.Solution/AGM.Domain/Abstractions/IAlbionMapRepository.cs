using AGM.Domain.Entities;
using AGM.Domain.Enums;

namespace AGM.Domain.Abstractions
{
    public interface IAlbionMapRepository
    {
        public Task<AlbionMap> GetByName(string name);
        public Task<AlbionMap> GetById(AlbionMapId Id);
        public Task<IEnumerable<AlbionMap>> GetAllByTypes(params AlbionMapType[] types);
        public Task<IEnumerable<AlbionMap>> GetAll();
    }
}
