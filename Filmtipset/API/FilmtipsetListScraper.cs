using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MediaPortal.GUI.Library;
using HtmlAgilityPack;
using System.Web;

namespace Filmtipset.API
{
    public class MovieList
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public string MovieCountLabel { get; set; }
    }

    public class FilmtipsetListScraper
    {
        private const string url = @"http://nyheter24.se/filmtipset/package_list.cgi";
        private Dictionary<string, List<MovieList>> lists = null;

        protected FilmtipsetListScraper()
        {
        }

        protected static FilmtipsetListScraper instance = null;
        internal static FilmtipsetListScraper Instance
        {
            get
            {
                if (instance == null) instance = new FilmtipsetListScraper();
                return instance;
            }
        }

        internal Dictionary<string, List<MovieList>> GetAllLists()
        {
            if (lists == null)
            {
                WebClient webClient = new WebClient();
                string data;
                try
                {
                    HtmlDocument doc = new HtmlDocument();
                    data = webClient.DownloadString(url);
                    doc.LoadHtml(data);
                    HtmlNode tr = doc.DocumentNode.SelectNodes("//tr").FirstOrDefault(t => t.SelectNodes("td/div[starts-with(@class, 'header')]") != null);
                    if (tr != null)
                    {
                        string currentHeading = null;
                        List<MovieList> currentLists = null;
                        foreach (HtmlNode td in tr.SelectNodes("td"))
                        {
                            if (currentHeading != null)
                            {
                                if (lists == null)
                                    lists = new Dictionary<string, List<MovieList>>();
                                lists.Add(currentHeading, currentLists ?? new List<MovieList>());
                            }
                            currentHeading = HttpUtility.HtmlDecode(td.SelectSingleNode("div").InnerText).Trim();
                            currentLists = new List<MovieList>();
                            foreach (HtmlNode li in td.Descendants("li"))
                            {
                                HtmlNode anchor = li.SelectNodes("a").FirstOrDefault(a =>a.GetAttributeValue("href", "").StartsWith("package_view.cgi?package="));
                                if (anchor != null)
                                {
                                    
                                    string idString = anchor.GetAttributeValue("href", "").Replace("package_view.cgi?package=", string.Empty);
                                    int id = 0;
                                    int.TryParse(idString, out id);
                                    string name = HttpUtility.HtmlDecode(anchor.InnerText).Trim();
                                    string countLabel = anchor.GetAttributeValue("title", "").Trim();
                                    if (id > 0 && !string.IsNullOrEmpty(name))
                                    {
                                        MovieList currentList = new MovieList()
                                        {
                                            Id = id,
                                            Name = name,
                                            MovieCountLabel = countLabel
                                        };
                                        currentLists.Add(currentList);
                                    }

                                    HtmlNode div = li.SelectSingleNode("div[starts-with(@class, 'header')]");
                                    if (div != null && currentHeading != null)
                                    {
                                        if (lists == null)
                                            lists = new Dictionary<string, List<MovieList>>();
                                        if (lists.ContainsKey(currentHeading))
                                            lists[currentHeading].Concat<MovieList>(currentLists);
                                        else
                                            lists.Add(currentHeading, currentLists);
                                        currentHeading = HttpUtility.HtmlDecode(div.InnerText).Trim();
                                        currentLists = new List<MovieList>();
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception e)
                {
                    Log.Error(string.Format("[Filmtipset] Error Getting lists, error: {0}.", e.Message));
                }
            }
            return lists;
        }

        internal List<string> GetListCategories()
        {
            return GetAllLists().Keys.ToList<string>();
        }

        internal string GetListCategory(int listId)
        {
            KeyValuePair<string,List<MovieList>> kvp = GetAllLists().FirstOrDefault(k => k.Value.Any(m => m.Id == listId));
            return kvp.Equals(new KeyValuePair<string, List<MovieList>>()) ? "" : kvp.Key;
        }
    }
}
