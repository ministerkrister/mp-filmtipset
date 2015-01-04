using Filmtipset.API;
using Filmtipset.Models;
using Filmtipset.Util;
using MediaPortal.GUI.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;

namespace Filmtipset.GUI
{
    #region Enums

    public enum Layout
    {
        List = 0,
        SmallIcons = 1,
        LargeIcons = 2,
        Filmstrip = 3,
        CoverFlow = 4,
    }


    public enum FilmtipsetGUIControls
    {
        Member = 2,
        Layout = 3,
        Facade = 50,
    }

    enum FilmtipsetGUIWindows
    {
        Main = 742100,
        Recommendations = 742101,
        Cinema = 742102,
        List = 80087,
        Dvd = 80187,
        Tv = 80287,
        Owned = 80387,
        Wanted = 80487,
        LatestSeen = 80587,
        Grades = 80088,
        Lists = 80089,
        Movie = 80090,
        RatingDialog = 742199
    }

    enum ContextMenuItem
    {
        AddToWatchList,
        AddToList,
        AddToLibrary,
        Rate,
        Shouts,
        Trailers,
        OnlineVideosTitle,
        OnlineVideosOrgTitle,
        TvWish
    }



    enum ExternalPluginWindows
    {
        OnlineVideos = 4755,
    }


    #endregion

    public class GUICommon
    {
        #region Layout
        internal static Layout ShowLayoutMenu(Layout currentLayout, int itemToSelect)
        {
            Layout newLayout = currentLayout;

            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            dlg.Reset();
            dlg.SetHeading(GetLayoutTranslation(currentLayout));

            foreach (Layout layout in Enum.GetValues(typeof(Layout)))
            {
                string menuItem = GetLayoutTranslation(layout);
                GUIListItem pItem = new GUIListItem(menuItem);
                if (layout == currentLayout) pItem.Selected = true;
                dlg.Add(pItem);
            }

            dlg.DoModal(GUIWindowManager.ActiveWindow);

            if (dlg.SelectedLabel >= 0)
            {
                var facade = GUIWindowManager.GetWindow(GUIWindowManager.ActiveWindow).GetControl((int)FilmtipsetGUIControls.Facade) as GUIFacadeControl;

                newLayout = (Layout)dlg.SelectedLabel;
                facade.SetCurrentLayout(Enum.GetName(typeof(Layout), newLayout));
                GUIControl.SetControlLabel(GUIWindowManager.ActiveWindow, (int)FilmtipsetGUIControls.Layout, GetLayoutTranslation(newLayout));
                // when loosing focus from the facade the current selected index is lost
                // e.g. changing layout from skin side menu
                facade.SelectIndex(itemToSelect);
            }
            return newLayout;
        }

        internal static string GetLayoutTranslation(Layout layout)
        {
            string strLine = string.Empty;
            switch (layout)
            {
                case Layout.List:
                    strLine = GUILocalizeStrings.Get(101);
                    break;
                case Layout.SmallIcons:
                    strLine = GUILocalizeStrings.Get(100);
                    break;
                case Layout.LargeIcons:
                    strLine = GUILocalizeStrings.Get(417);
                    break;
                case Layout.Filmstrip:
                    strLine = GUILocalizeStrings.Get(733);
                    break;
                case Layout.CoverFlow:
                    strLine = GUILocalizeStrings.Get(791);
                    break;
            }
            //Workaround. coverflow string starts with "Layout: " in sv. If translation changes, this will not break stuff...
            return strLine.StartsWith(GUILocalizeStrings.Get(95).Trim()) ? strLine : GUILocalizeStrings.Get(95) + strLine;
        }
        #endregion

        #region Member
        internal static Account ShowAccountMenu(Account currentAccount)
        {
            Account newAccount = currentAccount;

            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            dlg.Reset();
            dlg.SetHeading(GetAccountTranslation(currentAccount));

            Account defaultAccount = Util.Helpers.GetDefaultUser();
            string menuItem = GetAccountTranslation(defaultAccount);
            GUIListItem pItem = new GUIListItem(menuItem);
            if (defaultAccount.Id == currentAccount.Id) pItem.Selected = true;
            dlg.Add(pItem);
            foreach (Account account in FilmtipsetSettings.Accounts)
            {
                menuItem = GetAccountTranslation(account);
                pItem = new GUIListItem(menuItem);
                if (account.Name == currentAccount.Name) pItem.Selected = true;
                dlg.Add(pItem);
            }

            dlg.DoModal(GUIWindowManager.ActiveWindow);

            if (dlg.SelectedLabel >= 0)
            {
                newAccount = FilmtipsetSettings.Accounts.FirstOrDefault(a => GetAccountTranslation(a) == dlg.SelectedLabelText) ?? Helpers.GetDefaultUser();
                GUIControl.SetControlLabel(GUIWindowManager.ActiveWindow, (int)FilmtipsetGUIControls.Member, GetAccountTranslation(newAccount));
                // when loosing focus from the facade the current selected index is lost
                // e.g. changing layout from skin side menu
            }
            return newAccount;
        }

