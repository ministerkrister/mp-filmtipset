using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Filmtipset.Models
{
    [DataContract]
    public class Response<T>
    {
        [DataMember(Name = "source")]
        public string Source { get; set; }

        [DataMember(Name = "request")]
        public Request Request { get; set; }

        [DataMember(Name = "user")]
        public User User { get; set; }

        // Is this used in the API?
        [DataMember(Name = "version")]
        public string Version { get; set; }

        [DataMember(Name = "access-user")]
        public string AccessUser { get; set; }

        [DataMember(Name = "data")]
        public T Data { get; set; }

    }
}
