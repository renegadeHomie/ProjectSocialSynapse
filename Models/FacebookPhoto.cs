using Microsoft.AspNet.Facebook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookLoginMVC.Models
{
     public class FacebookPhoto
    {

        [JsonProperty("picture")] // This renames the property to picture.
        public string ThumbnailUrl { get; set; }

        public string Link { get; set; }

    }
}
