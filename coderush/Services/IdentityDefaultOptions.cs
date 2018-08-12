using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace coderush.Services
{
    public class IdentityDefaultOptions
    {
        //password settings
        public bool PasswordRequireDigit { get; set; }
        public int PasswordRequiredLength { get; set; }
        public bool PasswordRequireNonAlphanumeric { get; set; }
        public bool PasswordRequireUppercase { get; set; }
        public bool PasswordRequireLowercase { get; set; }
        public int PasswordRequiredUniqueChars { get; set; }

        //lockout settings
        public double LockoutDefaultLockoutTimeSpanInMinutes { get; set; }
        public int LockoutMaxFailedAccessAttempts { get; set; }
        public bool LockoutAllowedForNewUsers { get; set; }

        //user settings
        public bool UserRequireUniqueEmail { get; set; }
        public bool SignInRequireConfirmedEmail { get; set; }

        //cookie settings
        public bool CookieHttpOnly { get; set; }
        public double CookieExpiration { get; set; }
        public string LoginPath { get; set; }
        public string LogoutPath { get; set; }
        public string AccessDeniedPath { get; set; }
        public bool SlidingExpiration { get; set; }
    }
}
