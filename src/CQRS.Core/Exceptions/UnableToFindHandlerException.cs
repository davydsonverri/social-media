using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Exceptions
{
    public class UnableToFindHandlerException : Exception
    {
        public UnableToFindHandlerException(string handlerName) : base(FormatMessage(handlerName))
        {
            
        }

        private static string FormatMessage(string handlerName)
        {
            return $"Unable to find handler for {handlerName}";
        }        
    }
}
