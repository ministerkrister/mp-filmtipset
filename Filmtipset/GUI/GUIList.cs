﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using Filmtipset.Models;
using Filmtipset.API;
using Filmtipset.Util;
using Filmtipset.Extensions;
using MediaPortal.Util;

namespace Filmtipset.GUI
{
    public class GUIList : GUIWindow
    {
        #region Skin Controls

        [SkinControl(2)]
        protected GUIButtonControl memberButton = null;

        [SkinControl(3)]
        protected GUIButtonControl layoutButton = null;

        [SkinControl(4)]
        protected GUIButtonControl genreButton = null;

        [SkinControl(5)]
        protected GUIButtonControl gradeButton = null;

        [SkinControl(99)]
        protected GUIButtonControl reloadButton = null;

        [SkinControl(50)]
        protected GUIFacadeControl Facade = null;

        [SkinControlAttribute(60)]
        protected GUIImage FanartBackground = null;

        [SkinControlAttribute(61)]
        protected GUIImage FanartBackground2 = null;

        //[SkinControlAttribute(62)]
        protected GUIImage loadingImage = null;

        #endregion

        #region Constructor

        public GUIList()
        {
            backdrop = new ImageSwapper();
            backdrop.PropertyOne = "#Filmtipset.Fanart.1";
            backdrop.PropertyTwo = "#Filmtipset.Fanart.2";
        }

        #endregion

        #region Protected Variables

        protected int PreviousSelectedIndex { get; set; }
        protected ImageSwapper backdrop;
        protected DateTime LastRequest = new DateTime();

       protected Account CurrentUser = Helpers.GetDefaultUser();
        protected Layout CurrentLayout { get; set; }
        protected string currentListId = string.Empty;
        protected int currentGrade = 5;
        FilmtipsetAPIAction CurrentAction { get; set; }
        FilmtipsetAPIGenre CurrentGenre { get; set; }


        protected MoviesData Movies
        {
            get
            {
                if (_Movies == null || LastRequest < DateTime.UtcNow.Subtract(new TimeSpan(0, FilmtipsetSettings.WebRequestCacheMinutes, 0)))
                {
                    ImageDownloader.Instance.StopDownloads = true;

                    switch (CurrentAction)
                    {
                        case FilmtipsetAPIAction.list:
                            string[] doSort = { FilmtipsetAPIListType.bio.ToString(), FilmtipsetAPIListType.video.ToString() };
                            _Movies = FilmtipsetAPI.Instance.GetList(currentListId, currentListId == FilmtipsetAPIListType.grades.ToString() ? 5 : 0);
                            if (doSort.Contains(currentListId))
                            {
                                _Movies.Movies.Sort((x, y) => string.Compare(y.Movie.Grade.Value, x.Movie.Grade.Value));
                            }
                            break;
                        case FilmtipsetAPIAction.recommendations:
                            _Movies = FilmtipsetAPI.Instance.GetRecommendedMovies(CurrentGenre);
                            break;
                    }
                    LastRequest = DateTime.UtcNow;
                    PreviousSelectedIndex = 0;
                }
                return _Movies;
            }
            set
            {
                _Movies = value;
            }
        }
        protected MoviesData _Movies;

        #endregion

        #region overrides
        public override int GetID
        {
            get { return (int)FilmtipsetGUIWindows.List; }
        }

        public override bool Init()
        {
            return Load(GUIGraphicsContext.Skin + @"\Filmtipset.List.xml");
        }

        protected override void OnPageLoad()
        {
            base.OnPageLoad();
            ImageDownloader.Instance.StopDownloads = true;

            if (_loadParameter != null)
            {
                FilmtipsetLoadParam loadParams = _loadParameter.FromJSON<FilmtipsetLoadParam>();
                if (loadParams != null)
                {

                    int a = loadParams.Action;
                    if (Enum.IsDefined(typeof(FilmtipsetAPIAction), a))
                    {
                        if (CurrentAction != (FilmtipsetAPIAction)a)
                            Movies = null;

                        CurrentAction = (FilmtipsetAPIAction)a;
                        switch (CurrentAction)
                        {
                            case FilmtipsetAPIAction.list:
                                string listId = loadParams.ListId;
                                if (currentListId != listId)
                                {
                                    Movies = null;
                                }
                                currentListId = listId;
                                break;
                            case FilmtipsetAPIAction.recommendations:
                                break;
                        }

                        GUICommon.SetProperty("#header.label", loadParams.Title);
                    }
                }
            }
            if (CurrentUser.Id != FilmtipsetSettings.CurrentAccount.Id)
            {
                if (FilmtipsetSettings.Accounts.Any(a => a.Id == FilmtipsetSettings.CurrentAccount.Id))
                    CurrentUser = FilmtipsetSettings.CurrentAccount;
                else
                    CurrentUser = Helpers.GetDefaultUser();
                _Movies = null;
            }

            // Clear GUI Properties
            ClearProperties();

            // Init Properties
            InitProperties();

            // Load Movies
            LoadMovies();

        }

