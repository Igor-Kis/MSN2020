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
using System.Security.Policy;

namespace WindowsFormsMSN2020
{
    enum Consts
    {
        DU,         // интервал летаргий
        S_t,        // полное сечение
        S_f,        // сечение деления
        NU,         // число вторичных нейтронов
        S_c,        // сечение захвата
        S_in,       // сечение неупругого рассеяния
        S_z,        // сечение замедления
        S_e,        // сечение упругого рассеяния
        MU,         // средний косинус угла рассеяния
        HI,         // доля нейтронов деления
        KSI,        // логарифмический декремент энергии
        S_u         // сечение увода//
    }       
    enum Zones
    { AZ, R}

    public class Compute
    {
        public double[,,] MacroSection = new double[2, 26, 12];
        ///public List<double[,,]> IsotopesData = new List<double[,,]>();
        public List<(double AZ, double R)> NucDensity = new List<(double, double)>();
       
        public double[,] DJ = new double[2, 26];
        public double[] HI = new double[26] { 0.016, 0.088, 0.184, 0.27, 0.202, 0.141, 0.061, 0.024, 0.01, 0.003, 0.001, 0, 0,0,0,0,0,0,0,0,0,0,0,0,0,0 };
        public List<double[,]> InElasticMatrixes = new List<double[,]>();
        public double[,] MatrixSigmaScatteringAZ = new double[26, 26];
        public List<double[]> SigmaU= new List<double[]>();
        public double[] sigmaU = new double[26];
        public double su;
        public double a1;
        public double a2;
        public double a3;
        public double[,] FJ = new double[2, 26];
        public double[,] FJZ = new double[2, 26];
        
        public double[] SA = new double[2];
        public double[] DA = new double[2];
        public double[] NFSA = new double[2];
        
        public double[] Bg2 = new double[2];
        public double R0;
        public double R0F;
        public double NU;
        public double[,] HIMatrix = new double[12, 5] { {2.4, 2.6, 2.8, 3.0, 3.2},
                                                        {0.016, 0.017, 0.018, 0.020, 0.21 },
                                                        {0.088, 0.092, 0.095, 0.098, 0.101},
                                                        {0.184, 0.186, 0.188, 0.190, 0.192},
                                                        {0.270, 0.270, 0.269, 0.268, 0.267},
                                                        {0.202, 0.200, 0.198, 0.196, 0.194},
                                                        {0.141, 0.139, 0.137, 0.135, 0.133},
                                                        {0.061, 0.060, 0.059, 0.058, 0.057},
                                                        {0.024, 0.023, 0.023, 0.022, 0.022},
                                                        {0.010, 0.009, 0.009, 0.009, 0.009},
                                                        {0.003, 0.003, 0.003, 0.003, 0.003},
                                                        {0.001, 0.001, 0.001, 0.001, 0.001}};
        public void HIinterpolation()
        {
            double Sum1=0;
            double Sum2=0;
            double a = 0;
            double b = 0;
            int i=0;
            for (int Group=0;Group<26;Group++)
            { 
                Sum1 += MacroSection[0, Group, (int)Consts.S_f] * MacroSection[0, Group, (int)Consts.NU];
                Sum2 += MacroSection[0, Group, (int)Consts.S_f];
            }
            NU = Sum1 / Sum2;

            if (NU < HIMatrix[0, 1]&NU> HIMatrix[0, 0])
            { i = 1; };
            if (NU < HIMatrix[0, 2] & NU > HIMatrix[0, 1])
            { i = 2; };
            if (NU < HIMatrix[0, 3] & NU > HIMatrix[0, 2])
            { i = 3; };
            if (NU < HIMatrix[0, 4] & NU > HIMatrix[0, 3])
            { i = 4; };

            for (int Group = 0; Group < 26; Group++)
            {
                if (Group < 11)
                {
                    a = (HIMatrix[Group + 1, i] - HIMatrix[Group + 1, i - 1])/ (HIMatrix[0, i] - HIMatrix[0, i - 1]);
                    b = HIMatrix[Group + 1, i - 1] - a * HIMatrix[0, i - 1];
                    MacroSection[(int)Zones.AZ, Group, (int)Consts.HI] =  a * NU + b;
                }
                else
                {
                    MacroSection[(int)Zones.AZ, Group, (int)Consts.HI] = 0;
                }
                
            }

        }

