namespace GamingStore.EL.Models
{
    public enum OrderStatus
    {
        Pending = 0,         // Beklemede
        Completed = 1,       // Ödeme Tamamlandı
        Failed = 2,          // Başarısız
        Processing = 3,      // Hazırlanıyor
        Shipped = 4,         // Kargoda
        Delivered = 5        // Teslim Edildi
    }
}
