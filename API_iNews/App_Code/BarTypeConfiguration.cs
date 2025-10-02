using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace TTDH
{
    public class BarType
    {
        public string Name { get; set; }
        public string SceneName { get; set; }
        public string LangName { get; set; }
        public string BarNameEXP { get; set; }
        public string LocaltionEXP { get; set; }
        public string LogoEXP { get; set; }
        public string LogoPath { get; set; }
        public string Display { get; set; }
        public string On { get; set; }
    }
    public class BarTypeCollection
    {
        private List<BarType> barTypes;
        public List<BarType> BarTypes
        {
            get { return barTypes; }
        }
        public BarTypeCollection()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Directory.GetCurrentDirectory()+"//BarType.xml");
            barTypes = new List<BarType>();
            foreach(XmlElement n in doc.DocumentElement)
            {
                BarType bar = new BarType();
                bar.Name = n.Attributes["Name"].Value;
                bar.SceneName = n.Attributes["SceneName"].Value;
                bar.LangName = n.Attributes["LangName"].Value;
                bar.BarNameEXP = n.Attributes["BarNameEXP"].Value;
                bar.LocaltionEXP = n.Attributes["LocaltionEXP"].Value;
                bar.LogoEXP = n.Attributes["LogoEXP"].Value;
                bar.LogoPath = n.Attributes["LogoPath"].Value;
                bar.Display = n.Attributes["Display"].Value;
                barTypes.Add(bar);
            }
        }
        public BarType GetBar(string name)
        {
            BarType b = new BarType();
            if (barTypes.Count == 0)
                return b;
            else
            {
                b = barTypes.Find(n => n.Name == name);                
                if (b!=null && b.Name!=name)
                    return barTypes[0];
                else
                    return b;
            }
        }
        public void AddBarType(string bariNewsName, string logoPath, string barDefault)
        {
            
            BarType bar = this.GetBar(barDefault);
            if (bar.Display == bariNewsName || bar.LogoPath == logoPath)
                return;
            XmlDocument doc = new XmlDocument();
            doc.Load(Directory.GetCurrentDirectory() + "//BarType.xml");
            XmlElement n = doc.CreateElement("BarType");
            n.SetAttribute("Name", "Bar" + (this.BarTypes.Count + 1).ToString());
            n.SetAttribute("SceneName", bar.SceneName);
            n.SetAttribute("LangName", bar.LangName);
            n.SetAttribute("BarNameEXP", bar.BarNameEXP);
            n.SetAttribute("LocaltionEXP", bar.LocaltionEXP);
            n.SetAttribute("LogoEXP", bar.LogoEXP);
            n.SetAttribute("LogoPath", logoPath);
            n.SetAttribute("Display", bariNewsName);
            n.SetAttribute("On", bariNewsName);
            //them moi hoan toan
            doc.DocumentElement.AppendChild(n);
            doc.Save(Directory.GetCurrentDirectory() + "//BarType.xml");
        }
    }
}
