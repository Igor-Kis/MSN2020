using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Globalization;
using System.Diagnostics;


namespace Additionals
{
    public class CellSettings : List<string>
    {
        public string text;
        public int MergeAcross = 0;
        public int MergeDown = 0;
        public int Index = 0;
        public bool isNextIndex = false;
        public bool isNeedIndex = false;
        public bool isInsert = true;
        public int Level = 0;

        public string Style = "";
    }
    public class CellSettingsList : List<CellSettings>
    {
    }

    public class StringListObj
    {
        public string name = "";
        public List<string> HeadLines;
        public List<List<string>> DataGrid;
        public StringListObj()
        {
            HeadLines = new List<string>();
            DataGrid = new List<List<string>>();
        }
    }

    public class xlsFromXML
    {
        protected XmlDocument doc;
        protected NumberFormatInfo nfi = null;
        protected XmlNode root0 = null;//корневой элемент

        protected string xmlDTFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'Z'";
        protected string xmlDFormat = "dd.MM.yyyy";

        public xlsFromXML()
        {
            doc = new XmlDocument();
            nfi = new NumberFormatInfo();
            nfi.NumberDecimalSeparator = ".";
            LoadTemplate(doc);
            root0 = doc.DocumentElement;//корневой элемент
        }

        public void init(StringListObj slo)
        {
            if (slo != null)
            {
                XmlNode w = AddWorksheet(root0, slo.name);
                FillDataTable(w, slo);
                return;
            }
        }

        protected XmlNode GetNodeWithName(XmlNode root, string name)
        {
            foreach (XmlNode n in root.ChildNodes)
                if (n.Name == name) return n;
            return null;
        }


        public void FillDataTable(XmlNode w, StringListObj slo)
        {
            XmlNode t = GetNodeWithName(w, "Table");
            int RowCount = 0;
            int cols = 0;
            XmlNode r = null;
            if (t != null)
            {
                AddColumn(t, 120);
                //заголовок
                foreach (string s in slo.HeadLines)
                {
                    r = AddRow(t, ref RowCount, 0);
                    AddCell(r, "", s, 0, 0, 0);
                }
                //таблица
                int num = 0;
                foreach (List<string> str in slo.DataGrid)
                {
                    r = AddRow(t, ref RowCount, num == 0 ? 150 : 0);
                    for (int i = 0; i < str.Count; i++)
                    {
                        string s = str[i];
                        bool isNum = double.TryParse(s, out double d);
                        AddCell(r, (num == 0) ? "Vert" : "m01", s, 0, 0, 0, isNum ? "Number" : "String");
                    }
                    num++;
                    cols = str.Count;
                }
                ((XmlElement)t).SetAttribute("ExpandedColumnCount", "urn:schemas-microsoft-com:office:spreadsheet", cols.ToString());
                ((XmlElement)t).SetAttribute("ExpandedRowCount", "urn:schemas-microsoft-com:office:spreadsheet", RowCount.ToString());
            }
        }

        protected XmlNode AddCell(XmlNode r, string StyleID, string data, int MergeAcross, int MergeDown, int Index, string DataType = "String")
        {
            XmlElement root = r.OwnerDocument.CreateElement("Cell", "urn:schemas-microsoft-com:office:spreadsheet");
            //XmlElement root = r.OwnerDocument.CreateElement("Cell");
            if (Index > 0)
                root.SetAttribute("Index", "urn:schemas-microsoft-com:office:spreadsheet", Index.ToString());
            if (MergeAcross > 0)
                root.SetAttribute("MergeAcross", "urn:schemas-microsoft-com:office:spreadsheet", MergeAcross.ToString());
            if (MergeDown > 0)
                root.SetAttribute("MergeDown", "urn:schemas-microsoft-com:office:spreadsheet", MergeDown.ToString());
            if (StyleID != "")
                root.SetAttribute("StyleID", "urn:schemas-microsoft-com:office:spreadsheet", StyleID);
            XmlElement root1 = r.OwnerDocument.CreateElement("Data", "urn:schemas-microsoft-com:office:spreadsheet");
            root1.SetAttribute("Type", "urn:schemas-microsoft-com:office:spreadsheet", DataType);
            root1.InnerText = data;
            root.AppendChild(root1);
            r.AppendChild(root);
            return root;
        }

        protected XmlNode AddRow(XmlNode t, ref int RowCount, int Height)
        {
            XmlElement root = t.OwnerDocument.CreateElement("Row", "urn:schemas-microsoft-com:office:spreadsheet");
            //XmlElement root = t.OwnerDocument.CreateElement("Row");
            root.SetAttribute("AutoFitHeight", "urn:schemas-microsoft-com:office:spreadsheet", "0");
            if (Height != 0)
                root.SetAttribute("Height", "urn:schemas-microsoft-com:office:spreadsheet", Height.ToString());
            t.AppendChild(root);
            RowCount++;
            return root;
        }

