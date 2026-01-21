using System;
using System.Collections.Generic;
using System.Data;
using System.Xml;

namespace API_iNews
{
    public class StoryXmlParser
    {
        public static DataTable ToDataTable(List<string> xmlList, string fieldMapping)
        {
            var tbl = new DataTable();
            tbl.Columns.Add("StoryID");
            
            var fields = fieldMapping.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var f in fields)
            {
                var colName = f.Trim();
                if (!tbl.Columns.Contains(colName)) tbl.Columns.Add(colName);
            }
            tbl.Columns.Add("Content");
            tbl.Columns.Add("XmlContent");

            foreach (var xml in xmlList)
            {
                if (string.IsNullOrWhiteSpace(xml)) continue;
                try
                {
                    var doc = new XmlDocument();
                    doc.LoadXml(xml);
                    var root = doc.DocumentElement;
                    if (root == null || root.ChildNodes.Count < 3) continue;

                    var row = tbl.NewRow();

                    // 1. Header (Index 0) - Lấy StoryID
                    var headNode = root.ChildNodes[0];
                    if (headNode != null)
                    {
                        var idNode = headNode.SelectSingleNode("storyid"); // Tìm trong head thì thường ok
                        if (idNode != null) row["StoryID"] = idNode.InnerText;
                    }

                    // 2. Fields (Index 1) - Lấy Metadata
                    var fieldsNode = root.ChildNodes[1];
                    if (fieldsNode != null)
                    {
                        foreach (var f in fields)
                        {
                            var key = f.Trim();
                            var node = fieldsNode.SelectSingleNode($"string[@id='{key}']");
                            if (node != null) row[key] = node.InnerText;
                        }
                    }

                    // 3. Body (Index 2) - Lấy Content
                    var bodyNode = root.ChildNodes[2];
                    if (bodyNode != null)
                    {
                        string plainText = "";
                        string fullXml = "";
                        foreach (XmlNode child in bodyNode.ChildNodes)
                        {
                            fullXml += child.OuterXml + "\n";
                            if (!string.IsNullOrEmpty(child.InnerText)) plainText += child.InnerText + "\n";
                        }
                        row["Content"] = plainText;
                        row["XmlContent"] = fullXml;
                    }
                    tbl.Rows.Add(row);
                }
                catch { continue; }
            }
            return tbl;
        }
    }
}