# 🔗 Bağlı Liste Simülasyonu - Veri Yapıları Proje Ödevi

Bu proje, veri yapıları dersi kapsamında geliştirilen, farklı bağlı liste türlerini görsel ve etkileşimli olarak simüle eden bir ASP.NET Core uygulamasıdır. Proje, kullanıcıların tek yönlü, çift yönlü ve döngüsel bağlı listeler üzerinde çeşitli işlemler yapabilmesini sağlayan bir sistem sunar.

## 👨‍💻 Geliştirici Ekip
- ALPER CAN ÖZER        032190152      
- YUSIF HASANGULIYEV    032190144      
- TAHA YASİN ERDOĞAN    032190146
- OKAN ÇALIŞKAN         032190145

## 🎯 Proje Amacı
Veri yapıları konusunun temel konularından olan bağlı listeleri (linked list) öğrencilerin daha iyi kavrayabilmesi için interaktif bir ortamda simüle etmek. Proje ile kullanıcılar aşağıdaki işlemleri gerçekleştirebilir:

- Düğüm ekleme  
- Düğüm silme  
- Belirli bir düğümü arama  
- Listeyi baştan sona dolaşma (traverse)  
- Farklı bağlı liste türleri (tek yönlü, çift yönlü, döngüsel) arasında geçiş yapabilme

## 📌 Projenin Kapsamı

Bu proje, farklı bağlı liste türlerini (tek yönlü, çift yönlü ve döngüsel) kullanıcıya görsel olarak sunan ve temel liste işlemlerinin adım adım simülasyonunu gerçekleştiren bir web uygulamasıdır. ASP.NET Core teknolojisi kullanılarak geliştirilen proje, veri yapılarının soyut yapısını somutlaştırmayı ve kullanıcı etkileşimiyle pekiştirmeyi amaçlamaktadır.

### 🔧 Desteklenen Temel İşlevler

- **Liste Türü Seçimi:**  
  Kullanıcı, üç farklı bağlı liste türünden birini seçerek işlem yapabilir:
  - Tek yönlü bağlı liste (`Singly Linked List`)
  - Çift yönlü bağlı liste (`Doubly Linked List`)
  - Döngüsel bağlı liste (`Circular Linked List`)

- **Düğüm Ekleme:**  
  Seçilen liste türüne uygun şekilde yeni düğüm eklenir ve liste güncellenir.

- **Düğüm Silme:**  
  Girilen değere sahip düğüm listeden silinir. Liste bağlantıları uygun şekilde yeniden düzenlenir.

- **Düğüm Arama:**  
  Kullanıcı tarafından girilen değere sahip düğüm arama algoritmasıyla bulunur ve görsel olarak vurgulanır.

- **Listeyi Dolaşma (Traverse):**  
  Liste düğümleri sırayla gezilir ve tüm değerler görsel olarak kullanıcıya gösterilir.

### 🖼️ Görsel Temsil

- Her bir düğüm, sayısal değeri temsil eden kutucuklar şeklinde görselleştirilir.
- Liste türüne göre bağlantılar:
  - Tek yönlü listede → okları
  - Çift yönlü listede ⇄ okları
  - Döngüsel listede son düğümden ilk düğüme ok bağlantısı
- Arama sonucu bulunan düğüm, farklı bir renkle vurgulanır.

### ⚙️ Dinamik Güncellemeler

- Yapılan her işlem (ekleme, silme, arama, dolaşma) sonrasında liste animasyonları güncellenir.
- Sayfa yenilenmeden kullanıcıya anlık geri bildirim sağlanır.
- Liste görselleştirmesi JavaScript ile `list-container` div’i içine basılır.

Bu kapsam doğrultusunda proje, algoritma kavrayışı ile kullanıcı deneyimini birleştirmeyi hedeflemekte, hem eğitim hem de uygulama pratiği açısından zengin bir deneyim sunmaktadır.


