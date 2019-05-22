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
    class Drawer: IDisposable
    {
        private bool disposed = false;
        public Graphics Grap { get; set; }
        public Font ImageFont { get; set; }
        public SolidBrush SolBrush { get; set; }
            public Drawer()
        {
            
            ImageFont = new Font("Arial", 200);
            SolBrush = new SolidBrush(Color.Blue);
        }
        private SizeF GetSize(string str, Graphics e)
        {
            SizeF StringSize = e.MeasureString(str, ImageFont);
            return StringSize;
        }
        private float GetVertWidth(Image img, string str, Graphics e)
        {
            float vert = img.Width - (GetSize(str, e).Width + 100);
            return vert;
        }
        public Image DrawTheString(ImageStruct img)
        {
                Image resultimage = null;
                Graphics gr = Graphics.FromImage(img.Image);
                string date = img.Date;
                PointF point = new PointF(GetVertWidth(img.Image, date, gr), 150.0F);
                gr.DrawString(date, ImageFont, SolBrush, point);
                return resultimage = img.Image;

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
                    // нету полей
                }
                Grap.Dispose();
                ImageFont.Dispose();
                SolBrush.Dispose();
                disposed = true;
            }
        }
        ~Drawer()
        {

            DisposeAlgo(false);
        }
    }
}