        protected override void OnPageDestroy(int new_windowId)
        {
            ImageDownloader.Instance.StopDownloads = true;
            PreviousSelectedIndex = Facade.SelectedListItemIndex;
            // save settings
            if (FilmtipsetSettings.Accounts.Any(a => a.Id == CurrentUser.Id))
            {
                int i = FilmtipsetSettings.Accounts.FindIndex(a => a.Id == CurrentUser.Id);
                FilmtipsetSettings.Accounts[i].Layout = (int)CurrentLayout;
                FilmtipsetSettings.Accounts[i].RecommendationGenre = (int)CurrentGenre;
                FilmtipsetSettings.Accounts[i].grade = currentGrade;
            }
            base.OnPageDestroy(new_windowId);
        }

        protected override void OnClicked(int controlId, GUIControl control, MediaPortal.GUI.Library.Action.ActionType actionType)
        {
            // wait for any background action to finish
            if (Gui2UtilConnector.Instance.IsBusy) return;
            switch (controlId)
            {
                case (2):
                    //member
                    Account newCurrentUser = GUICommon.ShowAccountMenu(CurrentUser);
                    if (newCurrentUser.Id != CurrentUser.Id)
                    {
                        CurrentUser = newCurrentUser;
                        //keep genre and grade todo check action
                        CurrentUser.RecommendationGenre = (int)CurrentGenre;
                        CurrentUser.grade = currentGrade;
                        CurrentLayout = (Layout)CurrentUser.Layout;
                        //save current user
                        FilmtipsetSettings.CurrentAccount = CurrentUser;
                        //init props...
                        InitProperties();
                        ReloadMovies();
                    }
                    break;
                case (3):
                    CurrentLayout = GUICommon.ShowLayoutMenu(CurrentLayout, PreviousSelectedIndex);
                    CurrentUser.Layout = (int)CurrentLayout;
                    break;
                case (4): //Genre
                    ShowGenreMenu();
                    break;
                case (5): //Grade
                    break;
                case (50):
                    if (actionType == MediaPortal.GUI.Library.Action.ActionType.ACTION_SELECT_ITEM)
                    {
                        GUIListItem selectedItem = this.Facade.SelectedListItem;
                        if (selectedItem == null) return;

                        Movie selectedMovie = selectedItem.TVTag as Movie;
                        if (selectedMovie == null) return;

                        GUIWindowManager.ActivateWindow((int)FilmtipsetGUIWindows.Movie, selectedMovie.Id.ToString());
                    }
                    break;
                default:
                    break;
            }
        }

        protected override void OnShowContextMenu()
        {
            if (Gui2UtilConnector.Instance.IsBusy) return;
            GUIFilmtipsetListItem selectedItem = this.Facade.SelectedListItem as GUIFilmtipsetListItem;
            if (selectedItem == null) return;
            ShowContextMenu(selectedItem);
            base.OnShowContextMenu();
        }

        #endregion

        #region protected methods

        protected void ShowGenreMenu()
        {
            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            dlg.Reset();
            dlg.SetHeading(GenreItemName(CurrentGenre));

            Dictionary<string, FilmtipsetAPIGenre> items = new Dictionary<string, FilmtipsetAPIGenre>();
            foreach (string genre in Enum.GetNames(typeof(FilmtipsetAPIGenre)))
            {
                FilmtipsetAPIGenre g = (FilmtipsetAPIGenre)Enum.Parse(typeof(FilmtipsetAPIGenre), genre);
                string menuItem = GenreItemName(g);
                GUIListItem pItem = new GUIListItem(menuItem);
                items.Add(menuItem, g);
                if (g == CurrentGenre) pItem.Selected = true;
                dlg.Add(pItem);
            }

            dlg.DoModal(GUIWindowManager.ActiveWindow);

            if (dlg.SelectedLabel >= 0)
            {
                FilmtipsetAPIGenre g = items[dlg.SelectedLabelText];
                if (g != CurrentGenre)
                {
                    CurrentGenre = g;
                    GUIControl.SetControlLabel(GetID, genreButton.GetID, dlg.SelectedLabelText);
                    CurrentUser.RecommendationGenre = (int)CurrentGenre;
                    ReloadMovies();
                }
            }

        }
        private string GenreItemName(FilmtipsetAPIGenre Genre)
        {
            return string.Format(GUI.Translation.GenreItem, GUI.Translation.GetByName("Genre" + Genre.ToString()));
        }

