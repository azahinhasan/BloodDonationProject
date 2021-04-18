using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using BloodDonationProject.Models.MetaData;

namespace BloodDonationProject.Models
{
    [MetadataType(typeof(BloodBookMetaData))]
    public partial class BloodBook
    {

    }
}