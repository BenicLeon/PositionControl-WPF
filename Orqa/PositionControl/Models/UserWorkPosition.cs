using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PositionControl.Models
{
    public class UserWorkPosition
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int PositionId { get; set; }
        public WorkPosition WorkPosition { get; set; }
        public string Product { get; set; }
        public DateTime AssignDate { get; set; }
    }
}
