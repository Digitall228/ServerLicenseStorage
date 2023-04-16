using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerLicenseStorage
{
    public static class LicensePool
    {
        public static List<LicenseList> licenseLists = new List<LicenseList>();
        static LicensePool()
        {
            string[] names = Enum.GetNames(typeof(LicenseType));
            for (int i = 0; i < names.Length; i++)
            {
                LicenseType l = (LicenseType)Enum.Parse(typeof(LicenseType), names[i]);
                licenseLists.Add(new LicenseList() { licenseType = l});
            }
        }
        public static void AddLicense(License license)
        {
            LicenseList list = FindList(license.licenseType);
            if(list != null)
            {
                list.licenses.Add(license);
            }
        }
        static LicenseList FindList(LicenseType type)
        {
            for (int i = 0; i < licenseLists.Count; i++)
            {
                if(licenseLists[i].licenseType == type)
                {
                    return licenseLists[i];
                }
            }

            return null;
        }
        public static License FindLicense(string licenseKey)
        {
            for (int x = 0; x < licenseLists.Count; x++)
            {
                for (int y = 0; y < licenseLists[x].licenses.Count; y++)
                {
                    if(licenseLists[x].licenses[y].key == licenseKey)
                    {
                        return licenseLists[x].licenses[y];
                    }
                }
            }

            return null;
        }
    }

    public class LicenseList
    {
        public LicenseType licenseType { get; set; }
        public List<License> licenses { get; set; } = new List<License>();
    }
}
