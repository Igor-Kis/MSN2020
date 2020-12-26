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
            ///Тестовые значения ядерных плотностей
            /*dataGridView1[1, 4].Value = 54.95;
            dataGridView1[1, 14].Value = 42.31;
            dataGridView1[2, 14].Value = 42.31;
            dataGridView1[1, 20].Value = 64.84;
            dataGridView1[1, 21].Value = 259.4;
            
            */
            dataGridView1[1, 20].Value = 473.9;
            dataGridView1[2, 21].Value = 473.0;
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
            double ROld;
            double EP;
            CultureInfo culture;
            culture = CultureInfo.CreateSpecificCulture("eu-ES");
            (double AZ, double R) NucDens;
            for (int i = 0; i< dataGridView1.RowCount; i++)
            {
                NucDens = (0.0, 0.0);
                double value = -1;
                if (dataGridView1[1, i].Value != null)
                {
                    if (double.TryParse(dataGridView1[1, i].Value.ToString(), NumberStyles.Any, culture, out value))
                    {
                        NucDens.AZ = value * Math.Pow(10, 20);
                        ///double.Parse(dataGridView1[1, i].Value.ToString())
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Недопустимые символы в ячейке ввода");
                    }
                }
                if (dataGridView1[2, i].Value != null)
                {
                    if (double.TryParse(dataGridView1[2, i].Value.ToString(), NumberStyles.Any, culture, out value))
                    {
                        NucDens.R = value * Math.Pow(10, 20);
                    }
                    else
                    {
                        System.Windows.Forms.MessageBox.Show("Недопустимые символы в ячейке ввода");
                    }
                    ///Compute.NucDens.R = double.Parse(dataGridView1[2, i].Value.ToString()) * Math.Pow(10, 20); 
                }
                Compute.NucDensity.Add(NucDens);
            }
            int iteration = 0;
            Compute.Isotopes = Isotopes;
            Compute.CorrectNucDens(ref Compute.NucDensity);                         ///Учет линейного расширения
            Compute.LoadIsotopesData(ref Compute.MacroSection, Compute.NucDensity); ///Расчет макросечений
            Compute.HIinterpolation();                                              ///Интерполяция значений HI
            Compute.Potok((int)Zones.AZ, iteration);                                 ///Нулевая итерация           
            Compute.OneGroupConst((int)Zones.AZ);
            Compute.Radius((int)Zones.AZ);
            ROld = Compute.R0F;
            iteration++;
            do                                                                      ///1+ итерации
            {
                for(int zone=(int)Zones.AZ; zone<=(int)Zones.R;zone++)
                {
                    Compute.Potok(zone, iteration);
                    Compute.OneGroupConst(zone);
                }
                Compute.Transcendent();
                EP = Math.Abs(Compute.RNew - ROld) / ROld;
                ROld = Compute.RNew;
                iteration++;

            }
            while (EP > 0.0001 & iteration < 100);

            for (int i = 0; i < 1; i++)
            {
                System.Windows.Forms.MessageBox.Show(ROld.ToString());
            }
        }
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}                                                                               ///ВЕРСИЯ 1.91
