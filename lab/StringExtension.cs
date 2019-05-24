using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab
{
    public static class StringExtension
    {
        public static string RemoverInvalidSymbols(this string str)
        {

            string result = "";
            switch (true)
            {
                case bool r when r == str.Contains(':'):
                    result = str.Replace(':', '-');
                    break;
                case bool r when r == str.Contains('.'):
                    result = str.Replace('.', '-');
                    break;
                case bool r when r == str.Contains(','):
                    result = str.Replace(',', '-');
                    break;
                case bool r when r == str.Contains(';'):
                    result = str.Replace(';', '-');
                    break;
                case bool r when r == str.Contains(']'):
                    result = str.Replace(']', '-');
                    break;
                case bool r when r == str.Contains('['):
                    result = str.Replace('[', '-');
                    break;
                case bool r when r == str.Contains('='):
                    result = str.Replace('=', '-');
                    break;
                case bool r when r == str.Contains('+'):
                    result = str.Replace('+', '-');
                    break;
            }
            return result;
        }
    }
}
