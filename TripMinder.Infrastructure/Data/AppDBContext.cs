﻿using Microsoft.EntityFrameworkCore;
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
        #region

        DbSet<Accomodation> Accomodations { get; set; }
        DbSet<AccomodationClass> AccomodationsClasses { get; set; }
        DbSet<AccomodationDescription> AccomodationsDescriptions { get; set; }
        DbSet<AccomodationImage> AccomodationsImages { get; set; }
        DbSet<AccomodationSocialProfile> AccomodationsSocialProfiles { get; set; }
        DbSet<AccomodationSuggestion> AccomodationsSuggestions { get; set; }
        DbSet<BookMarkAccomodation> BookMarkAccomodations { get; set; }
        DbSet<BookmarkEntertainment> BookMarkEntertainments { get; set; }
        DbSet<BookMarkRestaurant> BookMarkRestaurants { get; set; }
        DbSet<BookMarkTourism> BookMarkTourisms { get; set; }
        DbSet<BookMarkTrip> BookMarkTrips { get; set; }
        DbSet<Entertainment> Entertainments { get; set; }
        DbSet<EntertainmentDescription> EntertainmentsDescriptions { get; set; }
        DbSet<EntertainmentImage> EntertainmentsImages { get; set; }
        DbSet<EntertainmentSocialProfile> EntertainmentsSocialProfiles { get; set; }
        DbSet<EntertainmentSuggestion> EntertainmentsSuggestions { get; set; }
        DbSet<FoodCategory> FoodCategories { get; set; }
        DbSet<GeneralClass> GeneralClasses { get; set; }
        DbSet<PlaceCategory> PlaceCategories { get; set; }
        DbSet<Restaurant> Restaurants { get; set; }
        DbSet<RestaurantDescription> RestaurantDescriptions { get; set; }
        DbSet<RestaurantFoodCategory> RestaurantFoodCategories { get; set; }
        DbSet<RestaurantImage> RestaurantImages { get; set; }
        DbSet<RestaurantSocialProfile> RestaurantSocialProfiles { get; set; }
        DbSet<RestaurantSuggestion> RestaurantSuggestions { get; set; }
        DbSet<TourismDescription> TourismAreasDescriptions { get; set; }
        DbSet<TourismArea> TourismAreas { get; set; }
        DbSet<TourismDescription> TourismDescriptions { get; set; }
        DbSet<TourismImage> TourismImages { get; set; }
        DbSet<TourismSocialProfile> TourismSocialProfiles { get; set; }
        DbSet<TourismSuggestion> TourismSuggestions { get; set; }
        DbSet<TripSuggestion> TripSuggestions { get; set; }
        DbSet<User> Users { get; set; }
        DbSet<UserBookMark> UsersBookMarks { get; set; }
        DbSet<UserHistory> UsersHistory { get; set; }
        DbSet<UserImage> UsersImages { get; set; }
        //DbSet<UserPreference> UserPreferences { get; set; }
        DbSet<UserRating> UsersRatings { get; set; }
        DbSet<UserSocialProfile> UsersSocialProfiles { get; set; }
        DbSet<Zone> Zones { get; set; }
        #endregion


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<RestaurantFoodCategory>()
                .HasKey(rfc => new { rfc.RestaurantId, rfc.FoodCategoryId });

            modelBuilder.Entity<RestaurantFoodCategory>()
                .HasOne(rfc => rfc.Restaurant)
                .WithMany(r => r.RestaurantFoodCategories)
                .HasForeignKey(rfc => rfc.RestaurantId);

            modelBuilder.Entity<RestaurantFoodCategory>()
                .HasOne(rfc => rfc.FoodCategory)
                .WithMany(fc => fc.RestaurantFoodCategories)
                .HasForeignKey(rfc => rfc.FoodCategoryId);



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
