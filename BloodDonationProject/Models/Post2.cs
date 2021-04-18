using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using BloodDonationProject.Models.MetaData;

namespace BloodDonationProject.Models
{
    [MetadataType(typeof(PostMetaData))]
    public partial class Post
    {
    }
}