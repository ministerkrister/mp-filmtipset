using MediaPortal.Dialogs;
using MediaPortal.GUI.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Filmtipset.GUI
{
    public class GUIUtils
    {
        public static string PluginName()
        {
            return "Filmtipset";
        }

        #region properties
        public static void SetProperty(string property, string value)
        {
            // prevent ugly display of property names
            if (string.IsNullOrEmpty(value))
                value = " ";

            GUIPropertyManager.SetProperty(property, value);
        }

        public static string GetProperty(string property)
        {
            string propertyVal = GUIPropertyManager.GetProperty(property);
            return propertyVal ?? string.Empty;
        }

        #endregion

        #region Dialog
        internal static void ShowNotifyDialog(string p, string p_2)
        {
            GUIDialogNotify dlg = (GUIDialogNotify)GUIWindowManager.GetWindow((int)GUIWindow.Window.WINDOW_DIALOG_NOTIFY);
            if (dlg != null)
            {
                dlg.Reset();
                dlg.SetHeading(p);
                dlg.SetText(p_2);
                dlg.DoModal(GUIWindowManager.ActiveWindow);
            }
        }
        #endregion

    }
}
