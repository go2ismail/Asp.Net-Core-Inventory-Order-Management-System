using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Services
{
    public class SmtpOptions
    {
        public string smtpUserName { get; set; }
        public string smtpPassword { get; set; }
        public string smtpHost { get; set; }
        public int smtpPort { get; set; }
        public bool smtpSSL { get; set; }
        public string fromEmail { get; set; }
        public string fromFullName { get; set; }
        public bool IsDefault { get; set; }

    }
}
