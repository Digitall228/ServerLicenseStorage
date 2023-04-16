using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLicenseStorage
{
    public class Answer
    {
        public AnwerType anwerType { get; set; }
        public string details { get; set; }
    }
    public enum AnwerType
    {
        Good,
        Bad
    }
}
