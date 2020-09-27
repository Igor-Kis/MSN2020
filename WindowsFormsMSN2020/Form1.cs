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
using System.Globalization;

namespace WindowsFormsMSN2020
{
    public partial class Form1 : Form
    {
        public Dictionary<string, IsotopeProperties> Isotopes;
        public Compute Compute = new Compute();
        
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
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void button1_Click(object sender, EventArgs e)
        {
            for (int i = 0; i< dataGridView1.RowCount; i++)
            {
                Compute.NucDens = (0.0, 0.0);
                double value = -1;
                if (dataGridView1[1, i].Value != null)
                {
                    if (double.TryParse(dataGridView1[1, i].Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                    {
                        Compute.NucDens.AZ = value * Math.Pow(10, 20);
                        ///double.Parse(dataGridView1[1, i].Value.ToString())
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Недопустимые символы в ячейке ввода");
                    }
                }
                if (dataGridView1[2, i].Value != null)
                {
                    if (double.TryParse(dataGridView1[2, i].Value.ToString(), NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                    {
                        Compute.NucDens.R = value * Math.Pow(10, 20);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Недопустимые символы в ячейке ввода");
                    }
                    ///Compute.NucDens.R = double.Parse(dataGridView1[2, i].Value.ToString()) * Math.Pow(10, 20); 
                }
                Compute.NucDensity.Add(Compute.NucDens);
            }
            Compute.LoadIsotopesData(ref Compute.MacroSection, Compute.NucDensity);
            Compute.HIinterpolation();
            Compute.Potok(0);
            Compute.OneGroupConst(0);
            Compute.Radius();
            for (int i = 0; i < 15; i++)
            {
                System.Windows.Forms.MessageBox.Show(Compute.HI[i].ToString());
                ///System.Windows.Forms.MessageBox.Show(Compute.R0.ToString());
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
