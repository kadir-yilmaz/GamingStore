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
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {

        public OrderRepository(AppDbContext context) : base(context)
        {
        }

        public IQueryable<Order> Orders => _context.Orders
    .Include(o => o.Lines)
    .ThenInclude(cl => cl.Product)
    .OrderBy(o => o.Shipped)
    .ThenByDescending(o => o.Id);

        public int NumberOfInProcess =>
            _context.Orders.Count(o => o.Shipped.Equals(false));

        public void Complete(int id)
        {
            var order = GetByCondition(o => o.Id.Equals(id), true).FirstOrDefault();
            if (order is null)
                throw new Exception("Order could not found!");
            order.Shipped = true;
        }

        //public Order? GetOneOrder(int id) =>
        //    GetByCondition(o => o.Id == id, false).FirstOrDefault();

        // bu kod ile mail sorunu çözüldü
        public Order? GetOneOrder(int id)
        {
            return _context.Orders
                           .Include(o => o.Lines)
                           .ThenInclude(cl => cl.Product)
                           .FirstOrDefault(o => o.Id == id);
        }


        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(l => l.Product));
            if (order.Id == 0)
            {
                _context.Orders.Add(order);
            }
            _context.SaveChanges();
        }
    }
}
