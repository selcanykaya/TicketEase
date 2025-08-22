# 🎟️ TicketEase API

TicketEase, etkinlik ve bilet yönetimini kolaylaştıran **modern bir ASP.NET Core web API projesidir**.  
Proje, SOLID prensiplerine uygun, güvenli ve ölçeklenebilir bir yapı sunmaktadır.  

---

## 📌 Proje Özeti

TicketEase API ile kullanıcılar etkinlikleri görüntüleyebilir, bilet satın alabilir ve organizatörler etkinliklerini yönetebilir.  
Proje **3 katmandan oluşur**:  

| Katman | Açıklama |
|--------|----------|
| 🗄 Data Layer | EF Core Code First ile veritabanı modelleri ve ilişkiler oluşturuldu. Çoktan-çoğa (many-to-many) ilişkiler Ticket ve Order tablolarında implement edildi. |
| ⚙️ Business Layer | İş mantığı burada yer almakta. Service ve Manager katmanları ile **Dependency Injection** kullanıldı. |
| 🌐 API Layer | RESTful endpointler (GET, POST, PUT, PATCH, DELETE) ile kullanıcı etkileşimi sağlanır. |

Projede ayrıca:  
- 🔑 **JWT Authentication** ile güvenli kimlik doğrulama  
- 🛡 **Authorization** ile rol bazlı yetkilendirme  
- 🧩 Middleware ile **global exception handling**  
- 📝 Action Filter ile endpoint bazlı ek kontroller  
- ✅ Model Validation ile veri doğrulama  
- 🔒 Data Protection ile şifreleme ve hassas veri güvenliği  

---

## 🛠 Kullanılan Teknolojiler

| Teknoloji | Açıklama |
|-----------|----------|
| C# & .NET 8 / ASP.NET Core 8 | Modern web API geliştirme |
| Entity Framework Core | Code First yaklaşımı ile veritabanı yönetimi |
| JWT | Kullanıcı kimlik doğrulama ve güvenliği |
| ASP.NET Core Identity / Custom User Management | Kullanıcı ve rol yönetimi |
| Middleware & Action Filters | Merkezi hata yönetimi ve endpoint kontrolü |
| SQL Server | Veritabanı |
| Dependency Injection | Servislerin yönetimi ve loose coupling |
| Data Protection | Hassas verilerin şifrelenmesi |
| Global Exception Handling | Hataların merkezi yönetimi |

---

## ⚡ API Özellikleri

- 🎫 **Etkinlik Yönetimi:** CRUD operasyonları, arama, sayfalama, sıralama  
- 🎟 **Bilet Yönetimi:** Kullanıcı bilet satın alabilir, etkinlik katılımcıları listelenebilir  
- 👤 **Kullanıcı Yönetimi:** Kayıt, giriş, rol yönetimi, yetkilendirme  
- 🔍 **Filtreleme & Sıralama:** Esnek arama ve sıralama mekanizmaları  

---

## 📝 Örnek Endpointler

| 🔹 Method | 🔹 URL | 🔹 Açıklama |
|-----------|--------|------------|
| GET | /api/events | Tüm etkinlikleri listele |
| POST | /api/events | Yeni etkinlik oluştur |
| PUT | /api/events/{id} | Etkinlik güncelle |
| PATCH | /api/events/{id} | Etkinlik bilgilerini kısmi güncelle |
| DELETE | /api/events/{id} | Etkinlik sil |
| GET | /api/venues/search | Mekan arama, pagination ve filtreleme |

---

## 🏗 Proje Yapısı

TicketEase/
│
├── TicketEase.API/ # 🌐 API Katmanı
├── TicketEase.Business/ # ⚙️ İş Mantığı
├── TicketEase.Data/ # 🗄 Entity ve Repository Katmanı


📬 İletişim

Herhangi bir soru veya öneri için: selcanykaya13@gmail.com
