namespace TripMinder.Data.AppMetaData
{
    public static class Router
    {
        public const string SingleRouter = "{id}/";
        public const string root = "Api/";
        public const string version = "v1/";

        public const string Rule = root + version;


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
    }
}
