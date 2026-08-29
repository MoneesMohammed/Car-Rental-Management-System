using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPIBusinessLayer
{
    public class clsJobTitle
    {
        public int JobTitleID { get; set; }
        public string JobTitle { get; set; }

        public JobTitleDTO JDTO
        {
            get { return new JobTitleDTO(this.JobTitleID, this.JobTitle); }
        }

        public clsJobTitle(JobTitleDTO JDTO)
        {
            this.JobTitleID = JDTO.JobTitleID;
            this.JobTitle = JDTO.JobTitle;
        }

        public static clsJobTitle? Find(int JobTitleID)
        {
            JobTitleDTO? JDTO = clsJobTitleData.GetJobTitleInfoByJobTitleID(JobTitleID);

            if (JDTO != null)
            {
                return new clsJobTitle(JDTO);
            }
            else
                return null;
        }

        public static List<JobTitleDTO> GetAllJobTitles()
        {
            return clsJobTitleData.GetAllJobTitles();
        }

    }

}
