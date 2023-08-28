using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CQRS.Core.Messages
{
    public record BaseEvent : Message
    {
        public int Version { get; set; }
        public string Type => GetType().Name;

        public BaseEvent()
        {
            
        }        
    }
}
