using Core.Enums;
using Core.Models.BaseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Ticket : BaseEntity
    {
        public Status Status { get; set; }
        public Guid UserId { get; set; }
        public List<Message> ChatHistory { get;  set; }
    }
}
