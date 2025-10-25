using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class WeeklyAdviceSeeder
    {
        public static void SeedData(this EntityTypeBuilder<WeeklyAdvice> entity)
        {
            entity.HasData(
                new WeeklyAdvice { Id = 1, WeekNumber = 1, AdviceText = "Prva sedmica trudnoće računa se od prvog dana posljednje menstruacije. Iako se oplodnja još nije dogodila, tvoje tijelo se već priprema za stvaranje novog života. Počni uzimati folnu kiselinu i jedi hranu bogatu željezom i kalcijem. Pokušaj izbjegavati stres i osiguraj dovoljno sna. Ako planiraš trudnoću, sada je savršeno vrijeme da uvedeš zdrave navike koje će trajati kroz naredne mjesece." },
                new WeeklyAdvice { Id = 2, WeekNumber = 2, AdviceText = "Ovo je tjedan kada tvoje tijelo ovulira i priprema se za moguću oplodnju. Održavaj uravnoteženu prehranu, povećaj unos voća, povrća i vode. Izbjegavaj alkohol, cigarete i brzu hranu jer mogu utjecati na kvalitetu jajne stanice. Odmaraj se dovoljno i pokušaj smanjiti stres kroz šetnju ili lagano istezanje. Tvoje tijelo radi važan posao i zaslužuje pažnju i brigu." },
                new WeeklyAdvice { Id = 3, WeekNumber = 3, AdviceText = "U ovoj sedmici oplodnja se vjerojatno dogodila, i embrion započinje svoje putovanje prema maternici. Moguće je da još ne znaš da si trudna, ali promjene već počinju. Održavaj hidraciju i izbjegavaj lijekove bez konsultacije s doktorom. Folna kiselina sada ima ključnu ulogu u razvoju nervnog sistema bebe. Iako još ne vidiš promjene, tvoje tijelo već radi čuda." },
                new WeeklyAdvice { Id = 4, WeekNumber = 4, AdviceText = "Čestitamo! Tvoj test na trudnoću može pokazati pozitivan rezultat. Hormoni trudnoće počinju naglo rasti, pa možeš osjećati blagu mučninu, osjetljivost dojki i umor. Jednostavni, česti obroci pomoći će ti da se osjećaš bolje. Izbjegavaj kofein i počni planirati svoj prvi prenatalni pregled. Uživaj u ovom posebnom trenutku — tek počinje jedno divno putovanje." },
                new WeeklyAdvice { Id = 5, WeekNumber = 5, AdviceText = "Tvoje tijelo sada stvara osnovu za posteljicu, a embrion raste brže nego ikad. Možda osjećaš mučninu, česte promjene raspoloženja ili blagu vrtoglavicu. Odmaraj se i jedi male, nutritivno bogate obroke. Ako ti neki mirisi smetaju, pokušaj izbjegavati jake arome. Ovo je vrijeme kada tvoje tijelo uči kako podržati novi život — budi nježna prema sebi." },
                new WeeklyAdvice { Id = 6, WeekNumber = 6, AdviceText = "U šestoj sedmici beba je veličine zrna graška i počinju se formirati crte lica. Hormoni trudnoće mogu uzrokovati umor i promjene apetita. Spavaj kad god osjetiš potrebu i unosi dovoljno tekućine. Izbjegavaj sirove namirnice i brini o higijeni hrane. Lagane šetnje na svježem zraku mogu pomoći da se osjećaš bolje i odmornije." },
                new WeeklyAdvice { Id = 7, WeekNumber = 7, AdviceText = "Srce tvoje bebe kuca stabilnim ritmom, a razvoj organa se ubrzava. Možda ćeš osjetiti češće mokrenje i promjene raspoloženja. Jedi često, ali u manjim porcijama i drži grickalice pri ruci. Unosi dovoljno kalcija i magnezija kroz mliječne proizvode ili zamjene. Ako ti se pojavi mučnina, pokušaj piti male gutljaje vode tokom dana." },
                new WeeklyAdvice { Id = 8, WeekNumber = 8, AdviceText = "Vrijeme je za tvoj prvi pregled kod ginekologa! Doktor će potvrditi trudnoću i napraviti osnovne testove. Tvoje tijelo prolazi kroz hormonalne promjene, pa su emotivne oscilacije normalne. Nastavi s unosom folne kiseline i zdravom prehranom. Počni razmišljati o dnevnim ritualima opuštanja — tvoj mir sada znači i mir tvoje bebe." },
                new WeeklyAdvice { Id = 9, WeekNumber = 9, AdviceText = "Bebini prsti i nožni prsti sada postaju vidljivi, a glava joj se oblikuje. Tvoje tijelo se širi, pa možeš osjetiti nelagodu u grudima ili blage grčeve. Lagano istezanje i topla kupka mogu pomoći. Ne zaboravi piti dovoljno vode i jesti vlaknaste namirnice za bolju probavu. Emotivne promjene su sasvim prirodne — daj sebi dozvolu da osjećaš sve." },
                new WeeklyAdvice { Id = 10, WeekNumber = 10, AdviceText = "Tvoje dijete je sada veličine šljive i svi osnovni organi su formirani. Možda primjećuješ promjene u kosi i koži zbog hormona. Nastavi s balansiranom ishranom i izbjegavaj previše slatkog. Kratke šetnje i svjež zrak pomoći će ti da se osjećaš energičnije. Uživaj u svakoj maloj promjeni jer tvoje tijelo radi nevjerovatan posao." },
                new WeeklyAdvice { Id = 11, WeekNumber = 11, AdviceText = "Mučnine bi sada mogle početi slabiti, a energija se polako vraća. Beba je duga oko 4 cm i kreće se unutar maternice. U prehranu uvedi više svježeg povrća, voća i integralnih žitarica. Izbjegavaj duže stajanje i odmori noge kad god možeš. Ako planiraš put, obavezno se posavjetuj s ljekarom." },
                new WeeklyAdvice { Id = 12, WeekNumber = 12, AdviceText = "Završava prvo tromjesečje – čestitamo! Rizik od pobačaja sada je značajno manji. Mnoge žene osjećaju olakšanje i stabilnije raspoloženje. Nastavi s blagim fizičkim aktivnostima poput joge ili laganih šetnji. Tvoje tijelo sada sjaji i pokazuje prve znakove trudnoće." },
                new WeeklyAdvice { Id = 13, WeekNumber = 13, AdviceText = "Ulaziš u drugo tromjesečje – najugodniji dio trudnoće. Beba raste brzo, a ti se vjerojatno osjećaš energičnije. Počni planirati zdravije obroke bogate proteinima i vlaknima. Hidratacija je i dalje važna, pa uvijek imaj bocu vode uz sebe. Uživaj u ovom razdoblju dok se tvoje tijelo prilagođava s lakoćom." },
                new WeeklyAdvice { Id = 14, WeekNumber = 14, AdviceText = "Bebino lice se formira, a počinje i razvoj fine kose na tijelu. Možda primjećuješ povećan apetit — iskoristi to da unosiš više zdravih namirnica. Ako imaš problema sa spavanjem, pokušaj spavati na lijevom boku. Emocionalna stabilnost se vraća, pa je ovo idealno vrijeme za blagu fizičku aktivnost. Prati svoje tijelo, ono zna šta mu treba." },
                new WeeklyAdvice { Id = 15, WeekNumber = 15, AdviceText = "Tvoje tijelo se sada prilagođava rastu maternice. Možda primjećuješ tamnije linije na stomaku – to je normalna pigmentacija. Beba reaguje na zvukove, pa joj možeš pričati ili pustiti laganu muziku. U prehranu dodaj namirnice bogate željezom i vitaminom D. Nastavi se kretati, ali izbjegavaj teške vježbe." },
                new WeeklyAdvice { Id = 16, WeekNumber = 16, AdviceText = "Beba sada teži oko 100 grama i kreće se aktivno. Možda osjetiš prve lagane pokrete koji podsjećaju na leptiriće. Uživaj u tom osjećaju povezanosti s bebom. Ako se javlja bol u leđima, koristi jastuke za potporu i spavaj na boku. Slušaj svoje tijelo i odmori kada ti zatreba." },
                new WeeklyAdvice { Id = 17, WeekNumber = 17, AdviceText = "Tvoje srce pumpa više krvi kako bi podržalo bebu, pa se možeš osjećati umorno. Jedi male, nutritivne obroke i izbjegavaj duga stajanja. Beba počinje čuti tvoj glas, pa s njom slobodno razgovaraj. Uključi partnera u pripreme i dijelite trenutke zajedno. Osjećaj umora je normalan – odmori su sada tvoja obaveza." },
                new WeeklyAdvice { Id = 18, WeekNumber = 18, AdviceText = "Vrijeme je za detaljan ultrazvuk! Ljekar će provjeriti razvoj organa tvoje bebe. Možda osjetiš bol u leđima zbog promjene držanja, pa pazi na ergonomiju. Počni razmišljati o trudničkim vježbama koje jačaju mišiće zdjelice. Uživaj u svakom pokretu koji osjetiš – tvoja beba raste i jača svakog dana." },
                new WeeklyAdvice { Id = 19, WeekNumber = 19, AdviceText = "Tvoja beba sada ima razvijene organe i počinje akumulirati masno tkivo. Možda se javlja žgaravica, pa jedi manje obroke češće. Pij dovoljno tekućine, ali izbjegavaj gazirane napitke. Masaža nogu može pomoći kod oticanja. Nastavi redovno šetati i održavaj pozitivan duh." },
                new WeeklyAdvice { Id = 20, WeekNumber = 20, AdviceText = "Polovina trudnoće – bravo! Beba sada teži oko 300 grama i ti si vjerovatno već osjetila prve jače pokrete. Prati držanje tijela i koristi jastuk za potporu. Ovo je pravo vrijeme da se pripremiš na promjene rasporeda sna. Uživaj u ovom posebnom periodu, jer svaki dan tvoje dijete postaje jače." },
                new WeeklyAdvice { Id = 21, WeekNumber = 21, AdviceText = "Bebine kosti postaju čvršće, a pokreti energičniji. Osjećaj težine u nogama može se pojačati – zato odmaraj s nogama podignutim. Jedi dovoljno kalcija i bjelančevina. Hidratacija i lagane šetnje pomažu cirkulaciji. Održavaj vedar duh i povezanost s bebom." },
                new WeeklyAdvice { Id = 22, WeekNumber = 22, AdviceText = "Beba već može čuti zvukove iz okoline i reagirati na glasove. Ako osjećaš bol u leđima, istezanje i topla kupka mogu pomoći. U ovom periodu možeš početi birati ime za bebu – uživaj u tom procesu. Jedi hranu bogatu vlaknima i pij dovoljno vode. Emocionalna stabilnost sada ti pomaže da uživaš u trudnoći punim plućima." },
                new WeeklyAdvice { Id = 23, WeekNumber = 23, AdviceText = "Tvoje tijelo nosi dodatnu težinu, pa pazi na pravilno držanje. Možeš osjetiti žgaravicu ili otečene zglobove, što je uobičajeno. Lagane šetnje i masaže pomažu u opuštanju. Beba već ima razvijen ritam sna i budnosti. Prati signale svog tijela i ne forsiraj se." },
                new WeeklyAdvice { Id = 24, WeekNumber = 24, AdviceText = "Ulaziš u šesti mjesec trudnoće i beba počinje prepoznavati tvoj glas. Održavaj unos tekućine i hranu bogatu proteinima. Ako se javlja grč u nogama, dodaj više magnezija u prehranu. Pokušaj spavati na lijevom boku radi bolje cirkulacije. Osloni se na podršku partnera i porodice." },
                new WeeklyAdvice { Id = 25, WeekNumber = 25, AdviceText = "Bebin sluh se poboljšava, pa joj možeš pjevati i pričati. Povećava se potreba za željezom – unosi zeleno povrće i orahe. Ako imaš problema sa spavanjem, kreiraj mirnu večernju rutinu. Započni s pripremama za dolazak bebe polako i bez stresa. Svaka tvoja emocija utiče i na nju, zato se fokusiraj na mir i radost." },
                new WeeklyAdvice { Id = 26, WeekNumber = 26, AdviceText = "Tvoje tijelo sada osjeća težinu maternice i promjene držanja. Pazi na stopala i nosi udobnu obuću. U prehranu uvedi više vlakana kako bi spriječila zatvor. Opuštanje uz muziku i disanje pomoći će kod napetosti. Svaki dan približava te trenutku susreta s bebom." },
                new WeeklyAdvice { Id = 27, WeekNumber = 27, AdviceText = "Treće tromjesečje počinje! Beba brzo dobija na težini i ti možeš osjećati češću potrebu za odmorom. Redovno se proteži i mijenjaj položaj tijela. Jedi lagano i često kako bi spriječila žgaravicu. Tvoj trudnički sjaj je sada u punom sjaju – ponosi se sobom." },
                new WeeklyAdvice { Id = 28, WeekNumber = 28, AdviceText = "Vrijeme je za kontrolu nivoa šećera u krvi. Ako osjećaš umor, ne oklijevaj da odmoriš. Pij dovoljno vode i jedi sezonsko voće. Beba sada prepoznaje svjetlost i reaguje na dodir kroz stomak. Opusti se uz laganu muziku ili meditaciju." },
                new WeeklyAdvice { Id = 29, WeekNumber = 29, AdviceText = "Tvoje tijelo se priprema za porod, pa možeš osjetiti blage kontrakcije. Povećaj unos proteina i kalcija. Masaža leđa može pomoći kod nelagode. Razgovaraj s partnerom o planovima za dolazak bebe. Svaki pokret tvoje bebe sada je znak njenog zdravog razvoja." },
                new WeeklyAdvice { Id = 30, WeekNumber = 30, AdviceText = "Trbuh je sve izraženiji, a beba zauzima više prostora. Diši polako i duboko da olakšaš pritisak na pluća. U prehranu dodaj više vlakana i tekućine. Počni pripremati torbu za porod i osnovne stvari za bebu. Uživaj u svakom pokretu i povezanosti koju osjećate." },
                new WeeklyAdvice { Id = 31, WeekNumber = 31, AdviceText = "Beba sada prepoznaje tvoj glas i reaguje na emocije. Moguća je nesanica zbog veličine stomaka – koristi jastuke za udobnost. Jedi manje obroke, ali češće. Pokušaj svaki dan šetati i istezati se. Održavaj kontakt s doktorom i ne zanemaruj znakove umora." },
                new WeeklyAdvice { Id = 32, WeekNumber = 32, AdviceText = "Beba brzo dobija na težini, a tvoje tijelo se priprema za porod. Odmaraj često i pazi na unos soli kako bi spriječila oticanje. Diši polako i koristi vježbe disanja. Ako još nisi, počni planirati porodilište. Uživaj u svakom trenutku trudnoće – kraj se bliži!" },
                new WeeklyAdvice { Id = 33, WeekNumber = 33, AdviceText = "Maternica se dodatno širi, pa možeš osjećati pritisak na leđa i noge. Lagano istezanje i masaža pomažu u opuštanju. Jedi lagane obroke i izbjegavaj teške začine. Tvoja beba sada vježba disanje u stomaku. Polako pripremaj svoj dom za njen dolazak." },
                new WeeklyAdvice
                {
                    Id = 34,
                    WeekNumber = 34,
                    AdviceText = "Beba zauzima gotovo cijelu maternicu i njeni pokreti su sada snažniji nego ikad. Možda se pojavi nesanica jer je sve teže pronaći udoban položaj za spavanje. Pokušaj spavati na lijevom boku i koristi jastuke za potporu stomaku i leđima. U ovoj fazi često se javlja umor, pa slušaj svoje tijelo i odmori kad god osjetiš potrebu. Uživaj u svakom trenutku trudnoće i mirno se pripremaj za dolazak svoje bebe."
                },
                new WeeklyAdvice
                {
                    Id = 35,
                    WeekNumber = 35,
                    AdviceText = "Beba sada teži oko 2,5 kilograma i priprema se za završne faze razvoja. Tvoje tijelo se može osjećati teže i umornije, pa se više odmaraj. Ako imaš problema sa spavanjem, pokušaj spavati na lijevom boku i koristi dodatne jastuke. Pripremi torbu za porod i provjeri plan transporta do bolnice. Ovo je dobro vrijeme da se posvetiš sebi i mirno dočekaš posljednje sedmice trudnoće."
                },
                new WeeklyAdvice
                {
                    Id = 36,
                    WeekNumber = 36,
                    AdviceText = "Tvoja beba je gotovo spremna za susret s tobom. Možda primjećuješ češće kontrakcije i osjećaj težine u karlici. Diši duboko i pokušaj ostati aktivna kroz lagane šetnje. Jedi lagano i izbjegavaj masnu hranu kako bi smanjila žgaravicu. Posljednji pregledi su važni, pa ih redovno obavljaj i slušaj savjete svog ljekara."
                },
                new WeeklyAdvice
                {
                    Id = 37,
                    WeekNumber = 37,
                    AdviceText = "Zvanično si u terminskoj trudnoći – beba može doći bilo kada! Ako osjećaš pritisak u donjem dijelu stomaka, to znači da se spušta u položaj za porod. Pripremi torbu, dokumente i neophodne stvari za bolnicu. Pokušaj ostati smirena i odmaraj više tokom dana. Svaka kontrakcija sada te korak po korak vodi prema najljepšem trenutku – rođenju tvoje bebe."
                },
                new WeeklyAdvice
                {
                    Id = 38,
                    WeekNumber = 38,
                    AdviceText = "Tvoje tijelo se potpuno priprema za porod, a beba sada vježba disanje i sisanje u stomaku. Možeš osjetiti bol u leđima i umor – to je normalno. Slušaj svoje tijelo i ne forsiraj se. Pokušaj se opustiti uz topli tuš, laganu muziku ili meditaciju. Tvoj mir sada znači i mir tvoje bebe koja uskoro stiže."
                },
                new WeeklyAdvice
                {
                    Id = 39,
                    WeekNumber = 39,
                    AdviceText = "Sve je spremno za porođaj! Beba je gotovo iste veličine kao pri rođenju i zauzima svoj položaj. Možda osjećaš učestale Braxton-Hicks kontrakcije, znak da se tvoje tijelo priprema. Pij dovoljno vode i jedi lagano kako bi imala snage za porođaj. Emocije su sada jake – strpljenje i smirenost su tvoja najveća snaga."
                },
                new WeeklyAdvice
                {
                    Id = 40,
                    WeekNumber = 40,
                    AdviceText = "Stigla si do kraja trudnoće – čestitamo! Svaki dan sada može biti onaj kada ćeš upoznati svoju bebu. Odmori se, jedi lagano i pokušaj se opustiti što više možeš. Ako još ne osjećaš kontrakcije, to je u redu – svaka trudnoća ima svoj ritam. Uživaj u posljednjim trenucima trudnoće i pripremi se za najposebniji susret u svom životu."
                }
            );
        }
    }
}
