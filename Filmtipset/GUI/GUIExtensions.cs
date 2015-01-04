using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MediaPortal.GUI.Library;
using System.Reflection;
using Filmtipset.Models;
using Filmtipset.Util;

namespace Filmtipset.GUI
{
    public static class GUIWindowExtensions
    {
        private static Dictionary<string, PropertyInfo> propertyCache = new Dictionary<string, PropertyInfo>();
        
        public static void SetCurrentLayout(this GUIFacadeControl self, string layout)
        {
            PropertyInfo property = GetPropertyInfo<GUIFacadeControl>("CurrentLayout", "View");
            property.SetValue(self, Enum.Parse(property.PropertyType, layout), null);
        }

        /// <summary>
        /// Gets the property info object for a property using reflection.
        /// The property info object will be cached in memory for later requests.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="newName">The name of the property in 1.2</param>
        /// <param name="oldName">The name of the property in 1.1</param>
        /// <returns>instance PropertyInfo or null if not found</returns>
        public static PropertyInfo GetPropertyInfo<T>(string newName, string oldName)
        {
            PropertyInfo property = null;
            Type type = typeof(T);
            string key = type.FullName + "|" + newName;

            if (!propertyCache.TryGetValue(key, out property))
            {
                property = type.GetProperty(newName);
                if (property == null)
                {
                    property = type.GetProperty(oldName);
                }

                propertyCache[key] = property;
            }

            return property;
        }

        public static void SelectIndex(this GUIFacadeControl self, int index)
        {
            if (index > self.Count) index = 0;
            if (index == self.Count) index--;
            GUIMessage msg = new GUIMessage(GUIMessage.MessageType.GUI_MSG_ITEM_SELECT, self.WindowId, 0, self.GetID, index, 0, null);
            GUIGraphicsContext.SendMessage(msg);
        }
        /// <summary>
        /// Sends a Thread message to select an item on a facade object. Will only send if itemid parameter is currently selected
        /// </summary>
        /// <param name="self">the list object</param>
        /// <param name="windowId">the window id containing list control</param>
        /// <param name="index">the item id in list to check if selected</param>
        /// <param name="controlId">the id of the list control, defaults to 50</param>
        public static void UpdateItemIfSelected(this GUIListItem self, int windowId, int index, int controlId = 50)
        {
            if (GUIWindowManager.ActiveWindow != windowId) return;

            GUIListItem selectedItem = GUIControl.GetSelectedListItem(windowId, controlId);

            // only send message if the current item is selected
            if (selectedItem == self)
            {
                GUIWindowManager.SendThreadMessage(new GUIMessage(GUIMessage.MessageType.GUI_MSG_ITEM_SELECT, GUIWindowManager.ActiveWindow, 0, controlId, index, 0, null));
            }
        }

        public static void ShowContextMenu(this GUIListItem self, Account currentAccount)
        {
            Movie selectedMovie = (Movie)self.TVTag;
            IDialogbox dlg = (IDialogbox)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_MENU);
            if (dlg == null) return;

            dlg.Reset();
            dlg.SetHeading(GUIUtils.PluginName());

            GUIListItem listItem = null;


            if (!string.IsNullOrEmpty(currentAccount.ApiKey))
            {
                listItem = new GUIListItem("Betygsätt");
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
                  //  GUICommon.DoRating(selectedMovie);
                    break;
                case ((int)ContextMenuItem.Trailers):
                    if (ExternalPlugins.IsTrailersAvailableAndEnabled)
                    {
                        GUICommon.CallTrailersPlugin(selectedMovie, self);
                    }
                    break;
                case ((int)ContextMenuItem.TvWish):
                    if (ExternalPlugins.IsTvWishListMPAvailableAndEnabled)
                    {
                        GUICommon.MakeTvWish(selectedMovie);
                    }
                    break;
                case ((int)ContextMenuItem.OnlineVideosTitle):
                    GUICommon.SearchOnlineVideos(selectedMovie.Name);
                    break;
                case ((int)ContextMenuItem.OnlineVideosOrgTitle):
                    GUICommon.SearchOnlineVideos(selectedMovie.OrgName);
                    break;

                default:
                    break;
            }
        }
    }
}
