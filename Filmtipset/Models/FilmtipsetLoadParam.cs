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
        [DataMember(Name = "title")]
        public string Title { get; set; }

        [DataMember(Name = "id")]
        public string Id { get; set; }

    }
}
