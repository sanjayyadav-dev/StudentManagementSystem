using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SMS.Core.DTOs.BlodGoupModel
{
    public class BloodGroup
    {
        public int BloodGroupId { get; set; }

        public string BloodGroupName { get; set; } = string.Empty;

        public int? CreatedBy { get; set; }

        public string? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        public string? Modifieddate { get; set; }
    }
}
