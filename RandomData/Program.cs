using Microsoft.EntityFrameworkCore;
using TripMinder.Infrastructure.Data;

namespace RandomData;

class Program
{
    static void Main(string[] args)
    {
        var ctxOptions = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlServer("Data Source=.;Initial Catalog=TripDbDemo;Integrated Security=True;Encrypt=False;Trust Server Certificate=True")
            .Options;

        using var ctx = new AppDBContext(ctxOptions);
        
        var random = new Random();

        random = new Random(12345);
        foreach (var item in ctx.Accomodations.ToList())
        {
            item.Rating = Math.Round(random.NextDouble() * 3.9 + 1.0, 1);
        }

        // Update Restaurants (Range: 1.0-4.9)
        random = new Random(67890);
        foreach (var item in ctx.Restaurants.ToList())
        {
            item.Rating = Math.Round(random.NextDouble() * 3.9 + 1.0, 1);
        }

        // Update Entertainments (Range: 1.0-4.9)
        random = new Random(54321);
        foreach (var item in ctx.Entertainments.ToList())
        {
            item.Rating = Math.Round(random.NextDouble() * 3.9 + 1.0, 1);
        }

        // Update TourismAreas (Range: 1.0-4.9)
        random = new Random(98765);
        foreach (var item in ctx.TourismAreas.ToList())
        {
            item.Rating = Math.Round(random.NextDouble() * 3.9 + 1.0, 1);
        }

        ctx.SaveChanges();
        Console.WriteLine("Random ratings (1.0-4.9) updated for all entities.");

    }
}