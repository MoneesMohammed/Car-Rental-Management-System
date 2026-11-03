using CarRentalDataAccessLayer;
using CarRentalDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalAPIBusinessLayer
{
    public class clsAuditLog
    {
        
        public int AuditLogID { get; set; }
        public int UserID { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public int? EntityID { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime CreatedAt { get; set; }


        public AuditLogDTO ALDTO
        {
            get { return new AuditLogDTO(this.AuditLogID, this.UserID, this.Action, this.EntityName, this.EntityID, this.OldValue, this.NewValue, this.CreatedAt); }
        }


        public clsAuditLog(AuditLogDTO ADTO)
        {
            this.AuditLogID = ADTO.AuditLogID;
            this.UserID = ADTO.UserID;
            this.Action = ADTO.Action;
            this.EntityName = ADTO.EntityName;
            this.EntityID = ADTO.EntityID;
            this.OldValue = ADTO.OldValue;
            this.NewValue = ADTO.NewValue;
            this.CreatedAt = ADTO.CreatedAt;

        }


        public static clsAuditLog? Find(int AuditLogID)
        {
            AuditLogDTO? ALDTO = clsAuditLogData.GetAuditLogInfoByAuditLogID(AuditLogID);

            if (ALDTO != null)
            {
                return new clsAuditLog(ALDTO);
            }
            else
                return null;
        }


        private bool _AddNewAuditLog()
        {
            this.AuditLogID = clsAuditLogData.AddNewAuditLog(ALDTO);
            return (AuditLogID != -1);
        }

        public bool Save()
        {
            if (_AddNewAuditLog())
            {
                return true;
            }

            return false;
        }


        public static List<AuditLogDTO> GetAllAuditLogs()
        {
            return clsAuditLogData.GetAllAuditLogs();
        }


    }

}
