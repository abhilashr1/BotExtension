using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotExtension.Models
{
    public class BotFile
    {
        public string name { get; set; }
        public string padlock { get; set; }
        public string version { get; set; }
        public Dictionary<string, string>[] services { get; set; }
    }
}
