using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SendMail.Model
{
    public class Mail
    {
        public string from { get; set; }
        public string to { get; set; }
        public string title { get; set; }
        public string content { get; set; }
        public bool isHtml { get; set; }
        public int port { get;set; }
        public string host { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }

    }
}
