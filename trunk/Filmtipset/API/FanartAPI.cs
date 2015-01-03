using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Net;
using Filmtipset.Models;
using Filmtipset.Extensions;
using MediaPortal.GUI.Library;

namespace Filmtipset.API
{
    internal class FanartCache
    {
        public string Imdb { get; set; }
        public string FanartUrl { get; set; }
        public string FanartPosterUrl { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    internal sealed class FanartAPI
    {
        #region constants
        private const string apiUrl = @"http://webservice.fanart.tv/v3/movies/";
        private const string proxyApiUrl = @"http://private-anon-1266fcffb-fanarttv.apiary-proxy.com/";
        //please no not use my key! Get your own at fanart.tv
        private const string fanartTvKey = "622887f5ccb37f84dd2812db41f54b83";
        #endregion

        #region Private parts
        private List<FanartCache> cache;
        private TimeSpan maxAge;
        #endregion

        # region Singleton
        private FanartAPI()
        {
            cache = new List<FanartCache>();
            maxAge = new TimeSpan(12, 0, 0);
        }

        private static volatile FanartAPI instance = null;
        private static object lockObj = new Object();
        internal static FanartAPI Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (lockObj)
                    {
                        if (instance == null)
                            instance = new FanartAPI();
                    }
                }

                return instance;
            }
        }
        #endregion

        #region private
        private FanartResponse GetFanart(string imdb)
        {
            FanartResponse fanart = null;
            if (string.IsNullOrEmpty(imdb))
                return null;
            imdb = "tt" + imdb.Replace("tt", string.Empty);
            WebClient webClient = new WebClient();
            webClient.Headers[HttpRequestHeader.Accept] = "application/json";
            if (FilmtipsetSettings.UseLocalDebugProxy) webClient.Proxy = new WebProxy(FilmtipsetSettings.LocalDebugProxyIp, FilmtipsetSettings.LocalDebugProxyPort);

            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("api_key", fanartTvKey);
            if (!string.IsNullOrEmpty(FilmtipsetSettings.PersonalFanartAPIKey.Trim()))
                parameters.Add("client_key", FilmtipsetSettings.PersonalFanartAPIKey.Trim());
            webClient.QueryString = parameters;
            try
            {
                string json = webClient.DownloadString(new Uri((FilmtipsetSettings.UseFanartTvApiProxy ? proxyApiUrl : apiUrl) + imdb));
                Log.Debug(string.Format("[Filmtipset] Adding to cache: {0} found", imdb));
                fanart = json.FromJSON<FanartResponse>();
            }
            catch (Exception e)
            {
                // Almost always 404, logging debug
                Log.Debug(string.Format("[Filmtipset] Error getting fanart for {0}, {1}", imdb, e.Message));
            }
            return fanart;
        }
    
        private string GetFanartBackgroundUrl(FanartResponse fanart, string language)
        {
            if (fanart != null && fanart.MovieBackgrounds != null)
            {
                List<Fanart> backgrounds = fanart.MovieBackgrounds;
                Fanart background = backgrounds.FirstOrDefault(f => f.Language == (language ?? "sv"));
                if (background == null)
                    background = backgrounds.FirstOrDefault(f => f.Language == "en");
                if (background == null)
                    background = backgrounds.FirstOrDefault();
                if (background == null)
                    return string.Empty;
                return background.Url;
            }
            return string.Empty;
        }

        private string GetFanartPosterUrl(FanartResponse fanart, string language)
        {
            if (fanart != null && fanart.MoviePosters != null)
            {
                List<Fanart> posters = fanart.MoviePosters;
                Fanart poster = posters.FirstOrDefault(f => f.Language == (language ?? "sv"));
                if (poster == null)
                    poster = posters.FirstOrDefault(f => f.Language == "en");
                if (poster == null)
                    poster = posters.FirstOrDefault();
                if (poster == null)
                    return string.Empty;
                return poster.Url;
            }
            return string.Empty;
        }

        #endregion

        #region public
        
        public void GetFanartUrls(string imdb, string language, out string fanartUrl, out string posterUrl)
        {
            FanartResponse fanart = GetFanart(imdb);
            fanartUrl = GetFanartBackgroundUrl(fanart, language);
            posterUrl = GetFanartPosterUrl(fanart, language);
        }

        public void GetFanartFromCache(string imdb, out bool notInCache, out string fanartUrl, out string posterUrl)
        {
            lock(lockObj)
            {
                cache.RemoveAll(c => (DateTime.Now - c.TimeStamp) > maxAge);
                FanartCache fc = cache.FirstOrDefault(c => c.Imdb == imdb);
                notInCache = fc == null;
                fanartUrl = string.Empty;
                posterUrl = string.Empty;
                if (!notInCache)
                {
                    fanartUrl = fc.FanartUrl;
                    posterUrl = fc.FanartPosterUrl;
                }
            }
        }

        public void AddToCache(string imdb, string fanartUrl, string posterUrl)
        {
            lock(lockObj)
            {
                FanartCache fc = new FanartCache() { Imdb = imdb, FanartPosterUrl = posterUrl, FanartUrl = fanartUrl, TimeStamp = DateTime.Now };
                cache.RemoveAll(c => c.Imdb == imdb);
                cache.Add(fc);
            }
        }
        #endregion

    }
}
