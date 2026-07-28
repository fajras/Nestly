namespace Nestly.ML
{
    /// <summary>
    /// Generates the training dataset for the Q&amp;A urgency triage model
    /// (see who-standards-dataset.md for the same methodology applied to
    /// the deviation-monitoring models). There is no real dataset of parent
    /// questions with doctor-assigned urgency labels, so this uses
    /// weak/programmatic supervision: question texts are assembled from
    /// Bosnian-language phrase banks (WHO IMCI-style danger signs for the
    /// "urgent" class, general parenting/development questions for the
    /// "routine" class) combined with randomized wrappers and filler
    /// values, with the label following directly from which bank the
    /// phrase was drawn from. Small label noise is injected so the
    /// classifier learns a soft boundary instead of memorizing exact
    /// phrases.
    /// </summary>
    public static class SyntheticQaUrgencyDatasetGenerator
    {
        private const double LabelNoiseRate = 0.03;

        private static readonly string[] UrgentPhrases =
        {
            "beba ima visoku temperaturu preko 39 stepeni",
            "dijete ne diše normalno i teško hvata zrak",
            "beba hripa i uvlači rebra pri disanju",
            "usne i koža oko usta su mu/joj modre",
            "primijetila sam krv u stolici kod bebe",
            "beba povraća bez prestanka već nekoliko sati",
            "dijete je jako pospano i teško ga je probuditi",
            "beba ima ukočen vrat i ne podnosi svjetlo",
            "dijete je dobilo grčeve/konvulzije",
            "beba je pala i sada povraća i jako je pospana",
            "beba ne mokri više od 8 sati",
            "koža bebe je postala jako blijeda ili žuta",
            "pojavio se osip koji se brzo širi po tijelu",
            "dijete je progutalo nešto strano ili moguć otrov",
            "temperatura ne pada ni nakon lijeka već satima",
            "beba jako plače i ne može se smiriti već duže vrijeme",
            "dijete diše veoma brzo i teško",
            "beba ima jaku bol u stomaku i ne miče se",
            "dijete je na trenutak izgubilo svijest",
            "koža oko usta i noktiju izgleda siva",
            "novorođenče odbija svaki obrok i letargično je",
            "beba ima temperaturu a mlađa je od tri mjeseca",
            "primijetila sam krv u povraćanju kod bebe",
            "dijete se opeklo i koža je jako crvena i mjehuri se",
            "beba se guši i ne može doći do daha"
        };

        private static readonly string[] RoutinePhrases =
        {
            "koliko sati sna je normalno za bebu od {age} mjeseci",
            "kada beba počinje jesti čvrstu hranu",
            "koje igračke su prikladne za uzrast od {age} mjeseci",
            "da li je normalno da beba često podriguje nakon hranjenja",
            "kada mogu očekivati da beba počne sjediti sama",
            "koliko puta dnevno treba mijenjati pelene",
            "koja hrana je najbolja za uvođenje u ishranu",
            "kako uspostaviti stabilnu rutinu spavanja",
            "da li je u redu koristiti cuclu",
            "koliko dugo obično traje faza kolika",
            "kada se preporučuje prva redovna vakcina",
            "kako naučiti bebu da spava cijelu noć",
            "koje vitamine treba davati bebi",
            "kada beba obično počinje govoriti prve riječi",
            "da li je normalno da beba puno spava tokom dana",
            "koje su preporuke za kupanje novorođenčeta",
            "kako pripremiti bebu za prvi izlet vani",
            "da li trebam brinuti ako beba ne voli određenu hranu",
            "koliko brzo bi trebalo beba da dobija na težini",
            "kada mogu početi sa uvođenjem sokova",
            "koliko je normalno da beba plače tokom dana",
            "kako izgleda zdrav ritam hranjenja za uzrast od {age} mjeseci",
            "da li je u redu da beba spava na stomaku uz nadzor",
            "koje su preporuke za prve cipelice",
            "kada je vrijeme za prvu posjetu zubaru"
        };

        private static readonly string[] Wrappers =
        {
            "Poštovani doktore, {phrase}?",
            "{phrase}. Šta mi savjetujete?",
            "Zanima me sljedeće: {phrase}?",
            "Molim savjet - {phrase}.",
            "Imam pitanje: {phrase}?",
            "Dobar dan, {phrase}?",
            "{phrase}, šta da radim?",
            "Treba mi savjet doktora: {phrase}.",
            "Brinem se jer {phrase}.",
            "{phrase}?"
        };

        public static IEnumerable<(string QuestionText, bool IsUrgent)> Generate(int count, int seed = 6)
        {
            var rng = new Random(seed);

            for (var i = 0; i < count; i++)
            {
                var isUrgent = rng.NextDouble() < 0.5;
                var phraseBank = isUrgent ? UrgentPhrases : RoutinePhrases;
                var phrase = phraseBank[rng.Next(phraseBank.Length)];

                phrase = phrase.Replace("{age}", rng.Next(1, 25).ToString());

                var wrapper = Wrappers[rng.Next(Wrappers.Length)];
                var text = wrapper.Replace("{phrase}", phrase);

                text = char.ToUpper(text[0]) + text[1..];

                var label = isUrgent;

                if (rng.NextDouble() < LabelNoiseRate)
                {
                    label = !label;
                }

                yield return (text, label);
            }
        }
    }
}
