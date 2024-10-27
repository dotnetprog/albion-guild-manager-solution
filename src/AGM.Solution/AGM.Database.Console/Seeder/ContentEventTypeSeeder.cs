using AGM.Database.Context;
using AGM.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AGM.Database.Console.Seeder
{
    public static class ContentEventTypeSeeder
    {

        public static async Task Seed(AGMDBContext dbContext, ILogger logger, string BaseFolder)
        {

            var existingTypes = await dbContext.ContentEventTypes.ToListAsync();
            logger.LogInformation("Content Event Types retrieved from database: {count}", existingTypes.Count);
            var typesFromFiles = File.ReadAllLines($"{BaseFolder}/ContentType.txt");
            foreach (var type in typesFromFiles)
            {
                var existingType = existingTypes.FirstOrDefault(t => t.Name.ToLower() == type.ToLower());
                if (existingType != null) { continue; }
                var ContentEventType = new ContentEventType
                {
                    Id = ContentEventTypeId.New(),
                    Name = type,
                };
                await dbContext.AddAsync(ContentEventType);
            }
            await dbContext.SaveChangesAsync();
            logger.LogInformation("Content Event Types Table Seeded.");
        }

    }
}
