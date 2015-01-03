using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Filmtipset.Models
{
    [DataContract]
    class FilmtipsetMovieIdentifier
    {
        [DataMember(Name = "type")] //imdb or filmtipset
        public string Type { get; set; }

        [DataMember(Name = "value")]
        public string Value { get; set; }
    }
}
