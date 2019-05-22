using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab
{
    class MetaDataReceiver: Fotoset, IDisposable
    {
        private bool disposed = false;
        public Drawer Drawer { get; set; }
        public MetaDataReceiver()
        {
            
        }
        public MetaDataReceiver(Drawer dr)
        {
            
            Drawer = dr;
        }
        
        private (uint first, uint second, uint third) Devider(Image foto, int Idprop)
        {
            uint first = 0;
            uint second = 0;
            uint third = 0;
            try
            {
                PropertyItem Prop2 = foto.GetPropertyItem(Idprop);
                first = BitConverter.ToUInt32(Prop2.Value, 0) / BitConverter.ToUInt32(Prop2.Value, 4);
                second = BitConverter.ToUInt32(Prop2.Value, 8) / BitConverter.ToUInt32(Prop2.Value, 12);
                third = BitConverter.ToUInt32(Prop2.Value, 16) / BitConverter.ToUInt32(Prop2.Value, 20);
            }
            catch (Exception ex)
            {
                first = 0;
                second = 0;
                third = 0;
            }
            //finally
            //{
                //first = 0;
                //second = 0;
                //third = 0;
            //}
            return (first, second, third);
        } 
        private string Getlong (Image foto)
        {
            var tulpe = Devider(foto, 4);
            string result = tulpe.first + "." + tulpe.second + tulpe.third;
            return result;
        }
        private string GetWidth(Image foto)
        {
            var tulpe = Devider(foto, 2);
            string result = tulpe.first + "." + tulpe.second + tulpe.third;
            return result;
        }
        //private string GetDate(Image foto)
        //{
        //PropertyItem propDate = null;
        //string stringDate = null;
        //try
        //{
        //propDate = foto.GetPropertyItem(0x0132);
        //stringDate = utf.GetString(propDate.Value);
        //}
        //catch (Exception ex) { stringDate = "#####"; }
        //finally { stringDate = "#####"; }
        //return stringDate;
        //}
        private string GetDate(Image foto)
        {
            PropertyItem propDate = null;
            DateTime date;

            string stringDate = "";
            try
            {
                
                propDate = foto.GetPropertyItem(0x0132);
                if (propDate != null)
                {
                    ASCIIEncoding enc = new ASCIIEncoding();
                    string interstring = enc.GetString(propDate.Value, 0, propDate.Len - 1);
                    CultureInfo provider = CultureInfo.InvariantCulture;
                    date = DateTime.ParseExact(interstring, "yyyy:MM:d H:m:s", provider);
                    stringDate = date.ToString();
                }
                else
                {
                    Console.WriteLine("Property has not found");
                }
            }
            catch (Exception ex)
            {
                stringDate = "undefinded";
            }

            return stringDate;
        }
        public void GetData()
        {
            for (int i = 0; i < ImageSet.Count; i++)
            {
                ImageStruct ist = ImageSet[i];
                ist.Date = GetDate(ist.Image).RemoverInvalidSymbols();
                if (ist.Date == "undefinded") { ist.Date = ist.CreationTime.RemoverInvalidSymbols(); }
                ist.Longitude = Getlong(ist.Image);
                ist.Widthude = GetWidth(ist.Image);
                
                ImageSet[i] = ist;
            }
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
                Drawer.Dispose();
                disposed = true;
            }
        }
        ~MetaDataReceiver()
        {

            DisposeAlgo(false);
        }
    }
}
