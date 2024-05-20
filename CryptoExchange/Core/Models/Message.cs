using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Models.BaseModels;

namespace Core.Models
{
    public class Message : BaseEntity
    {
        public string Value { get; set; }
        public Guid AuthorId { get; set; }
        public Guid TicketId { get; set; }
    }
}
