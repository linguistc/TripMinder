using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using TripMinder.Data.Entities;

namespace TripMinder.Infrastructure.Data
{
    public class AppDBContext : DbContext
    {
        public AppDBContext() { }


        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
        {
        
        }

        // Define DbSets
        #region DbSets

        public DbSet<Accomodation> Accomodations { get; set; }
        public DbSet<AccomodationClass> AccomodationsClasses { get; set; }
        public DbSet<AccomodationSuggestion> AccomodationsSuggestions { get; set; }
        public DbSet<AccomodationType> AccomodationTypes { get; set; }
        //
        public DbSet<BookMarkAccomodation> BookMarkAccomodations { get; set; }
        public DbSet<BookmarkEntertainment> BookMarkEntertainments { get; set; }
        public DbSet<BookMarkRestaurant> BookMarkRestaurants { get; set; }
        public DbSet<BookMarkTourism> BookMarkTourisms { get; set; }
        public DbSet<BookMarkTrip> BookMarkTrips { get; set; }
        //
        public DbSet<Governorate> Governorates { get; set; }
        public DbSet<Entertainment> Entertainments { get; set; }
        public DbSet<EntertainmentClass> EntertainmentsClasses { get; set; }
        public DbSet<EntertainmentSuggestion> EntertainmentsSuggestions { get; set; }
        public DbSet<EntertainmentType> EntertainmentTypes { get; set; }
        public DbSet<PlaceType> PlaceTypes { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<RestaurantClass> RestaurantsClasses { get; set; }
        public DbSet<RestaurantSuggestion> RestaurantSuggestions { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        
        //
        public DbSet<TourismArea> TourismAreas { get; set; }
        public DbSet<TourismAreaClass> TourismAreaClasses { get; set; }
        public DbSet<TourismSuggestion> TourismSuggestions { get; set; }
        public DbSet<TourismType> TourismAreaTypes { get; set; }
        public DbSet<TripSuggestion> TripSuggestions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserBookMark> UsersBookMarks { get; set; }
        public DbSet<UserHistory> UsersHistory { get; set; }
        public DbSet<UserImage> UsersImages { get; set; }
        //public DbSet<UserPreference> UserPreferences { get; set; }
        public DbSet<UserRating> UsersRatings { get; set; }
        public DbSet<UserSocialProfile> UsersSocialProfiles { get; set; }
        public DbSet<Zone> Zones { get; set; }
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();


            #region SQL BLOB
            modelBuilder.Entity<Accomodation>()
                .Property(a => a.ImgData)
                .HasColumnType("varbinary(max)");
            modelBuilder.Entity<Entertainment>()
                .Property(e => e.ImgData)
                .HasColumnType("varbinary(max)");
            modelBuilder.Entity<Restaurant>()
                .Property(r => r.ImgData)
                .HasColumnType("varbinary(max)");

            modelBuilder.Entity<TourismArea>()
                .Property(t => t.ImgData)
                .HasColumnType("varbinary(max)");
            
            modelBuilder.Entity<User>()
                .Property(u => u.ImgData)
                .HasColumnType("varbinary(max)");
            #endregion

            modelBuilder.Entity<UserBookMark>()
                .HasMany(ub => ub.BookMarkTrips)
                .WithOne(bt => bt.UserBookMark)
                .HasForeignKey(bt => bt.BookmarkId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserBookMark>()
                .HasMany(ub => ub.BookMarkAccomodations)
                .WithOne(ba => ba.UserBookMark)
                .HasForeignKey(ba => ba.BookmarkId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserBookMark>()
                .HasMany(ub => ub.BookMarkRestaurants)
                .WithOne(br => br.UserBookMark)
                .HasForeignKey(br => br.BookmarkId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserBookMark>()
                .HasMany(ub => ub.BookmarkEntertainments)
                .WithOne(be => be.UserBookMark)
                .HasForeignKey(be => be.BookmarkId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserBookMark>()
                .HasMany(ub => ub.BookMarkTourisms)
                .WithOne(bt => bt.UserBookMark)
                .HasForeignKey(bt => bt.BookmarkId)
                .OnDelete(DeleteBehavior.Cascade);

            // ضبط العلاقات مع المفاتيح المركبة
            modelBuilder.Entity<BookMarkTrip>()
                .HasKey(bt => new { bt.BookmarkId, bt.TripSuggestionId });
            modelBuilder.Entity<BookMarkAccomodation>()
                .HasKey(ba => new { ba.BookmarkId, ba.AccomodationId });
            modelBuilder.Entity<BookMarkRestaurant>()
                .HasKey(br => new { br.BookmarkId, br.RestaurantId });
            modelBuilder.Entity<BookmarkEntertainment>()
                .HasKey(be => new { be.BookmarkId, be.EntertainmentId });
            modelBuilder.Entity<BookMarkTourism>()
                .HasKey(bt => new { bt.BookmarkId, bt.TourismId });

            // العلاقات بين BookMarkEntities والكيانات المرتبطة بها
            modelBuilder.Entity<BookMarkTrip>()
                .HasOne(bt => bt.TripSuggestion)
                .WithMany()
                .HasForeignKey(bt => bt.TripSuggestionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookMarkAccomodation>()
                .HasOne(ba => ba.Accomodation)
                .WithMany()
                .HasForeignKey(ba => ba.AccomodationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookMarkRestaurant>()
                .HasOne(br => br.Restaurant)
                .WithMany()
                .HasForeignKey(br => br.RestaurantId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookmarkEntertainment>()
                .HasOne(be => be.Entertainment)
                .WithMany()
                .HasForeignKey(be => be.EntertainmentId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<BookMarkTourism>()
                .HasOne(bt => bt.TourismArea)
                .WithMany()
                .HasForeignKey(bt => bt.TourismId)
                .OnDelete(DeleteBehavior.Cascade);

        }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
    }
}
