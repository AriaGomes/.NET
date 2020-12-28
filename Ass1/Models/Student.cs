using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assign1.Models
{
    public class Student
    {
        public int ID { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Enrollment Date")]
        public DateTime EnrollmentDate { get; set; }

        public string FullName { 
            get { 
                return FirstName + ", " + LastName; 
            } 
        }

        public ICollection<CommunityMembership> CommunityMembership { get; set; }

        // Fix for "Unable to determine the relationship represented by navigation property 'CommunityMembership.Student' of type 'Student'. Either manually configure the relationship, or ignore this property using the '[NotMapped]' attribute"
    }
}
