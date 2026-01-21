using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Data;
using System.IO;

namespace TTDH
{   
    public class ProcessingXMl2Class
    {        
        public string FieldMapping { get; set; }
       
        public ProcessingXMl2Class()
        {
            string mapping = "title,vtv-nthien,vtv-bientap,page-number";
            FieldMapping = mapping;
        }
        public ProcessingXMl2Class(string mapping)
        {
            FieldMapping = mapping;
        }
        public DataTable GetDataRows(string folderPath)
        {            
            #region Read file in folder
            string[] lstFiles = Directory.GetFiles(folderPath, "*.xml");
            List<string> xmlString = new List<string>();
            foreach (string fileName in lstFiles)
            {
                xmlString.Add(File.ReadAllText(fileName));
            }            
            return GetDataRows(xmlString);
            #endregion
        }
        public DataTable GetDataRows(List<string> xmlString)
        {
            #region Init table
            DataTable tbl = new DataTable();
            AddColumn(tbl, "StoryID");
            //AddColumn(tbl, "FormName");
            string[] key = FieldMapping.Split(new char[] { ';', ',' });
            foreach (string k in key)
            {
                AddColumn(tbl, k);
            }
            AddColumn(tbl, "Content");
            //AddColumn(tbl, "OradPlugins");
            AddColumn(tbl, "XmlContent");
            #endregion
            foreach (string xml in xmlString)
            {
                if (string.IsNullOrEmpty(xml))
                    continue;
                DataRow row = tbl.NewRow();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                XmlNode root = doc.DocumentElement;
                //Get header
                XmlNode header = root.ChildNodes[0];
                if (header == null)
                    continue;
                //170415-thanhth:Bo sung cai nay de no biet la story kieu float hay break thi ko doc du lieu
               bool isFloat = header.SelectSingleNode("meta").Attributes["float"] != null ? header.SelectSingleNode("meta").Attributes["float"].Value == "true" : false;
                bool isBreak = header.SelectSingleNode("meta").Attributes["break"] != null ? header.SelectSingleNode("meta").Attributes["break"].Value == "true" : false;
                bool isHold = header.SelectSingleNode("meta").Attributes["hold"] != null ? header.SelectSingleNode("meta").Attributes["hold"].Value == "true" : false;
                if (isFloat || isBreak)
                    continue;
                row["FormName"] = header.SelectSingleNode("formname").InnerText;
                row["StoryID"] = header.SelectSingleNode("storyid").InnerText;
                //get title
                XmlNode body = root.ChildNodes[1];
                if (body == null)
                    continue;
                foreach (string k in key)
                {
                    XmlNode temp = body.SelectSingleNode("string[@id='" + k + "']");
                    if (temp != null)
                        row[k] = temp.InnerText;
                }
                //Get content
                XmlNode content = root.ChildNodes[2];
                string xmlContent = "";
                string xmlDataContent = "";
                foreach (XmlNode n in content.ChildNodes)
                {
                    //06122017- danh co viec lay du lieu xml thoi
                    xmlDataContent += n.OuterXml + "\n";
                    if (!string.IsNullOrEmpty(n.InnerText))
                        xmlContent += n.InnerText +"\n";                        
                        //xmlContent += n.OuterXml + "\n";
                    //xmlContent += n.InnerText + "\n";
                }
                row["XmlContent"] = xmlDataContent;
                row["Content"] = xmlContent;
                
                //if (root.ChildNodes.Count>4)
                //{
                //    XmlNode pluginOrad = root.ChildNodes[4];
                //    string xmlPlugins = "";
                //    foreach (XmlNode n in pluginOrad.ChildNodes)
                //    {
                //        if (!string.IsNullOrEmpty(n.InnerText))
                //            xmlPlugins += n.InnerText + "\n";
                        
                //    }
                //    //row["OradPlugins"] = xmlPlugins;
                //}
                //else
                //{
                //    row["OradPlugins"] = "";
                //}
                tbl.Rows.Add(row);
            }
            return tbl;
        }
        private void AddColumn(DataTable tbl, string colName)
        {
            DataColumn col = new DataColumn(colName);
            tbl.Columns.Add(col);
        }
        
    }
}
