using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab
{
    class MetaDataReceiver: Fotoset
    {
        
        public UTF8Encoding utf;
        public Drawer Drawer { get; set; }
        
        public MetaDataReceiver()
        {
            utf = new UTF8Encoding();
        }
        public MetaDataReceiver(Drawer dr)
        {
            utf = new UTF8Encoding();
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
                Console.WriteLine(ex.Message);
            }
            finally
            {
                first = 0;
                second = 0;
                third = 0;
            }
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
        private string GetDate(Image foto)
        {
            PropertyItem propDate = null;
            string stringDate = null;
            try
            {
                propDate = foto.GetPropertyItem(0x0132);
                stringDate = utf.GetString(propDate.Value);
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }
            finally { stringDate = "#####"; }
            


            return stringDate;
        }
        public void GetData()
        {
            for (int i = 0; i < ImageSet.Count; i++)
            {
                ImageStruct ist = ImageSet[i];
                ist.Date = GetDate(ist.Image);
                if (ist.Date == "#####") { ist.Date = ist.CreationTime.Replace(':', '-'); }
                ist.Longitude = Getlong(ist.Image);
                ist.Widthude = GetWidth(ist.Image);
                
                ImageSet[i] = ist;
            }
        }
    }
}
