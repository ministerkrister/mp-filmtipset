using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TvDatabase;
using MediaPortal.Plugins.TvWishList;
using MediaPortal.GUI.Library;
using Filmtipset.Models;

namespace Filmtipset.TvWishList
{


    internal enum TvWishesCodes
    {
        ok,
        error,
        timeout
    }

    internal class TvWishes
    {

        public TvWishesCodes MakeWish(Movie movie)
        {
            TvBusinessLayer layer = new TvBusinessLayer();
            Setting setting;
            TvWishProcessing myTvWishes = new TvWishProcessing();
            myTvWishes.Debug = true;
            try
            {
                //*****************************************************
                //Initialize once
                setting = layer.GetSetting("TvWishList_ColumnSeparator", ";");
                char TV_WISH_COLUMN_SEPARATOR = setting.Value[0];
                //default pre and post record from general recording settings
                setting = layer.GetSetting("preRecordInterval", "5");
                string prerecord = setting.Value;
                setting = layer.GetSetting("postRecordInterval", "5");
                string postrecord = setting.Value;
                var all = ChannelGroup.ListAll();
                myTvWishes.TvServerSettings(prerecord, postrecord, ChannelGroup.ListAll(), RadioChannelGroup.ListAll(), Channel.ListAll(), Card.ListAll(), TV_WISH_COLUMN_SEPARATOR);
                //*****************************************************
                //Lock TvWishList with timeout error
                bool success = false;
                int seconds = (FilmtipsetSettings.WebRequestTimeout / 1000) - 5;
                if (seconds < 10)
                    seconds = 0;
                for (int i = 0; i < seconds / 10; i++)
                {
                    success = myTvWishes.LockTvWishList("Filmtipset");
                    if (success)
                        break;
                    System.Threading.Thread.Sleep(10000); //sleep 10s to wait for BUSY=false 
                    Log.Debug("[Filmtipset] Waiting for old jobs " + (seconds - i * 10).ToString() + "s to finish", (int)LogSetting.DEBUG);
                }
                if (success == false)
                {
                    Log.Debug("Timeout Error: TvWishList did not finish old jobs - aborting", (int)LogSetting.DEBUG);
                    return TvWishesCodes.timeout;
                }
                //*****************************************************
                //Load Tv Wishes
                Log.Debug("Loading listview data");
                string listviewdata = "";
                bool VIEW_ONLY_MODE = !FilmtipsetSettings.TvWishListEmail; //use false for EmailMode and true for ViewOnlyMode
                if (VIEW_ONLY_MODE == true)
                {
                    listviewdata = myTvWishes.loadlongsettings("TvWishList_OnlyView");
                    //do never modify keywords must match MP plugin
                }
                else
                {
                    listviewdata = myTvWishes.loadlongsettings("TvWishList_ListView");
                }
                myTvWishes.Clear();
                myTvWishes.LoadFromString(listviewdata, true);

                //*****************************************************
                //Add a new Tvwish
                string name = string.Format("[Filmtipset] {0} - {1}", movie.Name, movie.OrgName);
                int wishId = myTvWishes.RetrieveByName(name);
                TvWish newwish;
                if (wishId > -1)
                    newwish = myTvWishes.RetrieveById(wishId.ToString());
                else
                    newwish = myTvWishes.DefaultData();//create new wish with defaultdata 

                string query;
                switch ((int)FilmtipsetSettings.TvWishSearchLogic)
                {
                    case ((int)TvWishesSearchSetting.title_TitleOrOrgTitle_And_Description_Year):
                        query = string.Format("((title = '{0}' OR title = '{1}') AND (description like '%{2}%'))", movie.Name, movie.OrgName, movie.Year);
                        break;
                    case ((int)TvWishesSearchSetting.title_Title_Or_Description_OrgTitleAndYear):
                        query = string.Format("((title = '{0}') OR (description like '%{1}%' AND description like '%{2}%'))", movie.Name, movie.OrgName, movie.Year);
                        break;
                    case ((int)TvWishesSearchSetting.title_TitleOrOrgTitle):
                    default:
                        query = string.Format("(title = '{0}' OR title = '{1}')", movie.Name, movie.OrgName);
                        break;
                }
                newwish.searchfor = query;
                newwish.name = name;
                newwish.matchtype = "Expression";
                if (wishId > -1)
                    myTvWishes.ReplaceAtTvWishId(wishId.ToString(), newwish);
                else
                    myTvWishes.Add(newwish);


                //*****************************************************
                //Save Tvwishes
                string listviewstring = myTvWishes.SaveToString();
                if (VIEW_ONLY_MODE == true) //use false for EmailMode and true for
                {
                    myTvWishes.save_longsetting(listviewstring, "TvWishList_OnlyView");
                    //do never modify keywords must match MP plugin
                }
                else
                {
                    myTvWishes.save_longsetting(listviewstring, "TvWishList_ListView");
                    //do never modify keywords must match MP plugin
                }
                //*****************************************************
                //unlock TvWishList
                myTvWishes.UnLockTvWishList();

                return TvWishesCodes.ok;

            }
            catch (Exception e)
            {
                Log.Error("[Filmtipset] Exception in adding TvWish: " + e.Message);
                return TvWishesCodes.error;
            }
        }
    }
}