        protected void ShowContextMenu(GUIFilmtipsetListItem selectedItem)
        {
            Movie selectedMovie = (Movie)selectedItem.TVTag;
            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg == null) return;

            dlg.Reset();
            dlg.SetHeading(GUIUtils.PluginName());

            GUIListItem listItem = null;


            if (!string.IsNullOrEmpty(CurrentUser.ApiKey))
            {
                listItem = new GUIListItem(selectedItem.IsPlayed ? Translation.GetByName("GradeChangeHeading") : Translation.GetByName("GradeSetHeading"));
                dlg.Add(listItem);
                listItem.ItemId = (int)ContextMenuItem.Rate;
            }

            if (ExternalPlugins.IsTrailersAvailableAndEnabled)
            {
                // Trailers
                listItem = new GUIListItem(Translation.Trailers);
                dlg.Add(listItem);
                listItem.ItemId = (int)ContextMenuItem.Trailers;
            }

            if (ExternalPlugins.IsOnlineVideosAvailableAndEnabled)
            {
                // OV
                listItem = new GUIListItem(Translation.OnlineVideosSearch + ": " + selectedMovie.Name);
                dlg.Add(listItem);
                listItem.ItemId = (int)ContextMenuItem.OnlineVideosTitle;
                if (!string.IsNullOrEmpty(selectedMovie.OrgName) && selectedMovie.Name != selectedMovie.OrgName)
                {
                    listItem = new GUIListItem(Translation.OnlineVideosOrgName + ": " + selectedMovie.OrgName);
                    dlg.Add(listItem);
                    listItem.ItemId = (int)ContextMenuItem.OnlineVideosOrgTitle;
                }
            }

            if (ExternalPlugins.IsTvWishListMPAvailableAndEnabled)
            {
                // TvWishList
                listItem = new GUIListItem("Lägg till TvWish"); //Todo
                dlg.Add(listItem);
                listItem.ItemId = (int)ContextMenuItem.TvWish;
            }

            // Show Context Menu
            dlg.DoModal(GUIWindowManager.ActiveWindow);
            if (dlg.SelectedId < 0) return;

            switch (dlg.SelectedId)
            {
                case ((int)ContextMenuItem.Rate):
                    if (!string.IsNullOrEmpty(CurrentUser.ApiKey))
                    {
                        GUICommon.DoRating(selectedItem, this.GetID, 50);
                        //Force update of movies next time (even if no grade has been set...)
                        _Movies = null;
                    }
                    break;
                case ((int)ContextMenuItem.Trailers):
                    if (ExternalPlugins.IsTrailersAvailableAndEnabled)
                    {
                        GUICommon.CallTrailersPlugin(selectedMovie, selectedItem);
                    }
                    break;
                case ((int)ContextMenuItem.TvWish):
                    if (ExternalPlugins.IsTvWishListMPAvailableAndEnabled)
                    {
                        GUICommon.MakeTvWish(selectedMovie);
                    }
                    break;
                case ((int)ContextMenuItem.OnlineVideosTitle):
                    if (ExternalPlugins.IsOnlineVideosAvailableAndEnabled)
                    {
                        GUICommon.SearchOnlineVideos(selectedMovie.Name);
                    }
                    break;
                case ((int)ContextMenuItem.OnlineVideosOrgTitle):
                    if (ExternalPlugins.IsOnlineVideosAvailableAndEnabled)
                    {
                        GUICommon.SearchOnlineVideos(selectedMovie.OrgName);
                    }
                    break;

                default:
                    break;
            }
        }

        protected void InitProperties()
        {
            // Fanart
            backdrop.GUIImageOne = FanartBackground;
            backdrop.GUIImageTwo = FanartBackground2;
            backdrop.LoadingImage = loadingImage;

            //First load the user todo havent i done this allready...
            //CurrentUser = FilmtipsetSettings.Accounts.FirstOrDefault(ac => ac.Id == FilmtipsetSettings.CurrentAccount.Id) ?? Helpers.GetDefaultUser();
            GUIControl.SetControlLabel(GetID, memberButton.GetID, GUICommon.GetAccountTranslation(CurrentUser));
            GUICommon.SetProperty("#Filmtipset.User.Name", CurrentUser.Name);


            // load last layout
            CurrentLayout = (Layout)CurrentUser.Layout;
            // update button label
            GUIControl.SetControlLabel(GetID, layoutButton.GetID, GUICommon.GetLayoutTranslation(CurrentLayout));

            //load genre
            CurrentGenre = (FilmtipsetAPIGenre)CurrentUser.RecommendationGenre;
            GUIControl.SetControlLabel(GetID, genreButton.GetID, GenreItemName(CurrentGenre));
            GUICommon.SetProperty("#Filmtipset.CurrentGenre.Label", Translation.GetByName("Genre" + CurrentGenre.ToString()));

            currentGrade = CurrentUser.grade;


        }

