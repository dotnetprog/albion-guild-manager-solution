using AGM.Database.Context;
using AGM.Domain.Abstractions;
using AGM.Domain.Entities;
using AGM.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace AGM.EntityFramework.Repositories
{
    public class EFAlbionMapRepository : IAlbionMapRepository
    {
        private readonly AGMDBContext _AgmContext;

        public EFAlbionMapRepository(AGMDBContext agmContext)
        {
            _AgmContext = agmContext;
        }

        public async Task<IEnumerable<AlbionMap>> GetAll()
        {
            return await _AgmContext.Maps.ToListAsync();
        }

        public async Task<IEnumerable<AlbionMap>> GetAllByTypes(params AlbionMapType[] types)
        {
            return await _AgmContext.Maps.Where(m => types.Contains(m.Type.Value)).ToListAsync();
        }

        public async Task<AlbionMap> GetById(AlbionMapId Id)
        {
            return await _AgmContext.Maps.FindAsync(Id);
        }

        public async Task<AlbionMap> GetByName(string name)
        {
            return await _AgmContext.Maps.FirstOrDefaultAsync(m => m.Name == name);
        }
    }
}

