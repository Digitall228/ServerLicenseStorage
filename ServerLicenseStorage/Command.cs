using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLicenseStorage
{
    public class Command
    {
        public License license { get; set; }
        public object data { get; set; }
        public CommanType commanType { get; set; }
    }
    public enum CommanType
    {
        CreateNewLicense,
        AddLicenseTime,
        DeleteLicense,
        CheckLicense
    }
}
