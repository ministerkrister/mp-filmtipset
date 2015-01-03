using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Filmtipset.Models
{
    [DataContract]
    public class FanartResponse
    {
        [DataMember(Name = "moviebackground")]
        public List<Fanart> MovieBackgrounds { get; set; }

        [DataMember(Name = "movieposter")]
        public List<Fanart> MoviePosters { get; set; }
    }

    [DataContract]
    public class Fanart
    {
        [DataMember(Name = "id")]
        public int Id { get; set; }

        [DataMember(Name = "url")]
        public string Url { get; set; }
        
        [DataMember(Name = "lang")]
        public string Language { get; set; }

        [DataMember(Name = "likes")]
        public int Likes { get; set; }
    }
}
