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
    public class ImageStruct: IDisposable
    {
        public int IdImage { get; set; }
        public Image Image { get; set; }
        public string Date { get; set; }
        public string Longitude { get; set; }
        public string Widthude { get; set; }
        public string Extension { get; set; }
        public string Location { get; set; }
        public string CreationTime { get; set; }
        private bool disposed = false;
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
                    Date = null;
                    Longitude = null;
                    Widthude = null;
                    Extension = null;
                    Location = null;
                    CreationTime = null;
                }
                Image.Dispose();
                disposed = true;
            }
        }
        ~ImageStruct()
        {

            DisposeAlgo(false);
        }
    }
}
