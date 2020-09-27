using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Additionals
{
    public interface IXmlSaveLoad
    {
        /// <summary>
        /// сохранение в XML
        /// </summary>
        void SaveToXml(XmlNode node);
        /// <summary>
        /// восстановление из XML
        /// </summary>
        void LoadFromXml(XmlNode node);
    }

    public class XmlHelper
    {
        public static XmlDocument CreateDocument(string rootElementName)
        {
            XmlDocument document = new XmlDocument();
            XmlNode root = document.CreateElement(rootElementName);
            document.AppendChild(root);
            return document;
        }

        public static XmlDocument LoadDocument(string fileName)
        {
            if (File.Exists(fileName))
            {
                XmlDocument document = new XmlDocument();
                document.Load(fileName);
                return document;
            }
            else
            {
                throw new FileNotFoundException("", fileName, null);
            }
        }

        public static XmlAttribute SetAttributeValueForNode(XmlNode targertNode, string attributeName, string Value)
        {
            XmlAttribute attr;
            if (targertNode.Attributes.GetNamedItem(attributeName) != null)
            {
                attr = targertNode.Attributes[attributeName];
            }
            else
            {
                attr = targertNode.OwnerDocument.CreateAttribute(attributeName);
                targertNode.Attributes.Append(attr);
            }

            attr.Value = Value;
            
            return attr;
        }

        public static XmlNode AddChildNodeToNode(XmlNode parentNode, string childNodeName)
        {
            XmlNode child = parentNode.OwnerDocument.CreateElement(childNodeName);
            parentNode.AppendChild(child);
            return child;
        }

        public static XmlNode GetChildNodeFromNode(XmlNode parentNode, string childNodeName)
        {
            foreach (XmlNode node in parentNode.ChildNodes)
            {
                if (node.Name == childNodeName)
                    return node;
            }

            return null;
        }

        public static int GetIntFromAttributeValue(XmlNode node, string attributeName, int defaultValue = 0)
        {
            if (node.Attributes.GetNamedItem(attributeName) != null)
            {
                int result = 0;
                if (!int.TryParse(node.Attributes[attributeName].Value, out result))
                    return defaultValue;
                return result;
            }
            return defaultValue;
        }

        public static short GetShortFromAttributeValue(XmlNode node, string attributeName, short defaultValue = 0)
        {
            if (node.Attributes.GetNamedItem(attributeName) != null)
            {
                short result = 0;
                if (!short.TryParse(node.Attributes[attributeName].Value, out result))
                    result = defaultValue;
                return result;
            }
            return defaultValue;
        }

        public static string GetStringFromAttributeValue(XmlNode node, string attributeName, string defaultValue = "")
        {
            if (node.Attributes.GetNamedItem(attributeName) != null)
            {
                return node.Attributes[attributeName].Value;
            }
            return defaultValue;
        }

        public static double GetDoubleFromAttributeValue(XmlNode node, string attributeName, double defaultValue = 0.0)
        {
            if (node.Attributes.GetNamedItem(attributeName) != null)
            {
                double result = 0;
                if (!double.TryParse(node.Attributes[attributeName].Value, out result))
                    result = defaultValue;
                return result;
            }
            return defaultValue;
        }

        public static float GetFloatFromAttributeValue(XmlNode node, string attributeName, float defaultValue = 0.0f)
        {
            return (float)GetDoubleFromAttributeValue(node, attributeName, defaultValue);
        }

        public static bool GetBoolFromAttributeValue(XmlNode node, string attributeName, bool defaultValue = true)
        {
            if (node.Attributes.GetNamedItem(attributeName) != null)
            {
                bool result;
                if (!bool.TryParse(node.Attributes[attributeName].Value, out result))
                    result = defaultValue;
                return result;
            }
            return defaultValue;
        }
    }
}
