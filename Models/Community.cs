using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Lab4.Models
{
    public class Community
    {
        [DisplayName("Registration Number")]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string ID
        {
            get;
            set;
        }

        [Required]
        [StringLength(50)]
        [MinLength(3)]
        public string Title
        {
            get;
            set;
        }

        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        public decimal Budget
        {
            get;
            set;
        }

        public ICollection<Student> Students { get; set; }
    }
}
