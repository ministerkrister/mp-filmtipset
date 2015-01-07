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
    }
}
