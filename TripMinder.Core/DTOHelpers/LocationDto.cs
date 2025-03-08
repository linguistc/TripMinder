namespace TripMinder.Core.DTOHelpers;

public class LocationDto
{
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public string Address { get; set; }
}

public class SocialProfileDto
{
    public string PlatformName { get; set; }  // Facebook, Twitter, etc.
    public string Url { get; set; }
}