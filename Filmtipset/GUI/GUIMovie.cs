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
using System.ComponentModel;

namespace Filmtipset.GUI
{
    public class GUIMovie : GUIWindow
    {
        #region Skin Controls

        [SkinControl(2)]
        protected GUIButtonControl memberButton = null;

        [SkinControlAttribute(60)]
        protected GUIImage FanartBackground = null;

        [SkinControlAttribute(61)]
        protected GUIImage FanartBackground2 = null;

        //[SkinControlAttribute(62)]
        protected GUIImage loadingImage = null;

        #endregion

        #region Constructor

        public GUIMovie()
        {
            backdrop = new ImageSwapper();
            backdrop.PropertyOne = "#Filmtipset.Fanart.1";
            backdrop.PropertyTwo = "#Filmtipset.Fanart.2";
        }

        #endregion

        #region Protected Variables

        protected ImageSwapper backdrop;
        protected DateTime LastRequest = new DateTime();

        protected Movie currentMovie = null;
        protected Account CurrentUser = Helpers.GetDefaultUser();

        protected Movie Movie
        {
            get
            {
                if (_Movie == null || LastRequest < DateTime.UtcNow.Subtract(new TimeSpan(0, FilmtipsetSettings.WebRequestCacheMinutes, 0)))
                {
                    ImageDownloader.Instance.StopDownloads = true;
                    IEnumerable<Movie> themovies = FilmtipsetAPI.Instance.GetMovie(currentMovie.Id.ToString());
                    _Movie = themovies.FirstOrDefault(m => m.Id == currentMovie.Id);
                    LastRequest = DateTime.UtcNow;
                }
                return _Movie;
            }
            set
            {
                _Movie = value;
            }
        }
        protected Movie _Movie;

        #endregion

        #region overrides
        public override int GetID
        {
            get { return (int)FilmtipsetGUIWindows.Movie; }
        }

        public override bool Init()
        {
            return Load(GUIGraphicsContext.Skin + @"\Filmtipset.Movie.xml");
        }

        protected override void OnPageLoad()
        {
            base.OnPageLoad();

            if (_loadParameter != null)
            {
                if (currentMovie == null || _loadParameter != currentMovie.Id.ToString())
                {
                    int id = 0;
                    int.TryParse(_loadParameter, out id);
                    currentMovie = new Movie() { Id = id };
                    _Movie = null;
                    GUICommon.SetProperty("#header.label", " ");
                }
            }
            if (currentMovie == null || currentMovie.Id < 1)
            {
                //TODO
                currentMovie = new Movie() { Id = 1250 };
                _Movie = null;
                GUICommon.SetProperty("#header.label", " ");
            }

            if (CurrentUser.Id != FilmtipsetSettings.CurrentAccount.Id)
            {
                if (FilmtipsetSettings.Accounts.Any(a => a.Id == FilmtipsetSettings.CurrentAccount.Id))
                    CurrentUser = FilmtipsetSettings.CurrentAccount;
                else
                    CurrentUser = Helpers.GetDefaultUser();
                _Movie = null;
            }

            // Clear GUI Properties
            ClearProperties();

            // Init Properties
            InitProperties();

            // Load Movie
            LoadMovie();
        }

        protected override void OnPageDestroy(int new_windowId)
        {
            ImageDownloader.Instance.StopDownloads = true;
            // save settings
            if (FilmtipsetSettings.Accounts.Any(a => a.Id == CurrentUser.Id))
            {

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
                        FilmtipsetSettings.CurrentAccount = CurrentUser;
                        ReloadMovie();
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region protected methods

        protected void InitProperties()
        {
            // Fanart
            backdrop.GUIImageOne = FanartBackground;
            backdrop.GUIImageTwo = FanartBackground2;
            backdrop.LoadingImage = loadingImage;

            //First load the user
            CurrentUser = FilmtipsetSettings.Accounts.FirstOrDefault(ac => ac.Id == FilmtipsetSettings.CurrentAccount.Id) ?? Helpers.GetDefaultUser();
            GUIControl.SetControlLabel(GetID, memberButton.GetID, GUICommon.GetAccountTranslation(CurrentUser));
            GUICommon.SetProperty("#Filmtipset.User.Name", CurrentUser.Name);
        }

        protected void ClearProperties()
        {
            GUICommon.SetProperty("#Filmtipset.User.Name", " ");
            GUICommon.ClearMovieProperties();
        }

        protected void LoadMovie()
        {
            GUICommon.SetProperty("#Filmtipset.User.Name", CurrentUser.Name);
            Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(() =>
            {
                return Movie;
            },
            delegate(bool success, object result)
            {
                if (success)
                {
                    currentMovie = result as Movie;
                    //Does this cleen?
                    GUIImageHandler.LoadFanart(backdrop, currentMovie.Images.FanartImageFilename);
                    INotifyPropertyChanged notifier = currentMovie.Images as INotifyPropertyChanged;
                    
                    if (notifier != null) notifier.PropertyChanged += (s, e) =>
                    {
                        if (s is MovieImages && e.PropertyName == "PosterImageFilename")
                        {
                            GUICommon.SetProperty("#Filmtipset.Poster", (s as MovieImages).PosterImageFilename);
                        }
                        if (s is MovieImages && e.PropertyName == "FanartImageFilename")
                        {
                            GUIImageHandler.LoadFanart(backdrop, currentMovie.Images.FanartImageFilename);
                        }

                    };
                    List<MovieImages> movieImages = new List<MovieImages>() { currentMovie.Images };
                    ImageDownloader.Instance.GetImages(movieImages);
                }
            }, "Getting movie", true); //TODO
        }

        protected void ReloadMovie()
        {
            ClearProperties();
            _Movie = null;
            LoadMovie();
        }

        #endregion
    }
}
