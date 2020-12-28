using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assign1.Models.ViewModels
{
    public class StudentViewModel
    {
        public Student Student { get; set; }
        public IEnumerable<Community> Communities { get; set; }
        public IEnumerable<CommunityMembership> CommunityMemberships { get; set; }
    }
}
