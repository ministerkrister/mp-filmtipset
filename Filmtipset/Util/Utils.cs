using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MediaPortal.Profile;
using MediaPortal.Configuration;
using Filmtipset.Models;
using Filmtipset.GUI;

namespace Filmtipset.Util
{
    internal class ExternalPlugins
    {
        public static bool IsPluginEnabled(string name)
        {
            using (Settings xmlreader = new MPSettings())
            {
                return xmlreader.GetValueAsBool("plugins", name, false);
            }
        }

        public static bool IsOnlineVideosAvailableAndEnabled
        {
            get
            {
                return File.Exists(Path.Combine(Config.GetSubFolder(Config.Dir.Plugins, "Windows"), "OnlineVideos.MediaPortal1.dll")) && (IsPluginEnabled("Online Videos") || IsPluginEnabled("OnlineVideos"));
            }
        }

        public static bool IsTrailersAvailableAndEnabled
        {
            get
            {
                return File.Exists(Path.Combine(Config.GetSubFolder(Config.Dir.Plugins, "Windows"), "Trailers.dll")) && (IsPluginEnabled("Trailers"));
            }
        }

        public static bool IsTvWishListMPAvailableAndEnabled
        {
            get
            {
                return File.Exists(Path.Combine(Config.GetSubFolder(Config.Dir.Plugins, "Windows"), "TvWishListMP.dll")) && File.Exists(Path.Combine(Config.GetFolder(Config.Dir.Base), "TvBusinessLayer.dll")) && File.Exists(Path.Combine(Config.GetFolder(Config.Dir.Base), "TVDatabase.dll")) && (IsPluginEnabled("TvWishListMP"));
            }
        }
    }

    internal class Helpers
    {
        public static Account GetDefaultUser()
        {
            return new Account() { ApiKey = "", Id = 0, Name = Translation.GetByName("MemberNoMember"), Layout = (int)Filmtipset.GUI.Layout.List, ListId = (int)Filmtipset.API.FilmtipsetAPIListType.bio, ListsId = "418"/*Startpaket 1*/, RecommendationGenre = (int)Filmtipset.API.FilmtipsetAPIGenre.GenerellaTips, grade = 5 }; //Todo translation
        }
    }
}
