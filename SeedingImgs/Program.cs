using Microsoft.EntityFrameworkCore;
using TripMinder.Infrastructure.Data;

namespace SeedingImgs;

class Program
{
    static void Main(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlServer("Data Source=.;Initial Catalog=TripDbDemo;Integrated Security=True;Encrypt=False;Trust Server Certificate=True")
            .Options;

        // using (var ctx = new AppDBContext(options))
        // {
        //     var map = ctx.Accomodations
        //         .Select(r => new {r.Name, r.Id})
        //         .ToDictionary(r => r.Name, x => x.Id);
        //     
        //     var folder = @"E:\college\g-data\Cairo Image\Cairo Image\cairo Accommodation";
        //     var files = Directory.GetFiles(folder);
        //
        //     foreach (var file in files)
        //     {
        //         string fileName = Path.GetFileNameWithoutExtension(file);
        //         string ext = Path.GetExtension(file);
        //
        //         if (map.TryGetValue(fileName, out int id))
        //         {
        //             string newPath = Path.Combine(folder, $"{id}{ext}");
        //             
        //             if(!File.Exists(newPath))
        //                 File.Move(file, newPath);
        //             else
        //                 Console.WriteLine($"File for ID {id} already exists, skipping.");
        //         }
        //         else 
        //             Console.WriteLine($"No DB record found for name '{fileName}'.");
        //     }
        // }
        
        using (var ctx = new AppDBContext(options))
        {
            var map = ctx.Entertainments
                .Select(r => new {r.Name, r.Id})
                .ToDictionary(r => r.Name, x => x.Id);
            
            var folder = @"E:\college\g-data\Cairo Image\Cairo Image\cairo Entertainment";
            var files = Directory.GetFiles(folder);

            foreach (var file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string ext = Path.GetExtension(file);

                if (map.TryGetValue(fileName, out int id))
                {
                    string newPath = Path.Combine(folder, $"{id}{ext}");
                    
                    if(!File.Exists(newPath))
                        File.Move(file, newPath);
                    else
                        Console.WriteLine($"File for ID {id} already exists, skipping.");
                }
                else 
                    Console.WriteLine($"No DB record found for name '{fileName}'.");
            }
        }
        
        using (var ctx = new AppDBContext(options))
        {
            var map = ctx.Restaurants
                .Select(r => new {r.Name, r.Id})
                .ToDictionary(r => r.Name, x => x.Id);
            
            var folder = @"E:\college\g-data\Cairo Image\Cairo Image\cairo Restaurants";
            var files = Directory.GetFiles(folder);

            foreach (var file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string ext = Path.GetExtension(file);

                if (map.TryGetValue(fileName, out int id))
                {
                    string newPath = Path.Combine(folder, $"{id}{ext}");
                    
                    if(!File.Exists(newPath))
                        File.Move(file, newPath);
                    else
                        Console.WriteLine($"File for ID {id} already exists, skipping.");
                }
                else 
                    Console.WriteLine($"No DB record found for name '{fileName}'.");
            }
        }
        
        using (var ctx = new AppDBContext(options))
        {
            var map = ctx.TourismAreas
                .Select(r => new {r.Name, r.Id})
                .ToDictionary(r => r.Name, x => x.Id);
            
            var folder = @"E:\college\g-data\Cairo Image\Cairo Image\cairo tourism";
            var files = Directory.GetFiles(folder);

            foreach (var file in files)
            {
                string fileName = Path.GetFileNameWithoutExtension(file);
                string ext = Path.GetExtension(file);

                if (map.TryGetValue(fileName, out int id))
                {
                    string newPath = Path.Combine(folder, $"{id}{ext}");
                    
                    if(!File.Exists(newPath))
                        File.Move(file, newPath);
                    else
                        Console.WriteLine($"File for ID {id} already exists, skipping.");
                }
                else 
                    Console.WriteLine($"No DB record found for name '{fileName}'.");
            }
        }
        
        // using (var ctx = new AppDBContext(options))
        // {
        //     var folder = @"E:\college\g-data\Cairo Image\Cairo Image\cairo Accommodation";
        //     var files = Directory.GetFiles(folder);
        //
        //     short counter = 0;
        //     foreach (var file in files)
        //     {
        //         var fileName = Path.GetFileNameWithoutExtension(file);
        //         
        //         var AcommodationItem = ctx.Accomodations
        //             .FirstOrDefault(i => i.Name == fileName);
        //
        //         if (AcommodationItem != null)
        //         {
        //             AcommodationItem.ImgData = File.ReadAllBytes(file);
        //         }
        //         
        //         if (++counter >= 20)
        //         {
        //             ctx.SaveChanges();
        //             counter = 0;
        //         }
        //     }
        //     
        //     ctx.SaveChanges();
        // }
        //
        // using (var ctx = new AppDBContext(options))
        // {
        //     var folder = @"E:\college\g-data\Cairo Image\Cairo Image\Cairo Entertainment";
        //     var files = Directory.GetFiles(folder);
        //
        //     short counter = 0;
        //     foreach (var file in files)
        //     {
        //         
        //         var fileName = Path.GetFileNameWithoutExtension(file);
        //         
        //         var EntertainmentItem = ctx.Entertainments
        //             .FirstOrDefault(i => i.Name == fileName);
        //
        //         if (EntertainmentItem != null)
        //         {
        //             EntertainmentItem.ImgData = File.ReadAllBytes(file);
        //         }
        //
        //         if (++counter >= 20)
        //         {
        //             ctx.SaveChanges();
        //             counter = 0;
        //         }
        //     }
        //     
        //     ctx.SaveChanges();
        // }
        //
        // using (var ctx = new AppDBContext(options))
        // {
        //     var folder = @"E:\college\g-data\Cairo Image\Cairo Image\cairo restaurants";
        //     var files = Directory.GetFiles(folder);
        //
        //     short counter = 0;
        //     foreach (var file in files)
        //     {
        //         var fileName = Path.GetFileNameWithoutExtension(file);
        //         
        //         var RestItem = ctx.Restaurants
        //             .FirstOrDefault(i => i.Name == fileName);
        //
        //         if (RestItem != null)
        //         {
        //             RestItem.ImgData = File.ReadAllBytes(file);
        //         }
        //         
        //         if (++counter >= 20)
        //         {
        //             ctx.SaveChanges();
        //             counter = 0;
        //         }
        //     }
        //     
        //     ctx.SaveChanges();
        // }
        //
        // using (var ctx = new AppDBContext(options))
        // {
        //     var folder = @"E:\college\g-data\Cairo Image\Cairo Image\Cairo Tourism";
        //     var files = Directory.GetFiles(folder);
        //
        //     short counter = 0;
        //     foreach (var file in files)
        //     {
        //         var fileName = Path.GetFileNameWithoutExtension(file);
        //         
        //         var TourItem = ctx.TourismAreas
        //             .FirstOrDefault(i => i.Name == fileName);
        //
        //         if (TourItem != null)
        //         {
        //             TourItem.ImgData = File.ReadAllBytes(file);
        //         }
        //         
        //         if (++counter >= 20)
        //         {
        //             ctx.SaveChanges();
        //             counter = 0;
        //         }
        //     }
        //     
        //     ctx.SaveChanges();
        // }
        //
    }
    
    
}