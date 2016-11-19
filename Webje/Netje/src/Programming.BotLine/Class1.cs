using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Bot.Connector.DirectLine;

namespace Programming.BotLine
{
    public class Class1
    {
        public Class1()
        {
            DirectLineClient test = new DirectLineClient(new Uri(""), new DirectLineClientCredentials(""));
        }
    }
}
