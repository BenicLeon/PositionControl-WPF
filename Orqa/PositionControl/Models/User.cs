using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PositionControl.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string PaswordHash { get; set; }

        public int RoleId { get; set; }
        public Role Role { get; set; }
        public ICollection<UserWorkPosition> UserWorkPositions { get; set; } 
         
    }
}
