using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace TTDH
{
    public class Utility
    {
       
        public void XmlTreeDisplay(TreeView treeXml,string xml)
        {
            treeXml.Nodes.Clear();            
            // Load the XML Document
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.LoadXml(xml);                
            }
            catch (Exception err)
            {

                MessageBox.Show(err.Message);
                return;
            }
            ConvertXmlNodeToTreeNode(doc, treeXml.Nodes);
            treeXml.Nodes[0].ExpandAll();
        }
        public static string[] Split(string str, string searchString)
        {
            return str.Split(new string[] { searchString }, StringSplitOptions.RemoveEmptyEntries);
        }
        private void ConvertXmlNodeToTreeNode(XmlNode xmlNode,
            TreeNodeCollection treeNodes)
        {

            TreeNode newTreeNode = treeNodes.Add(xmlNode.Name);

            switch (xmlNode.NodeType)
            {
                case XmlNodeType.ProcessingInstruction:
                case XmlNodeType.XmlDeclaration:
                    newTreeNode.Text = "<?" + xmlNode.Name + " " +
                      xmlNode.Value + "?>";
                    break;
                case XmlNodeType.Element:
                    newTreeNode.Text = "<" + xmlNode.Name + ">";
                    break;
                case XmlNodeType.Attribute:
                    newTreeNode.Text = "ATTRIBUTE: " + xmlNode.Name;
                    break;
                case XmlNodeType.Text:
                case XmlNodeType.CDATA:
                    newTreeNode.Text = xmlNode.Value;
                    break;
                case XmlNodeType.Comment:
                    newTreeNode.Text = "<!--" + xmlNode.Value + "-->";
                    break;
            }

            if (xmlNode.Attributes != null)
            {
                foreach (XmlAttribute attribute in xmlNode.Attributes)
                {
                    ConvertXmlNodeToTreeNode(attribute, newTreeNode.Nodes);
                }
            }
            foreach (XmlNode childNode in xmlNode.ChildNodes)
            {
                ConvertXmlNodeToTreeNode(childNode, newTreeNode.Nodes);
            }
        }
       
    }
}
