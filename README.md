# GamingStore E-Commerce

ASP.NET Core MVC kullanılarak geliştirilmiş, N-Tier (Katmanlı) mimari yapısı ve modern e-ticaret özelliklerini içeren kapsamlı bir oyun ve donanım satış platformudur.

## Proje İçeriği ve Teknik Özellikler

Bu proje kapsamında aşağıdaki mimari yapılar ve teknik konular ele alınmıştır:

- **N-Tier Architecture**: Core, Data, Business ve WebUI katmanları ile ayrıştırılmış, sürdürülebilir mimari.
- **Repository Pattern**: Veri erişim katmanında soyutlama ve bağımlılık yönetimi.
- **Identity Framework**: Role-based (Rol tabanlı) kullanıcı yetkilendirme ve kimlik doğrulama sistemi.
- **Payment Integration**: Iyzipay API entegrasyonu ile güvenli ödeme süreçleri.
- **Mail Service**: MailKit entegrasyonu ile SMTP üzerinden otomatik bildirim ve doğrulama e-postaları.
- **Global Error Handling**: Uygulama genelinde merkezi hata yakalama ve yönetimi.
- **Session & Cart Management**: Kullanıcı bazlı sepet veri yönetimi.
- **Dependency Injection**: Esnek ve test edilebilir bir yapı için servislerin merkezi yönetimi.
- **DTO & AutoMapper**: Veri transfer nesneleri ile katmanlar arası güvenli veri taşıma.

## Kullanılan Teknolojiler

- **Backend**: .NET 8.0 / ASP.NET Core MVC
- **ORM**: Entity Framework Core (SQL Server)
- **UI/UX**: Bootstrap, FontAwesome, JavaScript & jQuery
- **Service Integration**: Iyzipay, MailKit
- **Security**: ASP.NET Core Identity (Authentication & Authorization)

## Kurulum ve Çalıştırma

1. Projeyi bilgisayarınıza clonelayın: `git clone https://github.com/kullaniciadi/GamingStore.git`
2. SQL Server bağlantı dizenizi `appsettings.json` dosyasındaki `sqlConnection` alanına ekleyin.
3. `appsettings.Development.json` dosyasına Iyzipay ve Mail settings bilgilerinizi girin.
4. Terminalde `dotnet ef database update` komutunu çalıştırarak veritabanını oluşturun.
5. Projeyi `dotnet run` veya Visual Studio üzerinden başlatın.
