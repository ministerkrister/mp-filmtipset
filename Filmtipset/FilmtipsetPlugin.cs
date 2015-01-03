using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;
using Filmtipset.GUI;
using Filmtipset.Models;
using Filmtipset.Util;
using Filmtipset.Extensions;



namespace Filmtipset
{
    [PluginIcons("Filmtipset.Resources.Images.icon_normal.png", "Filmtipset.Resources.Images.icon_faded.png")]
    public class FilmtipsetPlugin : GUIWindow, ISetupForm
    {
        #region ISetupForm Members

        // Returns the author of the plugin which is shown in the plugin menu
        public string Author()
        {
            return "Ministerk";
        }

        // Indicates whether plugin can be enabled/disabled
        public bool CanEnable()
        {
            return true;
        }

        // Indicates if plugin is enabled by default;
        public bool DefaultEnabled()
        {
            return true;
        }

        // Returns the description of the plugin is shown in the plugin menu
        public string Description()
        {
            return "A Filmtipset plugin for MediaPortal. More info about Filmtipset at http://www.filmtipset.se";
        }

        /// <summary>
        /// If the plugin should have it's own button on the main menu of MediaPortal then it
        /// should return true to this method, otherwise if it should not be on home
        /// it should return false
        /// </summary>
        /// <param name="strButtonText">text the button should have</param>
        /// <param name="strButtonImage">image for the button, or empty for default</param>
        /// <param name="strButtonImageFocus">image for the button, or empty for default</param>
        /// <param name="strPictureImage">subpicture for the button or empty for none</param>
        /// <returns>true : plugin needs it's own button on home
        /// false : plugin does not need it's own button on home</returns>

        public bool GetHome(out string strButtonText, out string strButtonImage,
          out string strButtonImageFocus, out string strPictureImage)
        {
            strButtonText = PluginName();
            strButtonImage = String.Empty;
            strButtonImageFocus = String.Empty;
            strPictureImage = String.Empty;
            return true;
        }

        // Get Windows-ID
        public int GetWindowId()
        {
            // WindowID of windowplugin belonging to this setup
            // enter your own unique code
            return (int)FilmtipsetGUIWindows.Main;
        }

        // indicates if a plugin has it's own setup screen
        public bool HasSetup()
        {
            return true;
        }

        // Returns the name of the plugin which is shown in the plugin menu
        public string PluginName()
        {
            return GUIUtils.PluginName();
        }


        // show the setup dialog
        public void ShowPlugin()
        {
           Configuration config = new Configuration();
           config.ShowDialog();
        }


        #endregion

        #region skin controls
        [SkinControl(2)]
        protected GUIButtonControl userButton = null;
        [SkinControl(3)]
        protected GUIButtonControl recomendButton = null;
        #endregion

        #region GUIWindow

        public override string GetModuleName()
        {
            return GUIUtils.PluginName();
        }

        // With GetID it will be an window-plugin / otherwise a process-plugin
        // Enter the id number here again
        public override int GetID
        {
            get
            {
                return GetWindowId();
            }
            set { }
        }


        public override bool Init()
        {
            Translation.Init();
            FilmtipsetSettings.LoadSettings();
            return Load(GUIGraphicsContext.Skin + @"\Filmtipset.xml");
        }

        public override void DeInit()
        {
            if (Gui2UtilConnector.Instance.IsBusy) Gui2UtilConnector.Instance.StopBackgroundTask();
            FilmtipsetSettings.SaveSettings();
            base.DeInit();
        }

