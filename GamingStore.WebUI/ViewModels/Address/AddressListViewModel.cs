using AddressModel = GamingStore.EL.Models.Address;
using GamingStore.EL.Models;

namespace GamingStore.WebUI.ViewModels.Address
{
    public class AddressListViewModel
    {
        public List<AddressModel> DeliveryAddresses { get; set; } = new();
        public List<AddressModel> InvoiceAddresses { get; set; } = new();
        public AddressType ActiveTab { get; set; } = AddressType.Delivery;
    }
}
