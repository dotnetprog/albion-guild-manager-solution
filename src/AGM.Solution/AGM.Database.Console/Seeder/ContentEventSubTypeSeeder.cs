using AGM.Database.Context;
using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AGM.Database.Console.Seeder
{
    public static class ContentEventSubTypeSeeder
    {
        public static async Task Seed(AGMDBContext context, ILogger logger, string FolderPath)
        {
            await SeedGatheringTypes(context, logger, $"{FolderPath}/Gathering.txt");
            await SeedCastlesTypes(context, logger, $"{FolderPath}/Castles.txt");
            await SeedMonstersTypes(context, logger, $"{FolderPath}/Monsters.txt");
            await SeedCoreTypes(context, logger, $"{FolderPath}/Core.txt");
            await SeedOutpostTypes(context, logger, $"{FolderPath}/Outpost.txt");
            await SeedVortexTypes(context, logger, $"{FolderPath}/Vortex.txt");
            logger.LogInformation("Content Event Sub Types are seeded !");
        }
        private static async Task SeedGatheringTypes(AGMDBContext dbContext, ILogger logger, string FilePath)
            => await SeedSubType(dbContext, logger, "Gathering", FilePath);
        private static async Task SeedMonstersTypes(AGMDBContext dbContext, ILogger logger, string FilePath)
            => await SeedSubType(dbContext, logger, "Monsters", FilePath);
        private static async Task SeedCoreTypes(AGMDBContext dbContext, ILogger logger, string FilePath)
            => await SeedSubType(dbContext, logger, "Core", FilePath);
        private static async Task SeedCastlesTypes(AGMDBContext dbContext, ILogger logger, string FilePath)
            => await SeedSubType(dbContext, logger, "Castles", FilePath);
        private static async Task SeedOutpostTypes(AGMDBContext dbContext, ILogger logger, string FilePath)
            => await SeedSubType(dbContext, logger, "Outpost", FilePath);
        private static async Task SeedVortexTypes(AGMDBContext dbContext, ILogger logger, string FilePath)
            => await SeedSubType(dbContext, logger, "Vortex", FilePath);

        private static async Task SeedSubType(AGMDBContext dbContext, ILogger logger, string TypeName, string FilePath)
        {
            var lines = File.ReadAllLines(FilePath);
            var ParentType = dbContext.ContentEventTypes.Include(c => c.SubTypes).FirstOrDefault(x => x.Name == TypeName);
            logger.LogInformation("{TypeName} Type Id : {Id}", TypeName, ParentType.Id);
            logger.LogInformation("Existing {TypeName} Sub types Count: {count}", TypeName, ParentType.SubTypes.Count);

            foreach (var line in lines)
            {
                var rawSubType = line.Split(',');
                var existingSubType = ParentType.SubTypes.FirstOrDefault(st => st.Name == rawSubType[0]);
                if (existingSubType != null)
                {
                    if (existingSubType.Emoji == rawSubType[1])
                    {
                        continue;
                    }
                    existingSubType.Emoji = rawSubType[1];
                    dbContext.Update(existingSubType);
                }
                else
                {
                    var newSubType = new ContentEventSubType
                    {
                        Id = ContentEventSubTypeId.New(),
                        Emoji = rawSubType[1],
                        Name = rawSubType[0],
                        ContentEventTypeId = ParentType.Id
                    };
                    await dbContext.AddAsync(newSubType);
                }
            }
            await dbContext.SaveChangesAsync();
            logger.LogInformation("{TypeName} Sub types are seeded.", TypeName);
        }
    }
}
