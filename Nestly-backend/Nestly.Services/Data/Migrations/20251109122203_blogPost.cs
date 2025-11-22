using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Nestly.Services.Data.Migrations
{
    /// <inheritdoc />
    public partial class blogPost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "BlogPosts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Umor i izražena pospanost često su prvi znak trudnoće, čak i prije izostanka menstruacije. Mnoge žene primijete pojačanu osjetljivost ili bol u grudima, što je posljedica hormonalnih promjena. Rani hormonski disbalans može uzrokovati promjene raspoloženja, razdražljivost ili neočekivane emotivne reakcije. Kod nekih se javlja pojačan ili potpuno promijenjen apetit, kao i iznenadna odbojnost prema mirisu ili ukusu hrane koju su ranije voljele. Metalni ukus u ustima je još jedan čest, ali manje poznat simptom ranih sedmica trudnoće. Blage grčeve u donjem dijelu stomaka žene često pogrešno povežu s dolaskom menstruacije, iako mogu biti znak implantacije. Osjetljivost na mirise može biti toliko izražena da parfemi, hrana ili miris kuće postaju neugodni. Mučnina se ne javlja uvijek samo ujutro, već može trajati tokom cijelog dana ili u talasima. Pojačana potreba za mokrenjem može se pojaviti rano zbog promjena u cirkulaciji i hormona. Važno je osluškivati svoje tijelo i, uz kombinaciju više simptoma, uraditi test na trudnoću i javiti se ljekaru radi potvrde. Rano prepoznavanje simptoma omogućava pravovremenu brigu o ishrani, suplementima i zdravlju mame i bebe.", new DateTime(2025, 10, 15, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1787), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage1.png" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Prvi prenatalni pregled je ključan korak u praćenju trudnoće i obično se zakazuje između 8. i 10. sedmice. Prije pregleda zapišite datum posljednje menstruacije jer će ljekar na osnovu toga procijeniti gestacijsku dob. Sastavite listu lijekova, suplemenata i hroničnih stanja koja imate kako bi ljekar procijenio sigurnost terapije. Pripremite pitanja o prehrani, dozvoljenoj fizičkoj aktivnosti, putovanjima i simptomima koji vas brinu. Na pregledu se obično rade laboratorijske analize krvi i urina kako bi se provjerilo opće stanje organizma i vrijednosti važnih parametara. Ljekar može uraditi i ultrazvuk kako bi provjerio razvoj ploda i prisustvo otkucaja srca. Nemojte se ustručavati pričati o mučnini, umoru, strahovima ili emocionalnim promjenama, jer sve to spada u važan dio anamneze. Preporučuje se da sa sobom povedete partnera ili blisku osobu koja vam pruža podršku i može zapamtiti informacije umjesto vas. Zapišite preporuke koje dobijete, poput folne kiseline, unosa tečnosti i izbjegavanja određenih namirnica. Redovni prenatalni pregledi kasnije će pomoći da se potencijalni problemi otkriju na vrijeme i trudnoća prati sigurno. Ovaj prvi korak postavlja temelje povjerenja između vas i vašeg doktora, što je od velikog značaja za cijeli period trudnoće.", new DateTime(2025, 10, 18, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1793), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage2.png" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Koža novorođenčeta je tanka, osjetljiva i još uvijek prilagođava vanjskom okruženju. Kupanje bebe 2 do 3 puta sedmično u mlakoj vodi sasvim je dovoljno, dok je svakodnevno umivanje lica i pregiba blago vodom preporučljivo. Birajte blage, bezmirisne preparate bez agresivnih hemikalija, parabena i jakih mirisa. Nakon kupanja kožu samo lagano tapkajte mekanim peškirom umjesto trljanja kako biste izbjegli iritaciju. Posebnu pažnju obratite na pregibe vrata, pazuha, iza ušiju i pelensku regiju, gdje se znoj i vlaga zadržavaju. Za pelensku regiju koristite zaštitne kreme koje stvaraju barijeru, naročito ako primijetite crvenilo. Izbjegavajte pretjeranu upotrebu pudera ili parfemisanih proizvoda jer mogu začepiti pore i nadražiti kožu. Ako se pojave suhe mrlje ili blagi osip, često je dovoljno blago hidratantno ulje ili krema preporučena od pedijatra. Uvijek oblačite bebu u pamučnu odjeću koja diše i ne grebe kožu. Ako primijetite jače crvenilo, mjehuriće, žutilo ili bebu koja je izrazito nemirna na dodir, obavezno se javite pedijatru. Njega kože novorođenčeta je jednostavna kada se vodi računa o čistoći, blagim preparatima i izbjegavanju nepotrebnih proizvoda.", new DateTime(2025, 10, 20, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1794), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage3.png" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Kolike su čest razlog zabrinutosti roditelja u prvim mjesecima života bebe. Tipično se javljaju kao epizode intenzivnog plača koje traju više od tri sata dnevno, najmanje tri dana u sedmici. Beba često privlači nožice prema stomaku, stišće šačice i izgleda kao da je u grču. Plač se najčešće pojačava u popodnevnim ili večernjim satima, i teško ga je smiriti uobičajenim metodama. Uzrok kolika nije u potpunosti razjašnjen, ali se povezuje s nezrelim probavnim sistemom i osjetljivošću na stimulaciju. Pomažu blago nošenje bebe uspravno, kontakt koža na kožu i nježno ljuljanje. Masaža stomaka kružnim pokretima u smjeru kazaljke na satu može olakšati otpuštanje gasova. Provjerite odgovara li bebi mlijeko, položaj pri hranjenju i tempo hranjenja. Ako beba slabo napreduje, povraća, ima temperaturu ili promjene u stolici, odmah potražite ljekarsku pomoć jer to nije tipično za obične kolike. Iako su iscrpljujuće, kolike obično prolaze spontano do trećeg ili četvrtog mjeseca života. Roditeljima je važno znati da kolike nisu odraz njihove greške niti loše brige o bebi.", new DateTime(2025, 10, 21, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1796), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage4.png" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Prvo tromjesečje je period intenzivnog razvoja embriona, pa kvalitet ishrane ima veliku važnost. Folna kiselina je ključna za razvoj nervnog sistema bebe, pa u ishranu uvrstite zeleno lisnato povrće, citrusno voće i integralne žitarice. Namirnice bogate željezom, poput crvenog mesa u umjerenim količinama, jaja, leće i špinata, pomažu u sprječavanju anemije. Proteini iz jaja, piletine, ribe s niskim udjelom žive i mliječnih proizvoda važni su za rast tkiva. Ako imate mučnine, birajte manje, ali češće obroke, suhe krekere, banane i blage juhe. Hidratacija je jednako važna – pijte vodu, biljne čajeve koji su dozvoljeni u trudnoći i izbjegavajte zaslađene napitke. Smanjite unos kofeina i izbjegavajte alkohol potpuno. Neoprano povrće, sirova jaja, nepasterizirani sirevi i nedovoljno termički obrađeno meso mogu biti izvor infekcija koje su rizične u trudnoći. Ako ne možete unijeti dovoljno nutrijenata hranom, sa ginekologom dogovorite adekvatan prenatalni suplement. Slušajte svoje tijelo, ali pokušajte svaku žudnju uklopiti u što zdraviji izbor. Male promjene u prehrani već u prvom tromjesečju dugoročno doprinose zdravlju i mame i bebe.", new DateTime(2025, 10, 23, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1797), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage5.png" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Period dojenja zahtijeva dodatnu energiju i nutrijente kako bi se podržala proizvodnja mlijeka i oporavak tijela. Preporučuje se raznovrsna ishrana koja uključuje svježe voće, povrće, integralne žitarice i kvalitetne proteine. Ribe bogate omega-3 masnim kiselinama, poput lososa i sardine, doprinose razvoju bebinog mozga i nervnog sistema. Mlijeko, jogurt, sir i druge namirnice bogate kalcijem pomažu očuvanju zdravlja kostiju majke. Važno je unositi dovoljno tečnosti – voda, blagi čajevi i supice su dobar izbor. Neke bebe mogu reagovati na određene namirnice (npr. vrlo začinjenu hranu ili velike količine kofeina), pa pratite kako se beba ponaša nakon podoja. Umjesto striktnih dijeta, fokus stavite na balansirane obroke raspoređene tokom dana. Užine poput orašastih plodova, svježeg voća, integralnih krekera i humusa mogu pomoći da zadržite energiju. Preskakanje obroka može utjecati na nivo energije i raspoloženje, pa planirajte jednostavne, ali nutritivno bogate kombinacije. Ako imate dileme o određenim namirnicama ili ste vegan/vegetarijanac, konsultujte nutricionistu ili ljekara. Briga o vlastitoj ishrani je ujedno i briga o kvalitetu vremena koje provodite sa svojom bebom.", new DateTime(2025, 10, 25, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1798), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage6.png" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Prvi dani s bebom donose mješavinu sreće, umora, straha i ogromne odgovornosti. Normalno je osjećati nesigurnost, posebno ako vam je ovo prvo dijete. Pokušajte spavati kad god i koliko god beba spava, čak i ako to znači kraće drijemke tokom dana. Prihvatite pomoć porodice i prijatelja za kućne obaveze, kuhanje ili nabavku. Ne ustručavajte se govoriti partneru kako se osjećate i šta vam treba. Postepeno upoznajete ritam svoje bebe – način plača, signale gladi, umora ili nelagode. Nemojte se porediti s idealiziranim prikazima majčinstva na društvenim mrežama. Ako osjećate izraženu tugu, bezvoljnost ili se teško povezujete s bebom, razgovarajte s patronažnom sestrom ili ljekarom. Male svakodnevne rutine, poput kupanja, maženja i kontakta koža na kožu, jačaju vašu povezanost. Zapamtite da ne postoji savršena mama; postoji dovoljno dobra mama koja voli, brine se i uči iz dana u dan. Dajte sebi vremena da se prilagodite novoj ulozi i budite nježni prema sebi.", new DateTime(2025, 10, 27, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1799), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage7.png" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Nakon poroda, partner ima ključnu ulogu u pružanju emocionalne i praktične podrške. Preuzimanje dijela kućnih obaveza pomaže majci da se odmori i oporavi fizički. Partner može učestvovati u kupanju bebe, presvlačenju i uspavljivanju, čime gradi snažnu povezanost s djetetom. Otvorena komunikacija o umoru, strahovima i očekivanjima sprečava nagomilavanje tenzija. Važno je priznati da su promjene intenzivne i za partnera, ali da zajednički pristup olakšava period prilagodbe. Podrška u dojenju može biti jednostavna kao donošenje vode, jastuka ili stvaranje mirne atmosfere. Partner treba prepoznati znakove iscrpljenosti ili postporođajne depresije i ohrabriti majku da potraži pomoć. Vrijeme jedan-na-jedan s bebom osnažuje samopouzdanje partnera u brizi za dijete. Zajedničke odluke o rutini spavanja, posjetama i obavezama smanjuju nesporazume. Uloga partnera nije samo pomoć, već ravnopravan dio roditeljskog tima koji čuva dobrobit cijele porodice.", new DateTime(2025, 10, 30, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1800), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage8.png" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Baby blues je česta pojava u prvim danima nakon poroda i pogađa veliki broj majki. Manifestuje se kao plačljivost, nagle promjene raspoloženja, osjetljivost i osjećaj preopterećenosti. Ovi simptomi obično počinju nekoliko dana nakon poroda i prolaze unutar dvije sedmice. Postporođajna depresija je ozbiljnije stanje koje traje duže i može uključivati osjećaj bezvrijednosti, beznađa ili gubitak interesa za svakodnevne aktivnosti. Majka može imati poteškoće u povezivanju s bebom, osjećaj krivnje ili strah da nije dovoljno dobra. Ponekad su prisutne smetnje sna i apetita koje nisu samo posljedica zahtjeva oko bebe. Ako ovi osjećaji traju duže od dvije sedmice ili se pojačavaju, važno je potražiti stručnu pomoć. Razgovor s partnerom, porodicom i medicinskim osobljem prvi je korak ka podršci. Postoji efikasan tretman kroz psihoterapiju, podršku i, po potrebi, medikamentoznu terapiju. Traženje pomoći nije znak slabosti, nego hrabrosti i brige za sebe i svoju porodicu. Svaka majka zaslužuje podršku u ovom osjetljivom periodu, bez osude.", new DateTime(2025, 11, 1, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1801), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage9.png" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Trudnoća je predivan, ali često i stresan period zbog fizičkih promjena i briga o zdravlju bebe. Jednostavne vježbe dubokog disanja pomažu u smanjenju napetosti i smirivanju nervnog sistema. Kratke vođene meditacije ili molitva mogu pružiti osjećaj sigurnosti i fokusa. Prenatalna joga, prilagođena trudnicama, jača tijelo i poboljšava fleksibilnost bez pretjeranog opterećenja. Šetnje na svježem zraku doprinose boljoj cirkulaciji, snu i raspoloženju. Topla kupka (ne prevruća) može ublažiti bol u leđima i opustiti mišiće. Važno je smanjiti izloženost negativnim vijestima i komentarima koji pojačavaju strah. Razgovor s partnerom ili bliskom osobom o brigama često donosi olakšanje. Organizacija dana s malim ritualima opuštanja pomaže u stvaranju osjećaja kontrole. Ako se anksioznost pojačava, ometa san ili svakodnevno funkcionisanje, razgovarajte s ljekarom ili psihologom. Briga o mentalnom zdravlju u trudnoći jednako je važna kao i briga o fizičkom zdravlju.", new DateTime(2025, 11, 3, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1802), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage10.png" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Većina beba počinje samostalno sjediti između šestog i osmog mjeseca života. Prije toga prolaze kroz faze jačanja mišića vrata, leđa i core-a, posebno kroz igru na stomaku. Ponudite bebi siguran prostor na podu, umjesto da je predugo držite u ljuljama ili ležaljkama. Možete je blago poduprijeti jastucima sa strane dok uči balansirati. Nemojte forsirati sjedenje stavljanjem bebe u položaj za koji još nema snage. Svaka beba ima svoj ritam razvoja i poređenje s drugima može stvoriti nepotreban stres. Ako beba ne pokazuje pokušaje podizanja gornjeg dijela tijela ili kontrole glave nakon nekoliko mjeseci, konsultujte pedijatra. Podstičite igru s igračkama ispred bebe kako bi se prirodno naginjala naprijed i aktivirala mišiće. Pohvalite svaki mali napredak jer pozitivna interakcija jača bebino samopouzdanje. Sjedanje je važna prekretnica koja otvara nove mogućnosti istraživanja svijeta oko sebe. Uz strpljenje i sigurnu okolinu, beba će do ovog koraka doći u svoje vrijeme.", new DateTime(2025, 11, 5, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1803), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage11.png" });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "Content", "CreatedAt", "ImageUrl" },
                values: new object[] { "Razvoj govora počinje mnogo prije nego što dijete izgovori prve riječi. Već od rođenja, pričajte s bebom, imenujte predmete i opisujte šta radite tokom dana. Čitanje slikovnica, čak i vrlo jednostavnih, pomaže razvoju rječnika i pažnje. Pjevanje pjesmica i brojalica uči dijete ritmu, ponavljanju i novim riječima. Reagujte na bebine glasove, osmijeh i gestove kao da vodite pravi razgovor. Izbjegavajte prekomjernu upotrebu ekrana, posebno u najranijoj dobi, jer utiče na kvalitet interakcije. Postavljajte jednostavna pitanja poput 'Gdje je lopta?' i ohrabrite dijete da pokaže ili izgovori. Ne ispravljajte grubo pogrešan izgovor, već ponovite riječ ispravno i prirodno u rečenici. Ako dijete do 18 mjeseci ne izgovara nijednu riječ ili vrlo malo razumije, posavjetujte se s pedijatrom ili logopedom. Svako dijete napreduje svojim tempom, ali bogata, topla komunikacija uvijek je najbolji podsticaj. Roditeljska blizina, strpljenje i igra ključni su saveznici u razvoju govora.", new DateTime(2025, 11, 7, 12, 22, 2, 628, DateTimeKind.Utc).AddTicks(1805), "https://nestlystorage.blob.core.windows.net/blogpost/blogPostImage12.png" });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "BlogPosts");

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 1L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Umor, osjetljivost dojki i izostanak menstruacije najpoznatiji su znakovi trudnoće, ali mnoge žene iskuse i rane promjene raspoloženja, pojačan apetit ili metalni ukus u ustima. Tijelo se već u prvim sedmicama prilagođava novom životu koji raste u njemu.", new DateTime(2025, 10, 1, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6898) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 2L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Prvi prenatalni pregled obično se zakazuje između 8. i 10. sedmice trudnoće. Ljekar će provjeriti opće zdravstveno stanje, izmjeriti krvni pritisak i uraditi osnovne laboratorijske analize. Pripremite pitanja koja imate o ishrani, fizičkoj aktivnosti i simptomima koje osjećate.", new DateTime(2025, 10, 4, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6907) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 3L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Koža novorođenčeta je osjetljiva i sklona isušivanju. Kupanje 2–3 puta sedmično i korištenje blagih, neutralnih sapuna pomaže u očuvanju prirodne zaštitne barijere. Nakon kupanja, lagano umasirajte hipoalergijsku kremu ili ulje.", new DateTime(2025, 10, 6, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6909) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 4L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Kolike su česta pojava u prvim mjesecima života. Ako beba dugo plače, stisne nožice prema stomaku i teško se umiruje, moguće je da ima kolike. Pomaže nježna masaža trbuščića, topla pelena i podrigivanje nakon svakog obroka.", new DateTime(2025, 10, 7, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6910) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 5L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Hrana bogata folnom kiselinom, željezom i proteinima ključna je u ranim fazama trudnoće. Uključite zeleno lisnato povrće, integralne žitarice i jaja. Male, česte porcije pomažu kod mučnine i održavaju stabilan nivo energije.", new DateTime(2025, 10, 9, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6912) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 6L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Majčino mlijeko zahtijeva dodatne hranjive tvari. Fokusirajte se na svježe voće, povrće, ribe bogate omega-3 masnoćama i dovoljno vode. Izbjegavajte previše kofeina i začinjenu hranu ako utiču na bebu.", new DateTime(2025, 10, 11, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6913) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 7L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Prvih nekoliko sedmica donosi mnogo emocija – sreću, ali i nesigurnost. Odmorite kad god možete, prihvatite pomoć i ne osjećajte pritisak da sve mora biti savršeno. Beba osjeća vašu smirenost više nego išta drugo.", new DateTime(2025, 10, 13, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6914) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 8L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Partnerova podrška u prvim mjesecima nakon poroda izuzetno je važna. Pomaganje u hranjenju, kupanju ili jednostavno briga o kućnim poslovima pomaže majci da se fizički i emocionalno oporavi.", new DateTime(2025, 10, 16, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6915) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 9L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Blaga tuga i emocionalna nestabilnost normalni su nakon poroda i obično nestaju u roku od dvije sedmice. Ako osjećaj tuge traje duže, uz tjeskobu ili nesanicu, potražite stručnu pomoć – niste sami.", new DateTime(2025, 10, 18, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6917) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 10L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Disanje, meditacija i lagane prenatalne vježbe mogu značajno smanjiti stres. Posvećivanje nekoliko minuta dnevno sebi pomaže i fizičkom i psihičkom zdravlju buduće mame.", new DateTime(2025, 10, 20, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6919) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 11L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Većina beba počinje sjediti samostalno između 6. i 8. mjeseca. Prije toga razvijaju mišiće vrata i leđa kroz igru na stomaku. Ne forsirajte proces – svaka beba ima svoj ritam razvoja.", new DateTime(2025, 10, 22, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6920) });

            migrationBuilder.UpdateData(
                table: "BlogPosts",
                keyColumn: "Id",
                keyValue: 12L,
                columns: new[] { "Content", "CreatedAt" },
                values: new object[] { "Razgovarajte s bebom od rođenja. Opisujte šta radite, pjevajte i čitajte naglas. Djeca uče govor kroz interakciju, ton glasa i izraze lica roditelja.", new DateTime(2025, 10, 24, 19, 59, 52, 107, DateTimeKind.Utc).AddTicks(6921) });

            migrationBuilder.AddForeignKey(
                name: "FK_CalendarEvents_ParentProfiles_UserId",
                table: "CalendarEvents",
                column: "UserId",
                principalTable: "ParentProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
