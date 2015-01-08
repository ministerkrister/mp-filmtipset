using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Filmtipset.API;
using System.ComponentModel;

namespace Filmtipset.Models
{
    [DataContract]
    public class MovieWrapper
    {
        [DataMember(Name = "movie")]
        public Movie Movie { get; set; }
    }

    [DataContract]
    public class Movie
    {
        [DataMember(Name = "actorids")]
        public List<ActorWrapper> ActorIds { get; set; }
    
        [DataMember(Name = "actors")]
        public string Actors { get; set; }

        [DataMember(Name = "alt_title")]
        public string AltTitle { get; set; }

        [DataMember(Name = "country")]
        public string Country { get; set; }

        [DataMember(Name = "description")]
        public string Description { get; set; }

        [DataMember(Name = "director")]
        public string Director { get; set; }

        [DataMember(Name = "directorids")]
        public List<DirectorWrapper> DirectorIds { get; set; }

        [DataMember(Name = "filmtipsetgrade")]
        public FilmtipsetGrade FimltipsetGrade { get; set; }

        [DataMember(Name = "grade")]
        public Grade Grade { get; set; }


        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "image")]
        public string Image {get;set;}
        
        public MovieImages Images { 
            get 
            {
            if (_images == null)
                _images = new MovieImages() { MovieId = this.Id, Poster = this.Image, Imdb = this.Imdb };
            return _images;
            }
            set
            {
                _images = value;
            }
        }
        private MovieImages _images = null;

        [DataMember(Name = "imdb")]
        public string Imdb { get; set; }

        [DataMember(Name = "length")]
        public int Length { get; set; }

        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "orgname")]
        public string OrgName { get; set; }

        [DataMember(Name = "time")]
        public string TimeSeen { get; set; }

        [DataMember(Name = "tvInfo")]
        public TvInfo TvInfo { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }

        [DataMember(Name = "usergrades")]
        public UserGrades FimltipsetUserGrades { get; set; }

        [DataMember(Name = "writer")]
        public string Writers { get; set; }

        [DataMember(Name = "writerids")]
        public WriterWrapper WriterIds { get; set; }

        [DataMember(Name = "year")]
        public string Year { get; set; }

    }

    [DataContract]
    public class Grade
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

        internal string FriendlyType()
        {
            switch (this.Type)
            {
                case ("calculated"): return "Beräknat betyg";
                default: return "Betyg";
            }
        }

    }

    [DataContract]
    public class UserGrades
    {
        [DataMember(Name = "grade")]
        public UserGrade FilmtipsetUserGrade { get; set; }
    }

    [DataContract]
    public class UserGrade : Grade
    {
        [DataMember(Name = "user")]
        public int UserId { get; set; }
    }

    [DataContract]
    public class FilmtipsetGrade
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }

    }

    [DataContract]
    public class DirectorWrapper
    {
        [DataMember(Name = "director")]
        public Person Director { get; set; }
    }

    [DataContract]
    public class ActorWrapper
    {
        [DataMember(Name = "actor")]
        public Person Actor { get; set; }
    }

    [DataContract]
    public class WriterWrapper
    {
        [DataMember(Name = "writer")]
        public Person Writer { get; set; }
    }

    [DataContract]
    public class Person
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

    }

    [DataContract]
    public class TvInfo
    {
        [DataMember(Name = "channel")]
        public string Channel { get; set; }

        [DataMember(Name = "time")]
        public string Time { get; set; }

    }

    public class MovieImages : INotifyPropertyChanged
    {
        public int MovieId { get; set; }
        public string Imdb { get; set; }

        public string Poster
        {
            get
            {
                return _poster;
            }
            set
            {
                _poster = value;
            }
        }
        string _poster = string.Empty;

        public string Fanart
        {
            get
            {
                return _fanart;
            }
            set
            {
                _fanart = value;
            }
        }
        string _fanart = string.Empty;

        #region INotifyPropertyChanged

        /// <summary>
        /// Path to local poster image
        /// </summary>
        public string PosterImageFilename
        {
            get
            {
                string filename = string.Empty;
                if (!string.IsNullOrEmpty(Poster) && MovieId > 0)
                {
                    string folder = MediaPortal.Configuration.Config.GetSubFolder(MediaPortal.Configuration.Config.Dir.Thumbs, @"Filmtipset\Posters");
                    string posterUrl = Poster;
                    string movieid = MovieId.ToString();
                    filename = System.IO.Path.Combine(folder, movieid + System.IO.Path.GetExtension(posterUrl));
                }
                return filename;
            }
            set
            {
                _PosterImageFilename = value;
            }
        }
       string _PosterImageFilename = string.Empty;

        public string FanartImageFilename
        {
            get
            {
                string filename = string.Empty;
                if (!string.IsNullOrEmpty(Imdb))
                {
                    string folder = MediaPortal.Configuration.Config.GetSubFolder(MediaPortal.Configuration.Config.Dir.Thumbs, @"Filmtipset\Fanart");
                    //string fanartUrl = Fanart;
                    string imdbid = "tt" + Imdb;
                    filename = System.IO.Path.Combine(folder, imdbid + ".jpg");
                }
                return filename;
            }
            set
            {
                _FanartImageFilename = value;
            }
        }
        string _FanartImageFilename = string.Empty;

        /// <summary>
        /// Notify image property change during async image downloading
        /// Sends messages to facade to update image
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
