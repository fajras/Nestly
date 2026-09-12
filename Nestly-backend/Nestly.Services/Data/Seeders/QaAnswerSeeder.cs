using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class QaAnswerSeeder
    {
        public static void SeedData(this EntityTypeBuilder<QaAnswer> entity)
        {
            entity.HasData(
                new QaAnswer { Id = 9001, QuestionId = 9001, AnswerText = "Noćna buđenja su i dalje česta u ovom uzrastu, posebno tokom rasta zuba. Ako beba brzo zaspi nakon utjehe, nema razloga za brigu.", AnsweredById = 1, CreatedAt = new DateTime(2026, 9, 6, 14, 0, 0) },
                new QaAnswer { Id = 5001, QuestionId = 5001, AnswerText = "Da, jutarnja mučnina je vrlo česta u prvom trimestru i obično prolazi do 12-14. sedmice. Pijte dosta tečnosti i jedite manje, češće obroke.", AnsweredById = 1, CreatedAt = new DateTime(2026, 9, 2, 14, 0, 0) },
                new QaAnswer { Id = 5002, QuestionId = 5002, AnswerText = "Preporučuje se oko 2 do 2.5 litre tečnosti dnevno, ovisno o aktivnosti i tjelesnoj masi.", AnsweredById = 1, CreatedAt = new DateTime(2026, 9, 4, 14, 0, 0) },
                new QaAnswer { Id = 5003, QuestionId = 5004, AnswerText = "Kada kontrakcije postanu redovne (svakih 5 minuta, traju oko 60 sekundi) u trajanju od sat vremena, ili ako pukne vodenjak, vrijeme je za bolnicu.", AnsweredById = 1, CreatedAt = new DateTime(2026, 9, 5, 14, 0, 0) },
                new QaAnswer { Id = 5004, QuestionId = 5005, AnswerText = "Da, novorođenčad spava 16-18 sati dnevno u kratkim intervalima, to je potpuno normalno.", AnsweredById = 1, CreatedAt = new DateTime(2026, 9, 3, 14, 0, 0) },
                new QaAnswer { Id = 5005, QuestionId = 5007, AnswerText = "Obično 3 glavna obroka plus 1-2 manje užine, uz nastavak dojenja/formule po potrebi.", AnsweredById = 1, CreatedAt = new DateTime(2026, 9, 1, 14, 0, 0) },
                new QaAnswer { Id = 5006, QuestionId = 5008, AnswerText = "Raspon je širok, mnoga djeca prohodaju između 12. i 18. mjeseca. Ako postoji napredak (stajanje, hodanje uz pridržavanje), nema razloga za brigu.", AnsweredById = 1, CreatedAt = new DateTime(2026, 9, 6, 14, 0, 0) },
                new QaAnswer { Id = 5007, QuestionId = 5010, AnswerText = "Uključite ga u pripreme, govorite pozitivno o bebi i zadržite što više uobičajenu rutinu kako bi se osjećalo sigurno.", AnsweredById = 1, CreatedAt = new DateTime(2026, 8, 31, 14, 0, 0) },
                new QaAnswer { Id = 5008, QuestionId = 5012, AnswerText = "Preporuka je oko 6. mjeseca života, kada beba pokazuje znakove spremnosti poput samostalnog sjedenja i interesa za hranu.", AnsweredById = 1, CreatedAt = new DateTime(2026, 8, 29, 14, 0, 0) }
            );
        }
    }
}
