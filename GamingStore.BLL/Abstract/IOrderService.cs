using GamingStore.EL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamingStore.BLL.Abstract
{
    public interface IOrderService
    {
        IQueryable<Order> Orders { get; }
        Order? GetOneOrder(int id);
        Task CompleteAsync(int id);
        Task UpdateStatusAsync(int id, OrderStatus status);
        Task ShipOrderAsync(int id, string cargoCompany, string trackingNumber);
        Task CancelOrderAsync(int id);
        void SaveOrder(Order order);
        int NumberOfInProcess { get; }

    }
}
