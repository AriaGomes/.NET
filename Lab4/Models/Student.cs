using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Student
    {
        public int ID
        {
            get;
            set;
        }

        [Required]
        [StringLength(50)]
        public string LastName
        {
            get;
            set;
        }

        [DisplayName("LastName")]
        [Required]
        [StringLength(50)]
        public string FirstName
        {
            get;
            set;
        }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate
        {
            get;
            set;
        }

        public string FullName
        {
            get
            {
                return FirstName + " " + LastName;
            }
        }

        public Community Community { get; set; }
    }
}
