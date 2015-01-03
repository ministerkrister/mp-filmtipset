using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Text.RegularExpressions;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Localisation;
using Filmtipset.GUI;

namespace Filmtipset.GUI
{
    public static class Translation
    {
        #region Private variables

        private static Dictionary<string, string> translations;
        private static Regex translateExpr = new Regex(@"\$\{([^\}]+)\}");
        private static string path = string.Empty;

        #endregion

        #region Constructor

        static Translation()
        {

        }

        #endregion

        #region Public Properties

        /// <summary>
        /// Gets the translated strings collection in the active language
        /// </summary>
        public static Dictionary<string, string> Strings
        {
            get
            {
                if (translations == null)
                {
                    translations = new Dictionary<string, string>();
                    Type transType = typeof(Translation);
                    FieldInfo[] fields = transType.GetFields(BindingFlags.Public | BindingFlags.Static);
                    foreach (FieldInfo field in fields)
                    {
                        translations.Add(field.Name, field.GetValue(transType).ToString());
                    }
                }
                return translations;
            }
        }

        public static string CurrentLanguage
        {
            get
            {
                string language = string.Empty;
                try
                {
                    language = GUILocalizeStrings.GetCultureName(GUILocalizeStrings.CurrentLanguage());
                }
                catch (Exception)
                {
                    language = CultureInfo.CurrentUICulture.Name;
                }
                return language;
            }
        }
        public static string PreviousLanguage { get; set; }

        #endregion

        #region Public Methods

        public static void Init()
        {
            translations = null;

            path = Config.GetSubFolder(Config.Dir.Language, "Filmtipset");

            if (!System.IO.Directory.Exists(path))
                System.IO.Directory.CreateDirectory(path);

            string lang = PreviousLanguage = CurrentLanguage;
            LoadTranslations(lang);

            // publish all available translation strings
            // so skins have access to them
            foreach (string name in Strings.Keys)
            {
                GUIUtils.SetProperty("#Filmtipset.Translation." + name + ".Label", Translation.Strings[name]);
            }
        }

