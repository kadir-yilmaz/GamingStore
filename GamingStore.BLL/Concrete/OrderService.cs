using GamingStore.BLL.Abstract;
using GamingStore.DAL.Abstract;
using GamingStore.EL.Models;

namespace GamingStore.BLL.Concrete
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
        }

        public IQueryable<Order> Orders => _orderRepository.Orders;
        public int NumberOfInProcess => _orderRepository.NumberOfInProcess;

        public async Task CompleteAsync(int id)
        {
            // 1️⃣ Siparişi çek (Include ile Lines ve Product)
            var order = _orderRepository.GetOneOrder(id);
            if (order == null)
            {
                Console.WriteLine($"HATA: Sipariş #{id} bulunamadı.");
                return;
            }

            // 2️⃣ Siparişi tamamla
            order.Shipped = true;
            order.Status = OrderStatus.Completed;

            Console.WriteLine($"Sipariş #{order.Id} için işlemler başlatıldı.");
            Console.WriteLine($"Toplam {order.Lines.Count} adet farklı ürün sipariş edilmiş.");

            // 3️⃣ Stok güncelle
            foreach (var line in order.Lines)
            {
                var product = _productRepository.GetOneProduct(line.Product.Id, false);
                if (product == null)
                {
                    Console.WriteLine($"UYARI: Ürün ID {line.Product.Id} için ürün bulunamadı.");
                    continue;
                }

                Console.WriteLine($"Ürün: {product.Name}, Sipariş Miktarı: {line.Quantity}, Mevcut Stok: {product.Stock}");

                // Stok düşür
                product.Stock -= line.Quantity;
                if (product.Stock < 0)
                {
                    product.Stock = 0;
                }

                Console.WriteLine($"Güncellenen Stok: {product.Stock}");
            }

            // 4️⃣ Değişiklikleri kaydet
            _orderRepository.Save();
            _productRepository.Save();
            Console.WriteLine("Sipariş tamamlandı ve stok güncellendi.");
        }

        public async Task UpdateStatusAsync(int id, OrderStatus status)
        {
            var order = _orderRepository.GetOneOrder(id);
            if (order != null)
            {
                order.Status = status;
                _orderRepository.Save();
            }
        }

        public async Task ShipOrderAsync(int id, string cargoCompany, string trackingNumber)
        {
            var order = _orderRepository.GetOneOrder(id);
            if (order != null)
            {
                order.CargoCompany = cargoCompany;
                order.TrackingNumber = trackingNumber;
                order.Status = OrderStatus.Shipped;
                order.Shipped = true;
                _orderRepository.Save();
            }
        }

        public async Task CancelOrderAsync(int id)
        {
            var order = _orderRepository.GetOneOrder(id);
            if (order != null)
            {
                order.Status = OrderStatus.Failed;
                _orderRepository.Save();
            }
        }

        public Order? GetOneOrder(int id)
        {
            return _orderRepository.GetOneOrder(id);
        }

        public void SaveOrder(Order order)
        {
            _orderRepository.SaveOrder(order);
        }
    }
}