        protected XmlNode AddColumn(XmlNode t, int width)
        {
            XmlElement root = t.OwnerDocument.CreateElement("Column", "urn:schemas-microsoft-com:office:spreadsheet");
            //XmlElement root = t.OwnerDocument.CreateElement("Column");
            root.SetAttribute("AutoFitWidth", "urn:schemas-microsoft-com:office:spreadsheet", "0");
            root.SetAttribute("Width", "urn:schemas-microsoft-com:office:spreadsheet", width.ToString());
            t.AppendChild(root);
            return root;
        }

        protected void LoadTemplate(XmlDocument doc)
        {
            doc.LoadXml("<?xml version=\"1.0\"?> \n" +
            "<?mso-application progid=\"Excel.Sheet\"?> \n" +
            "<Workbook xmlns=\"urn:schemas-microsoft-com:office:spreadsheet\" " +
            "xmlns:o=\"urn:schemas-microsoft-com:office:office\" " +
            "xmlns:x=\"urn:schemas-microsoft-com:office:excel\" " +
            "xmlns:ss=\"urn:schemas-microsoft-com:office:spreadsheet\" " +
            "xmlns:html=\"http://www.w3.org/TR/REC-html40\"> \n" +
            "  <DocumentProperties xmlns=\"urn:schemas-microsoft-com:office:office\"> \n" +
            "    <Author>ScatisAnalitics</Author> \n" +
            "    <LastAuthor>ScatisAnalitics</LastAuthor> \n" +
            "    <Created>" + DateTime.Now.ToString(xmlDTFormat) + "</Created> \n" +
            "    <LastSaved>" + DateTime.Now.ToString(xmlDTFormat) + "</LastSaved> \n" +
            "    <Version>12.00</Version> \n" +
            "  </DocumentProperties> \n" +
            "  <ExcelWorkbook xmlns=\"urn:schemas-microsoft-com:office:excel\"> \n" +
            "    <WindowHeight>7500</WindowHeight> \n" +
            "    <WindowWidth>13395</WindowWidth> \n" +
            "    <WindowTopX>360</WindowTopX> \n" +
            "    <WindowTopY>120</WindowTopY> \n" +
            "    <ActiveSheet>1</ActiveSheet> \n" +
            "    <ProtectStructure>False</ProtectStructure> \n" +
            "    <ProtectWindows>False</ProtectWindows> \n" +
            "  </ExcelWorkbook> \n" +
            "  <Styles> \n" +
            "   <Style ss:ID=\"Default\" ss:Name=\"Normal\"> \n" +
            "      <Alignment ss:Vertical=\"Bottom\" /> \n" +
            "      <Borders /> \n" +
            "      <Font ss:FontName=\"Calibri\" x:CharSet=\"204\" x:Family=\"Swiss\" ss:Size=\"11\" ss:Color=\"#000000\" /> \n" +
            "      <Interior /> \n" +
            "      <NumberFormat /> \n" +
            "      <Protection /> \n" +
            "   </Style> \n" +
            "   <Style ss:ID=\"h01\" > \n" +
            "      <Alignment ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\" ss:WrapText=\"1\"/> \n" +
            "      <Borders> \n" +
            "       <Border ss:Position = \"Bottom\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "       <Border ss:Position = \"Left\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "       <Border ss:Position = \"Right\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "       <Border ss:Position = \"Top\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "      </Borders> \n" +
            "   </Style> \n" +
            "   <Style ss:ID=\"m01\" > \n" +
            "      <Alignment ss:Vertical=\"Bottom\" /> \n" +
            "      <Borders> \n" +
            "       <Border ss:Position = \"Bottom\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "       <Border ss:Position = \"Left\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "       <Border ss:Position = \"Right\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "       <Border ss:Position = \"Top\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "      </Borders> \n" +
            "   </Style> \n" +
            "   <Style ss:ID=\"Vert\" > \n" +
            "      <Alignment  ss:Horizontal=\"Center\" ss:Vertical=\"Bottom\" ss:Rotate=\"90\" ss:WrapText=\"1\"/> \n" +
            "      <Borders> \n" +
            "       <Border ss:Position = \"Bottom\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "       <Border ss:Position = \"Left\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "       <Border ss:Position = \"Right\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "       <Border ss:Position = \"Top\" ss:LineStyle = \"Continuous\" ss:Weight = \"1\" /> \n" +
            "      </Borders> \n" +
            "   </Style> \n" +
            "  </Styles> \n" +
            "</Workbook> \n");


            /*
             root = doc.CreateElement("Styles");
             XmlNode root1 = root.OwnerDocument.CreateElement("Style");
             XmlHelper.SetAttributeValueForNode(root1, "ss:ID", "Default");
             XmlHelper.SetAttributeValueForNode(root1, "ss:Name", "Normal");
             XmlNode root2 = root.OwnerDocument.CreateElement("Alignment");
             XmlHelper.SetAttributeValueForNode(root2, "ss:Vertical", "Bottom");
             root1.AppendChild(root2);
             root2 = root.OwnerDocument.CreateElement("Borders");
             root1.AppendChild(root2);
             root2 = root.OwnerDocument.CreateElement("Font");
             XmlHelper.SetAttributeValueForNode(root2, "ss:FontName", "Calibri");
             XmlHelper.SetAttributeValueForNode(root2, "x:CharSet", "204");
             XmlHelper.SetAttributeValueForNode(root2, "x:Family", "Swiss");
             XmlHelper.SetAttributeValueForNode(root2, "ss:Size", "11");
             XmlHelper.SetAttributeValueForNode(root2, "ss:Color", "#000000");       
             root1.AppendChild(root2);
             root2 = root.OwnerDocument.CreateElement("Interior");
             root1.AppendChild(root2);
             root2 = root.OwnerDocument.CreateElement("NumberFormat");
             root1.AppendChild(root2);
             root2 = root.OwnerDocument.CreateElement("Protection");
             root1.AppendChild(root2);
             root.AppendChild(root1);
             doc.AppendChild(root);

                        */
        }

