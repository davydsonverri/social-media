using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Exceptions
{
    public class BrokerException : Exception
    {
        public BrokerException(string message) : base(message)
        {
            
        }
    }
}
