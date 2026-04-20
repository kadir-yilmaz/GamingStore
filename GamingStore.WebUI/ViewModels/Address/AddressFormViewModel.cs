using AddressModel = GamingStore.EL.Models.Address;
using GamingStore.WebUI.ViewModels.Location;

namespace GamingStore.WebUI.ViewModels.Address
{
    public class AddressFormViewModel : IAddressViewModel
    {
        public AddressModel Address { get; set; } = new();
        public List<Province> Provinces { get; set; } = new();
    }
}