        protected override void OnPageLoad()
        {
            base.OnPageLoad();
            Account account = FilmtipsetSettings.Accounts.FirstOrDefault(a => a.Id == FilmtipsetSettings.CurrentAccount.Id) ?? Helpers.GetDefaultUser();
            GUIControl.SetControlLabel(GetID, userButton.GetID, account.Name);

            //GUIPropertyManager.SetProperty("#Filmtipset.recomendation.title", string.Empty);
            //GUIPropertyManager.SetProperty("#Filmtipset.seen.title", string.Empty);
            if (Gui2UtilConnector.Instance.IsBusy) return;
            /*
            Gui2UtilConnector.Instance.ExecuteInBackgroundAndCallback(delegate()
            {
                Dictionary<FilmtipsetAPIAction, IEnumerable<Movie>> ret = new Dictionary<FilmtipsetAPIAction, IEnumerable<Movie>>();
                ret.Add(FilmtipsetAPIAction.recommendations, FilmtipsetAPI.Instance.GetRecommendedMovies(FilmtipsetAPIGenre.Krig));
                //  ret.Add(FilmtipsetAPIAction.recommendations, FilmtipsetAPI.Instance.GetMovies(FilmtipsetAPIAction.list, FilmtipsetAPIGenre.GenerellaTips, FilmtipsetAPIListType.bio, FimtipsetApiGrade.all, 0, false, false));
                int[] ids = { 93258, 4943, 4942, 84114, 77430, 103777, 98927, 88604, 85957, 4717, 95164, 104593, 108096, 108899, 106467, 108581, 110887, 100770, 104595, 73443 };
                ret.Add(FilmtipsetAPIAction.movie, FilmtipsetAPI.Instance.GetMultipleMovie(ids));
                return ret;
            },
            delegate(bool success, object ret)
            {
                if (success)
                {
                    GUIControl.ClearControl(GetID, facadeLayoutsSeen.GetID);
                    GUIControl.ClearControl(GetID, facadeLayoutRecomendations.GetID);

                    GUIListItem item = null;
                    Dictionary<FilmtipsetAPIAction, IEnumerable<Movie>> returns = ret as Dictionary<FilmtipsetAPIAction, IEnumerable<Movie>>;
                    IEnumerable<Movie> recommendations = returns[FilmtipsetAPIAction.recommendations] as IEnumerable<Movie>;
                    IEnumerable<Movie> list = returns[FilmtipsetAPIAction.movie] as IEnumerable<Movie>;
                    if (recommendations != null)
                    {
                        foreach (Movie info in recommendations)
                        {
                            item = new GUIListItem();
                            item.RetrieveArt = false;
                            if (!string.IsNullOrEmpty(info.Image))
                            {
                                string thumb = Thumbs.Videos + @"\" + info.Id.ToString() + Path.GetExtension(info.Image);
                                if (System.IO.File.Exists(thumb))
                                {
                                    item.ThumbnailImage = thumb;
                                    item.IconImage = thumb;
                                }
                                else
                                {
                                    item.OnRetrieveArt += new GUIListItem.RetrieveCoverArtHandler(item_OnRetrieveArt);
                                    item.RetrieveArt = true;
                                }
                            }
                            item.Label = info.Name;
                            item.IsPlayed = info.Grade.Type == GradeType.seen.ToString();
                            item.TVTag = info;
                            item.ItemId = info.Id;
                            item.OnItemSelected += OnMovieSelected;
                            facadeLayoutRecomendations.Add(item);
                        }
                    }
                    if (list != null)
                    {
                        foreach (Movie info in list)
                        {
                            item = new GUIListItem();
                            item.RetrieveArt = false;
                            if (!string.IsNullOrEmpty(info.Image))
                            {
                                string thumb = Thumbs.Videos + @"\" + info.Id.ToString() + Path.GetExtension(info.Image);
                                if (System.IO.File.Exists(thumb))
                                {
                                    item.ThumbnailImage = thumb;
                                    item.IconImage = thumb;
                                }
                                else
                                {
                                    item.OnRetrieveArt += new GUIListItem.RetrieveCoverArtHandler(item_OnRetrieveArt);
                                    item.RetrieveArt = true;
                                }
                            }
                            item.Label = info.Name;
                            item.IsPlayed = info.Grade.Type == GradeType.seen.ToString();
                            item.TVTag = info;
                            item.ItemId = info.Id;
                            item.OnItemSelected += OnMovieSelected;
                            facadeLayoutsSeen.Add(item);
                            facadeLayoutFriends.Add(item);
                        }
                    }
                }
                facadeLayoutFriends.SelectedListItemIndex = 0;
                facadeLayoutsSeen.SelectedListItemIndex = 0;
                //facadeLayoutsSeen.
                facadeLayoutRecomendations.SelectedListItemIndex = 0;
                return;
            },
            "Hämta dashboard",
            true);*/
        }


        protected override void OnPageDestroy(int newWindowId)
        {
            if (Gui2UtilConnector.Instance.IsBusy) Gui2UtilConnector.Instance.StopBackgroundTask();
            base.OnPageDestroy(newWindowId);
        }

        protected override void OnClicked(int controlId, GUIControl control, MediaPortal.GUI.Library.Action.ActionType actionType)
        {
            // wait for any background action to finish
            if (Gui2UtilConnector.Instance.IsBusy) return;

            switch (controlId)
            {
                //user
                case (2):
                    IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
                    dlg.Reset();
                    string current = FilmtipsetSettings.CurrentAccount.Name;
                    dlg.SetHeading(current);
                    Account noAcc = Helpers.GetDefaultUser();
                    string menuItem = noAcc.Name;
                    GUIListItem pItem = new GUIListItem(menuItem);
                    if (menuItem == current) pItem.Selected = true;
                    dlg.Add(pItem);
                    foreach (Account account in FilmtipsetSettings.Accounts)
                    {
                        menuItem = account.Name;
                        pItem = new GUIListItem(menuItem);
                        if (menuItem == current) pItem.Selected = true;
                        dlg.Add(pItem);
                    }

                    dlg.DoModal(GUIWindowManager.ActiveWindow);

                    if (dlg.SelectedLabelText != current && !string.IsNullOrEmpty(dlg.SelectedLabelText))
                    {
                        FilmtipsetSettings.CurrentAccount = FilmtipsetSettings.Accounts.FirstOrDefault(a => a.Name == dlg.SelectedLabelText) ?? Helpers.GetDefaultUser();
                        GUIControl.SetControlLabel(GetID, this.userButton.GetID, dlg.SelectedLabelText);
                    }
                    break;
                // Recommend
                case (3):
                    GUIWindowManager.ActivateWindow((int)FilmtipsetGUIWindows.Recommendations);
                    break;
                default:
                    break;
            }
            base.OnClicked(controlId, control, actionType);
        }

        #endregion

    }
}
