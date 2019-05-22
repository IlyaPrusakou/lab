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
    class Requester: IDisposable
    {
        private bool disposed = false;
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
            WebResponse response = null;
            try
            {
                
                response = request.GetResponse();
                
            }
            catch (Exception ex)
            {
                response = null;
            }
            return response;
        }
        public string GetLocationOfImage(XMLWorker XWorker, string longit, string width)
        {
            string location = "";
            string longitude = longit;
            string Widthude = width; 
            WebResponse response = MakeRequest(longitude, Widthude);
            if (response != null)
            {
                location = XWorker.GetAddressFromXML(response);
            }
            else if (response == null)
            {
                location = "undefined"; 
            }
            return location;
        }
        public void Dispose()
        {
            DisposeAlgo(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void DisposeAlgo(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    YandexApi = null;
                    Webreq = null;
                }
                Webresp.Dispose();
                disposed = true;
            }
        }
        ~Requester()
        {

            DisposeAlgo(false);
        }
    }
}
