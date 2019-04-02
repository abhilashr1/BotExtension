using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BotExtension.Helper
{
    public class MSKEY
    {
        public string appId { get; set; }
        public string password { get; set; }
        public string url { get; set; }
    }

    public class LUISKEY
    {
        public string LuisAppId { get; set; }
        public string LuisKey { get; set; }
        public string Hostname { get; set; }
        public string Query { get; set; }
    }

    public class QNAKEY
    {
        public string KbId { get; set; }
        public string Key { get; set; }
        public string Hostname { get; set; }
        public string Query { get; set; }
    }
}
