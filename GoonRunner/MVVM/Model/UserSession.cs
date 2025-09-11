using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GoonRunner.Utils;

namespace GoonRunner.MVVM.Model
{
    public class UserSession
    {
        public int UserId { get; set; }
        public string DisplayName { get; set; }
        public EmployeeRoles.Roles Role { get; set; }
        public string RoleName { get; set; }
    }    
}