        public void LoadIsotopesData( ref double[,,] MacroSection, List<(double AZ, double R)> NucDensity)
        {
            int IsotopeNum = 0;
            string _filename = System.AppDomain.CurrentDomain.BaseDirectory + "\\Data.xml";
            string LastSuccess = "";
            if (File.Exists(_filename))
            {
                try
                {
                    XmlDocument main = new XmlDocument();
                    main.Load(_filename);
                    System.Xml.XmlNode root0 = main.DocumentElement;
                    if (root0 != null && root0.Name == "Isotopes")
                        foreach (XmlNode child in root0)

                            if (child.Name == "Isotope")
                            {
                                IsotopeProperties ip = new IsotopeProperties();
                                ip.LoadFromXml(child);
                                for (int Group = 0; Group < 26; Group++)
                                {
                                    su = 0;
                                    MacroSection[(int)Zones.AZ, Group, (int)Consts.S_t] += ip.SigmaTotal[Group]*NucDensity[IsotopeNum].AZ * Math.Pow(10, -24);
                                    MacroSection[(int)Zones.AZ, Group, (int)Consts.S_f] += ip.SigmaFission[Group] * NucDensity[IsotopeNum].AZ * Math.Pow(10, -24);
                                    MacroSection[(int)Zones.AZ, Group, (int)Consts.NU] += ip.SigmaFission[Group] * ip.NuFission[Group] * NucDensity[IsotopeNum].AZ * Math.Pow(10, -24);
                                    MacroSection[(int)Zones.AZ, Group, (int)Consts.S_c] += ip.SigmaCapture[Group] * NucDensity[IsotopeNum].AZ * Math.Pow(10, -24);
                                    MacroSection[(int)Zones.AZ, Group, (int)Consts.S_e] += ip.SigmaElasticScattering[Group] * NucDensity[IsotopeNum].AZ * Math.Pow(10, -24);
                                    MacroSection[(int)Zones.AZ, Group, (int)Consts.S_in] += ip.SigmaInelasticScattering[Group] * NucDensity[IsotopeNum].AZ * Math.Pow(10, -24);
                                    MacroSection[(int)Zones.AZ, Group, (int)Consts.S_z] += ip.SigmaElasticZam[Group] * NucDensity[IsotopeNum].AZ * Math.Pow(10, -24);
                                    MacroSection[(int)Zones.AZ, Group, (int)Consts.MU] += ip.SigmaElasticScattering[Group] * ip.MuElasticScattering[Group] * NucDensity[IsotopeNum].AZ * Math.Pow(10, -24);
                                    MacroSection[(int)Zones.AZ, Group, (int)Consts.KSI] += ip.SigmaElasticScattering[Group] * ip.KsiElasticScattering[Group] * NucDensity[IsotopeNum].AZ * Math.Pow(10, -24);
                                    
                                                                                                ///* ip.SigmaFission[Group] * NucDensity[IsotopeNum].AZ
                                    for (int Column = 0; Column < 26; Column++)
                                    {
                                        MatrixSigmaScatteringAZ[Group, Column] += ip.MatrixInelasticScattering[Group, Column] * NucDensity[IsotopeNum].AZ * Math.Pow(10, -24);
                                        if (Column>Group)
                                        { su += ip.MatrixInelasticScattering[Group, Column]; }
                                        
                                    }
                                    MacroSection[(int)Zones.AZ, Group, (int)Consts.S_u] += (ip.SigmaFission[Group] + ip.SigmaCapture[Group]+ ip.SigmaElasticZam[Group] + su)* NucDensity[IsotopeNum].AZ*Math.Pow(10, -24);
                                }
                                InElasticMatrixes.Add(ip.MatrixInelasticScattering);
                                
                                IsotopeNum ++;
                                LastSuccess = ip.Name;
                            };
                    for (int Group = 0; Group < 26; Group++)
                    {
                        MacroSection[(int)Zones.AZ, Group, (int)Consts.MU] = MacroSection[(int)Zones.AZ, Group, (int)Consts.MU] / MacroSection[(int)Zones.AZ, Group, (int)Consts.S_e];
                        MacroSection[(int)Zones.AZ, Group, (int)Consts.KSI] = MacroSection[(int)Zones.AZ, Group, (int)Consts.KSI] / MacroSection[(int)Zones.AZ, Group, (int)Consts.S_e];
                        if (MacroSection[(int)Zones.AZ, Group, (int)Consts.S_f] > 0)
                        {
                            MacroSection[(int)Zones.AZ, Group, (int)Consts.NU] = MacroSection[(int)Zones.AZ, Group, (int)Consts.NU] / MacroSection[(int)Zones.AZ, Group, (int)Consts.S_f];
                           /// MacroSection[(int)Zones.AZ, Group, (int)Consts.HI] = MacroSection[(int)Zones.AZ, Group, (int)Consts.HI] / MacroSection[(int)Zones.AZ, Group, (int)Consts.S_f];
                        }
                        else
                        {
                            MacroSection[(int)Zones.AZ, Group, (int)Consts.NU] = 0;
                            ///MacroSection[(int)Zones.AZ, Group, (int)Consts.HI] = 0;
                        }
                        DJ[(int)Zones.AZ, Group] = 1 / (3 * (MacroSection[(int)Zones.AZ, Group, (int)Consts.S_t] - MacroSection[(int)Zones.AZ, Group, (int)Consts.MU] * MacroSection[(int)Zones.AZ, Group, (int)Consts.S_e]));
                    }
                }
                catch (Exception e)
                {
                    System.Windows.Forms.MessageBox.Show("LoadIsotopesData. Проверьте правильность содержимого (" + _filename + ")! Возникла ошибка [" + e.Message + "]." + (LastSuccess == "" ? "" : "\nПоследнее успешное чтение:" + LastSuccess));
                }
            }
            else System.Windows.Forms.MessageBox.Show("LoadIsotopesData. Файл не найден (" + _filename + ")! ");
        }

