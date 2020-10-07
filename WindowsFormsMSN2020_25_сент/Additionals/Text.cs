using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Additionals
{
    public class Text : IXmlSaveLoad
    {
        public string value { get; set; }
        public string fontFamily { get; set; }
        public double fontSize { get; set; }
        public bool bold { get; set; }
        public bool italic { get; set; }
        public System.Drawing.Color color { get; set; }
        public System.Windows.HorizontalAlignment alignment { get; set; }

        #region IXmlSaveLoad Members

        public void SaveToXml(System.Xml.XmlNode node)
        {
            if (node == null)
                return;

            XmlHelper.SetAttributeValueForNode(node, "value", value);
            XmlHelper.SetAttributeValueForNode(node, "fontFamily", fontFamily);
            XmlHelper.SetAttributeValueForNode(node, "fontSize", fontSize.ToString());
            XmlHelper.SetAttributeValueForNode(node, "bold", (bold) ? "1" : "0");
            XmlHelper.SetAttributeValueForNode(node, "italic", (italic) ? "1" : "0");
            XmlHelper.SetAttributeValueForNode(node, "color", color.ToArgb().ToString());
            XmlHelper.SetAttributeValueForNode(node, "alignment", ((int)alignment).ToString());
        }

        public void LoadFromXml(System.Xml.XmlNode node)
        {
            if (node == null)
                return;

            value = XmlHelper.GetStringFromAttributeValue(node, "value", value);
            fontFamily = XmlHelper.GetStringFromAttributeValue(node, "fontFamily", fontFamily);
            fontSize = XmlHelper.GetDoubleFromAttributeValue(node, "fontSize", fontSize);
            bold = XmlHelper.GetBoolFromAttributeValue(node, "bold", bold);
            italic = XmlHelper.GetBoolFromAttributeValue(node, "italic", italic);
            color = System.Drawing.Color.FromArgb(XmlHelper.GetIntFromAttributeValue(node, "color", color.ToArgb()));
            alignment = (System.Windows.HorizontalAlignment)XmlHelper.GetIntFromAttributeValue(node, "alignment", (int)alignment);
        }

        #endregion

        public void SetFont(System.Drawing.Font font)
        {
            fontFamily = font.FontFamily.Name;
            fontSize = font.Size;
            bold = font.Bold;
            italic = font.Italic;
        }

        public System.Drawing.Font GetFont()
        {
            System.Drawing.Font result;
            try
            {
                System.Drawing.FontStyle fs = new System.Drawing.FontStyle();
                if (bold)
                    fs = fs | System.Drawing.FontStyle.Bold;
                if (italic)
                    fs = fs | System.Drawing.FontStyle.Italic;
                result = new System.Drawing.Font(fontFamily, (float)fontSize, fs);
            }
            catch
            {
                result = System.Drawing.SystemFonts.DefaultFont;
            }
            return result;
        }

        public Text()
        {
            value = "";
            fontFamily = "Segoe UI";
            fontSize = 9.0;
            bold = false;
            italic = false;
            color = System.Drawing.Color.Black;
            alignment = System.Windows.HorizontalAlignment.Left;
        }

        public void CopyTo(Text text)
        {
            text.value = value;
            text.fontFamily = fontFamily;
            text.fontSize = fontSize;
            text.bold = bold;
            text.italic = italic;
            text.color = color;
            text.alignment = alignment;
        }
    }
}
