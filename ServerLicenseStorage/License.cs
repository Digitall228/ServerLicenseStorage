using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLicenseStorage
{
    public class License
    {
        public string key { get; set; }
        public DateTime startTime{ get; set; }
        public TimeSpan expireTime{ get; set; }
        public bool isActivated { get; set; }
        public LicenseType licenseType { get; set; }
    }
    public enum LicenseType
    {
        SophisticatedProgram
    }
}
