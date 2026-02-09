using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace API_iNews.Services
{
    public class StoryExportService
    {
        public StoryExportService() { }

        /// <summary>
        /// Converts a list of raw XML stories into a structured DataTable based on configured fields.
        /// </summary>
        /// <param name="rawStories">List of raw XML strings from iNews.</param>
        /// <param name="commaSeparatedFields">Comma-separated list of fields to extract (e.g., "title,page-number").</param>
        /// <returns>DataTable with columns matching the requested fields.</returns>
        public DataTable CreateStoryTable(List<string> rawStories, string commaSeparatedFields)
        {
            DataTable dt = new DataTable();

            if (string.IsNullOrEmpty(commaSeparatedFields))
            {
                commaSeparatedFields = "title,page-number"; // Default fallback
            }

            // 1. Create Columns dynamically
            string[] fields = commaSeparatedFields.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries)
                                          .Select(f => f.Trim())
                                          .ToArray();

            foreach (string field in fields)
            {
                if (!dt.Columns.Contains(field))
                {
                    dt.Columns.Add(field);
                }
            }

            // 2. Extract Data
            if (rawStories != null)
            {
                foreach (string xml in rawStories)
                {
                    try
                    {
                        DataRow row = dt.NewRow();
                        bool hasData = false;

                        foreach (string field in fields)
                        {
                            string val = ExtractField(xml, field);
                            row[field] = val;
                            if (!string.IsNullOrEmpty(val)) hasData = true;
                        }

                        // Only add if at least one field has data
                        if (hasData)
                        {
                            dt.Rows.Add(row);
                        }
                    }
                    catch { }
                }
            }
            return dt;
        }

        /// <summary>
        /// Saves a DataTable to an XML file.
        /// Uses Column Names as XML Tags.
        /// </summary>
        /// <param name="dt">The source DataTable.</param>
        /// <param name="filePath">Full path to save the XML file.</param>
        public void SaveStoriesToXml(DataTable dt, string filePath)
        {
            if (dt == null || dt.Rows.Count == 0) return;

            // Use XDocument/XElement for clean XML creation
            System.Xml.Linq.XElement root = new System.Xml.Linq.XElement("Stories");

            foreach (DataRow row in dt.Rows)
            {
                System.Xml.Linq.XElement story = new System.Xml.Linq.XElement("Story");
                foreach (DataColumn col in dt.Columns)
                {
                    // Use the column name (from config) as the XML tag name
                    string cleanVal = row[col]?.ToString()?.Trim() ?? "";
                    story.Add(new System.Xml.Linq.XElement(col.ColumnName, cleanVal));
                }
                root.Add(story);
            }

            // Save with indentation for readability
            root.Save(filePath);
        }

        /// <summary>
        /// Extracts a specific field value from a raw iNews XML string using Regex.
        /// Handles attributes and excludes self-closing tags.
        /// </summary>
        /// <param name="xmlInfo">The raw XML string.</param>
        /// <param name="fieldName">The extracted field name (e.g., "title").</param>
        /// <returns>The content of the field, or empty string if not found.</returns>
        private string ExtractField(string xmlInfo, string fieldName)
        {
            try
            {
                // Regex pattern to match tags like <string id="fieldName">Content</string>
                // Excludes self-closing tags using negative lookbehind (?<!/)
                string pattern = $@"<(\w+)[^>]*\bid\s*=\s*([""']){fieldName}\2[^>]*(?<!/)\s*>(.*?)</\1>";

                var match = System.Text.RegularExpressions.Regex.Match(xmlInfo, pattern, 
                    System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.Singleline);

                if (match.Success)
                {
                    return match.Groups[3].Value.Trim();
                }

                return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }
    }
}
