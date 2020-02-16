using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyBudget.OperationsLoading.Tests
{
    public static class TestBankData
    {
        public static string PKOBP_Belchatow_TestAccount1 = "00 1020 3958 0000 1234 5678 9000";
        public static string Millenium_TestAccount1 = "00 1160 2202 0000 1234 5678 8000";
        public static string Millenium_TestAccount2 = "99 1160 2202 0000 1234 5678 9000";
        public static string BNPParibas_TestAccount1 = "00 2030 0045 0000 1234 5678 9000";

        public static string ExternalAccount_TestAccount1 = "11 2222 3333 4444 5555 6666 7777";
        public static string ExternalAccount_TestAccount2 = "11 2222 3333 4444 5555 6666 7778";

        public static string CardNo1 = "123456XXXXXX7890";
        public static string CardNo2 = "123456XXXXXX7891";

        public static string CardBNP_No1 = "512345------0010";
        public static string CardBNP_No1_CardHolder = "ANNA KOWALSKA";
        public static string CardBNP_No2 = "512345------0020";
        public static string CardBNP_No2_CardHolder = "JAN KOWALSKI";

        public static string Compact(this string input)
        {
            return input
                .Replace(" ", "")
                .Replace("\t", "");
        }

        public static Stream ToStream(this string text)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream, Encoding.UTF8);
            writer.Write(text);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public static Stream ToStream(this byte[] bytes)
        {
            MemoryStream stream = new MemoryStream();
            BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8);
            writer.Write(bytes);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}
