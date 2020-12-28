using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Assign1.Models
{
    public class CommunityMembership
    {
        
        public int StudentID { get; set; }

            // There was an error here System.ArgumentException: The properties expression 'c => new <>f__AnonymousType0`2(StudentID = c.StudentID, CommunityID = c.CommunityID)' is not valid. The expression should represent a simple property access: 't => t.MyProperty'. When specifying multiple properties use an anonymous type: 't => new { t.MyProperty1, t.MyProperty2 }'. (Parameter 'propertyAccessExpression')
            // Possible solve, adding getter/setter
        public string CommunityID { get; set; } 

        public Student Student { get; set; }
        public Community Community { get; set; }
    }
}
