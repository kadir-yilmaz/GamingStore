using GamingStore.DAL.Abstract;
using GamingStore.EL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.DAL.EFCore
{
    public class AddressRepository : GenericRepository<Address>, IAddressRepository
    {
        public AddressRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Address> GetUserAddresses(string userId, AddressType? type = null)
        {
            var query = _context.Addresses
                .Where(a => a.UserId == userId)
                .AsNoTracking();

            if (type.HasValue)
            {
                query = query.Where(a => a.AddressType == type.Value);
            }

            return query.OrderByDescending(a => a.IsDefault).ThenBy(a => a.Title);
        }

        public Address? GetDefaultAddress(string userId, AddressType type)
        {
            return _context.Addresses
                .Where(a => a.UserId == userId && a.AddressType == type && a.IsDefault)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public void SetAsDefault(int addressId, string userId, AddressType type)
        {
            // Önce aynı tipteki tüm adreslerin varsayılan durumunu kaldır
            var addresses = _context.Addresses
                .Where(a => a.UserId == userId && a.AddressType == type)
                .ToList();

            foreach (var addr in addresses)
            {
                addr.IsDefault = addr.Id == addressId;
            }

            _context.SaveChanges();
        }

        public Address? GetById(int id)
        {
            return _context.Addresses.Find(id);
        }
    }
}
