using Microsoft.AspNet.Facebook;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FacebookLoginMVC.Models
{
    public class MyAppUserFriend
    {
        public string id { get; set; }
        public string name { get; set; }
        public string link { get; set; }

        [JsonProperty("picture")] // This renames the property to picture.
        [FacebookFieldModifier("height(100).width(100)")] // This sets the picture height and width to 100px.
        public FacebookConnection<FacebookPicture> Picture { get; set; }
    }
}
