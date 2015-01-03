using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Filmtipset.Models
{
        [DataContract]
        public class Request
        {
            [DataMember(Name = "action")]
            public string Action { get; set; }

            [DataMember(Name = "id")]
            public string Id { get; set; }
        }
}