        public void Potok(int zone)
        {///вычисление потоков
            
            for (int Column = 0; Column < 26; Column++)
            {

                su = 0;
                if (Column>0)
                {
                    for (int Row = 0; Row < Column; Row++)
                    {
                        su += FJ[zone, Row] * MatrixSigmaScatteringAZ[Row, Column]; 
                    }
                    su += FJ[zone, Column - 1] * MacroSection[zone, Column - 1, (int)Consts.S_z];
                }
                if (zone == (int)Zones.AZ)
                { a1 = 0; }
                if (zone == (int)Zones.AZ)
                { a2 = MacroSection[(int)Zones.AZ, Column, (int)Consts.HI]; }
                FJ[zone, Column] = (a1 + a2 + su ) / (a3 + MacroSection[(int)Zones.AZ, Column, (int)Consts.S_u]);
            }
            
         ///вычисление ценностей
            for (int Row = 25; Row >= 0; Row--)
            {
                su = 0;
                if (Row < 25)
                {
                    for (int Column = Row + 1; Column < 26; Column++)
                    {
                        su += FJZ[zone, Column] * MatrixSigmaScatteringAZ[Row, Column];
                    }
                    su += FJZ[zone, Row+1] * MacroSection[zone, Row+1, (int)Consts.S_z];
                }
                if (zone == (int)Zones.AZ)
                { a1 = 0; }
                if (zone == (int)Zones.AZ)
                { a2 = MacroSection[(int)Zones.AZ, Row, (int)Consts.NU]* MacroSection[(int)Zones.AZ, Row, (int)Consts.S_f]; }
                FJZ[zone, Row] = (a1 + a2 + su) / (a3 + MacroSection[(int)Zones.AZ, Row, (int)Consts.S_u]);
            }
        }
        public void OneGroupConst(int zone)
        {
            double C;
            double B=0;
            for (int i = 0; i < 26; i++)
            {
                DA[zone] += DJ[zone, i] * FJ[zone, i] * FJZ[zone, i];
                C = 0;
                if(i>0)
                {
                    for (int j = 0; j < i; j++)
                    { 
                        C+=FJ[zone,j]* MatrixSigmaScatteringAZ[j, i]; 
                    }
                    C += FJ[zone, i - 1] * MacroSection[zone, i - 1, (int)Consts.S_z];
                }
                SA[zone] += FJZ[zone, i] * (MacroSection[zone, i, (int)Consts.S_u] * FJ[zone, i] - C);
                C = 0;
                for(int j=0; j<26; j++)
                {
                    C += MacroSection[zone, j, (int)Consts.NU] * MacroSection[zone, j, (int)Consts.S_f] * FJ[zone, j];
                }
                NFSA[zone] += MacroSection[zone, i, (int)Consts.HI] * FJZ[zone, i] * C;
                B += FJ[zone, i] * FJZ[zone, i];
            }
            DA[zone] = DA[zone] / B;
            SA[zone] = SA[zone] / B;
            NFSA[zone] = NFSA[zone] / B;
            if (zone== (int)Zones.AZ)
            {
                Bg2[zone] = (NFSA[zone] - SA[zone]) / DA[zone];
            }
            else
            {
                Bg2[zone] = (SA[zone]- NFSA[zone]) / DA[zone];
            }
        }
        public void Radius()
        {
            R0 = 3.142 / Math.Sqrt(Bg2[(int)Zones.AZ]); ///крит. радиус "голого" р-ра + экстраполированная добавка
            R0F = R0 - 2.13 * DA[(int)Zones.AZ]; ///физический размер "голого" реактора
        }


    }
}