        internal static string GetAccountTranslation(Account account)
        {
            return string.Format(Translation.MemberItem, account.Name);
        }
        #endregion

        #region Movie properties
        internal static void SetMovieProperties(Movie movie)
        {
            SetProperty("#Filmtipset.Movie.Actors", movie.Actors);
            SetProperty("#Filmtipset.Movie.AltTitle", movie.AltTitle);
            SetProperty("#Filmtipset.Movie.Country", movie.Country);
            SetProperty("#Filmtipset.Movie.Description", movie.Description); 
            SetProperty("#Filmtipset.Movie.Director", movie.Director);
            SetProperty("#Filmtipset.Movie.Id", movie.Id.ToString());
            SetProperty("#Filmtipset.Movie.Imdb", "tt" + movie.Imdb);
            SetProperty("#Filmtipset.Movie.Length", MediaPortal.Util.Utils.SecondsToHMString(movie.Length * 60));
            SetProperty("#Filmtipset.Movie.Name", movie.Name);
            SetProperty("#Filmtipset.Movie.OrgName", movie.OrgName);
            SetProperty("#Filmtipset.Movie.PosterImageFilename", movie.Images.PosterImageFilename);
            SetProperty("#Filmtipset.Movie.TimeSeen", movie.TimeSeen);
            SetProperty("#Filmtipset.Movie.Url", movie.Url);
            SetProperty("#Filmtipset.Movie.Writers", movie.Writers);
            SetProperty("#Filmtipset.Movie.Year", movie.Year);
            if (movie == null) return;
            
            if (movie.FimltipsetGrade != null)
            {
                SetProperty("#Filmtipset.Movie.FimltipsetGrade.Count", movie.FimltipsetGrade.Count.ToString());
                SetProperty("#Filmtipset.Movie.FimltipsetGrade.Grade", movie.FimltipsetGrade.Value);
                int v = 0;
                int.TryParse(movie.FimltipsetGrade.Value, out v);
                v = v * 2;
                SetProperty("#Filmtipset.Movie.FimltipsetGrade.Stars", v.ToString());
            }
            if (movie.FimltipsetGrade != null)
            {
                int v = 0;
                int.TryParse(movie.Grade.Value, out v);

                SetProperty("#Filmtipset.Movie.Grade.Type", GUI.Translation.GetByName("GradeType" + movie.Grade.Type));
                SetProperty("#Filmtipset.Movie.Grade.Grade", (v * 2).ToString());
            }
        }

        internal static void ClearMovieProperties()
        {
            GUIUtils.SetProperty("#Filmtipset.Movie.Actors", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.AltTitle", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Country", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Description", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Director", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.FimltipsetGrade.Count", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.FimltipsetGrade.Grade", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.FimltipsetGrade.Stars", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Grade.Type", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Grade.Grade", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Id", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Imdb", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Length", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Name", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.OrgName", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.PosterImageFilename", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.TimeSeen", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Url", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Writers", string.Empty);
            GUIUtils.SetProperty("#Filmtipset.Movie.Year", string.Empty);
        }

        internal static void SetProperty(string property, string value)
        {
            string propertyValue = string.IsNullOrEmpty(value) ? Translation.GetByName("NotAvailable") : value;
            //string propertyValue = string.IsNullOrEmpty(value) ? " " : value;
            GUIUtils.SetProperty(property, propertyValue);
        }

        #endregion

        #region external plugin dll calls,

        //do not call this method if you don't check if Trailers plugin dll exsists!!
        internal static void CallTrailersPlugin(Movie selectedMovie, GUIListItem selectedItem)
        {
            int y = 0;
            int.TryParse(selectedMovie.Year, out y);
            GUICommon.SetProperty("#Play.Current.OnlineVideos.SiteName", Translation.GetByName("Filmtipset"));
            Trailers.Trailers.SearchForTrailers(new Trailers.Providers.MediaItem() { IMDb = !string.IsNullOrEmpty(selectedMovie.Imdb) ? "tt" + selectedMovie.Imdb : "", Title = string.IsNullOrEmpty(selectedMovie.OrgName) ? selectedMovie.Name : selectedMovie.OrgName, MediaType = Trailers.Providers.MediaItemType.Movie, Year = y, Poster = selectedItem.HasThumbnail ? selectedItem.ThumbnailImage : "" });
        }
        #endregion
    }
    internal sealed class ImageDownloader
    {

