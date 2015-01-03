using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Profile;
using MediaPortal.GUI.Library;
using Filmtipset.API;
using Filmtipset.GUI;
using Filmtipset.Models;
using Filmtipset.Extensions;

namespace Filmtipset
{
    internal enum TvWishesSearchSetting
    {
        title_TitleOrOrgTitle = 0,
        title_TitleOrOrgTitle_And_Description_Year = 1,
        title_Title_Or_Description_OrgTitleAndYear = 2
    }

    internal class TvWishesSearchSettingItem
    {
        private TvWishesSearchSetting setting;
        private string title;

        internal TvWishesSearchSettingItem(TvWishesSearchSetting setting, string title)
        {
            Title = title;
            Setting = setting;
        }

        public string Title
        {
            get { return title; }
            set { title = value; }
        }

        public TvWishesSearchSetting Setting
        {
            get { return setting; }
            set { setting = value; }
        }

        public override string ToString()
        {
            return title;
        }

    }

    public class FilmtipsetSettings
    {
        #region App Proerties
        internal static bool UseLocalDebugProxy { get { return false; } }
        internal static string LocalDebugProxyIp { get { return "127.0.0.1"; } }
        internal static int LocalDebugProxyPort { get { return 8888; } }
        internal static bool UseFanartTvApiProxy { get { return false; } }
        internal static int ThumbWidth { get { return 379; } } //379 //180
        internal static int ThumbHeight { get { return 563; } } //563 //250
        internal static bool SkipOverlay = true;
        internal static int FanartCacheTimeout = 12; //In hours


        #endregion

        #region Constants

        private const string cFilmtipset = "Filmtipset";
        private const string cFilmtipsetVersion = "FilmtipsetVersion";

        private const string cEnableFanart = "EnableFanart";
        private const string cPreferFanartPoster = "PreferFanartPoster";
        private const string cPersonalFanartAPIKey = "PersonalFanartAPIKey";
        private const string cEnableViaplay = "EnableViaplay";
        private const string cEnableDreamfilm = "EnableDreamfilm";
        private const string cEnableSwefilmer = "EnableSwefilmer";
        private const string cEnableMovHunter = "EnableMovHunter";
        private const string cEnableTfPlay = "EnableTfPlay";
        private const string cEnableSweflix = "EnableSweflix";
        private const string cOnlineVideosExtraSites = "OnlineVideosExtraSites";

        private const string cAccounts = "Accounts";
        private const string cCurrentAccount = "CurrentAccount";

        private const string cWebRequestTimeout = "WebRequestTimeout";
        private const string cWebRequestCacheMinutes = "WebRequestCacheMinutes";

        private const string cTvWishListEmail = "TvWishListEmail";
        private const string cTvWishSearchLogic = "TvWishSearchLogic";

        #endregion


        #region properties

        public static bool EnableFanart { get; set; }
        public static bool PreferFanartPoster { get; set; }
        public static string PersonalFanartAPIKey { get; set; }
        public static bool EnableViaplay { get; set; }
        public static bool EnableDreamfilm { get; set; }
        public static bool EnableSwefilmer { get; set; }
        public static bool EnableMovHunter { get; set; }
        public static bool EnableTfPlay { get; set; }
        public static bool EnableSweflix { get; set; }
        public static string OnlineVideosExtraSites { get; set; }

        public static List<Account> Accounts { get; set; }
        public static Account CurrentAccount { get; set; }

        public static int WebRequestTimeout { get; set; }
        public static int WebRequestCacheMinutes { get; set; }

        public static bool TvWishListEmail { get; set; }
        public static int TvWishSearchLogic { get; set; }


        #endregion

