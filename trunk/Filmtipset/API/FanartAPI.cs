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
    internal sealed class FanartAPI
    {
        #region constants
        private const string apiUrl = @"http://webservice.fanart.tv/v3/movies/";
        private const string proxyApiUrl = @"http://private-anon-1266fcffb-fanarttv.apiary-proxy.com/";
        //please no not use my key! Get your own at fanart.tv
        private const string fanartTvKey = "622887f5ccb37f84dd2812db41f54b83";
        #endregion

        #region Private parts
        private IDictionary<string, FanartResponse> cache;
        #endregion

        # region Singleton
        private FanartAPI()
        {
            cache = new Dictionary<string, FanartResponse>();
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

        #region public
        private FanartResponse GetFanart(string imdb)
        {
            lock (lockObj)
            {
                if (string.IsNullOrEmpty(imdb))
                    return null;
                imdb = "tt" + imdb.Replace("tt", string.Empty);
                if (!cache.ContainsKey(imdb))
                {
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
                        cache.Add(imdb, json.FromJSON<FanartResponse>());
                    }
                    catch (Exception e)
                    {
                        cache.Add(imdb, null);
                        if (e.GetType().Name == "WebException")
                        {
                            WebException we = (WebException)e;
                            HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;
                            if (response.StatusCode == HttpStatusCode.NotFound)
                            {
                                Log.Debug("[Filmtipset] No fanart found for {0}", imdb);
                                Log.Debug(string.Format("[Filmtipset] Adding to cache: {0} notfound", imdb));
                            }
                            else
                            {
                                Log.Error(string.Format("[Filmtipset] Error getting fanart for {0}, {1}", imdb, we.Message));
                                Log.Debug(string.Format("[Filmtipset] Adding null to cache: {0} error", imdb));
                            }
                        }
                        else
                        {
                            Log.Error(string.Format("[Filmtipset] Error getting fanart for {0}, {1}", imdb, e.Message));
                            Log.Debug(string.Format("[Filmtipset] Adding null to cache: {0} error", imdb));
                        }

                    }
                }
                return cache.ContainsKey(imdb) ? cache[imdb] : null;
            }
        }
    
        public string GetFanartBackgroundUrl(string imdb, string language)
        {
            FanartResponse fanart = GetFanart(imdb);
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

        public string GetFanartPosterUrl(string imdb, string language)
        {
            FanartResponse fanart = GetFanart(imdb);
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
    }
}
