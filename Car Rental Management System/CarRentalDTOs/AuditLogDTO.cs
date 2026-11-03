using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarRentalDTOs
{
    public class AuditLogDTO
    {
        public int AuditLogID { get; set; }
        public int UserID { get; set; }
        public string Action { get; set; }
        public string EntityName { get; set; }
        public int? EntityID { get; set; }
        public string? OldValue { get; set; }
        public string? NewValue { get; set; }
        public DateTime CreatedAt { get; set; }

        public AuditLogDTO(int AuditLogID, int UserID, string Action, string EntityName, int? EntityID, string? OldValue, string? NewValue, DateTime CreatedAt)
        {
            this.AuditLogID = AuditLogID;
            this.UserID = UserID;
            this.Action = Action;
            this.EntityName = EntityName;
            this.EntityID = EntityID;
            this.OldValue = OldValue;
            this.NewValue = NewValue;
            this.CreatedAt = CreatedAt;
        }
    }


}
