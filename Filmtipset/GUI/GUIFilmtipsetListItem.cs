using Filmtipset.Models;
using Filmtipset.Util;
using MediaPortal.GUI.Library;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Filmtipset.GUI
{
    public class GUIFilmtipsetListItem : GUIListItem
    {
        public GUIFilmtipsetListItem(string strLabel, int windowId) : base(strLabel) { WindowID = windowId; }

        public int WindowID { get; set; }

        /// <summary>
        /// Sends a Thread message to select an item on a facade object. Will only send if itemid parameter is currently selected
        /// </summary>
        /// <param name="self">the list object</param>
        /// <param name="windowId">the window id containing list control</param>
        /// <param name="index">the item id in list to check if selected</param>
        /// <param name="controlId">the id of the list control, defaults to 50</param>
        public void UpdateItemIfSelected(int windowId, int index, int controlId = 50)
        {
            if (GUIWindowManager.ActiveWindow != windowId) return;

            GUIListItem selectedItem = GUIControl.GetSelectedListItem(windowId, controlId);

            // only send message if the current item is selected
            if (selectedItem == this)
            {
                GUIWindowManager.SendThreadMessage(new GUIMessage(GUIMessage.MessageType.GUI_MSG_ITEM_SELECT, GUIWindowManager.ActiveWindow, 0, controlId, index, 0, null));
            }
        }


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
