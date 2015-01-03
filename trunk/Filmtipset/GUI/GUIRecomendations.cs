using System;
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
    public class GUIRecomendations : GUIWindow
    {
        #region Skin Controls

        [SkinControl(2)]
        protected GUIButtonControl memberButton = null;

        [SkinControl(3)]
        protected GUIButtonControl layoutButton = null;

        [SkinControl(4)]
        protected GUIButtonControl genreButton = null;

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

        public GUIRecomendations()
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

        FilmtipsetAPIGenre CurrentGenre { get; set; }
        protected Account CurrentUser = Helpers.GetDefaultUser();
        protected Layout CurrentLayout { get; set; }

        protected MoviesData Movies
        {
            get
            {
                if (_Movies == null || LastRequest < DateTime.UtcNow.Subtract(new TimeSpan(0, FilmtipsetSettings.WebRequestCacheMinutes, 0)))
                {
                    _Movies = FilmtipsetAPI.Instance.GetRecommendedMovies(CurrentGenre);
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
            get { return (int)FilmtipsetGUIWindows.Recommendations; }
        }

        public override bool Init()
        {
            return Load(GUIGraphicsContext.Skin + @"\Filmtipset.Recommendations.xml");
        }

        protected override void OnPageLoad()
        {
            base.OnPageLoad();

            if (CurrentUser.Id != FilmtipsetSettings.CurrentAccount.Id)
            {
                if (FilmtipsetSettings.Accounts.Any(a => a.Id == FilmtipsetSettings.CurrentAccount.Id))
                    CurrentUser = FilmtipsetSettings.CurrentAccount;
                else
                    CurrentUser = Helpers.GetDefaultUser();
                Movies = null;
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
                        CurrentUser.RecommendationGenre = (int)CurrentGenre;
                        CurrentLayout = (Layout)CurrentUser.Layout;
                        GUIControl.SetControlLabel(GUIWindowManager.ActiveWindow, (int)FilmtipsetGUIControls.Layout, GUICommon.GetLayoutTranslation(CurrentLayout));
                        FilmtipsetSettings.CurrentAccount = CurrentUser;
                        ReloadMovies();
                    }
                    break;
                case (3):
                    CurrentLayout = GUICommon.ShowLayoutMenu(CurrentLayout, PreviousSelectedIndex);
                    CurrentUser.Layout = (int)CurrentLayout;
                    break;
                case (4):
                    ShowGenreMenu();

                    break;
                case (5):
                    //reload
                    ReloadMovies();
                    break;
                default:
                    break;
            }
        }

        protected override void OnShowContextMenu()
        {
            if (Gui2UtilConnector.Instance.IsBusy) return;
            GUIListItem selectedItem = this.Facade.SelectedListItem;
            if (selectedItem == null) return;
            selectedItem.ShowContextMenu(CurrentUser);
            base.OnShowContextMenu();
        }

        #endregion

        protected void InitProperties()
        {
            // Fanart
            backdrop.GUIImageOne = FanartBackground;
            backdrop.GUIImageTwo = FanartBackground2;
            backdrop.LoadingImage = loadingImage;

            //First load the user
            CurrentUser = FilmtipsetSettings.Accounts.FirstOrDefault(ac => ac.Id == FilmtipsetSettings.CurrentAccount.Id) ?? Helpers.GetDefaultUser();
            GUIControl.SetControlLabel(GetID, memberButton.GetID, GUICommon.GetAccountTranslation(CurrentUser));

            // load last layout
            CurrentLayout = (Layout)CurrentUser.Layout;
            // update button label
            GUIControl.SetControlLabel(GetID, layoutButton.GetID, GUICommon.GetLayoutTranslation(CurrentLayout));

            CurrentGenre = (FilmtipsetAPIGenre)CurrentUser.RecommendationGenre;
            GUIControl.SetControlLabel(GetID, genreButton.GetID, GenreItemName(CurrentGenre));
        }

        protected void ClearProperties()
        {
            GUICommon.SetProperty("#Filmtipset.CurrentGenre.Label", " ");
            GUICommon.ClearMovieProperties();
        }

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
                    GUICommon.SetProperty("#Filmtipset.CurrentGenre.Label", dlg.SelectedLabelText);

                    CurrentUser.RecommendationGenre = (int)CurrentGenre;
                    ReloadMovies();
                }
            }

        }

        protected void PublishMovieSkinProperties(Movie movie)
        {
            /*
             */
            GUICommon.SetMovieProperties(movie);
        }

        protected void SendMoviesToFacade(IEnumerable<Movie> movies)
        {
            // clear facade
            GUIControl.ClearControl(GetID, Facade.GetID);

            if (movies.Count() == 0)
            {
                GUIUtils.ShowNotifyDialog(GUIUtils.PluginName(), Translation.GetByName("NoMoviesFound"));
                GUIControl.FocusControl(GetID, genreButton.GetID);
                return;
            }

            var movieList = movies.ToList();

            List<MovieImages> movieImages = new List<MovieImages>();

            foreach (var movie in movieList)
            {
                GUIFilmtipsetRecommendetionItem item = new GUIFilmtipsetRecommendetionItem(movie.Name, GetID);

                int grade = 0;
                int.TryParse(movie.Grade.Value, out grade);
                item.TVTag = movie;
                item.Item = movie.Images;
                item.ItemId = movie.Id;
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


        #region protected methods

        protected void LoadMovies()
        {
            GUICommon.SetProperty("#Filmtipset.CurrentGenre.Label", GenreItemName(CurrentGenre));

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
            }, "Getting rec. movies", true); 
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

        private string GenreItemName(FilmtipsetAPIGenre Genre)
        {
            return string.Format(GUI.Translation.GenreItem, GUI.Translation.GetByName("Genre" + Genre.ToString()));
        }
 
        #endregion

    }
}
