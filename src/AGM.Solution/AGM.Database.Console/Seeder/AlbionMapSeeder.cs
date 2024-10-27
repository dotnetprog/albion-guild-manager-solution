using AGM.Database.Context;
using AGM.Domain.Entities;
using AGM.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AGM.Database.Console.Seeder
{
    public static class AlbionMapSeeder
    {
        public static async Task Seed(AGMDBContext dbContext, ILogger logger, string BaseMapFolder)
        {

            var Maps = await dbContext.Maps.ToListAsync();
            logger.LogInformation("Number of maps retrieved from database: {mapcount}", Maps.Count);
            await AlbionMapSeeder.SeedAvalonRoads(dbContext, Maps, $"{BaseMapFolder}/AvalonRoads.txt");
            logger.LogInformation("Avalon roads were seeded.");

            await AlbionMapSeeder.SeedBlackZones(dbContext, Maps, $"{BaseMapFolder}/Blackzones.txt");
            logger.LogInformation("Blackzones were seeded.");
            await AlbionMapSeeder.SeedBlueZones(dbContext, Maps, $"{BaseMapFolder}/Bluezones.txt");
            logger.LogInformation("Bluezones were seeded.");
            await AlbionMapSeeder.SeedYellowZones(dbContext, Maps, $"{BaseMapFolder}/Yellowzones.txt");
            logger.LogInformation("Yellowzones were seeded.");
            await AlbionMapSeeder.SeedRedZones(dbContext, Maps, $"{BaseMapFolder}/Redzones.txt");
            logger.LogInformation("Redzones were seeded.");
            await AlbionMapSeeder.SeedCities(dbContext, Maps, $"{BaseMapFolder}/Cities.txt");
            logger.LogInformation("Cities were seeded.");
        }
        private static async Task SeedAvalonRoads(AGMDBContext dbContext, IReadOnlyCollection<AlbionMap> ExistingMaps, string AvalonRoadsFilePath)
       => await SeedMaps(dbContext, ExistingMaps, AvalonRoadsFilePath, AlbionMapType.Road);
        private static async Task SeedBlackZones(AGMDBContext dbContext, IReadOnlyCollection<AlbionMap> ExistingMaps, string BlackZonesFilePath)
            => await SeedMaps(dbContext, ExistingMaps, BlackZonesFilePath, AlbionMapType.Blackzone);
        private static async Task SeedRedZones(AGMDBContext dbContext, IReadOnlyCollection<AlbionMap> ExistingMaps, string RedZonesFilePath)
            => await SeedMaps(dbContext, ExistingMaps, RedZonesFilePath, AlbionMapType.RedZone);
        private static async Task SeedYellowZones(AGMDBContext dbContext, IReadOnlyCollection<AlbionMap> ExistingMaps, string YellowZonesFilePath)
            => await SeedMaps(dbContext, ExistingMaps, YellowZonesFilePath, AlbionMapType.YellowZone);
        private static async Task SeedBlueZones(AGMDBContext dbContext, IReadOnlyCollection<AlbionMap> ExistingMaps, string BlueZonesFilePath)
            => await SeedMaps(dbContext, ExistingMaps, BlueZonesFilePath, AlbionMapType.Bluezone);
        private static async Task SeedCities(AGMDBContext dbContext, IReadOnlyCollection<AlbionMap> ExistingMaps, string CitiesFilePath)
            => await SeedMaps(dbContext, ExistingMaps, CitiesFilePath, AlbionMapType.City);
        private static async Task SeedMaps(AGMDBContext dbContext, IReadOnlyCollection<AlbionMap> ExistingMaps, string FilePath, AlbionMapType TypeToLoad)
        {

            var lines = File.ReadAllLines(FilePath);

            foreach (var line in lines)
            {
                var existingMap = ExistingMaps.FirstOrDefault(x => x.Name.ToLower() == line.ToLower());
                if (existingMap != null)
                {
                    if (existingMap.Type == TypeToLoad)
                    {
                        continue;
                    }
                    existingMap.Type = TypeToLoad;
                    dbContext.Update(existingMap);
                }
                else
                {
                    var newMap = new AlbionMap
                    {
                        Id = AlbionMapId.New(),
                        Name = line,
                        Type = TypeToLoad
                    };
                    await dbContext.AddAsync(newMap);
                }

            }
            await dbContext.SaveChangesAsync();

        }

    }
}
