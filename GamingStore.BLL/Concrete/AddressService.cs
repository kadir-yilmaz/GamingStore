using GamingStore.BLL.Abstract;
using GamingStore.DAL.Abstract;
using GamingStore.EL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.BLL.Concrete
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }

        public IEnumerable<Address> GetUserAddresses(string userId, AddressType? type = null)
        {
            return _addressRepository.GetUserAddresses(userId, type).ToList();
        }

        public Address? GetAddressById(int id, string userId)
        {
            var address = _addressRepository.GetById(id);
            
            // Güvenlik kontrolü: Adres bu kullanıcıya ait mi?
            if (address != null && address.UserId != userId)
            {
                return null;
            }

            return address;
        }

        public Address? GetDefaultAddress(string userId, AddressType type)
        {
            return _addressRepository.GetDefaultAddress(userId, type);
        }

        public void CreateAddress(Address address)
        {
            // İlk adres ise varsayılan yap
            var existingAddresses = _addressRepository
                .GetUserAddresses(address.UserId!, address.AddressType)
                .ToList();

            if (!existingAddresses.Any())
            {
                address.IsDefault = true;
            }

            _addressRepository.Create(address);
            _addressRepository.Save();
        }

        public void UpdateAddress(Address address, string userId)
        {
            // Güvenlik kontrolü
            var existingAddress = _addressRepository.GetById(address.Id);
            if (existingAddress == null || existingAddress.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bu adresi düzenleme yetkiniz yok.");
            }

            // UserId değiştirilememeli
            address.UserId = userId;

            _addressRepository.Update(address);
            _addressRepository.Save();
        }

        public void DeleteAddress(int id, string userId)
        {
            var address = _addressRepository.GetById(id);
            
            if (address == null || address.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bu adresi silme yetkiniz yok.");
            }

            bool wasDefault = address.IsDefault;
            var addressType = address.AddressType;

            _addressRepository.Delete(address);
            _addressRepository.Save();

            // Eğer silinen adres varsayılandı ise, ilk adresi varsayılan yap
            if (wasDefault)
            {
                var remainingAddresses = _addressRepository
                    .GetUserAddresses(userId, addressType)
                    .ToList();

                if (remainingAddresses.Any())
                {
                    SetAsDefault(remainingAddresses.First().Id, userId, addressType);
                }
            }
        }

        public void SetAsDefault(int addressId, string userId, AddressType type)
        {
            var address = _addressRepository.GetById(addressId);
            
            if (address == null || address.UserId != userId)
            {
                throw new UnauthorizedAccessException("Bu adresi varsayılan yapma yetkiniz yok.");
            }

            _addressRepository.SetAsDefault(addressId, userId, type);
        }
    }
}
