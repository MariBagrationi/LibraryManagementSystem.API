using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Exceptions.PatronExceptions
{
    public class PatronDoesNotExistEx : Exception
    {
        public static string Code { get; private set; } = "PatronDoesNotExist";
        public PatronDoesNotExistEx(string message) : base(message) { }
    }
}
