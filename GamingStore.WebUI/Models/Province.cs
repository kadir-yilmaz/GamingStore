namespace GamingStore.WebUI.ViewModels.Location
{
    public class Province
    {
        public string Name { get; set; } = string.Empty;
        public List<District> Districts { get; set; } = new();
    }
}
