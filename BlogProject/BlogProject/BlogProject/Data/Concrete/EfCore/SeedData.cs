using BlogProject.Data.Concrete.EfCore;
using BlogProject.Entity;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BlogProject.Data
{
    public static class SeedData
    {
        public static void TestVerileriniDoldur(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetService<BlogContext>();

            if (context != null)
            {
                if (context.Database.GetPendingMigrations().Any())
                {
                    context.Database.Migrate();
                }

                // Veritabanında hiç kullanıcı yoksa seed data'yı ekle
                if (!context.Users.Any())
                {
                    // Kullanıcılar
                    var users = new List<User>
                    {
                        new User
                        {
                            UserName = "admin",
                            Name = "Site Yöneticisi",
                            Email = "admin@blog.com",
                            Password = "admin123",
                            Image = "admin.jpg",
                            About ="hakkımda"
                        },
                        new User
                        {
                            UserName = "mehmet",
                            Name = "Mehmet Yılmaz",
                            Email = "mehmet@example.com",
                            Password = "mehmet123",
                            Image = "mehmet_1.jpg",
                            About ="hakkımda"
                        },
                        new User
                        {
                            UserName = "ayse",
                            Name = "Ayşe Demir",
                            Email = "ayse@example.com",
                            Password = "ayse123",
                            Image = "ayse.jpg",
                            About ="hakkımda"
                        }
                    };

                    context.Users.AddRange(users);
                    context.SaveChanges();

                    // Etiketler
                    var tags = new List<Tag>
                    {
                        new Tag
                        {
                            Text = "Yapay Zeka",
                            Url = "yapay-zeka",
                            Color = TagColors.primary
                        },
                        new Tag
                        {
                            Text = "İş Dünyası",
                            Url = "is-dunyasi",
                            Color = TagColors.info
                        },
                        new Tag
                        {
                            Text = "Teknoloji",
                            Url = "teknoloji",
                            Color = TagColors.warning
                        },
                        new Tag
                        {
                            Text = "Sağlıklı Yaşam",
                            Url = "saglikli-yasam",
                            Color = TagColors.success
                        },
                        new Tag
                        {
                            Text = "Beslenme",
                            Url = "beslenme",
                            Color = TagColors.secondary
                        },
                        new Tag
                        {
                            Text = "Ekolojik Moda",
                            Url = "ekolojik-moda",
                            Color = TagColors.danger
                        },
                        new Tag
                        {
                            Text = "Doğa",
                            Url = "doga",
                            Color = TagColors.primary 
                        },
                        new Tag
                        {
                            Text = "Sürdürülebilirlik",
                            Url = "surdurulebilirlik",
                            Color = TagColors.info  
                        },
                        new Tag
                        {
                            Text = "Diğer",
                            Url = "diger",
                            Color = TagColors.info  
                        }
                    };

                        context.Tags.AddRange(tags);
                        context.SaveChanges();

                    // Postlar (her tag için bir post)
                    var posts = new List<Post>
                    {
                      
                        new Post
                        {
                            Title = "Yapay Zeka ile Geleceğe Hazırlanmak",
                            Content = @"
                                        Günümüzde yapay zeka (YZ), iş dünyasından sağlığa, eğitimden ulaşıma kadar pek çok alanda devrim yaratıyor. 
                                        YZ sistemleri, büyük veri analizi yaparak kalıpları tespit edebiliyor ve insan beyninin çok ötesinde hızla öğrenebiliyor. 
                                        Bu makalede, YZ’nin temel kavramlarına ve pratik uygulama örneklerine odaklanacağız.

                                        Birinci bölümde, yapay zekanın tarihçesine kısaca değineceğiz. 1950’lerde Turing testiyle başlayan yolculuk, 
                                        1980’lerde makine öğrenimi (ML) kavramıyla ivme kazandı. Günümüzde derin öğrenme (deep learning) algoritmaları, 
                                        sinir ağları üzerinden görüntü tanıma ve doğal dil işleme (NLP) gibi karmaşık görevleri başarıyla yerine getiriyor.

                                        İkinci bölümde, iş dünyasında YZ’nin kullanım alanlarına göz atacağız. Müşteri hizmetlerinde chatbot’lar, 
                                        finans sektöründe kredi risk analizi, perakendede stok optimizasyonu ve pazarlamada kişiselleştirilmiş öneri 
                                        sistemleri, yapay zekanın öne çıkan örnekleridir. Bu uygulamalar, işletmelerin verimliliğini artırırken maliyetleri düşürüyor.

                                        Son olarak, yapay zekanın gelecekteki rolünü tartışacağız. Otonom araçlar, akıllı şehirler ve sağlıkta erken 
                                        teşhis sistemleri gibi alanlarda YZ’nin sınırları her geçen gün genişliyor. Etik ve güvenlik konuları da bu 
                                        yolculuğun önemli parçaları; şeffaf algoritmalar ve veri gizliliği standartları, güvenli bir gelecek için kritik.

                                        ",
                            Description = "Yapay Zeka ve uygulama alanları",
                            Url = "yapay-zeka-gefuture",
                            Image = "yapayzeka.jpg",
                            PublishedOn = DateTime.Now.AddDays(-30),
                            IsActive = true,
                            UserId = users[0].UserId,
                            TagId = tags[0].TagId,
                            Tag = tags[0]
                        },
                        
                        new Post
                        {
                            Title = "Dijital Dönüşüm ve İş Dünyası",
                            Content = @"
                                        Dijital dönüşüm, işletmelerin tüm süreçlerini dijital teknolojilerle yeniden tasarlama sürecidir. 
                                        Bu dönüşüm; verimliliği artırmak, müşteri deneyimini iyileştirmek ve rekabet avantajı elde etmek amacıyla 
                                        yeni iş modelleri oluşturmayı hedefler.

                                        İlk bölümde, dijital dönüşümün temel bileşenlerini ele alacağız: bulut bilişim, büyük veri analitiği, 
                                        nesnelerin interneti (IoT) ve yapay zeka. Her bir teknoloji, farklı departmanlarda nasıl entegre edilebilir, 
                                        örneklerle inceleyeceğiz.

                                        İkinci bölümde, KOBİ’lerden büyük ölçekli şirketlere kadar farklı ölçeklerdeki firmaların dijitalleşme 
                                        serüvenlerini paylaşacağız. Başarı hikayeleri arasında tedarik zinciri optimizasyonu, uzaktan çalışma 
                                        infrastrüktürleri ve e-ticaret platformlarının hızlı adaptasyonu yer alıyor.

                                        Üçüncü bölümde ise dijital dönüşümün zorluklarını tartışacağız. Kurum kültürü, yetenek açığı ve siber güvenlik 
                                        riskleri, dönüşüm projelerinin önündeki başlıca engeller. Stratejik yol haritası ve değişim yönetimi yaklaşımlarıyla 
                                        bu zorlukların üstesinden nasıl gelinebileceğini anlatacağız.

",
                            Description = "Dijital dönüşümün iş dünyasına etkisi",
                            Url = "dijital-donusum-is-dunyasi",
                            Image = "isdunyasi.jpg",
                            PublishedOn = DateTime.Now.AddDays(-28),
                            IsActive = true,
                            UserId = users[1].UserId,
                            TagId = tags[1].TagId,
                            Tag = tags[1]
                        },
                        
                        new Post
                        {
                            Title = "2025'in Öne Çıkan Teknoloji Trendleri",
                            Content = @"
                                        Her yıl, teknoloji dünyasında yeni trendler şekilleniyor. 2025’te odaklanmanız gereken beş önemli trendi bu yazıda 
                                        derledik: kuantum bilişim, 6G haberleşme, biyoteknolojide devrim, sürdürülebilir enerji çözümleri ve uzay teknolojileri.

                                        Kuantum bilişim, klasik bilgisayarların çözmekte zorlandığı karmaşık problemleri çözecek potansiyele sahip. 
                                        İlaç keşfinden finansal modellere kadar geniş bir yelpazede kullanılacak.

                                        6G teknolojisi, 5G’nin sunduğu yüksek hız ve düşük gecikme avantajını bir üst seviyeye taşıyacak. 
                                        Otonom araçlar ve uzaktan ameliyat gibi gerçek zamanlı uygulamalar için kritik olacak.

                                        Biyoteknoloji alanında CRISPR ve gen düzenleme teknikleri, tarım ve tıp alanında kişiselleştirilmiş 
                                        tedaviler sunmaya başlayacak. Sürdürülebilir enerji çözümleri ise güneş ve rüzgar enerjisinin 
                                        verimliliğini artırarak karbon ayak izini azaltacak.

                                        Son olarak, ticari uzay uçuşları ve uzay madenciliği projeleri, önümüzdeki beş yılda hız kazanacak. 
                                        Özellikle Mars misyonları ve ay üsleri, insanlığın yeni sınırlarını belirleyecek.

                                        ",
                            Description = "2025 teknoloji trendleri",
                            Url = "2025-teknoloji-trendleri",
                            Image = "teknoloji.jpg",
                            PublishedOn = DateTime.Now.AddDays(-25),
                            IsActive = true,
                            UserId = users[2].UserId,
                            TagId = tags[2].TagId,
                            Tag = tags[2]
                        },
                        
                        new Post
                        {
                            Title = "Günlük Yaşamda Sağlıklı Alışkanlıklar",
                            Content = @"
                                        Sağlıklı bir yaşam, sadece spor yapmak veya diyet uygulamakla sınırlı değil; zihinsel ve duygusal denge de 
                                        aynı derecede önemli. Bu rehberde, günlük rutininize ekleyebileceğiniz beş temel alışkanlığı inceleyeceğiz.

                                        Birinci alışkanlık, düzenli uyku: Yetişkinler için 7–9 saatlik kaliteli uyku, bağışıklık sistemi ve 
                                        bilişsel fonksiyonlar için hayati. İkinci alışkanlık, dengeli beslenme: İşlenmiş gıdalardan kaçınarak 
                                        tam tahıllar, taze meyve ve sebzeler tercih edin.

                                        Üçüncü alışkanlık, düzenli egzersiz: Haftada en az 150 dakika orta yoğunlukta aerobik aktivite 
                                        veya 75 dakika yüksek yoğunluklu antrenman önerilir. Dördüncü alışkanlık, stres yönetimi: 
                                        Meditasyon, nefes egzersizleri ve doğada vakit geçirmek zihinsel sağlığı destekler.

                                        Beşinci ve son alışkanlık ise sosyal bağlar: Aile ve arkadaşlarla kaliteli zaman geçirmek, 
                                        duygusal dayanıklılığı güçlendirir. Bu beş adımı uygulayarak, daha dengeli ve enerjik bir hayat 
                                        sürdürebilirsiniz.

                                        ",
                            Description = "Sağlıklı yaşam ipuçları",
                            Url = "saglikli-aliskanliklar",
                            Image = "saglik.jpg",
                            PublishedOn = DateTime.Now.AddDays(-22),
                            IsActive = true,
                            UserId = users[0].UserId,
                            TagId = tags[3].TagId,
                            Tag = tags[3]
                        },
                        
                        new Post
                        {
                            Title = "Bitkisel Beslenmenin Faydaları",
                            Content = @"
                                        Bitkisel beslenme, hem sağlık hem de çevresel sürdürülebilirlik açısından tercih ediliyor. 
                                        Bu rehberde, bitkisel beslenmeye geçiş sürecini kolaylaştıracak pratik adımları ele alacağız.

                                        İlk adım, haftada bir öğünü bitkisel olarak planlamak. Zamanla bu sayıyı artırarak tamamen bitkisel 
                                        beslenmeye geçiş yapabilirsiniz. İkinci adım, tam tahıllar ve baklagiller gibi protein kaynaklarını 
                                        menünüze eklemek.

                                        Üçüncü adım, vitamin ve mineral takviyelerini öğrenmek: B12, D vitamini ve demir alımına dikkat 
                                        edin. Dördüncü adım, lezzetli tarifler: Sebzeli kinoa salatası, nohutlu sebze çorbası gibi kolay 
                                        yemeklerle çeşitlilik sağlayın.

                                        Beşinci adım, topluluk desteği: Online forumlar ve yerel gruplar, deneyim paylaşımı için harika 
                                        bir kaynak. Bu adımları izleyerek, sağlıklı ve dengeli bir bitkisel beslenme alışkanlığı 
                                        oluşturabilirsiniz.

                                        ",
                            Description = "Bitkisel beslenme rehberi",
                            Url = "bitkisel-beslenme",
                            Image = "beslenme.jpg",
                            PublishedOn = DateTime.Now.AddDays(-20),
                            IsActive = true,
                            UserId = users[1].UserId,
                            TagId = tags[4].TagId,
                            Tag = tags[4]
                        },
                       
                        new Post
                        {
                            Title = "Sürdürülebilir Kumaş Seçimleri",
                           Content = @"
                                        Ekolojik moda, sadece kıyafet seçiminden öte bir yaşam tarzı. Bu yazıda, sürdürülebilir moda ilkelerini 
                                        ve gardırobunuzu nasıl dönüştürebileceğinizi adım adım açıklıyoruz.

                                        Öncelikle, kaliteli ve uzun ömürlü parçalar seçin. Fast fashion yerine yerel üreticilerin etik olarak 
                                        ürettikleri ürünlere yönelin. İkinci olarak, ikinci el alışveriş: Vintage mağazalar ve online 
                                        platformlar, benzersiz parçalar bulmanızı sağlar.

                                        Üçüncü adım, bakım ve onarım: Basit dikiş teknikleriyle eski kıyafetlerinizi canlandırın. 
                                        Dördüncü adım, geri dönüşüm: Artık giymediğiniz tekstilleri geri dönüştürmek için atık toplama 
                                        programlarına katılın.

                                        Beşinci adım, minimalizm: Gardırobunuzda temel parçalarla kombinler oluşturarak hem sade hem de 
                                        şık bir stil yakalayın. Bu yaklaşımlar, modanın çevreye etkisini azaltırken, stilinizi de güçlendirir.

                                        ",
                            Description = "Ekolojik kumaş rehberi",
                            Url = "surdurulebilir-kumaslar",
                            Image = "ekomoda.jpg",
                            PublishedOn = DateTime.Now.AddDays(-18),
                            IsActive = true,
                            UserId = users[2].UserId,
                            TagId = tags[5].TagId,
                            Tag = tags[5]
                        },
                        
                        new Post
                        {
                            Title = "Şehirde Doğa Kaçamakları",
                             Content = @"
                                        Büyük şehirlerde bile gizli doğa köşeleri bulmak mümkün. Bu yazıda, İstanbul ve Ankara gibi 
                                        metropollerde keşfedebileceğiniz beş yeşil alanı derledik.

                                        1. Park A: Yürüyüş yolları ve göletleriyle huzur bulacağınız bir vaha.  
                                        2. Koruluk B: Kuş gözlem noktaları ve bisiklet parkurlarıyla doğayla iç içe bir deneyim.  
                                        3. Bahçe C: Botanik koleksiyonuyla bitki çeşitliliğini inceleyebileceğiniz küçük bir bahçe.  
                                        4. Vadi D: Hafif bir doğa yürüyüşü için ideal, su sesi eşliğinde dinlenme alanları.  
                                        5. Mesire Alanı E: Piknik ve kamp için uygun, şehir merkezine yakın bir mesire.

                                        Bu alanlar, hafta sonu veya hafta içi molalarında doğayla buluşmanızı sağlayacak.

                                        ",
                            Description = "Şehirde doğa önerileri",
                            Url = "sehirde-doga",
                            Image = "doga.jpg",
                            PublishedOn = DateTime.Now.AddDays(-15),
                            IsActive = true,
                            UserId = users[0].UserId,
                            TagId = tags[6].TagId,
                            Tag = tags[6]
                        },
                      
                        new Post
                        {
                            Title = "Evde Sürdürülebilir Yaşam İpuçları",
                            Content = @"
                                        Sürdürülebilir bir yaşam, günlük alışkanlıklarla başlar. Evde enerji tasarrufu ve atık azaltma 
                                        için uygulayabileceğiniz basit ama etkili yöntemleri paylaşıyoruz.

                                        Enerji tasarrufu için LED ampullere geçiş, su tasarrufu için düşük akışlı musluk başlıkları 
                                        ve kompost yapımı, atık yönetiminde üç kovalı ayırma sistemi öneriyoruz.

                                        Ayrıca, doğal temizlik ürünleri (sirke, karbonat) kullanarak kimyasal atıkları azaltabilirsiniz. 
                                        Alışverişlerinizde yeniden kullanılabilir torbalar ve şişeler tercih etmek de büyük fark yaratır.

                                        Son olarak, sürdürülebilir tüketim bilinci: Her alışveriş öncesi gerçekten ihtiyaç var mı 
                                        diye düşünün. Bu küçük adımlar, hem çevreye hem de bütçenize katkı sağlar.

                                        ",
                            Description = "Evde sürdürülebilirlik",
                            Url = "evde-surdurulebilirlik",
                            Image = "surdurulebilirlik.jpg",
                            PublishedOn = DateTime.Now.AddDays(-12),
                            IsActive = true,
                            UserId = users[1].UserId,
                            TagId = tags[7].TagId,
                            Tag = tags[7]
                        },
                       
                        new Post
                        {
                            Title = "Minimalist Teknoloji: Az Eşya, Çok İşlev",
                             Content = @"
                                        Teknoloji ürünleri her geçen gün daha kompakt ve çok işlevli hale geliyor. Minimalist yaşam tarzı 
                                        için en uygun cihazları ve aksesuarları inceledik.

                                        Akıllı saatler, hem bildirimleri hem de sağlık verilerini takip ediyor; telefon ihtiyacını azaltıyor.  
                                        Katlanabilir ekranlı telefonlar ve tabletler, tek bir cihazla hem telefon hem de tablet deneyimi sunuyor.  
                                        Taşınabilir projektörler, küçük alanları anında sinema salonuna çeviriyor.  
                                        Modüler klavye ve mouse setleri, yalnızca ihtiyacınız olan parçaları bir arada tutuyor.

                                        Bu cihazlar, hem yaşam alanınızı sadeleştiriyor hem de çok yönlü kullanım imkânı sağlıyor.

                                        ",
                            Description = "Minimalist teknoloji rehberi",
                            Url = "minimalist-teknoloji",
                            Image = "diger.jpg",
                            PublishedOn = DateTime.Now.AddDays(-10),
                            IsActive = true,
                            UserId = users[2].UserId,
                            TagId = tags[8].TagId,
                            Tag = tags[8]
                        }
                    };

                    context.Posts.AddRange(posts);
                    context.SaveChanges();

                    // Yorumlar 
                    var comments = new List<Comment>
                    {
                        new Comment
                        {
                            Text = "Bu yazı bana gerçekten ilham verdi, teşekkürler!",
                            PublishedOn = DateTime.Now.AddDays(-29),
                            PostId = posts[0].PostId,
                            UserId = users[1].UserId
                        },
                        new Comment
                        {
                            Text = "Dijital dönüşümde özellikle bulut teknolojilerini daha fazla görmek isterim.",
                            PublishedOn = DateTime.Now.AddDays(-27),
                            PostId = posts[1].PostId,
                            UserId = users[2].UserId
                        },
                        new Comment
                        {
                            Text = "2025 trendleri gerçekten ufuk açıcı, liste çok faydalı oldu.",
                            PublishedOn = DateTime.Now.AddDays(-24),
                            PostId = posts[2].PostId,
                            UserId = users[0].UserId
                        },
                        new Comment
                        {
                            Text = "Günlük alışkanlıklarıma ekledim, enerji seviyem fark edilir şekilde arttı.",
                            PublishedOn = DateTime.Now.AddDays(-21),
                            PostId = posts[3].PostId,
                            UserId = users[2].UserId
                        },
                        new Comment
                        {
                            Text = "Bitkisel beslenme rehberiniz çok yararlı, deneyeceğim tarifleri.",
                            PublishedOn = DateTime.Now.AddDays(-19),
                            PostId = posts[4].PostId,
                            UserId = users[0].UserId
                        },
                        new Comment
                        {
                            Text = "Ekolojik kumaşlar hakkında detaylı bilgi için teşekkürler.",
                            PublishedOn = DateTime.Now.AddDays(-17),
                            PostId = posts[5].PostId,
                            UserId = users[1].UserId
                        },
                        new Comment
                        {
                            Text = "Bu rota önerileri hafta sonu kaçamağım için harika oldu.",
                            PublishedOn = DateTime.Now.AddDays(-14),
                            PostId = posts[6].PostId,
                            UserId = users[2].UserId
                        },
                        new Comment
                        {
                            Text = "Evde su tasarrufu için verdiğiniz ipuçları çok işe yaradı.",
                            PublishedOn = DateTime.Now.AddDays(-11),
                            PostId = posts[7].PostId,
                            UserId = users[0].UserId
                        },
                        new Comment
                        {
                            Text = "Minimalist teknoloji gerçekten sade ve kullanışlı ürünler sunuyor.",
                            PublishedOn = DateTime.Now.AddDays(-9),
                            PostId = posts[8].PostId,
                            UserId = users[1].UserId
                        }
                    };

                    context.Comments.AddRange(comments);
                    context.SaveChanges();

                    
                    var tagDict = context.Tags.ToDictionary(t => t.Text, t => t.TagId);

                 
                    posts[0].TagId = tagDict["Yapay Zeka"];
                    posts[1].TagId = tagDict["İş Dünyası"];
                    posts[2].TagId = tagDict["Teknoloji"];
                    posts[3].TagId = tagDict["Sağlıklı Yaşam"];
                    posts[4].TagId = tagDict["Beslenme"];
                    posts[5].TagId = tagDict["Ekolojik Moda"];
                    posts[6].TagId = tagDict["Doğa"];
                    posts[7].TagId = tagDict["Sürdürülebilirlik"];
                    posts[8].TagId = tagDict["Diğer"];

                    context.SaveChanges();


                }
            }
        }
    }
}