        protected void ClearProperties()
        {
            GUIUtils.SetProperty("#Filmtipset.User.Name", "");
            GUIUtils.SetProperty("#Filmtipset.Show.Grade", "");
            GUIUtils.SetProperty("#Filmtipset.Show.Genre", "");
            GUICommon.ClearMovieProperties();
        }

        protected void PublishMovieSkinProperties(Movie movie)
        {
            GUICommon.SetMovieProperties(movie);
        }

        protected void SendMoviesToFacade(IEnumerable<Movie> movies)
        {
            // clear facade
            GUIControl.ClearControl(GetID, Facade.GetID);

            if (movies.Count() == 0)
            {
                GUIUtils.ShowNotifyDialog(GUIUtils.PluginName(), Translation.GetByName("NoMoviesFound"));
                return;
            }

            var movieList = movies.ToList();

            List<MovieImages> movieImages = new List<MovieImages>();

            foreach (var movie in movieList)
            {
                GUIFilmtipsetListItem item = new GUIFilmtipsetListItem(movie.Name, GetID);

                int grade = 0;
                int.TryParse(movie.Grade.Value, out grade);
                item.TVTag = movie;
                item.Item = movie.Images;
                item.ItemId = movie.Id + FilmtipsetSettings.MovieItemIdPadding;
                item.IsPlayed = movie.Grade.Type == GradeType.seen.ToString();
                item.IconImage = GUIImageHandler.GetGradeIcon(grade);
                item.IconImageBig = GUIImageHandler.GetDefaultPoster(false);
                item.ThumbnailImage = GUIImageHandler.GetDefaultPoster();
                item.PinImage = GUIImageHandler.GetWatchedIcon(item.IsPlayed);
                item.OnItemSelected += OnMovieSelected;
                Utils.SetDefaultIcons(item);
                Facade.Add(item);
                // add image for download
                movieImages.Add(movie.Images);
            }

            // Set Facade Layout
            Facade.SetCurrentLayout(Enum.GetName(typeof(Layout), CurrentLayout));
            GUIControl.FocusControl(GetID, Facade.GetID);

            if (PreviousSelectedIndex >= movies.Count())
                Facade.SelectIndex(PreviousSelectedIndex - 1);
            else
                Facade.SelectIndex(PreviousSelectedIndex);

            // set facade properties
            GUIUtils.SetProperty("#itemcount", movies.Count().ToString());

            // Download movie images Async and set to facade
            ImageDownloader.Instance.GetImages(movieImages);
        }

        protected void LoadMovies()
        {
            //todo move? But they work here...
            GUICommon.SetProperty("#Filmtipset.User.Name", CurrentUser.Name);
            GUIUtils.SetProperty("#Filmtipset.Show.Grade", (CurrentAction == FilmtipsetAPIAction.list && currentListId == FilmtipsetAPIListType.grades.ToString()) ? "show" : "");
            GUIUtils.SetProperty("#Filmtipset.Show.Genre", (CurrentAction == FilmtipsetAPIAction.recommendations) ? "show" : "");

            Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(() =>
            {
                return Movies;
            },
            delegate(bool success, object result)
            {
                if (success)
                {
                    MoviesData movieData = result as MoviesData;
                    List<Movie> movies = new List<Movie>();

                    if (movieData.Movies != null)
                    {
                        foreach (MovieWrapper mw in movieData.Movies)
                        {
                            movies.Add(mw.Movie);
                        }
                    }
                    //SetListProperties();
                    SendMoviesToFacade(movies);
                }
            }, "Getting movies", true); //todo
        }

        protected void ReloadMovies()
        {
            PreviousSelectedIndex = this.Facade.SelectedListItemIndex;
            ClearProperties();
            GUIControl.ClearControl(GetID, Facade.GetID);
            _Movies = null;
            LoadMovies();
        }

        protected void OnMovieSelected(GUIListItem item, GUIControl parent)
        {
            PreviousSelectedIndex = Facade.SelectedListItemIndex;
            Movie movie = item.TVTag as Movie;
            PublishMovieSkinProperties(movie);
            GUIImageHandler.LoadFanart(backdrop, movie.Images.FanartImageFilename);
        }

        #endregion

    }
}
