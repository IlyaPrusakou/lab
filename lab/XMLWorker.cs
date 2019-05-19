using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net;
using System.Threading;
using System.Xml;
using System.Xml.Linq;

namespace lab
{
    class XMLWorker
    {
        private string StringListReducer(List<string> str) // можно было бы перенести в xmlworker
        {
            string result = "";
            for (int i = 0; i < str.Count; i++)
            {
                if (i == 0) { result = str[i]; }
                else if (i > 0) { result = result + ", " + str[i]; }
            }
            return result;
        }
        public string GetAddressFromXML(WebResponse response)
        {
            List<string> stringArray = new List<string>();
            using (Stream stream = response.GetResponseStream())
            {
                XmlReader d = XmlReader.Create(stream);
                XDocument doc = XDocument.Load(d);
                XElement coll = doc.Root;
                XNamespace aw = "http://maps.yandex.ru/address/1.x";
                IEnumerable<XElement> coll2 = coll.Descendants(aw + "formatted");
                foreach (XElement item in coll2)
                {
                    stringArray.Add(item.Value);
                }
            }
            response.Close();
            string result = StringListReducer(stringArray);
            return result;
        }
    }
}
