using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab6.Models
{
    public class StudentBase
    {
        [Required]
        [DisplayName("FirstName")]
        public string FirstName
        {
            get;
            set;
        }

        [Required]
        [DisplayName("LastName")]
        public string LastName
        {
            get;
            set;
        }

        [Required]
        [DisplayName("Program")]
        public string Program
        {
            get;
            set;
        }
    }
}
