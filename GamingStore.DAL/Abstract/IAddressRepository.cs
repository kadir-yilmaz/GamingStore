using GamingStore.EL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.DAL.Abstract
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        /// <summary>
        /// Kullanıcının adreslerini getirir
        /// </summary>
        IQueryable<Address> GetUserAddresses(string userId, AddressType? type = null);

        /// <summary>
        /// Kullanıcının varsayılan adresini getirir
        /// </summary>
        Address? GetDefaultAddress(string userId, AddressType type);

        /// <summary>
        /// Belirtilen adresi varsayılan yapar, diğerlerinin varsayılan durumunu kaldırır
        /// </summary>
        void SetAsDefault(int addressId, string userId, AddressType type);

        /// <summary>
        /// Tek bir adresi ID ile getirir
        /// </summary>
        Address? GetById(int id);
    }
}
