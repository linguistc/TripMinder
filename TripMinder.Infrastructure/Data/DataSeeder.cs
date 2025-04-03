using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using TripMinder.Data.Entities;
using TripMinder.Data;

namespace TripMinder.Infrastructure.Data;

public class DataSeeder
{
    private readonly AppDBContext _context;
    
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true, // تجاهل حساسية حالة الأحرف في أسماء الحقول
        ReadCommentHandling = JsonCommentHandling.Skip, // تجاهل التعليقات في ملفات الجيسون
        AllowTrailingCommas = true // السماح بالفواصل الزائدة في نهاية العناصر
    };


    public DataSeeder(AppDBContext context)
    { 
        this._context = context ?? throw new ArgumentNullException(nameof(context));
        
    }

    public async Task SeedAsync()
    {
        // Ensure created database
        await this._context.Database.MigrateAsync();
        
        // Seed data if needed
        await SeedTablesAsync<AccomodationClass>("AccomodationClass.json", this._context.AccomodationsClasses);
        await SeedTablesAsync<AccomodationType>("AccomodationType.json", this._context.AccomodationTypes);
        await SeedTablesAsync<EntertainmentClass>("EntertainmentClass.json", this._context.EntertainmentsClasses);
        await SeedTablesAsync<EntertainmentType>( "EntertainmentType.json", this._context.EntertainmentTypes);
        await SeedTablesAsync<FoodCategory>("FoodCategory.json", this._context.FoodCategories);
        await SeedTablesAsync<RestaurantClass>("RestaurantClass.json", this._context.RestaurantsClasses);
        await SeedTablesAsync<TourismAreaClass>("TourismClass.json", this._context.TourismAreaClasses);
        await SeedTablesAsync<TourismType>("TourismType.json", this._context.TourismAreaTypes);
        
        // Main Entities
        await SeedTablesAsync<Accomodation>("Accomodations.json", this._context.Accomodations);
        await SeedTablesAsync<Restaurant>("Restaurants.json", this._context.Restaurants);
        await SeedTablesAsync<Entertainment>("Entertainments.json", this._context.Entertainments);
        await SeedTablesAsync<TourismArea>("TourismAreas.json", this._context.TourismAreas);
    }

    private async Task SeedTablesAsync<T>(string fileName, DbSet<T> dbSet) where T : class
    {
        if (await dbSet.AnyAsync()) return;  // Skip if data already exists in the database
        
        var filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "SeedData", fileName);
        filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "TripMinder.Data", "SeedData", fileName);
        if (!File.Exists(filePath))
        {
            Console.WriteLine($"Seed file not found: {filePath}");
            return;
        }

        try
        {
            var jsonData = await File.ReadAllTextAsync(filePath);
            var entities = JsonSerializer.Deserialize<List<T>>(jsonData, _jsonOptions);

            if (entities == null || !entities.Any())
            {
                Console.WriteLine($"No data to seed from: {filePath}");
                return;
            }

            // Reset Ids to ensure EF generates them automatically
            foreach (var entity in entities)
            {
                var idProperty = typeof(T).GetProperty("Id");
                if (idProperty != null && idProperty.PropertyType == typeof(int))
                {
                    idProperty.SetValue(entity, 0); // Set Id to 0 so EF generates a new one
                }
            }
            
            await dbSet.AddRangeAsync(entities);
            await this._context.SaveChangesAsync();
            Console.WriteLine($"Successfully seeded data from: {filePath}");
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"Error deserializing JSON from {filePath}: {ex.Message}");
        }
        catch (DbUpdateException ex)
        {
            Console.WriteLine($"Error saving data to database from {filePath}: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error seeding data from {filePath}: {ex.Message}");
        }
    }
}