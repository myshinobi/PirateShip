using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PirateShip
{
    public static class Registry
    {
        public static RegistryKey baseRegistryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE", true);
        public static string subKey = "NinjaPlanner";

        public static RegistryKey OpenKey(string sub = "")
        {

            RegistryKey rk = baseRegistryKey;
            // Open a subKey as read-only
            RegistryKey sk1 = rk.OpenSubKey(subKey + (string.IsNullOrEmpty(sub) ? "" : "\\" + sub), RegistryKeyPermissionCheck.ReadWriteSubTree);

            if (sk1 == null)
            {
                return CreateKey(sub);
            }

            return sk1;
        }

        public static RegistryKey CreateKey(string sub = "")
        {
            // Setting
            RegistryKey rk = baseRegistryKey;
            // I have to use CreateSubKey 
            // (create or open it if already exits), 
            // 'cause OpenSubKey open a subKey as read-only

            RegistryKey sk1 = rk.CreateSubKey(subKey + (string.IsNullOrEmpty(sub) ? "" : "\\" + sub), RegistryKeyPermissionCheck.ReadWriteSubTree);
            return sk1;
        }

        public static bool Write(string KeyName, object Value, string sub = "")
        {
            try
            {
                RegistryKey sk1 = CreateKey(sub);
                // Save the value
                sk1.SetValue(KeyName.ToUpper(), Value);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return false;
            }
        }

        public static int Read(string keyName, int defaultValue, string sub = "")
        {
            return Convert.ToInt32(Read(keyName, (object)defaultValue, sub));
        }


        public static string Read(string keyName, string defaultValue, string sub = "")
        {
            return Read(keyName, (object)defaultValue, sub).ToString();
        }

        public static double Read(string keyName, double defaultValue, string sub = "")
        {
            return Convert.ToInt64(Read(keyName, (object)defaultValue, sub));
        }

        public static bool Read(string keyName, bool defaultValue, string sub = "")
        {
            return Convert.ToBoolean(Read(keyName, (object)defaultValue, sub));
        }

        public static object Read(string KeyName, object defaultValue, string sub = "")
        {
            // Open a subKey as read-only
            RegistryKey sk1 = OpenKey(sub);
            // If the RegistrySubKey doesn't exist -> (null)
            if (sk1 == null)
            {
                return defaultValue;
            }
            else
            {
                try
                {
                    // If the RegistryKey exists I get its value
                    // or null is returned.
                    return sk1.GetValue(KeyName.ToUpper(), defaultValue);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    return defaultValue;
                }
            }
        }
    }
}
