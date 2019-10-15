using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Extranet_EF
{
    [Table("TicketWB")]
    public partial class TicketWB
    {
        [Key]
        public int ID_Ticket { get; set; }
        public int Data { get; set; }
    }
}
