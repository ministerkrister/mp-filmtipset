using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;


namespace Filmtipset.Models
{
    [DataContract]
    public class User
    {
        [DataMember(Name = "name")]
        public string Name { get; set; }

        [DataMember(Name = "id")]
        public int Id { get; set; }
    }

    [DataContract]
    public class Account : User
    {
        [DataMember(Name = "apiKey")]
        public string ApiKey { get; set; }

        [DataMember(Name = "layout")]
        public int Layout { get; set; }

        [DataMember(Name = "listsId")]
        public string ListsId { get; set; }

        [DataMember(Name = "listId")]
        public int ListId { get; set; }

        [DataMember(Name = "grade")]
        public int grade { get; set; }

        [DataMember(Name = "recommendationGenre")]
        public int RecommendationGenre { get; set; }
    }

}

