using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class BlogPostSeeder
    {
        public static void SeedData(this EntityTypeBuilder<BlogPost> entity)
        {
            entity.HasData(
                new BlogPost
                {
                    Id = 1,
                    Title = "Prvi simptomi trudnoće koje možda niste očekivali",
                    Content = "Umor, osjetljivost dojki i izostanak menstruacije najpoznatiji su znakovi trudnoće, ali mnoge žene iskuse i rane promjene raspoloženja, pojačan apetit ili metalni ukus u ustima. Tijelo se već u prvim sedmicama prilagođava novom životu koji raste u njemu.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new BlogPost
                {
                    Id = 2,
                    Title = "Kako se pripremiti za prvi prenatalni pregled",
                    Content = "Prvi prenatalni pregled obično se zakazuje između 8. i 10. sedmice trudnoće. Ljekar će provjeriti opće zdravstveno stanje, izmjeriti krvni pritisak i uraditi osnovne laboratorijske analize. Pripremite pitanja koja imate o ishrani, fizičkoj aktivnosti i simptomima koje osjećate.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-22)
                },

                new BlogPost
                {
                    Id = 3,
                    Title = "Kako njegovati kožu novorođenčeta",
                    Content = "Koža novorođenčeta je osjetljiva i sklona isušivanju. Kupanje 2–3 puta sedmično i korištenje blagih, neutralnih sapuna pomaže u očuvanju prirodne zaštitne barijere. Nakon kupanja, lagano umasirajte hipoalergijsku kremu ili ulje.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new BlogPost
                {
                    Id = 4,
                    Title = "Kako prepoznati kolike kod beba",
                    Content = "Kolike su česta pojava u prvim mjesecima života. Ako beba dugo plače, stisne nožice prema stomaku i teško se umiruje, moguće je da ima kolike. Pomaže nježna masaža trbuščića, topla pelena i podrigivanje nakon svakog obroka.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-19)
                },

                new BlogPost
                {
                    Id = 5,
                    Title = "Namirnice koje pomažu u prvom tromjesečju",
                    Content = "Hrana bogata folnom kiselinom, željezom i proteinima ključna je u ranim fazama trudnoće. Uključite zeleno lisnato povrće, integralne žitarice i jaja. Male, česte porcije pomažu kod mučnine i održavaju stabilan nivo energije.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-17)
                },
                new BlogPost
                {
                    Id = 6,
                    Title = "Zdravi obroci za dojilje",
                    Content = "Majčino mlijeko zahtijeva dodatne hranjive tvari. Fokusirajte se na svježe voće, povrće, ribe bogate omega-3 masnoćama i dovoljno vode. Izbjegavajte previše kofeina i začinjenu hranu ako utiču na bebu.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                },

                new BlogPost
                {
                    Id = 7,
                    Title = "Kako se prilagoditi prvim danima s bebom",
                    Content = "Prvih nekoliko sedmica donosi mnogo emocija – sreću, ali i nesigurnost. Odmorite kad god možete, prihvatite pomoć i ne osjećajte pritisak da sve mora biti savršeno. Beba osjeća vašu smirenost više nego išta drugo.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-13)
                },
                new BlogPost
                {
                    Id = 8,
                    Title = "Uloga partnera nakon poroda",
                    Content = "Partnerova podrška u prvim mjesecima nakon poroda izuzetno je važna. Pomaganje u hranjenju, kupanju ili jednostavno briga o kućnim poslovima pomaže majci da se fizički i emocionalno oporavi.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },

                new BlogPost
                {
                    Id = 9,
                    Title = "Baby blues ili postporođajna depresija?",
                    Content = "Blaga tuga i emocionalna nestabilnost normalni su nakon poroda i obično nestaju u roku od dvije sedmice. Ako osjećaj tuge traje duže, uz tjeskobu ili nesanicu, potražite stručnu pomoć – niste sami.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new BlogPost
                {
                    Id = 10,
                    Title = "Tehnike opuštanja u trudnoći",
                    Content = "Disanje, meditacija i lagane prenatalne vježbe mogu značajno smanjiti stres. Posvećivanje nekoliko minuta dnevno sebi pomaže i fizičkom i psihičkom zdravlju buduće mame.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-6)
                },

                new BlogPost
                {
                    Id = 11,
                    Title = "Kada beba počinje sjediti?",
                    Content = "Većina beba počinje sjediti samostalno između 6. i 8. mjeseca. Prije toga razvijaju mišiće vrata i leđa kroz igru na stomaku. Ne forsirajte proces – svaka beba ima svoj ritam razvoja.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new BlogPost
                {
                    Id = 12,
                    Title = "Podsticanje govora kod mališana",
                    Content = "Razgovarajte s bebom od rođenja. Opisujte šta radite, pjevajte i čitajte naglas. Djeca uče govor kroz interakciju, ton glasa i izraze lica roditelja.",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                }
            );
        }
    }
}
