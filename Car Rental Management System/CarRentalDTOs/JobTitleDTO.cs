using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class JobTitleDTO
    {
        public int JobTitleID { get; set; }
        public string JobTitle { get; set; }

        public JobTitleDTO(int JobTitleID, string JobTitle)
        {
            this.JobTitleID = JobTitleID;
            this.JobTitle = JobTitle;
        }
    }
}
