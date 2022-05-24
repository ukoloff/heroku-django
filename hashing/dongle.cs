using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SoftSentinel
{
    class Dongle
    {
        static public void Test()
        {
            Console.WriteLine("Hello World!");
            Console.WriteLine("Authorized: " + Authorized());
            Console.WriteLine(MyFolder());
            Console.WriteLine(LicFile());
            var req = MyReq();
            Console.WriteLine(req);
            var resp = GenResponse(req);
            Console.WriteLine(resp);
            Console.WriteLine(CheckResponse(req, resp));
            Console.WriteLine(CheckResponse(req, resp + 1));
            Console.WriteLine(CheckResponse(resp));
        }

        static string MyBin()
        {
            return System.Reflection.Assembly.GetExecutingAssembly().Location;
        }

        static string MyFolder()
        {
            return Path.GetDirectoryName(MyBin());
        }

        static uint MyHash(string s)
        {
            ulong result = 0;
            foreach (var b in SHA1.Create().ComputeHash(Encoding.UTF8.GetBytes(s)))
            {
                result = (256 * result + b) % 2147483647;
            }
            return (uint)result;
        }

        static public uint MyReq()
        {
            var i = new DirectoryInfo(MyFolder());
            var ctime = i.CreationTimeUtc;
            return MyHash(String.Format("<{0:u}|ReQuEsT>", ctime));
        }

        static uint GenResponse(uint req, char salt)
        {
            return MyHash(String.Format("<{0}|{1}|ReSpOnSe>", req, salt));
        }

        static public uint GenResponse(uint req)
        {
            var r = new Random();
            return GenResponse(req, (char)(r.Next('Z' - 'A' + 1) + 'A'));
        }

        static bool CheckResponse(uint req, uint resp)
        {
            for (var c = 'A'; c <= 'Z'; ++c)
            {
                if (resp == GenResponse(req, c))
                    return true;
            }
            return false;
        }

        static string LicFile()
        {
            return Path.Combine(MyFolder(), "license.key");
        }

        static public bool Authorized()
        {
            try
            {
                return CheckResponse(MyReq(), uint.Parse(File.ReadAllText(LicFile())));
            }
            catch
            {
                return false;
            }

        }

        static public bool CheckResponse(uint resp)
        {
            if (!CheckResponse(MyReq(), resp))
                return false;
            try
            {
                File.WriteAllText(LicFile(), resp.ToString());
            } catch { }
            return true;
        }
    }
}
