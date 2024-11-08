using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API.SSO.Infras.Shared
{
    public class RegexConstant
    {
        public const string EMAIL = @"^(?:\+84|0[35789])([0-9]{8})\b";
        public const string PASSWORD = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[!@#$%^&*()_+{}\[\]:;<>,.?~\\/-]).{8,}$";
    }
}
