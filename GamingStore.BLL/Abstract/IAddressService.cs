using GamingStore.EL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.BLL.Abstract
{
    public interface IAddressService
    {
        /// <summary>
        /// Kullanıcının tüm adreslerini getirir
        /// </summary>
        IEnumerable<Address> GetUserAddresses(string userId, AddressType? type = null);

        /// <summary>
        /// Tek bir adresi ID ile getirir
        /// </summary>
        Address? GetAddressById(int id, string userId);

        /// <summary>
        /// Kullanıcının varsayılan adresini getirir
        /// </summary>
        Address? GetDefaultAddress(string userId, AddressType type);

        /// <summary>
        /// Yeni adres oluşturur
        /// </summary>
        void CreateAddress(Address address);

        /// <summary>
        /// Adresi günceller
        /// </summary>
        void UpdateAddress(Address address, string userId);

        /// <summary>
        /// Adresi siler
        /// </summary>
        void DeleteAddress(int id, string userId);

        /// <summary>
        /// Adresi varsayılan yapar
        /// </summary>
        void SetAsDefault(int addressId, string userId, AddressType type);
    }
}
