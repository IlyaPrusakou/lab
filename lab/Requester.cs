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
    class Requester
    {
        public string YandexApi { get; set; }
        public WebRequest Webreq { get; set; }
        public WebResponse Webresp { get; set; }
        
        
        public Requester()
        {

        }
       
        private string GetRequestString(string longitude, string altitude)
        {
            string Requestpath = $"https://geocode-maps.yandex.ru/1.x/?apikey={YandexApi}&geocode={longitude},{altitude}";
            return Requestpath;
        }

        private WebResponse MakeRequest(string str, string str2)
        {
            WebRequest request = WebRequest.Create(GetRequestString(str, str2));
            WebResponse response = request.GetResponse();
            return response;
        }
        public string GetLocationOfImage(XMLWorker XWorker, string longit, string width)
        {
                string longitude = longit;
                string Widthude = width;
                WebResponse response = MakeRequest(longitude, Widthude);
                string location = XWorker.GetAddressFromXML(response);
                return location;
        }
    }
}
