using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Collections.Specialized;
using Filmtipset.Models;
using Filmtipset.Extensions;
using MediaPortal.GUI.Library;

namespace Filmtipset.API
{
    internal class FilmtipsetAPI
    {
        #region constants
        #endregion

        #region Private parts
        // userid, movieid,the movie
        private IDictionary<int,IDictionary<int, MovieWrapper>> cache;
        #endregion


        # region Singleton
        protected FilmtipsetAPI()
        {
            //userKey = "ie9t8thm";
            //userId = null;
            //userName = null;
            cache = new Dictionary<int, IDictionary<int, MovieWrapper>>();
        }

        protected static FilmtipsetAPI instance = null;
        internal static FilmtipsetAPI Instance
        {
            get
            {
                if (instance == null) instance = new FilmtipsetAPI();
                return instance;
            }
        }
        #endregion

        #region GetWebData
        protected string GetWebData(NameValueCollection parameters, int usernr = 0)
        {
            string apiUrl = @"http://www.filmtipset.se/api/api.cgi";
            //please no not use my key! Get your own at filmtipset.se
            string accessKey = "ZDBA9vCFmMRPAhRXJ6Mmw";
            int userId = FilmtipsetSettings.CurrentAccount.Id;
            string userKey = FilmtipsetSettings.CurrentAccount.ApiKey;

            WebClient webClient = new WebClient();
            // Debug proxy for me...
            if (FilmtipsetSettings.UseLocalDebugProxy) webClient.Proxy = new WebProxy(FilmtipsetSettings.LocalDebugProxyIp, FilmtipsetSettings.LocalDebugProxyPort);

            if (parameters == null)
                parameters = new NameValueCollection();
            if (usernr > 0)
                parameters.Add("usernr", usernr.ToString());
            else if (!string.IsNullOrEmpty(userKey))
                parameters.Add("userkey", userKey);
            else if (userId > 0)
                parameters.Add("usernr", userId.ToString());

            parameters.Add("accesskey", accessKey);
            parameters.Add("returntype", FilmtipsetAPIReturntype.json.ToString());
            webClient.QueryString = parameters;
            string json;
            try
            {
                json = webClient.DownloadString(new Uri(apiUrl));
            }
            catch (Exception e)
            {
                if (e.GetType().Name == "WebException")
                {
                    WebException we = (WebException)e;
                    HttpWebResponse response = (System.Net.HttpWebResponse)we.Response;
                    Log.Error(string.Format("[Filmtipset] Error GetWebData. Response code: {0}. Message: {1}", response != null ? response.StatusCode.ToString() : "unknown", we.Message));
                }
                else
                {
                    Log.Error(string.Format("[Filmtipset] Error GetWebData, error: {0}.", e.Message));
                }
                json = "";
            }
            return json;
        }
        #endregion

        #region user

        internal Account GetAccount(Account account)
        {
            Account _old = FilmtipsetSettings.CurrentAccount;
            FilmtipsetSettings.CurrentAccount = account;
            string data = GetWebData(new NameValueCollection());
            FilmtipsetSettings.CurrentAccount = _old;
            IEnumerable<Response<object>> o = data.FromJSONArray<Response<object>>();
            if (o != null && o.Count() > 0)
            {
                User u = o.First().User;
                if (!string.IsNullOrEmpty(u.Name))
                {
                    account.Name = u.Name;
                    account.Id = u.Id;
                }
            }
            return account;
        }

        #endregion
        #region Recomendations

