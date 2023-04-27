using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Denis.UserList.BLL.Core
{
    internal class BLLException : Exception
    {
        public BLLException() : base("BLL exception")
        {
        }

        public BLLException(string? message) : base(message)
        {
        }

        public BLLException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
