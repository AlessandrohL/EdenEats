using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EdenEats.Application.Email
{
    public sealed class Message
    {
        public IEnumerable<string> To { get; private set; }
        public string Subject { get; private set; } = string.Empty;
        public string Content { get; private set; } = string.Empty;

        public Message(IEnumerable<string> to, string subject, string content)
        {
            To = to;
            Subject = subject;
            Content = content;
        }
    }
}