## 🧰 Kullanılan Teknolojiler
- ASP.NET Core (Razor Pages)
- C# (Object-Oriented Programming)
- HTML & CSS
- JavaScript (DOM işlemleri)
- Chart.js (isteğe bağlı animasyon ve görsel destek)

## 🗂️ Proje Yapısı

### 📁 Models Klasörü
- `Node.cs`: Bağlı listedeki düğüm sınıfı. Düğümün değeri ve sonraki/önceki düğümleri tutar.
- `SinglyLinkedList.cs`: Tek yönlü bağlı liste işlemlerini yönetir.
- `DoublyLinkedList.cs`: Çift yönlü bağlı liste işlemlerini içerir.
- `CircularLinkedList.cs`: Döngüsel bağlı liste yapısını tanımlar.
- `ListManager.cs`: Liste türleri arasında geçişi ve işlem yürütmeyi sağlayan yönetici sınıf.
- `ListSerializer.cs`: Liste yapısını JSON formatına dönüştürerek arayüze aktarır.

### 📁 Pages Klasörü
- `Index.cshtml`: Ana sayfa. Kullanıcıyı simülasyon sayfasına yönlendirir.
- `Pages/LinkedList/Index.cshtml`: Simülasyon arayüzünü içerir.
- `Pages/LinkedList/Index.cshtml.cs`: Arayüzle backend arasında veri bağlamasını ve işlemleri kontrol eder.

## 🖥️ Uygulama Arayüzü

- Ana sayfa (`Index.cshtml`) sade ve yönlendirici bir tasarıma sahiptir. Kullanıcıyı "Simülasyona git" butonuyla yönlendirir.
- Simülasyon sayfasında kullanıcı:
  - Düğüm değeri girebilir
  - Liste türü seçebilir (`tek yönlü`, `çift yönlü`, `döngüsel`)
  - İşlem türü seçebilir (`ekle`, `sil`, `ara`, `gez`)
- Düğümler kutucuklar halinde görselleştirilir. 
  - Tek yönlü listelerde düğümler sağa doğru → işaretiyle bağlanır.
  - Çift yönlü listelerde ⇄ oklarıyla çift yönlü bağlantılar gösterilir.
  - Döngüsel listelerde son düğüm tekrar ilk düğüme bağlanarak dairesel yapı simgelenir.
- Arama fonksiyonunda bulunan düğüm animasyonla vurgulanır.
- İşlem sonrası liste yapısı gerçek zamanlı olarak güncellenir.

## ✨ Öne Çıkan Özellikler
- 🔄 Dinamik liste güncellemeleri
- 🎨 Liste türüne göre özelleştirilmiş görselleştirme
- 🔍 Arama işlemi sırasında düğüm vurgulama animasyonu
- 🖱️ Kullanıcı dostu form tasarımı
- 🔧 Modüler kod yapısı sayesinde kolayca yeni algoritmalar veya özellikler eklenebilir

## 🧪 Kullanım Talimatları

1. Projeyi Visual Studio ile açın.
2. `Ctrl + F5` ile çalıştırın.
3. Ana sayfada çıkan bağlantıya tıklayarak simülasyon ekranına geçin.
4. Düğüm değeri, liste türü ve yapılacak işlemi seçerek "Gönder" butonuna tıklayın.
5. Liste görseli ekranın altında güncellenir.

## 📸 Ekran Görüntüleri

> Simülasyon ekranı, bağlı liste türleri ve arama animasyonu gibi bölümlerin görselleri rapora eklenecektir.

## 📌 Notlar
- Tüm liste türleri için ayrı ayrı veri yapıları ve işlem mantığı kodlanmıştır.
- Proje sırasında OOP prensiplerine dikkat edilmiştir.
- Kod yapısı, ileri seviye projelere temel oluşturabilecek şekilde modüler tasarlanmıştır.

## 📄 Lisans
Bu proje, Uludağ Üniversitesi Bilgisayar Mühendisliği "Veri Yapıları" dersi kapsamında eğitim amacıyla geliştirilmiştir.
