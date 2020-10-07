using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Xml;

namespace WindowsFormsMSN2020
{
    public partial class Form1 : Form
    {
        MSNCalculation MSNCalc;
        public Dictionary<string, IsotopeProperties> Isotopes;
        public Form1()
        {
            InitializeComponent();
            Isotopes = new Dictionary<string, IsotopeProperties>();
            LoadData();
            if (Isotopes.Count > 0)
            {
                FillGrid();
            }

        }

        private void FillGrid()
        {
            dataGridView1.RowCount = Isotopes.Count;
            int pos = 0;
            foreach (var item in Isotopes)
            {
                dataGridView1.Rows[pos].Cells[0].Value = item.Value.Name;
                pos++;
            }
        }

        private void LoadData()
        {
            Isotopes.Clear();
            string _filename = System.AppDomain.CurrentDomain.BaseDirectory + "\\Data.xml";
            string LastSuccess = "";
            if (File.Exists(_filename))
            {
                try
                {
                    XmlDocument main = new XmlDocument();
                    main.Load(_filename);
                    System.Xml.XmlNode root0 = main.DocumentElement;
                    if (root0 != null && root0.Name== "Isotopes")
                        foreach (XmlNode child in root0)
                            if (child.Name == "Isotope")
                            {
                                IsotopeProperties ip = new IsotopeProperties();
                                ip.LoadFromXml(child);
                                Isotopes.Add(ip.Name, ip);
                                LastSuccess = ip.Name;
                            }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("LoadData. Проверьте правильность содержимого (" + _filename + ")! Возникла ошибка [" + e.Message + "]." + (LastSuccess == "" ? "" : "\nПоследнее успешное чтение:" + LastSuccess));
                }
            }
            else System.Windows.Forms.MessageBox.Show("LoadData. Файл не найден (" + _filename + ")! ");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            double T = 293;
            bool isSelfScreening = false;
            bool isInfinityReflector = true;
            double ReflectorTicknessRatio = 0.3;
            double HeatOutput = 100;
            MSNCalc = new MSNCalculation(T, isSelfScreening, isInfinityReflector, ReflectorTicknessRatio, HeatOutput);
            MacroIsotopePropertiesList ac = new MacroIsotopePropertiesList();
            ac.Add(new MacroIsotopeProperties(Isotopes["U235"], 1.2e+21, MSNCalc));
            ac.Add(new MacroIsotopeProperties(Isotopes["U238"], 1.2e+22, MSNCalc));
            ac.Add(new MacroIsotopeProperties(Isotopes["C"], 3.2e+23, MSNCalc));
            MacroIsotopePropertiesList r = new MacroIsotopePropertiesList();
            ac.Add(new MacroIsotopeProperties(Isotopes["C"], 3.3e+23, MSNCalc));
            MSNCalc.Init(ac, r);
        }
    }
}
