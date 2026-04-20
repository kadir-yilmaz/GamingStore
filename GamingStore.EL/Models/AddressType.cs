namespace GamingStore.EL.Models
{
    /// <summary>
    /// Adres kullanım tipi
    /// </summary>
    public enum AddressType
    {
        Delivery = 1,  // Teslimat adresi
        Invoice = 2    // Fatura adresi
    }

    /// <summary>
    /// Adres kategorisi (Ev veya İş yeri)
    /// </summary>
    public enum AddressCategory
    {
        Home = 1,       // Ev
        Workplace = 2   // İş yeri
    }
}
