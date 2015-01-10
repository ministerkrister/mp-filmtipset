using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Filmtipset.Models
{
    [DataContract]
    internal class FilmtipsetLoadParam
    {
        [DataMember(Name = "Title")]
        public string Title { get; set; }

        [DataMember(Name = "ListId")]
        public string ListId { get; set; }

        [DataMember(Name = "Action")]
        public int Action { get; set; }

    }
}
