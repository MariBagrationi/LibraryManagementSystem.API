using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagementSystem.Application.Exceptions.PatronExceptions
{
    public class PatronAlreadyExistsEx : Exception
    {
        public static string Code { get; private set; } = "PatronAlreadyExists";
        public PatronAlreadyExistsEx(string message) : base(message) { }
    }
}
