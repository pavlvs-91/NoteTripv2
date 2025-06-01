namespace NoteTrip.Models
{
    public class StatisticsViewModel
    {
        public List<CountryStat> CountryStats { get; set; } = new();
        public List<CityStat> CityStats { get; set; } = new();
    }

    public class CountryStat
    {
        public string CountryName { get; set; }
        public int VisitedCount { get; set; }
        public int TotalAttractions { get; set; }
    }

    public class CityStat
    {
        public string CityName { get; set; }
        public string RegionName { get; set; }
        public string CountryName { get; set; }
        public int TotalAttractions { get; set; }
        public int VisitedAttractions { get; set; }

        public double VisitedPercentage =>
            TotalAttractions > 0
            ? Math.Round((double)VisitedAttractions / TotalAttractions * 100, 1)
            : 0;
    }
}
