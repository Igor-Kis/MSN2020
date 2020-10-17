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
        public Dictionary<string, IsotopeProperties> Isotopes = new Dictionary<string, IsotopeProperties>();
        public double[,,] MacroSection = new double[2, 26, 12];
        ///public List<double[,,]> IsotopesData = new List<double[,,]>();
        public List<(double AZ, double R)> NucDensity = new List<(double, double)>();
       
        public double[,] DJ = new double[2, 26];
        ///public double[] HI = new double[26] { 0.016, 0.088, 0.184, 0.27, 0.202, 0.141, 0.061, 0.024, 0.01, 0.003, 0.001, 0, 0,0,0,0,0,0,0,0,0,0,0,0,0,0 };
        public List<double[,]> InElasticMatrixes = new List<double[,]>();
        public double[,,] MatrixSigmaScattering = new double[2,26, 26];
        ///public List<double[]> SigmaU= new List<double[]>();
        ///public double[] sigmaU = new double[26];
        
        public double[,] FJ = new double[2, 26];
        public double[,] FJZ = new double[2, 26];
        
        public double[] SA = new double[2];
        public double[] DA = new double[2];
        public double[] NFSA = new double[2];
        
        public double[] Bg2 = new double[2];
        public double R0;
        public double R0F;
        public double NU;
        public double[,] HIMatrix = new double[11, 5] { 
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
        public double[] NUArray = new double[5] { 2.4, 2.6, 2.8, 3.0, 3.2 };
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

            if (NU < NUArray[1]&NU> NUArray[0])
            { i = 1; };
            if (NU < NUArray[2] & NU > NUArray[1])
            { i = 2; };
            if (NU < NUArray[3] & NU > NUArray[2])
            { i = 3; };
            if (NU < NUArray[4] & NU > NUArray[3])
            { i = 4; };
            if (NU>0)
            {
                for (int Group = 0; Group < 26; Group++)
                {
                    if (Group < 11)
                    {
                        a = (HIMatrix[Group, i] - HIMatrix[Group, i - 1]) / (NUArray[i] - NUArray[i - 1]);
                        b = HIMatrix[Group, i - 1] - a * NUArray[i - 1];
                        MacroSection[(int)Zones.AZ, Group, (int)Consts.HI] = a * NU + b;
                    }
                    else
                    {
                        MacroSection[(int)Zones.AZ, Group, (int)Consts.HI] = 0;
                    }

                }
            }
            else { System.Windows.Forms.MessageBox.Show("HIinterpolation. Не получается рассчитать значения HI"); }
            

        }

        public void LoadIsotopesData( ref double[,,] MacroSection, List<(double AZ, double R)> NucDensity)
        {            
            int pos = 0;
            int IsotopeNum = 0;
            double su1;
            double NuclearDensity;

            for (int Zone = 0; Zone < 2; Zone++)
            {
                IsotopeNum = 0;
                foreach (var isotope in Isotopes)
                {

                    for (int Group = 0; Group < 26; Group++)
                    {
                        su1 = 0;
                        if (Zone==0)
                        { NuclearDensity = NucDensity[IsotopeNum].AZ; }
                        else
                        { NuclearDensity = NucDensity[IsotopeNum].R; }
                        MacroSection[Zone, Group, (int)Consts.S_t] += isotope.Value.SigmaTotal[Group] * NuclearDensity * Math.Pow(10, -24);
                        MacroSection[Zone, Group, (int)Consts.S_f] += isotope.Value.SigmaFission[Group] * NuclearDensity * Math.Pow(10, -24);
                        MacroSection[Zone, Group, (int)Consts.NU] += isotope.Value.SigmaFission[Group] * isotope.Value.NuFission[Group] * NuclearDensity * Math.Pow(10, -24);
                        MacroSection[Zone, Group, (int)Consts.S_c] += isotope.Value.SigmaCapture[Group] * NuclearDensity * Math.Pow(10, -24);
                        MacroSection[Zone, Group, (int)Consts.S_e] += isotope.Value.SigmaElasticScattering[Group] * NuclearDensity * Math.Pow(10, -24);
                        MacroSection[Zone, Group, (int)Consts.S_in] += isotope.Value.SigmaInelasticScattering[Group] * NuclearDensity * Math.Pow(10, -24);
                        MacroSection[Zone, Group, (int)Consts.S_z] += isotope.Value.SigmaElasticZam[Group] * NuclearDensity * Math.Pow(10, -24);
                        MacroSection[Zone, Group, (int)Consts.MU] += isotope.Value.SigmaElasticScattering[Group] * isotope.Value.MuElasticScattering[Group] * NuclearDensity * Math.Pow(10, -24);
                        MacroSection[Zone, Group, (int)Consts.KSI] += isotope.Value.SigmaElasticScattering[Group] * isotope.Value.KsiElasticScattering[Group] * NuclearDensity * Math.Pow(10, -24);

                        for (int Column = 0; Column < 26; Column++)
                        {
                            MatrixSigmaScattering[Zone, Group, Column] += isotope.Value.MatrixInelasticScattering[Group, Column] * NuclearDensity * Math.Pow(10, -24);
                            if (Column > Group)
                            {
                                su1 += isotope.Value.MatrixInelasticScattering[Group, Column];
                            }
                        }
                        MacroSection[Zone, Group, (int)Consts.S_u] += (isotope.Value.SigmaFission[Group] + isotope.Value.SigmaCapture[Group] + isotope.Value.SigmaElasticZam[Group] + su1) * NuclearDensity * Math.Pow(10, -24);
                    }
                    InElasticMatrixes.Add(isotope.Value.MatrixInelasticScattering);
                    IsotopeNum++;
                };
                for (int Group = 0; Group < 26; Group++)
                {
                    MacroSection[Zone, Group, (int)Consts.MU] = MacroSection[Zone, Group, (int)Consts.MU] / MacroSection[Zone, Group, (int)Consts.S_e];
                    MacroSection[Zone, Group, (int)Consts.KSI] = MacroSection[Zone, Group, (int)Consts.KSI] / MacroSection[Zone, Group, (int)Consts.S_e];
                    if (MacroSection[Zone, Group, (int)Consts.S_f] > 0)
                    {
                        MacroSection[Zone, Group, (int)Consts.NU] = MacroSection[Zone, Group, (int)Consts.NU] / MacroSection[Zone, Group, (int)Consts.S_f];
                        /// MacroSection[Zone, Group, (int)Consts.HI] = MacroSection[Zone, Group, (int)Consts.HI] / MacroSection[Zone, Group, (int)Consts.S_f];
                    }
                    else
                    {
                        MacroSection[Zone, Group, (int)Consts.NU] = 0;
                        ///MacroSection[Zone, Group, (int)Consts.HI] = 0;
                    }
                    DJ[Zone, Group] = 1 / (3 * (MacroSection[Zone, Group, (int)Consts.S_t] - MacroSection[Zone, Group, (int)Consts.MU] * MacroSection[Zone, Group, (int)Consts.S_e]));
                    pos++;
                }

            }

        }

        public void Potok(int zone, int iteration)
        {///вычисление потоков
            double a1=0;
            double a2=0;
            double a3=0;
            double su1=0;
            double su2=0;
            if (zone== (int)Zones.R & iteration > 1)
            {
                su2 = 0;
                for (int Group = 0; Group<26; Group++)
                {
                    su2+= MacroSection[zone, Group, (int)Consts.NU] * MacroSection[zone, Group, (int)Consts.S_f] * FJ[zone, Group];
                }
            }
            for (int Column = 0; Column < 26; Column++)
            {
                su1 = 0;
                if (Column>0)
                {
                    for (int Row = 0; Row < Column; Row++)
                    {
                        su1 += FJ[zone, Row] * MatrixSigmaScattering[zone, Row, Column]; 
                    }
                    su1 += FJ[zone, Column - 1] * MacroSection[zone, Column - 1, (int)Consts.S_z];
                }
                if (zone == (int)Zones.AZ)
                { a1 = 0; }
                else
                {
                    if (iteration>0)
                    {
                        a1 = DJ[(int)Zones.AZ, Column] * Bg2[(int)Zones.AZ] * FJ[(int)Zones.AZ, Column];
                    }
                    else
                    {
                        a1 = 0;
                    }
                }
                if (zone == (int)Zones.AZ)
                { a2 = MacroSection[zone, Column, (int)Consts.HI]; }
                else 
                {
                    if (iteration == 1)
                    {a2 = 0;}
                    else
                    {
                        if (iteration > 1)
                        {a2 = su2 * MacroSection[zone, Column, (int)Consts.HI];}
                        else
                        { a2 = 0; }
                    }
                }
                if (zone == (int)Zones.AZ & iteration>0)
                { a3 = Bg2[zone] * DJ[zone, Column]; }
                else
                { a3 = 0; }

                FJ[zone, Column] = (a1 + a2 + su1 ) / (a3 + MacroSection[zone, Column, (int)Consts.S_u]);
            }

            ///вычисление ценностей
            if (zone == (int)Zones.R & iteration > 1)
            {
                su2 = 0;
                for (int Group = 0; Group < 26; Group++)
                {
                    su2 += MacroSection[zone, Group, (int)Consts.HI] * FJ[zone, Group];
                }
            }
            for (int Row = 25; Row >= 0; Row--)
            {
                if (zone== (int)Zones.R & iteration==0)
                { FJZ[zone, Row] = 0; }
                else
                {
                    su1 = 0;
                    if (Row < 25)
                    {
                        for (int Column = Row + 1; Column < 26; Column++)
                        {
                            su1 += FJZ[zone, Column] * MatrixSigmaScattering[zone, Row, Column];
                        }
                        su1 += FJZ[zone, Row + 1] * MacroSection[zone, Row + 1, (int)Consts.S_z];
                    }
                    if (zone == (int)Zones.AZ)
                    { a1 = 0; }
                    else
                    {
                        if (iteration>0)
                        { a1 = DJ[(int)Zones.AZ, Row]*Bg2[(int)Zones.AZ]*FJZ[(int)Zones.AZ,Row]; }
                        else 
                        { a1 = 0; }
                    }
                    if (zone == (int)Zones.AZ)
                    { a2 = MacroSection[(int)Zones.AZ, Row, (int)Consts.NU] * MacroSection[(int)Zones.AZ, Row, (int)Consts.S_f]; }
                    else
                    {
                        if(iteration<2)
                        { a2 = 0; }
                        else
                        { a2 = MacroSection[zone, Row, (int)Consts.NU] * MacroSection[zone, Row, (int)Consts.S_f] * su2; }
                    }
                    if(zone== (int)Zones.AZ & iteration>0)
                    { a3 = DJ[(int)Zones.AZ, Row] * Bg2[(int)Zones.AZ]; }
                    else { a3 = 0; }
                    FJZ[zone, Row] = (a1 + a2 + su1) / (a3 + MacroSection[zone, Row, (int)Consts.S_u]);
                }
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
                        C+=FJ[zone,j]* MatrixSigmaScattering[zone, j, i]; 
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






