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
    class Fotoset
    {
        
        
        public List<ImageStruct> ImageSet { get; set; }
        private List<string> Pathes { get; set; }
        private List<string> Extensions { get; set; }
        private List<DateTime> CreationTime { get; set; }


        public Fotoset()
        {
            Pathes = new List<string>();
            Extensions = new List<string>();
            ImageSet = new List<ImageStruct>();
            CreationTime = new List<DateTime>();

        }
        private void GetPathandExtension(DirectoryInfo dir)
        {
            FileInfo[] files =  dir.GetFiles();
            foreach (FileInfo item in files)
            {
                 Pathes.Add(item.FullName);
                 Extensions.Add(item.Extension);
                 CreationTime.Add(item.CreationTime);
            }
        }
        public void GetListImage(DirectoryInfo dir)
        {
            GetPathandExtension(dir);
            for (int i = 0; i < Pathes.Count; i++)
            {
                string str = Pathes[i];
                ImageStruct structura = new ImageStruct
                {
                    IdImage = i,
                    Image = Bitmap.FromFile(str),
                    Extension = Extensions[i],
                    CreationTime = CreationTime[i].ToString()
                };
                ImageSet.Add(structura);
            } 
        }
    }
}
