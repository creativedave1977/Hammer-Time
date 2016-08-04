using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace HammerTime.Classes
{
    public class App
    {
        public enum AdjustmentType
        {
            Increment,
            Decrement
        }

        public static string GetFirstLine(string text)
        {
            using (var reader = new StringReader(text))
            {
                return reader.ReadLine();
            }
        }
    }
}