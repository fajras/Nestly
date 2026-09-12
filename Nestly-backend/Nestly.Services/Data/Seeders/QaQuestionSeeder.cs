using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class QaQuestionSeeder
    {
        public static void SeedData(this EntityTypeBuilder<QaQuestion> entity)
        {
            entity.HasData(
                new QaQuestion { Id = 9001, QuestionText = "Beba od 6 mjeseci se budi nekoliko puta tokom noći, da li je to normalno?", AskedById = 1, CreatedAt = new DateTime(2026, 9, 5, 9, 0, 0) },
                new QaQuestion { Id = 9002, QuestionText = "Koliko obroka čvrste hrane dnevno je preporučeno za bebu od 6 mjeseci?", AskedById = 1, CreatedAt = new DateTime(2026, 9, 8, 9, 0, 0) },
                new QaQuestion { Id = 5001, QuestionText = "Da li je normalno imati jutarnju mučninu u 10. sedmici trudnoće?", AskedById = 2, CreatedAt = new DateTime(2026, 9, 1, 9, 0, 0) },
                new QaQuestion { Id = 5002, QuestionText = "Koliko je vode potrebno piti dnevno u drugom trimestru?", AskedById = 3, CreatedAt = new DateTime(2026, 9, 3, 9, 0, 0) },
                new QaQuestion { Id = 5003, QuestionText = "Osjećam bolove u leđima u 36. sedmici, je li to normalno?", AskedById = 4, CreatedAt = new DateTime(2026, 9, 5, 9, 0, 0) },
                new QaQuestion { Id = 5004, QuestionText = "Kada tačno trebam ići u bolnicu kada počnu trudovi?", AskedById = 5, CreatedAt = new DateTime(2026, 9, 4, 9, 0, 0) },
                new QaQuestion { Id = 5005, QuestionText = "Beba puno spava, da li je to normalno u prvom mjesecu?", AskedById = 6, CreatedAt = new DateTime(2026, 9, 2, 9, 0, 0) },
                new QaQuestion { Id = 5006, QuestionText = "Kada mogu početi uvoditi kašice bebi?", AskedById = 7, CreatedAt = new DateTime(2026, 9, 6, 9, 0, 0) },
                new QaQuestion { Id = 5007, QuestionText = "Koliko obroka dnevno je preporučeno za bebu od 8 mjeseci?", AskedById = 8, CreatedAt = new DateTime(2026, 8, 31, 9, 0, 0) },
                new QaQuestion { Id = 5008, QuestionText = "Beba od 13 mjeseci još ne hoda sama, da li da se zabrinem?", AskedById = 9, CreatedAt = new DateTime(2026, 9, 5, 9, 0, 0) },
                new QaQuestion { Id = 5009, QuestionText = "Koliko sati sna je normalno za dijete od 19 mjeseci?", AskedById = 10, CreatedAt = new DateTime(2026, 9, 6, 9, 0, 0) },
                new QaQuestion { Id = 5010, QuestionText = "Kako pripremiti starije dijete na dolazak bebe?", AskedById = 11, CreatedAt = new DateTime(2026, 8, 30, 9, 0, 0) },
                new QaQuestion { Id = 5011, QuestionText = "Koje namirnice je najbolje izbjegavati tokom trudnoće?", AskedById = 2, CreatedAt = new DateTime(2026, 9, 7, 9, 0, 0) },
                new QaQuestion { Id = 5012, QuestionText = "Kada uvesti čvrstu hranu bebi?", AskedById = 1, CreatedAt = new DateTime(2026, 8, 28, 9, 0, 0) }
            );
        }
    }
}
