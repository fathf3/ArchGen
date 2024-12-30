# ArchGen - .NET Architecture Generator Tool

ArchGen, .NET projeleriniz için N-Layer ve Onion mimarilerini hızlıca oluşturmanızı sağlayan bir komut satırı aracıdır.

## Özellikler

- N-Layer mimari oluşturma
  - Core Layer
  - Business Layer
  - Data Access Layer
  - Entities Layer
  - API Layer

- Onion mimari oluşturma
  - Domain Layer
  - Application Layer
  - Infrastructure Layer
  - Persistence Layer
  - API Layer

- Her katman için temel sınıflar ve arayüzler
- Entity Framework Core entegrasyonu
- Repository ve Service pattern implementasyonları
- REST API endpoint'leri
- Dependency Injection desteği

## Kurulum

```bash
dotnet tool install --global ArchGen
```

## Kullanım

### N-Layer Mimari Oluşturma

```bash
archgen nlayer ProjectName
```

Bu komut aşağıdaki yapıyı oluşturur:
```
ProjectName/
├── ProjectName.Core/
├── ProjectName.Business/
├── ProjectName.DataAccess/
├── ProjectName.Entities/
└── ProjectName.API/
```

### Onion Mimari Oluşturma

```bash
archgen onion ProjectName
```

Bu komut aşağıdaki yapıyı oluşturur:
```
ProjectName/
├── ProjectName.Domain/
├── ProjectName.Application/
├── ProjectName.Infrastructure/
├── ProjectName.Persistence/
└── ProjectName.API/
```

### Yardım Komutu

```bash
archgen --help
```

## Oluşturulan Proje Özellikleri

### N-Layer Mimari

- **Core Layer**: Temel arayüzler ve yardımcı sınıflar
  - IEntity
  - IRepository
  - Base interfaces

- **Entities Layer**: Veritabanı modelleri
  - BaseEntity
  - Domain models

- **Data Access Layer**: Veritabanı işlemleri
  - EntityFramework Core
  - Repository implementations
  - Database context

- **Business Layer**: İş mantığı
  - Services
  - Business rules
  - Validations

- **API Layer**: REST API
  - Controllers
  - DTOs
  - API configurations

### Onion Mimari

- **Domain Layer**: İş modelleri ve arayüzler
  - Entities
  - Value objects
  - Domain events

- **Application Layer**: İş mantığı
  - Interfaces
  - Services
  - DTOs

- **Infrastructure Layer**: Harici servisler
  - Email service
  - File storage
  - Third-party integrations

- **Persistence Layer**: Veritabanı işlemleri
  - EntityFramework Core
  - Repositories
  - Database context

- **API Layer**: REST API
  - Controllers
  - API configurations
  - Middleware

## Gereksinimler

- .NET 7.0 SDK veya üzeri
- Entity Framework Core araçları

## Güncelleme

```bash
dotnet tool update -g ArchGen
```

## Kaldırma

```bash
dotnet tool uninstall -g ArchGen
```


## Lisans

Bu proje MIT lisansı altında lisanslanmıştır. Detaylar için [LICENSE](LICENSE) dosyasına bakın.

