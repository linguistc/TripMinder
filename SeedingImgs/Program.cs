using System;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TripMinder.Infrastructure.Data; // Assuming AppDBContext is defined here

namespace SeedingImgs
{
    class Program
    {
        static void Main(string[] args)
        {
            // Declare and initialize ctxOptions in Main
            var ctxOptions = new DbContextOptionsBuilder<AppDBContext>()
                .UseSqlServer("Data Source=.;Initial Catalog=TripDbDemo;Integrated Security=True;Encrypt=False;Trust Server Certificate=True")
                .Options;

            // Pass ctxOptions to RenameFiles
            RenameFiles(ctx => ctx.Accomodations, @"E:\college\g-data\acco", ctxOptions);
            RenameFiles(ctx => ctx.Entertainments, @"E:\college\g-data\entertain", ctxOptions);
            RenameFiles(ctx => ctx.Restaurants, @"E:\college\g-data\rest", ctxOptions);
            RenameFiles(ctx => ctx.TourismAreas, @"E:\college\g-data\tour", ctxOptions);

            Console.WriteLine("All done. Press any key to exit…");
            Console.ReadKey();
        }

        /// <summary>
        /// Renames files in a folder based on a Name-to-Id mapping from the database.
        /// </summary>
        static void RenameFiles<TEntity>(
            Func<AppDBContext, IQueryable<TEntity>> tableSelector,
            string folder,
            DbContextOptions<AppDBContext> ctxOptions) // Accept ctxOptions as a parameter
            where TEntity : class
        {
            // Use ctxOptions to create the DbContext
            using var ctx = new AppDBContext(ctxOptions);

            // Build a dictionary of Name → Id from the database
            var map = tableSelector(ctx)
                .AsEnumerable()
                .GroupBy(e => (string)e.GetType().GetProperty("Name").GetValue(e))
                .Select(g => g.First())
                .ToDictionary(
                    e => (string)e.GetType().GetProperty("Name").GetValue(e),
                    e => (int)e.GetType().GetProperty("Id").GetValue(e));

            Console.WriteLine($"Renaming in '{folder}' — {map.Count} unique names found.");

            // Rename files based on the dictionary
            var files = Directory.GetFiles(folder);
            foreach (var file in files)
            {
                var fileName = Path.GetFileNameWithoutExtension(file);
                var ext = Path.GetExtension(file);

                if (!map.TryGetValue(fileName, out var id))
                {
                    Console.WriteLine($"[SKIP] No DB record for '{fileName}'.");
                    continue;
                }

                var newPath = Path.Combine(folder, $"{id}{ext}");
                if (File.Exists(newPath))
                {
                    Console.WriteLine($"[SKIP] '{newPath}' already exists.");
                    continue;
                }

                File.Move(file, newPath);
                Console.WriteLine($"[RENAMED] {fileName}{ext} → {id}{ext}");
            }
        }
    }
}