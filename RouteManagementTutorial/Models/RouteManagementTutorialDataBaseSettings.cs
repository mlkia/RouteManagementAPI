namespace RouteManagementTutorial.Models
{
    public class RouteManagementTutorialDataBaseSettings
    {
        public string ConnectionString { get; set; } = null!;
        public string DatabaseName { get; set; } = null!;
        public string DriversCollectionName { get; set; } = null!;
        public string AdminsCollectionName { get; set; } = null!;
        public string RoutesCollectionName { get; set; } = null!;
    }
}