        internal MoviesData GetRecommendedMovies(FilmtipsetAPIGenre genre, int usernr = 0)
        {
            List<Movie> movies = new List<Movie>();
            int userId = FilmtipsetSettings.CurrentAccount.Id;
            string userKey = FilmtipsetSettings.CurrentAccount.ApiKey;
            // Recomendations need usernr or userkey
            if (usernr > 0 || userId > 0 || !string.IsNullOrEmpty(userKey))
            {
                NameValueCollection parameters = new NameValueCollection();
                parameters.Add("action", FilmtipsetAPIAction.recommendations.ToString());
                parameters.Add("id", ((int)genre).ToString());
                string json = GetWebData(parameters);
                IEnumerable<Response<IEnumerable<MoviesData>>> o = json.FromJSONArray<Response<IEnumerable<MoviesData>>>();
                if (o != null && o.Count() > 0 && o.FirstOrDefault() != null)
                {
                    return o.First().Data.FirstOrDefault();
                }
            }
            return new MoviesData();
        }

        #endregion

        #region Movie

        internal IEnumerable<Movie> GetMultipleMovie(int[] ids, bool forceUpdate = false)
        {
            int cu = FilmtipsetSettings.CurrentAccount.Id;
            if (!cache.ContainsKey(cu))
                cache.Add(cu, new Dictionary<int, MovieWrapper>());
            string idsToGet = "";
            foreach (int id in ids)
            {
                if (forceUpdate && cache[cu].ContainsKey(id))
                    cache[cu].Remove(id);
                if (!cache[cu].ContainsKey(id))
                    idsToGet += id.ToString() + ",";
            }
            if (!string.IsNullOrEmpty(idsToGet))
            {
                idsToGet = idsToGet.Remove(idsToGet.LastIndexOf(","));
            }

            NameValueCollection pms = new NameValueCollection();
            pms.Add("action", FilmtipsetAPIAction.movie.ToString());
            pms.Add("id", idsToGet);

            string listJson = this.GetWebData(pms);

            IEnumerable<Response<IEnumerable<MovieWrapper>>> response = listJson.FromJSONArray<Response<IEnumerable<MovieWrapper>>>();
            IEnumerable<MovieWrapper> movieWraps = response.First().Data;
            foreach (MovieWrapper movieWrap in movieWraps)
            {
                cache[cu].Add(movieWrap.Movie.Id, movieWrap);
            }

            List<Movie> movies = new List<Movie>();

            foreach (int id in ids)
            {
                movies.Add(cache[cu][id].Movie);
            }
            return movies;
        }

        internal IEnumerable<Movie> GetMovie(int id, bool forceUpdate = false)
        {
            int[] ids = { id };
            return GetMultipleMovie(ids, forceUpdate);
        }

        #endregion

        #region list
        internal IEnumerable<Movie> GetPublicList(FilmtipsetAPIListType listType)
        {
            List<Movie> movies = new List<Movie>();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("action", FilmtipsetAPIAction.list.ToString());
            parameters.Add("id", (listType.ToString()));
            string json = GetWebData(parameters);
            IEnumerable<Response<IEnumerable<MoviesRecomendationData>>> o = json.FromJSONArray<Response<IEnumerable<MoviesRecomendationData>>>();
            if (o != null && o.Count() > 0)
            {
                var firstMovies = o.First().Data.First().Movies;
                foreach (MovieWrapper movie in firstMovies)
                {
                    movies.Add(movie.Movie);
                }
            }

            return movies;
        }

        internal MoviesData GetList(string listId, int grade)
        {
            List<Movie> movies = new List<Movie>();
            NameValueCollection parameters = new NameValueCollection();
            parameters.Add("action", FilmtipsetAPIAction.list.ToString());
            parameters.Add("id", listId);
            if (listId == FilmtipsetAPIListType.grades.ToString())
                parameters.Add("grade", grade.ToString());
            string json = GetWebData(parameters);
            IEnumerable<Response<IEnumerable<MoviesData>>> o = json.FromJSONArray<Response<IEnumerable<MoviesData>>>();
            try
            {
                return o.First().Data.First();
            }
            catch (Exception e)
            {
                Log.Error(string.Format("[Filmtipset] Error getting list with id: {0}. Error: {1}", listId, e.Message));
                MoviesData md = new MoviesData();
                md.Title = string.Empty;
                md.Movies = new List<MovieWrapper>();
                return md;
            }
        }
        #endregion
    }
}