        protected XmlNode AddWorksheet(XmlNode root0, string name)
        {
            XmlElement root = root0.OwnerDocument.CreateElement("Worksheet", "urn:schemas-microsoft-com:office:spreadsheet");
            root.SetAttribute("Name", "urn:schemas-microsoft-com:office:spreadsheet", name);

            XmlElement root1 = root.OwnerDocument.CreateElement("Table", "urn:schemas-microsoft-com:office:spreadsheet");
            root1.SetAttribute("ExpandedColumnCount", "urn:schemas-microsoft-com:office:spreadsheet", "1");
            root1.SetAttribute("ExpandedRowCount", "urn:schemas-microsoft-com:office:spreadsheet", "1");
            root1.SetAttribute("FullColumns", "urn:schemas-microsoft-com:office:excel", "1");
            root1.SetAttribute("FullRows", "urn:schemas-microsoft-com:office:excel", "1");
            root1.SetAttribute("DefaultRowHeight", "urn:schemas-microsoft-com:office:spreadsheet", "15");
            root.AppendChild(root1);

            root1 = root.OwnerDocument.CreateElement("WorksheetOptions");
            XmlHelper.SetAttributeValueForNode(root1, "xmlns", "urn:schemas-microsoft-com:office:excel");
            XmlNode root2 = root.OwnerDocument.CreateElement("PageSetup");

            XmlElement root3 = root.OwnerDocument.CreateElement("Header");
            //XmlHelper.SetAttributeValueForNode(root3, "x:Margin", "0.3");
            root3.SetAttribute("Margin", "urn:schemas-microsoft-com:office:excel", "0.3");
            root2.AppendChild(root3);
            root3 = root.OwnerDocument.CreateElement("Footer");
            //XmlHelper.SetAttributeValueForNode(root3, "x:Margin", "0.3");
            root3.SetAttribute("Margin", "urn:schemas-microsoft-com:office:excel", "0.3");
            root2.AppendChild(root3);
            root3 = root.OwnerDocument.CreateElement("PageMargins");
            //XmlHelper.SetAttributeValueForNode(root3, "x:Bottom", "0.75");
            //XmlHelper.SetAttributeValueForNode(root3, "x:Left", "0.7");
            //XmlHelper.SetAttributeValueForNode(root3, "x:Right", "0.7");
            //XmlHelper.SetAttributeValueForNode(root3, "x:Top", "0.75");
            root3.SetAttribute("Bottom", "urn:schemas-microsoft-com:office:excel", "0.75");
            root3.SetAttribute("Left", "urn:schemas-microsoft-com:office:excel", "0.75");
            root3.SetAttribute("Right", "urn:schemas-microsoft-com:office:excel", "0.75");
            root3.SetAttribute("Top", "urn:schemas-microsoft-com:office:excel", "0.75");
            root2.AppendChild(root3);
            root3 = root.OwnerDocument.CreateElement("ProtectObjects");
            root3.InnerText = "False";
            root2.AppendChild(root3);
            root3 = root.OwnerDocument.CreateElement("ProtectScenarios");
            root3.InnerText = "False";
            root2.AppendChild(root3);
            //root2.OwnerDocument.LoadXml("<ProtectObjects>False</ProtectObjects>");
            //root2.OwnerDocument.LoadXml("<ProtectScenarios>False</ProtectScenarios>");
            root1.AppendChild(root2);
            root.AppendChild(root1);

            root0.AppendChild(root);
            return root;
        }

        public bool SaveToFile(string filename, bool withMsg = true)
        {

            try
            {
                doc.Save(filename);
                if (withMsg) System.Windows.Forms.MessageBox.Show("Файл " + filename + " успешно сохранен.");
                return true;
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Проблема с сохранением файла " + filename + ". Error:" + e.Message);
            }
            return false;
        }
    }

    public static class RunExcel
    { 
        public static void StartExcel_ShellExecute(string f_name)
        {
            try
            {
                Process myProcess = new Process();
                myProcess.StartInfo.UseShellExecute = true;
                myProcess.StartInfo.FileName = f_name;
                myProcess.Start();
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show("Запуск Excel. " + ex.Message);
            }
        }
    }
}