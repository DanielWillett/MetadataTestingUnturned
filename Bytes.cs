using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetadataTesting.Functions
{
    public class Bytes
    {
        public static byte[] stringToByte(string @string)
        {
            string[] output2 = @string.Split(',');
            List<byte> bytesList = new List<byte> { };
            foreach (string str in output2)
            {
                bytesList.Add(Byte.Parse(str));
            }
            return bytesList.ToArray();
        }

        public static string bytesToCDString(byte[] bytes)
        {
            string output = "";
            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                if (i == bytes.Length - 1)
                {
                    output += bytes[i].ToString();
                }
                else
                {
                    output += bytes[i].ToString() + ",";
                }
            }
            return output;
        }
    }
}
