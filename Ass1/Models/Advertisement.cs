using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Assign1.Models
{

    public class Advertisement
    { 
        public int AdvertisementId { get; set; }
        public string CommunityID { get; set; }

        [Required]
        [DisplayName("File Name")]
        public string FileName { get; set; }

        [Required]
        [Url]
        public string Url { get; set; }

        
        public Community Community { get; set; }

    }
}
