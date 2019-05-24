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
        public static DirectoryInfo NewDir { get; set; }
        public static DirectoryInfo TargetDir { get; set; }
        public static MetaDataReceiver Metadata { get; set; }

        public static void Getloc (ImageData img, XMLWorker xmlworker, Requester requester)
        {
            string longit = img.Longitude;
            string width = img.Widthude;
            string Location = requester.GetLocationOfImage(xmlworker, longit, width);
            img.Location = Location;
        }
        

        public static void SaveImageCustom(ImageData imgstruct, Stream fs)
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
                        NewDir = new DirectoryInfo(DirectoryPath);
                        TargetDir = NewDir.CreateSubdirectory("rename");
                        Metadata = new MetaDataReceiver();
                        Metadata.GetListImage(NewDir);
                        Metadata.GetData();
                        for (int i = 0; i< Metadata.ImageSet.Count; i++)
                        {
                            List<ImageData> local = Metadata.ImageSet;
                            string name = local[i].Date.RemoverInvalidSymbols();
                            string path = TargetDir.FullName + @"\" + name + local[i].Extension;
                            if (File.Exists(path) == true) { path = TargetDir.FullName + @"\" + name +i + local[i].Extension;  }
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
                        NewDir = new DirectoryInfo(DirectoryPath2);
                        TargetDir = NewDir.CreateSubdirectory("drawstring");
                        Drawer drw = new Drawer();
                        Metadata = new MetaDataReceiver(drw);
                        Metadata.GetListImage(NewDir);
                        Metadata.GetData();
                        for (int i = 0; i < Metadata.ImageSet.Count; i++)
                        {
                            Image StringImage = null;
                            List<ImageData> local = Metadata.ImageSet;
                            string path = TargetDir.FullName + @"\" + local[i].IdImage + local[i].Extension;
                            if (File.Exists(path) == true) { path = TargetDir.FullName + @"\" + local[i].IdImage + i + local[i].Extension; }
                            FileStream fs = File.Create(path);
                            StringImage = Metadata.Drawer.DrawTheString(local[i]);
                            SaveImageCustom(local[i], fs);
                            fs.Flush();
                            fs.Close();
                        }

                        Console.WriteLine("finished");

                        break;
                    case "3":
                        Console.WriteLine("Enter directory");
                        string DirectoryPath3 = @"D:\ДЗ\lab\source"; // типо ввожу
                        NewDir = new DirectoryInfo(DirectoryPath3);
                        TargetDir = NewDir.CreateSubdirectory("sortbyyear");
                        Metadata = new MetaDataReceiver();
                        Metadata.GetListImage(NewDir);
                        Metadata.GetData();
                        for (int i = 0; i < Metadata.ImageSet.Count; i++)
                        {
                          
                            List<ImageData> local = Metadata.ImageSet;
                            string dirpath = TargetDir.FullName + @"\" + local[i].Date.Substring(6, 4);
                            string path = TargetDir.FullName + @"\" + local[i].IdImage + local[i].Extension; // тоже не нужно, заменить на  пустую строку
                            if (!Directory.Exists(dirpath))
                            {
                                TargetDir.CreateSubdirectory(local[i].Date.Substring(6, 4));
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
                        NewDir = new DirectoryInfo(DirectoryPath4);
                        TargetDir = NewDir.CreateSubdirectory("location");
                        Requester requester = new Requester();
                        requester.YandexApi = "24e5f3e7-2bb0-4052-b2fc-5254ecd09eea";
                        Metadata = new MetaDataReceiver();
                        XMLWorker xmlworker = new XMLWorker();
                        Metadata.GetListImage(NewDir);
                        Metadata.GetData();
                        
                        for (int i = 0; i < Metadata.ImageSet.Count; i++)
                        {
                            ImageData img = Metadata.ImageSet[i];
                            Getloc(img, xmlworker, requester);
                            

                        }
                        
                        
                        
                        for (int i = 0; i < Metadata.ImageSet.Count; i++)
                        {

                            List<ImageData> local = Metadata.ImageSet;
                            string dirpath = TargetDir.FullName + @"\" + local[i].Location;
                            string path = TargetDir.FullName + @"\" + local[i].IdImage + local[i].Extension; //это не нужно
                            if (!Directory.Exists(dirpath))
                            {
                                TargetDir.CreateSubdirectory(local[i].Location);
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
                        // последние фото поределились как атлантический океан???
                        // структуру переделать в класс
                        // в метода реквеста надо сделать трай кэтч
                        // блак файнали все перезатирает!!!! поправить
                        // проверить даты на допустимые символы -- поправил но надо прочитать про форматы дат
                        // в третьем варианте путаница с форматами дат, неправильно выделяется год
                        // в первом варианте название даты  попадает в путь и содержит недопустимые знаки
                        // если локация не найдена то присвоить значение undefined
                        // в методе getdate  не нужeн блок else
                        // remove invalid symbols  сделай методом расширения
                        // WebResponse, graphics, solidbrush: brush, font, Image реализует idispose
                        break;
                }
                Flag = false;
            }
            Console.ReadLine();
        }
    }
}
