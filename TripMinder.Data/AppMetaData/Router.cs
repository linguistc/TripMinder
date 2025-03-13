namespace TripMinder.Data.AppMetaData
{
    public static class Router
    {
        public const string SingleRouter = "{id}/";
        public const string root = "Api/";
        public const string version = "v1/";

        public const string Rule = root + version;


        #region Main Entities Routing
        public static class RestaurantRouting
        {
            public const string Prefix = Rule + "restaurant/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;

            public const string Create = Prefix + "create/";
            public const string Update = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;

            public const string Paginate = Prefix + "paginate/";
        }

        public static class AccomodationRouting
        {
            public const string Prefix = Rule + "accommodation/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;
            public const string Create = Prefix + "create/";
            public const string Edit = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;
            public const string Paginate = Prefix + "paginate/";
        }

        public static class EntertainmentRouting
        {
            public const string Prefix = Rule + "entertainment/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;
            public const string Create = Prefix + "create/";
            public const string Edit = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;
            public const string Paginate = Prefix + "paginate/";
        }

        public static class TourismAreaRouting
        {
            public const string Prefix = Rule + "tourismarea/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;
            public const string Create = Prefix + "create/";
            public const string Edit = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;
            public const string Paginate = Prefix + "paginate/";
        }
        #endregion

        #region Nav Properties Routing
        public static class ZoneRouting
        {
            public const string Prefix = Rule + "zone/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;
            public const string Create = Prefix + "create/";
            public const string Edit = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;
            public const string Paginate = Prefix + "paginate/";
        }

        public static class AccomodationClassRouting
        {
            public const string Prefix = Rule + "accommodationclass/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;
            public const string Create = Prefix + "create/";
            public const string Edit = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;
            public const string Paginate = Prefix + "paginate/";
        }

        public static class AccomodationTypeRouting
        {
            public const string Prefix = Rule + "accommodationtype/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;
            public const string Create = Prefix + "create/";
            public const string Edit = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;
            public const string Paginate = Prefix + "paginate/";
        }

        public static class EntertainmentClassRouting
        {
            public const string Prefix = Rule + "entertainmentclass/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;
            public const string Create = Prefix + "create/";
            public const string Edit = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;
            public const string Paginate = Prefix + "paginate/";
        }

        public static class EntertainmentTypeRouting
        {
            public const string Prefix = Rule + "entertainmenttype/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;
            public const string Create = Prefix + "create/";
            public const string Edit = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;
            public const string Paginate = Prefix + "paginate/";
        }

        public static class RestaurantClassRouting
        {
            public const string Prefix = Rule + "restaurantclass/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;
            public const string Create = Prefix + "create/";
            public const string Edit = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;
            public const string Paginate = Prefix + "paginate/";
        }

        public static class FoodCategoryRouting
        {
            public const string Prefix = Rule + "foodcategory/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;
            public const string Create = Prefix + "create/";
            public const string Edit = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;
            public const string Paginate = Prefix + "paginate/";
        }

        public static class PlaceTypeRouting
        {
            public const string Prefix = Rule + "placetype/";
            public const string List = Prefix + "list/";
            public const string GetById = Prefix + SingleRouter;
            public const string Create = Prefix + "create/";
            public const string Edit = Prefix + "edit/";
            public const string Delete = Prefix + SingleRouter;
            public const string Paginate = Prefix + "paginate/";
        }

        public static class TourismAreaClassRouting
            {
                public const string Prefix = Rule + "tourismareaclass/";
                public const string List = Prefix + "list/";
                public const string GetById = Prefix + SingleRouter;
                public const string Create = Prefix + "create/";
                public const string Edit = Prefix + "edit/";
                public const string Delete = Prefix + SingleRouter;
                public const string Paginate = Prefix + "paginate/";
            }

        public static class TourismAreaTypeRouting
            {
                public const string Prefix = Rule + "tourismareatype/";
                public const string List = Prefix + "list/";
                public const string GetById = Prefix + SingleRouter;
                public const string Create = Prefix + "create/";
                public const string Edit = Prefix + "edit/";
                public const string Delete = Prefix + SingleRouter;
                public const string Paginate = Prefix + "paginate/";
            }
        
        #endregion
    }
}