        # region Singleton
        private ImageDownloader()
        {
        }

        private static volatile ImageDownloader instance = null;
        private static object lockObj = new Object();

        internal static ImageDownloader Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                            instance = new ImageDownloader();
                    }
                }

                return instance;
            }
        }
        #endregion

        internal bool StopDownloads
        {
            get
            {
                bool stop;
                lock (lockObj)
                    stop = _stopDownload;
                return stop;
            }
            set
            {
                lock (lockObj)
                    _stopDownload = false;
            }
        }
        private bool _stopDownload;

        internal void GetImages(List<MovieImages> itemsWithThumbs)
        {
            lock (lockObj)
                _stopDownload = false;

            // split the downloads in 5+ groups and do multithreaded downloading
            int groupSize = (int)Math.Max(1, Math.Floor((double)itemsWithThumbs.Count / 5));
            int groups = (int)Math.Ceiling((double)itemsWithThumbs.Count() / groupSize);

            for (int i = 0; i < groups; i++)
            {
                List<MovieImages> groupList = new List<MovieImages>();
                for (int j = groupSize * i; j < groupSize * i + (groupSize * (i + 1) > itemsWithThumbs.Count ? itemsWithThumbs.Count - groupSize * i : groupSize); j++)
                {
                    groupList.Add(itemsWithThumbs[j]);
                }

                new Thread(delegate(object o)
                {
                    List<MovieImages> items = (List<MovieImages>)o;
                    foreach (MovieImages item in items)
                    {
                        string remoteFanartPoster = string.Empty;
                        string remoteFanart = string.Empty;
                        bool getFanartUrls = true;
                        if (FilmtipsetSettings.EnableFanart)
                        {
                            FanartAPI.Instance.GetFanartFromCache(item.Imdb, out getFanartUrls, out remoteFanart, out remoteFanartPoster);
                        }
                        #region FanartPoster
                        bool havePoster = false;
                        if (FilmtipsetSettings.EnableFanart)
                        {

                            if (FilmtipsetSettings.PreferFanartPoster && !GUIImageHandler.DoLocalFileExist(item.PosterImageFilename))
                            {
                                // stop download if we have exited window
                                lock (lockObj)
                                    if (_stopDownload) break;
                                string localPoster = item.PosterImageFilename;
                                if (getFanartUrls)
                                {
                                    FanartAPI.Instance.GetFanartUrls(item.Imdb, GUI.Translation.CurrentLanguage, out remoteFanart, out remoteFanartPoster);
                                    FanartAPI.Instance.AddToCache(item.Imdb, remoteFanart, remoteFanartPoster);
                                    getFanartUrls = false;
                                }
                                localPoster = item.PosterImageFilename;


                                if (!string.IsNullOrEmpty(remoteFanartPoster) && !string.IsNullOrEmpty(localPoster))
                                {
                                    if (GUIImageHandler.DownloadImage(remoteFanartPoster, localPoster))
                                    {
                                        // notify that image has been downloaded
                                        havePoster = true;
                                        item.NotifyPropertyChanged("PosterImageFilename");
                                    }
                                }
                            }
                        }
                        #endregion

                        #region Poster
                        if (!havePoster)
                        {
                            // stop download if we have exited window
                            lock (lockObj)
                                if (_stopDownload) break;

                            string remotePoster = item.Poster;
                            string localPoster = item.PosterImageFilename;

                            if (!string.IsNullOrEmpty(remotePoster) && !string.IsNullOrEmpty(localPoster))
                            {
                                if (GUIImageHandler.DownloadImage(remotePoster, localPoster))
                                {
                                    // notify that image has been downloaded
                                    item.NotifyPropertyChanged("PosterImageFilename");
                                }
                            }
                        }
                        #endregion
                        #region Fanart
                        // stop download if we have exited window
                        lock (lockObj)
                            if (_stopDownload) break;
                        if (!FilmtipsetSettings.EnableFanart) continue;
                        string localFanart = item.FanartImageFilename;
                        if (!string.IsNullOrEmpty(localFanart) && GUIImageHandler.DoLocalFileExist(localFanart))
                        {
                            // notify that image has been "downloaded"
                            item.NotifyPropertyChanged("FanartImageFilename");
                        }
                        else
                        {
                            if (getFanartUrls)
                            {
                                FanartAPI.Instance.GetFanartUrls(item.Imdb, GUI.Translation.CurrentLanguage, out remoteFanart, out remoteFanartPoster);
                                FanartAPI.Instance.AddToCache(item.Imdb, remoteFanart, remoteFanartPoster);
                                getFanartUrls = false;
                            }
                            item.Fanart = remoteFanart;
                            localFanart = item.FanartImageFilename;

                            if (!string.IsNullOrEmpty(remoteFanart) && !string.IsNullOrEmpty(localFanart))
                            {
                                if (GUIImageHandler.DownloadImage(remoteFanart, localFanart))
                                {
                                    // notify that image has been downloaded
                                    item.NotifyPropertyChanged("FanartImageFilename");
                                }
                            }
                        }
                        #endregion

                    }
                })
                {
                    IsBackground = true,
                    Name = "ImageDownloader" + i.ToString()
                }.Start(groupList);
            }
        }

    }
    public class GUIFilmtipsetRecommendetionItem : GUIListItem
    {
        public GUIFilmtipsetRecommendetionItem(string strLabel, int windowId) : base(strLabel) { WindowID = windowId; }

        public int WindowID { get; set; }
        public object Item
        {
            get { return _Item; }
            set
            {
                _Item = value;
                INotifyPropertyChanged notifier = value as INotifyPropertyChanged;
                if (notifier != null) notifier.PropertyChanged += (s, e) =>
                {
                    if (s is MovieImages && e.PropertyName == "PosterImageFilename")
                        SetImageToGui((s as MovieImages).PosterImageFilename);
                    if (s is MovieImages && e.PropertyName == "FanartImageFilename")
                        this.UpdateItemIfSelected(WindowID, ItemId);

                };
            }
        } protected object _Item;

        /// <summary>
        /// Loads an Image from memory into a facade item
        /// </summary>
        /// <param name="imageFilePath">Filename of image</param>
        protected void SetImageToGui(string imageFilePath)
        {
            if (!FilmtipsetSettings.SkipOverlay)
            {
                #region overlay
                try
                {
                    if (string.IsNullOrEmpty(imageFilePath)) return;
                    Movie movie = TVTag as Movie;
                    if (movie == null) return;

                    MainOverlayImage mainOverlay = MainOverlayImage.None;

                    //if (movie.Grade.Type == GradeType.seen.ToString())
                    //    mainOverlay = MainOverlayImage.Seen;

                    int rating = 0;
                    int.TryParse(movie.Grade.Value, out rating);
                    RatingOverlayImage ratingOverlay = (RatingOverlayImage)rating;

                    // get a reference to a MediaPortal Texture Identifier
                    string suffix = mainOverlay.ToString().Replace(", ", string.Empty) + Enum.GetName(typeof(RatingOverlayImage), ratingOverlay);
                    string texture = GUIImageHandler.GetTextureIdentFromFile(imageFilePath, suffix);

                    // build memory image
                    Image memoryImage = null;
                    if (mainOverlay != MainOverlayImage.None || ratingOverlay != RatingOverlayImage.None)
                    {
                        memoryImage = GUIImageHandler.DrawOverlayOnPoster(imageFilePath, mainOverlay, ratingOverlay, new Size(FilmtipsetSettings.ThumbWidth, FilmtipsetSettings.ThumbHeight));
                        if (memoryImage == null) return;

                        // load texture into facade item
                        if (GUITextureManager.LoadFromMemory(memoryImage, texture, 0, 0, 0) > 0)
                        {
                            ThumbnailImage = texture;
                            IconImageBig = texture;
                        }
                    }
                    else
                    {
                        ThumbnailImage = imageFilePath;
                        IconImageBig = imageFilePath;
                    }

                    // if selected and is current window force an update of thumbnail
                    this.UpdateItemIfSelected(WindowID, ItemId);
                }
                catch (Exception e)
                {
                    Log.Error(string.Format("[Filmtipset] Error in SetImageToGui, memory isues? {0}", e.Message));
                }
                #endregion
            }
            else
            {
                ThumbnailImage = imageFilePath;
                IconImageBig = imageFilePath;
                this.UpdateItemIfSelected(WindowID, ItemId);
            }
        }
    }
}
