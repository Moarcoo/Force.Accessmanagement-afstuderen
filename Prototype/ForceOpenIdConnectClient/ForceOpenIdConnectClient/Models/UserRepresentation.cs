using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace ForceOpenIdConnectClient.Models
{
    /// <summary>
    /// A class object representing a Keycloak user presentation
    /// The JsonProperty's make sure the json is mapped properly
    /// </summary>
    public class UserRepresentation
    {
        [JsonProperty("username")]
        public string username { get; set; }

        [JsonProperty("firstName")]
        public string firstName { get; set; }

        [JsonProperty("lastName")]
        public string lastName { get; set; }

        [JsonProperty("enabled")]
        public bool enabled { get; set; }

        [JsonProperty("attributes")]
        public Dictionary<string, string[]> attributes { get; set; }

    }
}