using AddressModel = GamingStore.EL.Models.Address;
using GamingStore.WebUI.ViewModels.Location;

namespace GamingStore.WebUI.ViewModels.Address
{
    public interface IAddressViewModel
    {
        AddressModel Address { get; set; }
        List<Province> Provinces { get; set; }
    }
}
