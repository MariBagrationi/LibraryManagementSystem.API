using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Exceptions.UserExceptions
{
    public class UserAreadyExistsEx : Exception
    {
        public static string Code { get; private set; } = "UserAreadyExist";
        public UserAreadyExistsEx(string message) : base(message) { }
    }
}
