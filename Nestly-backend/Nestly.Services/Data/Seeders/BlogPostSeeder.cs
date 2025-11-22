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
                    Content =
                        "Umor i izražena pospanost često su prvi znak trudnoće, čak i prije izostanka menstruacije. " +
                        "Mnoge žene primijete pojačanu osjetljivost ili bol u grudima, što je posljedica hormonalnih promjena. " +
                        "Rani hormonski disbalans može uzrokovati promjene raspoloženja, razdražljivost ili neočekivane emotivne reakcije. " +
                        "Kod nekih se javlja pojačan ili potpuno promijenjen apetit, kao i iznenadna odbojnost prema mirisu ili ukusu hrane koju su ranije voljele. " +
                        "Metalni ukus u ustima je još jedan čest, ali manje poznat simptom ranih sedmica trudnoće. " +
                        "Blage grčeve u donjem dijelu stomaka žene često pogrešno povežu s dolaskom menstruacije, iako mogu biti znak implantacije. " +
                        "Osjetljivost na mirise može biti toliko izražena da parfemi, hrana ili miris kuće postaju neugodni. " +
                        "Mučnina se ne javlja uvijek samo ujutro, već može trajati tokom cijelog dana ili u talasima. " +
                        "Pojačana potreba za mokrenjem može se pojaviti rano zbog promjena u cirkulaciji i hormona. " +
                        "Važno je osluškivati svoje tijelo i, uz kombinaciju više simptoma, uraditi test na trudnoću i javiti se ljekaru radi potvrde. " +
                        "Rano prepoznavanje simptoma omogućava pravovremenu brigu o ishrani, suplementima i zdravlju mame i bebe.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage1.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-25)
                },
                new BlogPost
                {
                    Id = 2,
                    Title = "Kako se pripremiti za prvi prenatalni pregled",
                    Content =
                        "Prvi prenatalni pregled je ključan korak u praćenju trudnoće i obično se zakazuje između 8. i 10. sedmice. " +
                        "Prije pregleda zapišite datum posljednje menstruacije jer će ljekar na osnovu toga procijeniti gestacijsku dob. " +
                        "Sastavite listu lijekova, suplemenata i hroničnih stanja koja imate kako bi ljekar procijenio sigurnost terapije. " +
                        "Pripremite pitanja o prehrani, dozvoljenoj fizičkoj aktivnosti, putovanjima i simptomima koji vas brinu. " +
                        "Na pregledu se obično rade laboratorijske analize krvi i urina kako bi se provjerilo opće stanje organizma i vrijednosti važnih parametara. " +
                        "Ljekar može uraditi i ultrazvuk kako bi provjerio razvoj ploda i prisustvo otkucaja srca. " +
                        "Nemojte se ustručavati pričati o mučnini, umoru, strahovima ili emocionalnim promjenama, jer sve to spada u važan dio anamneze. " +
                        "Preporučuje se da sa sobom povedete partnera ili blisku osobu koja vam pruža podršku i može zapamtiti informacije umjesto vas. " +
                        "Zapišite preporuke koje dobijete, poput folne kiseline, unosa tečnosti i izbjegavanja određenih namirnica. " +
                        "Redovni prenatalni pregledi kasnije će pomoći da se potencijalni problemi otkriju na vrijeme i trudnoća prati sigurno. " +
                        "Ovaj prvi korak postavlja temelje povjerenja između vas i vašeg doktora, što je od velikog značaja za cijeli period trudnoće.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage2.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-22)
                },
                new BlogPost
                {
                    Id = 3,
                    Title = "Kako njegovati kožu novorođenčeta",
                    Content =
                        "Koža novorođenčeta je tanka, osjetljiva i još uvijek prilagođava vanjskom okruženju. " +
                        "Kupanje bebe 2 do 3 puta sedmično u mlakoj vodi sasvim je dovoljno, dok je svakodnevno umivanje lica i pregiba blago vodom preporučljivo. " +
                        "Birajte blage, bezmirisne preparate bez agresivnih hemikalija, parabena i jakih mirisa. " +
                        "Nakon kupanja kožu samo lagano tapkajte mekanim peškirom umjesto trljanja kako biste izbjegli iritaciju. " +
                        "Posebnu pažnju obratite na pregibe vrata, pazuha, iza ušiju i pelensku regiju, gdje se znoj i vlaga zadržavaju. " +
                        "Za pelensku regiju koristite zaštitne kreme koje stvaraju barijeru, naročito ako primijetite crvenilo. " +
                        "Izbjegavajte pretjeranu upotrebu pudera ili parfemisanih proizvoda jer mogu začepiti pore i nadražiti kožu. " +
                        "Ako se pojave suhe mrlje ili blagi osip, često je dovoljno blago hidratantno ulje ili krema preporučena od pedijatra. " +
                        "Uvijek oblačite bebu u pamučnu odjeću koja diše i ne grebe kožu. " +
                        "Ako primijetite jače crvenilo, mjehuriće, žutilo ili bebu koja je izrazito nemirna na dodir, obavezno se javite pedijatru. " +
                        "Njega kože novorođenčeta je jednostavna kada se vodi računa o čistoći, blagim preparatima i izbjegavanju nepotrebnih proizvoda.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage3.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-20)
                },
                new BlogPost
                {
                    Id = 4,
                    Title = "Kako prepoznati kolike kod beba",
                    Content =
                        "Kolike su čest razlog zabrinutosti roditelja u prvim mjesecima života bebe. " +
                        "Tipično se javljaju kao epizode intenzivnog plača koje traju više od tri sata dnevno, najmanje tri dana u sedmici. " +
                        "Beba često privlači nožice prema stomaku, stišće šačice i izgleda kao da je u grču. " +
                        "Plač se najčešće pojačava u popodnevnim ili večernjim satima, i teško ga je smiriti uobičajenim metodama. " +
                        "Uzrok kolika nije u potpunosti razjašnjen, ali se povezuje s nezrelim probavnim sistemom i osjetljivošću na stimulaciju. " +
                        "Pomažu blago nošenje bebe uspravno, kontakt koža na kožu i nježno ljuljanje. " +
                        "Masaža stomaka kružnim pokretima u smjeru kazaljke na satu može olakšati otpuštanje gasova. " +
                        "Provjerite odgovara li bebi mlijeko, položaj pri hranjenju i tempo hranjenja. " +
                        "Ako beba slabo napreduje, povraća, ima temperaturu ili promjene u stolici, odmah potražite ljekarsku pomoć jer to nije tipično za obične kolike. " +
                        "Iako su iscrpljujuće, kolike obično prolaze spontano do trećeg ili četvrtog mjeseca života. " +
                        "Roditeljima je važno znati da kolike nisu odraz njihove greške niti loše brige o bebi.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage4.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-19)
                },
                new BlogPost
                {
                    Id = 5,
                    Title = "Namirnice koje pomažu u prvom tromjesečju",
                    Content =
                        "Prvo tromjesečje je period intenzivnog razvoja embriona, pa kvalitet ishrane ima veliku važnost. " +
                        "Folna kiselina je ključna za razvoj nervnog sistema bebe, pa u ishranu uvrstite zeleno lisnato povrće, citrusno voće i integralne žitarice. " +
                        "Namirnice bogate željezom, poput crvenog mesa u umjerenim količinama, jaja, leće i špinata, pomažu u sprječavanju anemije. " +
                        "Proteini iz jaja, piletine, ribe s niskim udjelom žive i mliječnih proizvoda važni su za rast tkiva. " +
                        "Ako imate mučnine, birajte manje, ali češće obroke, suhe krekere, banane i blage juhe. " +
                        "Hidratacija je jednako važna – pijte vodu, biljne čajeve koji su dozvoljeni u trudnoći i izbjegavajte zaslađene napitke. " +
                        "Smanjite unos kofeina i izbjegavajte alkohol potpuno. " +
                        "Neoprano povrće, sirova jaja, nepasterizirani sirevi i nedovoljno termički obrađeno meso mogu biti izvor infekcija koje su rizične u trudnoći. " +
                        "Ako ne možete unijeti dovoljno nutrijenata hranom, sa ginekologom dogovorite adekvatan prenatalni suplement. " +
                        "Slušajte svoje tijelo, ali pokušajte svaku žudnju uklopiti u što zdraviji izbor. " +
                        "Male promjene u prehrani već u prvom tromjesečju dugoročno doprinose zdravlju i mame i bebe.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage5.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-17)
                },
                new BlogPost
                {
                    Id = 6,
                    Title = "Zdravi obroci za dojilje",
                    Content =
                        "Period dojenja zahtijeva dodatnu energiju i nutrijente kako bi se podržala proizvodnja mlijeka i oporavak tijela. " +
                        "Preporučuje se raznovrsna ishrana koja uključuje svježe voće, povrće, integralne žitarice i kvalitetne proteine. " +
                        "Ribe bogate omega-3 masnim kiselinama, poput lososa i sardine, doprinose razvoju bebinog mozga i nervnog sistema. " +
                        "Mlijeko, jogurt, sir i druge namirnice bogate kalcijem pomažu očuvanju zdravlja kostiju majke. " +
                        "Važno je unositi dovoljno tečnosti – voda, blagi čajevi i supice su dobar izbor. " +
                        "Neke bebe mogu reagovati na određene namirnice (npr. vrlo začinjenu hranu ili velike količine kofeina), pa pratite kako se beba ponaša nakon podoja. " +
                        "Umjesto striktnih dijeta, fokus stavite na balansirane obroke raspoređene tokom dana. " +
                        "Užine poput orašastih plodova, svježeg voća, integralnih krekera i humusa mogu pomoći da zadržite energiju. " +
                        "Preskakanje obroka može utjecati na nivo energije i raspoloženje, pa planirajte jednostavne, ali nutritivno bogate kombinacije. " +
                        "Ako imate dileme o određenim namirnicama ili ste vegan/vegetarijanac, konsultujte nutricionistu ili ljekara. " +
                        "Briga o vlastitoj ishrani je ujedno i briga o kvalitetu vremena koje provodite sa svojom bebom.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage6.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-15)
                },
                new BlogPost
                {
                    Id = 7,
                    Title = "Kako se prilagoditi prvim danima s bebom",
                    Content =
                        "Prvi dani s bebom donose mješavinu sreće, umora, straha i ogromne odgovornosti. " +
                        "Normalno je osjećati nesigurnost, posebno ako vam je ovo prvo dijete. " +
                        "Pokušajte spavati kad god i koliko god beba spava, čak i ako to znači kraće drijemke tokom dana. " +
                        "Prihvatite pomoć porodice i prijatelja za kućne obaveze, kuhanje ili nabavku. " +
                        "Ne ustručavajte se govoriti partneru kako se osjećate i šta vam treba. " +
                        "Postepeno upoznajete ritam svoje bebe – način plača, signale gladi, umora ili nelagode. " +
                        "Nemojte se porediti s idealiziranim prikazima majčinstva na društvenim mrežama. " +
                        "Ako osjećate izraženu tugu, bezvoljnost ili se teško povezujete s bebom, razgovarajte s patronažnom sestrom ili ljekarom. " +
                        "Male svakodnevne rutine, poput kupanja, maženja i kontakta koža na kožu, jačaju vašu povezanost. " +
                        "Zapamtite da ne postoji savršena mama; postoji dovoljno dobra mama koja voli, brine se i uči iz dana u dan. " +
                        "Dajte sebi vremena da se prilagodite novoj ulozi i budite nježni prema sebi.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage7.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-13)
                },
                new BlogPost
                {
                    Id = 8,
                    Title = "Uloga partnera nakon poroda",
                    Content =
                        "Nakon poroda, partner ima ključnu ulogu u pružanju emocionalne i praktične podrške. " +
                        "Preuzimanje dijela kućnih obaveza pomaže majci da se odmori i oporavi fizički. " +
                        "Partner može učestvovati u kupanju bebe, presvlačenju i uspavljivanju, čime gradi snažnu povezanost s djetetom. " +
                        "Otvorena komunikacija o umoru, strahovima i očekivanjima sprečava nagomilavanje tenzija. " +
                        "Važno je priznati da su promjene intenzivne i za partnera, ali da zajednički pristup olakšava period prilagodbe. " +
                        "Podrška u dojenju može biti jednostavna kao donošenje vode, jastuka ili stvaranje mirne atmosfere. " +
                        "Partner treba prepoznati znakove iscrpljenosti ili postporođajne depresije i ohrabriti majku da potraži pomoć. " +
                        "Vrijeme jedan-na-jedan s bebom osnažuje samopouzdanje partnera u brizi za dijete. " +
                        "Zajedničke odluke o rutini spavanja, posjetama i obavezama smanjuju nesporazume. " +
                        "Uloga partnera nije samo pomoć, već ravnopravan dio roditeljskog tima koji čuva dobrobit cijele porodice.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage8.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-10)
                },
                new BlogPost
                {
                    Id = 9,
                    Title = "Baby blues ili postporođajna depresija?",
                    Content =
                        "Baby blues je česta pojava u prvim danima nakon poroda i pogađa veliki broj majki. " +
                        "Manifestuje se kao plačljivost, nagle promjene raspoloženja, osjetljivost i osjećaj preopterećenosti. " +
                        "Ovi simptomi obično počinju nekoliko dana nakon poroda i prolaze unutar dvije sedmice. " +
                        "Postporođajna depresija je ozbiljnije stanje koje traje duže i može uključivati osjećaj bezvrijednosti, beznađa ili gubitak interesa za svakodnevne aktivnosti. " +
                        "Majka može imati poteškoće u povezivanju s bebom, osjećaj krivnje ili strah da nije dovoljno dobra. " +
                        "Ponekad su prisutne smetnje sna i apetita koje nisu samo posljedica zahtjeva oko bebe. " +
                        "Ako ovi osjećaji traju duže od dvije sedmice ili se pojačavaju, važno je potražiti stručnu pomoć. " +
                        "Razgovor s partnerom, porodicom i medicinskim osobljem prvi je korak ka podršci. " +
                        "Postoji efikasan tretman kroz psihoterapiju, podršku i, po potrebi, medikamentoznu terapiju. " +
                        "Traženje pomoći nije znak slabosti, nego hrabrosti i brige za sebe i svoju porodicu. " +
                        "Svaka majka zaslužuje podršku u ovom osjetljivom periodu, bez osude.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage9.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-8)
                },
                new BlogPost
                {
                    Id = 10,
                    Title = "Tehnike opuštanja u trudnoći",
                    Content =
                        "Trudnoća je predivan, ali često i stresan period zbog fizičkih promjena i briga o zdravlju bebe. " +
                        "Jednostavne vježbe dubokog disanja pomažu u smanjenju napetosti i smirivanju nervnog sistema. " +
                        "Kratke vođene meditacije ili molitva mogu pružiti osjećaj sigurnosti i fokusa. " +
                        "Prenatalna joga, prilagođena trudnicama, jača tijelo i poboljšava fleksibilnost bez pretjeranog opterećenja. " +
                        "Šetnje na svježem zraku doprinose boljoj cirkulaciji, snu i raspoloženju. " +
                        "Topla kupka (ne prevruća) može ublažiti bol u leđima i opustiti mišiće. " +
                        "Važno je smanjiti izloženost negativnim vijestima i komentarima koji pojačavaju strah. " +
                        "Razgovor s partnerom ili bliskom osobom o brigama često donosi olakšanje. " +
                        "Organizacija dana s malim ritualima opuštanja pomaže u stvaranju osjećaja kontrole. " +
                        "Ako se anksioznost pojačava, ometa san ili svakodnevno funkcionisanje, razgovarajte s ljekarom ili psihologom. " +
                        "Briga o mentalnom zdravlju u trudnoći jednako je važna kao i briga o fizičkom zdravlju.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage10.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-6)
                },
                new BlogPost
                {
                    Id = 11,
                    Title = "Kada beba počinje sjediti?",
                    Content =
                        "Većina beba počinje samostalno sjediti između šestog i osmog mjeseca života. " +
                        "Prije toga prolaze kroz faze jačanja mišića vrata, leđa i core-a, posebno kroz igru na stomaku. " +
                        "Ponudite bebi siguran prostor na podu, umjesto da je predugo držite u ljuljama ili ležaljkama. " +
                        "Možete je blago poduprijeti jastucima sa strane dok uči balansirati. " +
                        "Nemojte forsirati sjedenje stavljanjem bebe u položaj za koji još nema snage. " +
                        "Svaka beba ima svoj ritam razvoja i poređenje s drugima može stvoriti nepotreban stres. " +
                        "Ako beba ne pokazuje pokušaje podizanja gornjeg dijela tijela ili kontrole glave nakon nekoliko mjeseci, konsultujte pedijatra. " +
                        "Podstičite igru s igračkama ispred bebe kako bi se prirodno naginjala naprijed i aktivirala mišiće. " +
                        "Pohvalite svaki mali napredak jer pozitivna interakcija jača bebino samopouzdanje. " +
                        "Sjedanje je važna prekretnica koja otvara nove mogućnosti istraživanja svijeta oko sebe. " +
                        "Uz strpljenje i sigurnu okolinu, beba će do ovog koraka doći u svoje vrijeme.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage11.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-4)
                },
                new BlogPost
                {
                    Id = 12,
                    Title = "Podsticanje govora kod mališana",
                    Content =
                        "Razvoj govora počinje mnogo prije nego što dijete izgovori prve riječi. " +
                        "Već od rođenja, pričajte s bebom, imenujte predmete i opisujte šta radite tokom dana. " +
                        "Čitanje slikovnica, čak i vrlo jednostavnih, pomaže razvoju rječnika i pažnje. " +
                        "Pjevanje pjesmica i brojalica uči dijete ritmu, ponavljanju i novim riječima. " +
                        "Reagujte na bebine glasove, osmijeh i gestove kao da vodite pravi razgovor. " +
                        "Izbjegavajte prekomjernu upotrebu ekrana, posebno u najranijoj dobi, jer utiče na kvalitet interakcije. " +
                        "Postavljajte jednostavna pitanja poput 'Gdje je lopta?' i ohrabrite dijete da pokaže ili izgovori. " +
                        "Ne ispravljajte grubo pogrešan izgovor, već ponovite riječ ispravno i prirodno u rečenici. " +
                        "Ako dijete do 18 mjeseci ne izgovara nijednu riječ ili vrlo malo razumije, posavjetujte se s pedijatrom ili logopedom. " +
                        "Svako dijete napreduje svojim tempom, ali bogata, topla komunikacija uvijek je najbolji podsticaj. " +
                        "Roditeljska blizina, strpljenje i igra ključni su saveznici u razvoju govora.",
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage12.png",
                    AuthorId = 1,
                    CreatedAt = DateTime.UtcNow.AddDays(-2)
                }
            );
        }
    }
}
