using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Facebook;
using Microsoft.AspNet.Facebook;


namespace FacebookLoginMVC.Models
{
    public class MyAppUser
    {

        public string id { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string link { get; set; }
        public string birthday { get; set; }
        public UserLocation location { get; set; }

        [JsonProperty("picture")] // This renames the property to picture.
        [FacebookFieldModifier("type(large)")] // This sets the picture size to large.
        public FacebookConnection<FacebookPicture> ProfilePicture { get; set; }

        [FacebookFieldModifier("limit(10)")] // This sets the size of the friend list to 10, remove it to get all friends.
        public FacebookGroupConnection<MyAppUserFriend> Friends { get; set; }

        [FacebookFieldModifier("limit(12)")] // This sets the size of the photo list to 16, remove it to get all photos.
        public FacebookGroupConnection<FacebookPhoto> Photos { get; set; }
    }
}