        public static int LoadTranslations(string lang)
        {
            XmlDocument doc = new XmlDocument();
            Dictionary<string, string> TranslatedStrings = new Dictionary<string, string>();
            string langPath = string.Empty;
            try
            {
                langPath = Path.Combine(path, lang + ".xml");
                doc.Load(langPath);
            }
            catch (Exception e)
            {
                if (lang == "sv")
                    return 0; // otherwise we are in an endless loop!

                if (e.GetType() == typeof(FileNotFoundException))
                    Log.Warn("[Filmtipset] Cannot find translation file {0}. Falling back to Swedish", langPath);
                else
                    Log.Error("[Filmtipset] Error in translation xml file: {0}. Falling back to Swedish", lang);

                return LoadTranslations("sv");
            }
            foreach (XmlNode stringEntry in doc.DocumentElement.ChildNodes)
            {
                if (stringEntry.NodeType == XmlNodeType.Element)
                {
                    try
                    {
                        string key = stringEntry.Attributes.GetNamedItem("name").Value;
                        if (!TranslatedStrings.ContainsKey(key))
                        {
                            TranslatedStrings.Add(key, stringEntry.InnerText.NormalizeTranslation());
                        }
                        else
                        {
                            Log.Error("[Filmtipset] Error in Translation Engine, the translation key '{0}' already exists.", key);
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.Error("[Filmtipset] Error in Translation Engine: {0}", ex.Message);
                    }
                }
            }

            Type TransType = typeof(Translation);
            FieldInfo[] fieldInfos = TransType.GetFields(BindingFlags.Public | BindingFlags.Static);
            foreach (FieldInfo fi in fieldInfos)
            {
                if (TranslatedStrings != null && TranslatedStrings.ContainsKey(fi.Name))
                    TransType.InvokeMember(fi.Name, BindingFlags.SetField, null, TransType, new object[] { TranslatedStrings[fi.Name] });
                else
                    Log.Warn("[Filmtipset] Translation not found for field: {0}. Using hard-coded Swedish default.", fi.Name);
            }
            return TranslatedStrings.Count;
        }

        public static string GetByName(string name)
        {
            if (!Strings.ContainsKey(name))
                return name;

            return Strings[name];
        }

        public static string GetByName(string name, params object[] args)
        {
            return String.Format(GetByName(name), args);
        }

        /// <summary>
        /// Takes an input string and replaces all ${named} variables with the proper translation if available
        /// </summary>
        /// <param name="input">a string containing ${named} variables that represent the translation keys</param>
        /// <returns>translated input string</returns>
        public static string ParseString(string input)
        {
            MatchCollection matches = translateExpr.Matches(input);
            foreach (Match match in matches)
            {
                input = input.Replace(match.Value, GetByName(match.Groups[1].Value));
            }
            return input;
        }

        /// <summary>
        /// Temp workaround to remove unwanted chars from Transifex
        /// </summary>
        public static string NormalizeTranslation(this string input)
        {
            input = input.Replace("\\'", "'");
            input = input.Replace("\\\"", "\"");
            return input;
        }
        #endregion

        #region Translations / Strings

        /// <summary>
        /// These will be loaded with the language files content
        /// if the selected lang file is not found, it will first try to load en(us).xml as a backup
        /// if that also fails it will use the hardcoded strings as a last resort.
        /// </summary>

        // F
        public static string Filmtipset = "Filmtipset";
        // G
        public static string Genre = "Genre";
        public static string GenreItem = "Genre: {0}";
        public static string GenreGenerellaTips = "Alla";
        public static string GenreDrama = "Drama";
        public static string GenreKortfilm = "Kortfilm";
        public static string GenreKomedi = "Komedi";
        public static string GenreDokumentar = "Dokumentär";
        public static string GenreAnimerad = "Animerad";
        public static string GenreVuxenfilm = "Vuxenfilm";
        public static string GenreFamiljefilm = "Familjefilm";
        public static string GenreAction = "Action";
        public static string GenreKriminalare = "Kriminalare";
        public static string GenreRomantik = "Romantik";
        public static string GenreThriller = "Thriller";
        public static string GenreMusikal = "Musikal";
        public static string GenreAventyr = "Äventyr";
        public static string GenreWestern = "Western";
        public static string GenreSkrack = "Skräck";
        public static string GenreScienceFiction = "Science Fiction";
        public static string GenreFantasy = "Fantasy";
        public static string GenreMysterium = "Mysterium";
        public static string GenreKrig = "Krig";
        public static string GenreFilmNoir = "Film-Noir";
        public static string GenreScenshow = "Scenshow";
        public static string GenreAnime = "Anime";
        public static string GenreMiniSerie = "Mini-serie";
        public static string GenreStumfilm = "Stumfilm";
        public static string GenreAmatorfilm = "Amatörfilm";
        public static string GenreExperimentfilm = "Experimentfilm";
        public static string GenreRoadmovie = "Roadmovie";
        public static string GenreBiografi = "Biografi";

        public static string GradeItem = "Betyg: {0}";
        public static string GradeTypeseen = "Betygsatt";
        public static string GradeTypecalculated = "Beräknat betyg";
        public static string GradeTypenone = "Beräknat betyg";
        public static string GradeTypeofficial = "Filmtipset-betyg";
        public static string GradeCount = "antal betyg";
        public static string GradeFilmtipset = "Filmtipset-betyg";



        //L
        public static string list = "Lista";
        public static string listItem = "Lista: {0}";
        public static string listbio = "Bio";
        public static string listtv = "TV";
        public static string listvideo = "DVD";
        public static string listowned = "Min filmsamling";
        public static string listwantedlist = "Vill se-listan";
        public static string listseen = "Senast betygsatta filmer";
        public static string listgrades = "Mina betyg";
        public static string Lists = "Listor";


        //M
        public static string MemberItem = "Medlem: {0}";
        public static string MemberNoMember = "Ingen medlem vald";
        public static string movie = "Film";
        public static string Movie = "Film";

        //N
        public static string NotAvailable = "-";
        public static string NoMoviesFound = "Inga filmer hittades";

        //O
        public static string OnlineVideosSearch = "Sök i OnlineVideos";
        public static string OnlineVideosOrgName = "Sök i OnlineVideos med orginaltitel";

        //R
        public static string recommendations = "Rekommendationer";
        public static string RecommendedMovies = "Rekommendationer";

        //T
        public static string Trailers = "Trailers";

        #endregion

    }
}