        /// <summary>
        /// Loads the Settings
        /// </summary>
        internal static void LoadSettings()
        {
            Log.Debug("[Filmtipset] Loading Settings");
            using (Settings xmlreader = new MPSettings())
            {
                EnableFanart = xmlreader.GetValueAsBool(cFilmtipset, cEnableFanart, true);
                PreferFanartPoster = xmlreader.GetValueAsBool(cFilmtipset, cPreferFanartPoster, false);
                PersonalFanartAPIKey = xmlreader.GetValueAsString(cFilmtipset, cPersonalFanartAPIKey, string.Empty);
                EnableViaplay = xmlreader.GetValueAsBool(cFilmtipset, cEnableViaplay, true);
                EnableDreamfilm = xmlreader.GetValueAsBool(cFilmtipset, cEnableDreamfilm, true);
                EnableSwefilmer = xmlreader.GetValueAsBool(cFilmtipset, cEnableFanart, true);
                EnableMovHunter = xmlreader.GetValueAsBool(cFilmtipset, cEnableMovHunter, true);
                EnableTfPlay = xmlreader.GetValueAsBool(cFilmtipset, cEnableFanart, true);
                EnableSweflix = xmlreader.GetValueAsBool(cFilmtipset, cEnableSweflix, true);
                OnlineVideosExtraSites = xmlreader.GetValueAsString(cFilmtipset, cOnlineVideosExtraSites, "");

                Accounts = xmlreader.GetValueAsString(cFilmtipset, cAccounts, "").FromJSONArray<Account>().ToList();
                int currentAccountId = xmlreader.GetValueAsInt(cFilmtipset, cCurrentAccount, 0);
                CurrentAccount = Accounts.FirstOrDefault(a => a.Id == currentAccountId) ?? Filmtipset.Util.Helpers.GetDefaultUser();

                WebRequestCacheMinutes = xmlreader.GetValueAsInt(cFilmtipset, cWebRequestCacheMinutes, 60);
                WebRequestTimeout = xmlreader.GetValueAsInt(cFilmtipset, cWebRequestTimeout, 30000);

                TvWishListEmail = xmlreader.GetValueAsBool(cFilmtipset, cTvWishListEmail, false);
                TvWishSearchLogic = xmlreader.GetValueAsInt(cFilmtipset, cTvWishSearchLogic, (int)TvWishesSearchSetting.title_TitleOrOrgTitle);
            }
        }
        /// <summary>
        /// Saves the Settings
        /// </summary>
        internal static void SaveSettings()
        {
            Log.Debug("[Filmtipset] Saving Settings");
            using (Settings xmlwriter = new MPSettings())
            {
                xmlwriter.SetValueAsBool(cFilmtipset, cEnableFanart, EnableFanart);
                xmlwriter.SetValueAsBool(cFilmtipset, cPreferFanartPoster, PreferFanartPoster);
                xmlwriter.SetValue(cFilmtipset, cPersonalFanartAPIKey, PersonalFanartAPIKey);
                xmlwriter.SetValueAsBool(cFilmtipset, cEnableViaplay, EnableViaplay);
                xmlwriter.SetValueAsBool(cFilmtipset, cEnableDreamfilm, EnableDreamfilm);
                xmlwriter.SetValueAsBool(cFilmtipset, cEnableSwefilmer, EnableSwefilmer);
                xmlwriter.SetValueAsBool(cFilmtipset, cEnableMovHunter, EnableMovHunter);
                xmlwriter.SetValueAsBool(cFilmtipset, cEnableTfPlay, EnableTfPlay);
                xmlwriter.SetValueAsBool(cFilmtipset, cEnableSweflix, EnableSweflix);
                xmlwriter.SetValue(cFilmtipset, cOnlineVideosExtraSites, OnlineVideosExtraSites);

                if (Accounts != null)
                    xmlwriter.SetValue(cFilmtipset, cAccounts, Accounts.ToJSON());
                if (CurrentAccount != null)
                    xmlwriter.SetValue(cFilmtipset, cCurrentAccount, CurrentAccount.Id);

                xmlwriter.SetValue(cFilmtipset, cWebRequestCacheMinutes, WebRequestCacheMinutes);
                xmlwriter.SetValue(cFilmtipset, cWebRequestTimeout, WebRequestTimeout);

                xmlwriter.SetValueAsBool(cFilmtipset, cTvWishListEmail, TvWishListEmail);
                xmlwriter.SetValue(cFilmtipset, cTvWishSearchLogic, TvWishSearchLogic);
            }
        }
    }
}
