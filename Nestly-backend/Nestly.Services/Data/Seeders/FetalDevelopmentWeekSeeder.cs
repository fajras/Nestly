using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nestly.Model.Entity;

namespace Nestly.Services.Data.Seeders
{
    public static class FetalDevelopmentWeekSeeder
    {
        public static void SeedData(this EntityTypeBuilder<FetalDevelopmentWeek> entity)
        {
            entity.HasData(
                new FetalDevelopmentWeek
                {
                    Id = 1,
                    WeekNumber = 1,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development1.png",
                    BabyDevelopment = "Trudnoća se računa od posljednje menstruacije. Oplodnja se još nije dogodila; tijelo se priprema za ovulaciju i zadebljava sluznicu materice.",
                    MotherChanges = "Još nema specifičnih simptoma trudnoće. Moguće su uobičajene PMS promjene raspoloženja, nadutost i osjetljivost dojki."
                },
                new FetalDevelopmentWeek
                {
                    Id = 2,
                    WeekNumber = 2,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development1.png",
                    BabyDevelopment = "Blizu sredine ciklusa dešava se ovulacija. Ako spermij oplodi jajnu ćeliju, nastaje zigota koja kreće ka materici.",
                    MotherChanges = "Još nema trudnoćom uzrokovanih promjena. Bazalna temperatura može porasti nakon ovulacije; neki primijete pojačan cervikalni sekret."
                },
                new FetalDevelopmentWeek
                {
                    Id = 3,
                    WeekNumber = 3,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development1.png",
                    BabyDevelopment = "Oplođena jajna ćelija (zigota) se dijeli i formira blastocistu. Počinje implantacija u sluznicu materice.",
                    MotherChanges = "Moguće je blago tačkasto krvarenje usljed implantacije. Hormoni hCG i progesteron polako rastu i mogu izazvati umor."
                },
                new FetalDevelopmentWeek
                {
                    Id = 4,
                    WeekNumber = 4,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development4.png",
                    BabyDevelopment = "Blastocista se čvrsto ugnijezdila. Formiraju se amnion i žumančana vreća; embrij je veličine sjemenke maka.",
                    MotherChanges = "Test na trudnoću može biti pozitivan. Česti rani znaci su umor, osjetljive dojke i blago kašnjenje menstruacije."
                },
                new FetalDevelopmentWeek
                {
                    Id = 5,
                    WeekNumber = 5,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development5.png",
                    BabyDevelopment = "Formira se srčana cijev i neuralna cijev iz koje nastaju mozak i kičma. Embrij je dug oko 2 mm.",
                    MotherChanges = "Mučnine i češće mokrenje su uobičajeni. Hormonske promjene mogu izazvati promjene raspoloženja i osjetljivost mirisa."
                },
                new FetalDevelopmentWeek
                {
                    Id = 6,
                    WeekNumber = 6,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development6.png",
                    BabyDevelopment = "Srce počinje kucati i moguće je vidjeti aktivnost na UZ. Pojavljuju se pupoljci ruku i nogu; formiraju se oči i uši.",
                    MotherChanges = "Mučnina ujutro može biti izraženija. Umor, napetost dojki i učestalo mokrenje su česti."
                },
                new FetalDevelopmentWeek
                {
                    Id = 7,
                    WeekNumber = 7,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development7.png",
                    BabyDevelopment = "Mozak se ubrzano razvija; srce ima četiri šupljine. Lice dobija prepoznatljive crte, a udovi se izdužuju.",
                    MotherChanges = "Zbog hormona moguća su pojačana salivacija i gađenje prema mirisima. Umor i mučnine obično traju."
                },
                new FetalDevelopmentWeek
                {
                    Id = 8,
                    WeekNumber = 8,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development8.png",
                    BabyDevelopment = "Prsti su još spojeni opnama; formiraju se pluća, jetra i probavni trakt. Embrij se sve više pokreće.",
                    MotherChanges = "Mučnine i promjene raspoloženja su česte. Moguće nadimanje, blagi grčevi i osjetljivost dojki."
                },
                new FetalDevelopmentWeek
                {
                    Id = 9,
                    WeekNumber = 9,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development9.png",
                    BabyDevelopment = "Nestaje embrionalni ‘rep’; formiraju se kapci, vrh nosa i ušni listići. Fetus dobija ljudskiji izgled.",
                    MotherChanges = "Žgaravica i promjene apetita se mogu pojaviti. Umor je i dalje izražen, a mokrenje češće."
                },
                new FetalDevelopmentWeek
                {
                    Id = 10,
                    WeekNumber = 10,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development10.png",
                    BabyDevelopment = "Embrij postaje fetus; svi glavni organi su formirani i sazrijevaju. Počinju pokreti koje još ne osjećaš.",
                    MotherChanges = "Mučnine mogu početi popuštati. Stomak se lagano zaokružuje kako materica raste iznad stidne kosti."
                },
                new FetalDevelopmentWeek
                {
                    Id = 11,
                    WeekNumber = 11,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development11.png",
                    BabyDevelopment = "Formiraju se nokti i zubi unutar desni. Vanjske genitalije se razvijaju, iako je spol još teško vidjeti UZ-om.",
                    MotherChanges = "Energija se postepeno vraća. Moguće su nadutost i promjene u probavi."
                },
                new FetalDevelopmentWeek
                {
                    Id = 12,
                    WeekNumber = 12,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development12.png",
                    BabyDevelopment = "Fetus vježba refleksne pokrete; bubrezi počinju stvarati urin. Proporcije glave i tijela se izjednačavaju.",
                    MotherChanges = "Mučnine često slabe krajem prvog trimestra. Možda primijetiš manje umora i stabilnije raspoloženje."
                },
                new FetalDevelopmentWeek
                {
                    Id = 13,
                    WeekNumber = 13,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development13.png",
                    BabyDevelopment = "Kosti počinju očvrsnuti (osifikacija). Fetus može sisati palac i gutati plodovu vodu.",
                    MotherChanges = "Početak drugog trimestra često donosi više energije. Moguća je žgaravica i začepljen nos."
                },
                new FetalDevelopmentWeek
                {
                    Id = 14,
                    WeekNumber = 14,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development14.png",
                    BabyDevelopment = "Na koži se pojavljuje lanugo (fina dlaka). Jetra i slezena rade na stvaranju krvnih ćelija.",
                    MotherChanges = "Raspoloženje se stabilizuje. Možda primijetiš promjene kože i sjajniju kosu."
                },
                new FetalDevelopmentWeek
                {
                    Id = 15,
                    WeekNumber = 15,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development15.png",
                    BabyDevelopment = "Na UZ se jasnije vide kosti i nos. Formira se uzorak kose na vlasištu.",
                    MotherChanges = "Bolovi u leđima mogu započeti. Povećava se apetit; neke trudnice osjećaju vrtoglavice."
                },
                new FetalDevelopmentWeek
                {
                    Id = 16,
                    WeekNumber = 16,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development16.png",
                    BabyDevelopment = "Oči se pomjeraju; uši su funkcionalnije. Počinje razvijanje mišićnog tonusa.",
                    MotherChanges = "Moguće je osjetiti prve lagane pokrete (posebno kod višerotki). Trbuh se vidljivije zaokružuje."
                },
                new FetalDevelopmentWeek
                {
                    Id = 17,
                    WeekNumber = 17,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development17.png",
                    BabyDevelopment = "Počinje taloženje masnog tkiva i razvija se koža. Skelet dodatno čvrsti.",
                    MotherChanges = "Grčevi u nogama i bol u leđima su češći. Možda primijetiš povećan apetit."
                },
                new FetalDevelopmentWeek
                {
                    Id = 18,
                    WeekNumber = 18,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development18.png",
                    BabyDevelopment = "Uši su na mjestu; fetus čuje zvukove. Pokreti postaju izraženiji i koordinisaniji.",
                    MotherChanges = "Mogu se javiti vrtoglavica pri naglom ustajanju. Veća težina utiče na držanje."
                },
                new FetalDevelopmentWeek
                {
                    Id = 19,
                    WeekNumber = 19,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development19.png",
                    BabyDevelopment = "Kožu prekriva vernix (zaštitni sloj). Nervni sistem sazrijeva; osjet dodira je bolji.",
                    MotherChanges = "Koža trbuha se rasteže i može svrbiti. Strije se ponekad pojavljuju."
                },
                new FetalDevelopmentWeek
                {
                    Id = 20,
                    WeekNumber = 20,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development20.png",
                    BabyDevelopment = "Vrijeme za detaljan UZ pregled anatomije. Beba se snažnije kreće; spol je obično vidljiv.",
                    MotherChanges = "‘Quickening’ – pokrete jasno osjećaš. Žgaravica i bol u leđima mogu biti češći."
                },
                new FetalDevelopmentWeek
                {
                    Id = 21,
                    WeekNumber = 21,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development21.png",
                    BabyDevelopment = "Fetus guta plodovu vodu i stvara mekonij u crijevima. Pojačana kontrola pokreta.",
                    MotherChanges = "Oticanje stopala i gležnjeva je moguće. Apetit raste; potreba za odmorom veća."
                },
                new FetalDevelopmentWeek
                {
                    Id = 22,
                    WeekNumber = 22,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development22.png",
                    BabyDevelopment = "Formiraju se obrve i trepavice; osjeti zvuk i vibracije. Proporcije tijela postaju skladnije.",
                    MotherChanges = "Pritisak materice može uzrokovati bol u karlici. Zadržavanje tečnosti i strije su mogući."
                },
                new FetalDevelopmentWeek
                {
                    Id = 23,
                    WeekNumber = 23,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development23.png",
                    BabyDevelopment = "Pluća započinju produkciju surfaktanta, ključnog za disanje nakon rođenja. Koža je još naborana.",
                    MotherChanges = "Kratak dah pri naporu i umor su češći. Moguće su noćne grčeve nogu."
                },
                new FetalDevelopmentWeek
                {
                    Id = 24,
                    WeekNumber = 24,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development24.png",
                    BabyDevelopment = "Grananje disajnih puteva napreduje; mozak se brzo razvija. Beba reaguje na dodir i zvuk.",
                    MotherChanges = "Žgaravica i otoci zglobova su češći. Braxton–Hicks kontrakcije se mogu pojaviti."
                },
                new FetalDevelopmentWeek
                {
                    Id = 25,
                    WeekNumber = 25,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development25.png",
                    BabyDevelopment = "Kičma jača; mišići se razvijaju. Pokreti su ritmičniji, a beba može reagovati na svjetlo.",
                    MotherChanges = "Nesanica i češće noćno mokrenje. Umor i bol u leđima su izraženiji."
                },
                new FetalDevelopmentWeek
                {
                    Id = 26,
                    WeekNumber = 26,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development26.png",
                    BabyDevelopment = "Alveole u plućima se formiraju; nervni sistem sazrijeva. Mogu se bilježiti ciklusi sna.",
                    MotherChanges = "Povremene Braxton–Hicks kontrakcije su normalne. Može se javiti otežano disanje pri naporu."
                },
                new FetalDevelopmentWeek
                {
                    Id = 27,
                    WeekNumber = 27,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development27.png",
                    BabyDevelopment = "Počinje treći trimestar. Mozak dobija sve složenije veze; pokreti su snažni.",
                    MotherChanges = "Žgaravica, bol u leđima i umor postaju učestaliji. Apetit može varirati."
                },
                new FetalDevelopmentWeek
                {
                    Id = 28,
                    WeekNumber = 28,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development28.png",
                    BabyDevelopment = "Oči se otvaraju; beba trepće i reaguje na svjetlo. Rast potkožnog masnog tkiva ubrzava.",
                    MotherChanges = "Otoci nogu su češći; moguća nelagoda pri spavanju. Kratkoća daha pri hodu."
                },
                new FetalDevelopmentWeek
                {
                    Id = 29,
                    WeekNumber = 29,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development29.png",
                    BabyDevelopment = "Brže taloženje masti pomaže termoregulaciji nakon rođenja. Mišići jačaju.",
                    MotherChanges = "Grčevi u listovima i hemoroidi mogu smetati. Braxton–Hicks kontrakcije češće."
                },
                new FetalDevelopmentWeek
                {
                    Id = 30,
                    WeekNumber = 30,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development30.png",
                    BabyDevelopment = "Mozak razvija brazde i vijuge; pokreti postaju glatkiji. Beba zauzima više prostora.",
                    MotherChanges = "Učestalije kontrakcije bezbolne prirode. Umor i bol u krstima su izraženiji."
                },
                new FetalDevelopmentWeek
                {
                    Id = 31,
                    WeekNumber = 31,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development31.png",
                    BabyDevelopment = "Rasporedi spavanja i budnosti su uočljiviji. Pluća i dalje sazrijevaju.",
                    MotherChanges = "Kratak dah i potreba za češćim odmorom. Nelagoda pri sjedenju i spavanju."
                },
                new FetalDevelopmentWeek
                {
                    Id = 32,
                    WeekNumber = 32,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development32.png",
                    BabyDevelopment = "Nokti i kosa su duži; beba dobija na težini. Verniks i lanugo i dalje prisutni.",
                    MotherChanges = "Češće mokrenje zbog pritiska na bešiku. Žgaravica i noćne nelagode su učestale."
                },
                new FetalDevelopmentWeek
                {
                    Id = 33,
                    WeekNumber = 33,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development33.png",
                    BabyDevelopment = "Majčina antitijela prelaze na bebu i jačaju imunitet. Tonus mišića je bolji.",
                    MotherChanges = "Otoci ruku i nogu, trnjenje prstiju i nelagoda u karlici su mogući."
                },
                new FetalDevelopmentWeek
                {
                    Id = 34,
                    WeekNumber = 34,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development34.png",
                    BabyDevelopment = "Pluća i nervni sistem znatno zreliji. Beba vježba disajne pokrete.",
                    MotherChanges = "Pritisak u karlici i češće kontrakcije. Umor i nesanica se pojačavaju."
                },
                new FetalDevelopmentWeek
                {
                    Id = 35,
                    WeekNumber = 35,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development35.png",
                    BabyDevelopment = "Stisak šake je snažan; beba dobija na težini. Prostor u materici je sve manji.",
                    MotherChanges = "Kiselina u želucu i zadihanost su česte. Teže je pronaći udoban položaj za spavanje."
                },
                new FetalDevelopmentWeek
                {
                    Id = 36,
                    WeekNumber = 36,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development36.png",
                    BabyDevelopment = "Većina beba je okrenuta glavom prema dolje. Pokreti su rjeđi zbog manjka prostora, ali i dalje primjetni.",
                    MotherChanges = "‘Lightening’ – spuštanje bebe može olakšati disanje, ali pojačati pritisak na bešiku."
                },
                new FetalDevelopmentWeek
                {
                    Id = 37,
                    WeekNumber = 37,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development37.png",
                    BabyDevelopment = "Rani termin (early term). Organi su funkcionalni, pluća blizu pune zrelosti.",
                    MotherChanges = "Lažne kontrakcije češće i pravilnije. Povećan vaginalni iscjedak i mogući znakovi skorog poroda."
                },
                new FetalDevelopmentWeek
                {
                    Id = 38,
                    WeekNumber = 38,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development38.png",
                    BabyDevelopment = "Završno sazrijevanje pluća i nervnog sistema. Beba skladišti energiju i masnoću.",
                    MotherChanges = "Pritisak nisko u karlici, umor i nesanica. Mogući su znakovi početka poroda."
                },
                new FetalDevelopmentWeek
                {
                    Id = 39,
                    WeekNumber = 39,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development39.png",
                    BabyDevelopment = "Puni termin (full term). Beba je spremna za rođenje, nastavlja dobivati na težini.",
                    MotherChanges = "Kontrakcije postaju pravilnije i jače. Moguća pojava ‘bloody show’ i pucanje vodenjaka."
                },
                new FetalDevelopmentWeek
                {
                    Id = 40,
                    WeekNumber = 40,
                    ImageUrl = "https://nestlystorage.blob.core.windows.net/fetaldevelopment/development40.png",
                    BabyDevelopment = "Termin; većina organa je potpuno funkcionalna. Beba je spremna za vanjski svijet.",
                    MotherChanges = "Porod može početi u bilo kojem trenutku. Kontrakcije se intenziviraju i postaju učestalije."
                }
            );
        }
    }
}
