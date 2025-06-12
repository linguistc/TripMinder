using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using TripMinder.Infrastructure.Data;

namespace Cloudinary;

class Program
{
    static void Main(string[] args)
    {
        Env.Load();  // يقرأ ملف .env في جذر المشروع

        var cloudName = Environment.GetEnvironmentVariable("CLOUDINARY_CLOUD_NAME");
        var apiKey    = Environment.GetEnvironmentVariable("CLOUDINARY_API_KEY");
        var apiSecret = Environment.GetEnvironmentVariable("CLOUDINARY_API_SECRET");
        
        if (string.IsNullOrEmpty(cloudName) ||
            string.IsNullOrEmpty(apiKey) ||
            string.IsNullOrEmpty(apiSecret))
        {
            Console.Error.WriteLine("ERROR: Missing Cloudinary credentials in environment.");
            return;
        }
        
        var account = new Account(cloudName, apiKey, apiSecret);
        
        var cloudinary = new CloudinaryDotNet.Cloudinary(account);

        
        var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseSqlServer("Data Source=.;Initial Catalog=TripDbDemo;Integrated Security=True;Encrypt=False;Trust Server Certificate=True")
            .Options;

        // using (var ctx = new AppDBContext(options))
        // {
        //     var governorates = ctx.Governorates.ToList();
        //
        //     string folder = @"E:\governorates";
        //     
        //     foreach (var g in governorates)
        //     {
        //         string localPath = Path.Combine(folder, $"{g.Id}.webp");
        //
        //         if (!File.Exists(localPath))
        //         {
        //             Console.WriteLine($"[Missing] Image for ID={g.Id} not found.");
        //             continue;
        //         }
        //
        //         var uploadParams = new ImageUploadParams
        //         {
        //             File = new FileDescription(localPath),
        //             Folder = "Governorates",
        //             PublicId = g.Id.ToString()
        //         };
        //
        //         var uploadResult = cloudinary.Upload(uploadParams);
        //
        //         g.ImageUrl = uploadResult.SecureUrl.ToString();
        //         Console.WriteLine($"[Uploaded] {g.Id} → {g.ImageUrl}");
        //     }
        //     
        //     ctx.SaveChanges();
        //     Console.WriteLine("Database updated with ImageUrl for all governorate records.");
        // }
        
        using (var ctx = new AppDBContext(options))
        {
            var accomodations = ctx.Accomodations.ToList();
            
            string folder = @"E:\college\g-data\acco";
        
            foreach (var a in accomodations)
            {
                string localPath = Path.Combine(folder, $"{a.Id}.webp");
        
                if (!File.Exists(localPath))
                {
                    Console.WriteLine($"[Missing] Image for ID={a.Id} not found.");
                    continue;
                }
        
                var uploadParams = new ImageUploadParams
                {
        
                    File = new FileDescription(localPath),
                    Folder = "Accomodation",
                    PublicId = a.Id.ToString()
                };
        
                var uploadResult = cloudinary.Upload(uploadParams);
        
                a.ImageUrl = uploadResult.SecureUrl.ToString();
                Console.WriteLine($"[Uploaded] {a.Id} → {a.ImageUrl}");
            }
            
            ctx.SaveChanges();
            Console.WriteLine("Database updated with ImageUrl for all accomodation records.");
        }
        
        using (var ctx = new AppDBContext(options))
        {
            var restaurants = ctx.Restaurants.ToList();
        
            string folder = @"E:\college\g-data\rest";
        
            foreach (var r in restaurants)
            {
                string localPath = Path.Combine(folder, $"{r.Id}.webp");
        
                if (!File.Exists(localPath))
                {
                    Console.WriteLine($"[Missing] Image for ID={r.Id} not found.");
                    continue;
                }
        
                var uploadParams = new ImageUploadParams
                {
        
                    File = new FileDescription(localPath),
                    Folder = "Restaurant",
                    PublicId = r.Id.ToString()
                };
        
                var uploadResult = cloudinary.Upload(uploadParams);
        
                r.ImageUrl = uploadResult.SecureUrl.ToString();
                Console.WriteLine($"[Uploaded] {r.Id} → {r.ImageUrl}");
            }
        
            ctx.SaveChanges();
            Console.WriteLine("Database updated with ImageUrl for all restaurant records.");
        }
        
        using (var ctx = new AppDBContext(options))
        {
            var entertainments = ctx.Entertainments.ToList();
        
            string folder = @"E:\college\g-data\entertain";
        
            foreach (var e in entertainments)
            {
                string localPath = Path.Combine(folder, $"{e.Id}.webp");
        
                if (!File.Exists(localPath))
                {
                    Console.WriteLine($"[Missing] Image for ID={e.Id} not found.");
                    continue;
                }
        
                var uploadParams = new ImageUploadParams
                {
        
                    File = new FileDescription(localPath),
                    Folder = "Entertainment",
                    PublicId = e.Id.ToString()
                };
        
                var uploadResult = cloudinary.Upload(uploadParams);
        
                e.ImageUrl = uploadResult.SecureUrl.ToString();
                Console.WriteLine($"[Uploaded] {e.Id} → {e.ImageUrl}");
            }
        
            ctx.SaveChanges();
            Console.WriteLine("Database updated with ImageUrl for all entertainment records.");
        }
        
        using (var ctx = new AppDBContext(options))
        {
            var tourismAreas = ctx.TourismAreas.ToList();
        
            string folder = @"E:\college\g-data\tour";
            
            foreach (var t in tourismAreas)
            {
                string localPath = Path.Combine(folder, $"{t.Id}.webp");
        
                if (!File.Exists(localPath))
                {
                    Console.WriteLine($"[Missing] Image for ID={t.Id} not found.");
                    continue;
                }
        
                var uploadParams = new ImageUploadParams
                {
        
                    File = new FileDescription(localPath),
                    Folder = "Tourism",
                    PublicId = t.Id.ToString()
                };
        
                var uploadResult = cloudinary.Upload(uploadParams);
        
                t.ImageUrl = uploadResult.SecureUrl.ToString();
                Console.WriteLine($"[Uploaded] {t.Id} → {t.ImageUrl}");
            }
        
            ctx.SaveChanges();
            Console.WriteLine("Database updated with ImageUrl for all tourism records.");
        }
    }
}