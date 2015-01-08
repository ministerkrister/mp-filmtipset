using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Filmtipset.Models
{
    [DataContract]
    public class MoviesData
    {
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "movies")]
        public List<MovieWrapper> Movies { get; set; }

        [DataMember(Name = "offset")]
        public int Offset { get; set; }

        [DataMember(Name = "count")]
        public int Count { get; set; }

    }

    [DataContract]
    public class MoviesListData : MoviesData
    {
        [DataMember(Name = "count")]
        public int Count { get; set; }

        [DataMember(Name = "offset")]
        public int Offset { get; set; }

        [DataMember(Name = "list")]
        public string ListType { get; set; }

    }

    [DataContract]
    public class MoviesRecomendationData : MoviesData
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }

}
