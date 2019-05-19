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
    class Program
    {
        public static bool Flag = true;
        public static void SaveImageCustom(ImageStruct imgstruct, Stream fs)
        {
            switch (imgstruct.Extension)
            {
                case ".jpg":
                    imgstruct.Image.Save(fs, ImageFormat.Jpeg);
                    break;
                case ".png":
                    imgstruct.Image.Save(fs, ImageFormat.Png);
                    break;
                case ".bmp":
                    imgstruct.Image.Save(fs, ImageFormat.Bmp);
                    break;
                case ".tiff":
                    imgstruct.Image.Save(fs, ImageFormat.Tiff);
                    break;
                case ".gif":
                    imgstruct.Image.Save(fs, ImageFormat.Gif);
                    break;
            }
        }
        static void Main(string[] args)
        {
            Console.WriteLine("Choose the Comand.");
            Console.WriteLine("1 to Rename as DataTime");
            Console.WriteLine("2 to Add DataTime");
            Console.WriteLine("3 to Sort by year");
            Console.WriteLine("4 to Sort by location");
            Console.WriteLine("q to Exit");
            Console.WriteLine("Press 1 or 2 or 3 or 4 or q");
            string choosen = Console.ReadLine();
            while (Flag)
            {
                switch (choosen)
                {
                    case "1":
                        Console.WriteLine("Enter directory");
                        string DirectoryPath = @"D:\ДЗ\lab\source"; // типо ввожу
                        DirectoryInfo newdir = new DirectoryInfo(DirectoryPath);
                        DirectoryInfo target = newdir.CreateSubdirectory("rename");
                        MetaDataReceiver metadata = new MetaDataReceiver();
                        metadata.GetListImage(newdir);
                        metadata.GetData();
                        for (int i = 0; i< metadata.ImageSet.Count; i++)
                        {
                            List<ImageStruct> local = metadata.ImageSet;
                            string path = target.FullName + @"\" + local[i].Date + local[i].Extension;
                            if (File.Exists(path) == true) { path = target.FullName + @"\" + local[i].Date +i + local[i].Extension;  }
                            FileStream fs = File.Create(path);
                            SaveImageCustom(local[i], fs);
                            fs.Flush();
                            fs.Close();
                        }
                        Console.WriteLine("finished");
                        break;
                    case "2":
                        Console.WriteLine("Enter directory");
                        string DirectoryPath2 = @"D:\ДЗ\lab\source"; // типо ввожу
                        DirectoryInfo newdir2 = new DirectoryInfo(DirectoryPath2);
                        DirectoryInfo target2 = newdir2.CreateSubdirectory("drawstring");
                        Drawer drw = new Drawer();
                        MetaDataReceiver metadata2 = new MetaDataReceiver(drw);
                        metadata2.GetListImage(newdir2);
                        metadata2.GetData();
                        for (int i = 0; i < metadata2.ImageSet.Count; i++)
                        {
                            Image StringImage = null;
                            List<ImageStruct> local = metadata2.ImageSet;
                            string path = target2.FullName + @"\" + local[i].IdImage + local[i].Extension;
                            if (File.Exists(path) == true) { path = target2.FullName + @"\" + local[i].IdImage + i + local[i].Extension; }
                            FileStream fs = File.Create(path);
                            StringImage = metadata2.Drawer.DrawTheString(local[i]);
                            SaveImageCustom(local[i], fs);
                            fs.Flush();
                            fs.Close();
                        }

                        Console.WriteLine("finished");

                        break;
                    case "3":
                        Console.WriteLine("Enter directory");
                        string DirectoryPath3 = @"D:\ДЗ\lab\source"; // типо ввожу
                        DirectoryInfo newdir3 = new DirectoryInfo(DirectoryPath3);
                        DirectoryInfo target3 = newdir3.CreateSubdirectory("sortbyyear");
                        MetaDataReceiver metadata3 = new MetaDataReceiver();
                        metadata3.GetListImage(newdir3);
                        metadata3.GetData();
                        for (int i = 0; i < metadata3.ImageSet.Count; i++)
                        {
                          
                            List<ImageStruct> local = metadata3.ImageSet;
                            string dirpath = target3.FullName + @"\" + local[i].Date.Substring(6, 4);
                            string path = target3.FullName + @"\" + local[i].IdImage + local[i].Extension; // тоже не нужно, заменить на  пустую строку
                            if (!Directory.Exists(dirpath))
                            {
                                target3.CreateSubdirectory(local[i].Date.Substring(6, 4));
                                path = dirpath + @"\" + local[i].IdImage + local[i].Extension;
                                FileStream fs = File.Create(path);
                                SaveImageCustom(local[i], fs);
                                fs.Flush();
                                fs.Close();
                            }
                            else if (Directory.Exists(dirpath))
                            {
                                path = dirpath + @"\" + local[i].IdImage + local[i].Extension;
                                if (File.Exists(path) == true) { path = dirpath + @"\" + local[i].IdImage + i + local[i].Extension; }
                                FileStream fs = File.Create(path);
                                SaveImageCustom(local[i], fs);
                                fs.Flush();
                                fs.Close();
                            }
                        }
                        Console.WriteLine("finished");
                        break;
                    case "4":
                        Console.WriteLine("Enter directory");
                        string DirectoryPath4 = @"D:\ДЗ\lab\source";
                        DirectoryInfo newdir4 = new DirectoryInfo(DirectoryPath4);
                        DirectoryInfo target4 = newdir4.CreateSubdirectory("location");
                        Requester requester = new Requester();
                        requester.YandexApi = "24e5f3e7-2bb0-4052-b2fc-5254ecd09eea";
                        MetaDataReceiver metadata4 = new MetaDataReceiver();
                        XMLWorker xmlworker = new XMLWorker();
                        metadata4.GetListImage(newdir4);
                        metadata4.GetData();
                        for (int i = 0; i < metadata4.ImageSet.Count; i++)
                        {
                            ImageStruct img = metadata4.ImageSet[i];
                            string longit = img.Longitude;
                            string width = img.Widthude;
                            string Location = requester.GetLocationOfImage(xmlworker, longit, width);
                            img.Location = Location;
                        }
                        
                        
                        for (int i = 0; i < metadata4.ImageSet.Count; i++)
                        {

                            List<ImageStruct> local = metadata4.ImageSet;
                            string dirpath = target4.FullName + @"\" + local[i].Location;
                            string path = target4.FullName + @"\" + local[i].IdImage + local[i].Extension; //это не нужно
                            if (!Directory.Exists(dirpath))
                            {
                                target4.CreateSubdirectory(local[i].Location);
                                path = dirpath + @"\" + local[i].IdImage + local[i].Extension;
                                FileStream fs = File.Create(path);
                                SaveImageCustom(local[i], fs);
                                fs.Flush();
                                fs.Close();
                            }
                            else if (Directory.Exists(dirpath))
                            {
                                path = dirpath + @"\" + local[i].IdImage + local[i].Extension;
                                if (File.Exists(path) == true) { path = dirpath + @"\" + local[i].IdImage + i + local[i].Extension; }
                                FileStream fs = File.Create(path);
                                SaveImageCustom(local[i], fs);
                                fs.Flush();
                                fs.Close();
                            }
                        }

                        break;
                }
                Flag = false;
            }
            










































            Image foto = Bitmap.FromFile(@"D:\ДЗ\lab\source\dddfffggg.jpg");
            
            
            DirectoryInfo dir = new DirectoryInfo(@"D:\ДЗ\lab\source\");
            FileInfo[] t = dir.GetFiles();
            Console.WriteLine(t[0].Extension); // выдает .jpg
            Console.WriteLine(t[0].FullName); // выдает полный путь с именем файла
            Image newimg;
            using (FileStream fs = new FileStream(t[1].FullName, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                newimg = new Bitmap(fs);
            }
            Console.WriteLine("Stream   " +newimg.PropertyItems.Length);
            Image foto2 = Bitmap.FromFile(@"D:\ДЗ\lab\source\file2.jpg");
            Console.WriteLine("without stream" + foto2.PropertyItems.Length);
            Console.WriteLine("control   " + foto.PropertyItems.Length);

            Graphics e = Graphics.FromImage(foto);
            Console.WriteLine(e.PageUnit);
            // Create string to draw.
            string drawString = "Sample Text";

            // Create font and brush.
            Font drawFont = new Font("Arial", 200);
            SolidBrush drawBrush = new SolidBrush(Color.Blue);
            SizeF StringSize = e.MeasureString(drawString, drawFont);
            float vert = foto.Width - (StringSize.Width + 100);
            Console.WriteLine("vert  " + vert);
            Console.WriteLine("foto.Width  " + foto.Width);
            Console.WriteLine("MeasureString" + e.MeasureString(drawString, drawFont).Width);
            // Create point for upper-left corner of drawing.
            PointF drawPoint = new PointF(vert, 150.0F);
            //Rectangle ghg = new Rectangle(drawPoint, StringSize);
            // Draw string to screen.
            e.DrawString(drawString, drawFont, drawBrush, drawPoint);
            foto.Save(@"D:\ДЗ\lab\source\file2.jpg");
            
            PropertyItem[] massprop = foto.PropertyItems;
            ASCIIEncoding enc = new ASCIIEncoding();
            UTF8Encoding enc2 = new UTF8Encoding();
            UTF7Encoding enc5 = new UTF7Encoding();
            PropertyItem Prop =  foto.GetPropertyItem(0x0132);
            PropertyItem Prop2 = foto.GetPropertyItem(0x0004);
            string ggg = enc2.GetString(Prop.Value);
            Console.WriteLine(Prop.Value.Length);
            var ddd = BitConverter.ToUInt32(Prop2.Value, 0);
            var ddd2 = BitConverter.ToUInt32(Prop2.Value, 4);
            var ddd3 = BitConverter.ToUInt32(Prop2.Value, 8);
            var ddd4 = BitConverter.ToUInt32(Prop2.Value, 12);
            var ddd5 = BitConverter.ToUInt32(Prop2.Value, 16);
            var ddd6 = BitConverter.ToUInt32(Prop2.Value, 20);
            List<uint> lll = new List<uint>();
            lll.Add(ddd);
            lll.Add(ddd2);
            lll.Add(ddd3);
            lll.Add(ddd4);
            lll.Add(ddd5);
            lll.Add(ddd6);
            foreach (var i in lll)
            {
                Console.WriteLine(i);
            }
            Console.WriteLine("TEST    " + ggg);
            //to make method async GetGeoData ---- Begin
            WebRequest request = WebRequest.Create("https://geocode-maps.yandex.ru/1.x/?apikey=24e5f3e7-2bb0-4052-b2fc-5254ecd09eea&geocode=27.318294,53.547975");
            WebResponse response = request.GetResponse();
            using (Stream stream = response.GetResponseStream())
            {

                XmlReader d =  XmlReader.Create(stream);
                //Console.WriteLine("ddd" + d.Value);
                XDocument doc = XDocument.Load(d);
                List<XElement> elements = new List<XElement>();
                
                
                XElement coll = doc.Root;
                XNamespace aw = "http://maps.yandex.ru/address/1.x";
                
                IEnumerable<XElement> coll2 = coll.Descendants(aw + "formatted");
                foreach (XElement item in coll2)
                {
                    Console.WriteLine("Test111   " + item.Name.LocalName + "----" + item.Value);
                }
                
                Console.WriteLine("12434534556   " + elements.Count);

            }
            response.Close();
            Console.WriteLine("end");
            //to make method async GetGeoData ---- End
            //to name from string result


            Console.ReadLine();
        }
    }
}
