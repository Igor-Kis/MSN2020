using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Additionals;
using System.Xml;

namespace WindowsFormsMSN2020
{
    public class IsotopeProperties : IXmlSaveLoad
    {
        /// <summary> 
        /// Название изотопа
        /// </summary>
        public string Name;
        /// <summary> 
        /// Молярная масса изотопа (а.е.м.)
        /// </summary>
        public double Mass;
        public bool canFusion;
        public bool isSourceble;

        public double[] SigmaTotal;
        public double[] SigmaFission;
        public double[] NuFission;
        public double[] SigmaCapture;
        public double[] SigmaElasticScattering;
        public double[] SigmaInelasticScattering;
        public double[] SigmaElasticZam;
        public double[] MuElasticScattering;
        public double[] KsiElasticScattering;

        public double[,] MatrixElasticScattering;
        public double[,] MatrixInelasticScattering;

        public LinearExpansion leProperties;
        public ResonanceBlockKoefs rbcProperties;

        public IsotopeProperties()
        {/*
            SigmaTotal = new double[26];
            SigmaFission = new double[26];
            NuFission = new double[26];
            SigmaCapture = new double[26];
            SigmaElasticScattering = new double[26];
            SigmaInelasticScattering = new double[26];
            SigmaElasticZamedlenie = new double[26];
            MuElasticScattering = new double[26];
            KsiElasticScattering = new double[26];
            MatrixElasticScattering = new double[26, 26];
            MatrixInelasticScattering = new double[26, 26];*/
            leProperties = new LinearExpansion();
            rbcProperties = new ResonanceBlockKoefs();
        }
    
        //сигма0 и температура и набор коэффициентов для интерполяции и номер группы
        public double GetF(double sigma0, double T, ResBlockKoef rbk, int GrNum)
        {
            double F = 0;
            double[] FArr = new double[9];
            for (int i = 0; i < 9; i++)
                FArr[i] = rbk.ResBlockMatrix300[GrNum, i] + (rbk.ResBlockMatrix900[GrNum, i] - rbk.ResBlockMatrix300[GrNum, i]) * (T - 300) / 600;//600=900-300
            for (int i = 0; i < 7; i++)
                if (sigma0 >= AddConsts.S0Arr[i] && (sigma0 <= AddConsts.S0Arr[i + 1]))
                {
                    return FArr[i] + (FArr[i + 1] - FArr[i]) * (sigma0 - AddConsts.S0Arr[i]) / (AddConsts.S0Arr[i + 1] - AddConsts.S0Arr[i]);
                }
            return F;
        }

        public void LoadFromXml(XmlNode node)
        {
            if (node == null) return;
            Name = XmlHelper.GetStringFromAttributeValue(node, "Name", "");
            isSourceble = XmlHelper.GetBoolFromAttributeValue(node, "isSourceble", false);
            Mass = XmlHelper.GetDoubleFromAttributeValue(node, "Mass", 1);
            System.Xml.XmlNode ch = Additionals.XmlHelper.GetChildNodeFromNode(node, "GroupConsts");
            if (ch != null)
            {
                xmlHelp.GetDataLineArray(ch, ref SigmaTotal, 26, "SigmaTotal");
                xmlHelp.GetDataLineArray(ch, ref SigmaFission, 26, "SigmaFission");
                xmlHelp.GetDataLineArray(ch, ref NuFission, 26, "NuFission");
                xmlHelp.GetDataLineArray(ch, ref SigmaCapture, 26, "SigmaCapture");
                xmlHelp.GetDataLineArray(ch, ref SigmaElasticScattering, 26, "SigmaElastic");
                xmlHelp.GetDataLineArray(ch, ref SigmaInelasticScattering, 26, "SigmaInelastic");
                xmlHelp.GetDataLineArray(ch, ref SigmaElasticZam, 26, "SigmaElasticZam");
                xmlHelp.GetDataLineArray(ch, ref MuElasticScattering, 26, "MuElastic");
                xmlHelp.GetDataLineArray(ch, ref KsiElasticScattering, 26, "KsiElastic");
            }
            ch = Additionals.XmlHelper.GetChildNodeFromNode(node, "ElasticMatrix");
            MatrixElasticScattering = xmlHelp.GetMatrixFromNode(ch, 26, 26);
            ch = Additionals.XmlHelper.GetChildNodeFromNode(node, "InelasticMatrix");
            MatrixInelasticScattering = xmlHelp.GetMatrixFromNode(ch, 26, 26);
            ch = Additionals.XmlHelper.GetChildNodeFromNode(node, "LinearExpansion");
            leProperties.LoadFromXml(ch);
            ch = Additionals.XmlHelper.GetChildNodeFromNode(node, "ResonanceBlockKoefs");
            rbcProperties.LoadFromXml(ch);
        }


        public void SaveToXml(XmlNode node)
        {

        }

    }

    public static class AddConsts
    {
        public static double[] S0Arr = { 0, 1, 10, 100, 1000, 10000, 100000, 1000000 }; // массив значений сигма0
    }

    public class ResonanceBlockKoefs : IXmlSaveLoad
    {
        public ResBlockKoef Ft;
        public ResBlockKoef Ff;
        public ResBlockKoef Fc;
        public ResBlockKoef Fe;
        public ResonanceBlockKoefs()
        {
            Ft = new ResBlockKoef();
            Ff = new ResBlockKoef();
            Fc = new ResBlockKoef();
            Fe = new ResBlockKoef();
        }
        public void LoadFromXml(XmlNode node)
        {
            if (node == null) return;
            foreach (XmlNode child in node)
            {
                if (child.Name == "rbkFt") Ft.LoadFromXml(child);
                else if (child.Name == "rbkFf") Ff.LoadFromXml(child);
                else if (child.Name == "rbkFc") Fc.LoadFromXml(child);
                else if (child.Name == "rbkFe") Fe.LoadFromXml(child);
            }
        }
        public void SaveToXml(XmlNode node)
        {

        }
    }

    public class ResBlockKoef : IXmlSaveLoad
    {
        public double[,] ResBlockMatrix300;
        public double[,] ResBlockMatrix900;
        public ResBlockKoef()
        {
            ResBlockMatrix300 = new double[26, 9];
            ResBlockMatrix900 = new double[26, 9];
            for (int i = 0; i < 26; i++)
                for (int j = 0; j < 9; j++) { ResBlockMatrix300[i, j] = 1; ResBlockMatrix900[i, j] = 1; }

        }
        public void LoadFromXml(XmlNode node)
        {
            double[,] FillArr = null;
            if (node == null) return;
            foreach (XmlNode child in node)
                if (child.Name == "Temperature")
                {
                    string s = XmlHelper.GetStringFromAttributeValue(child, "T", "");
                    if (s == "300") FillArr = ResBlockMatrix300;
                    else if (s == "900") FillArr = ResBlockMatrix900;
                    for (int i = 0; i < 26; i++)
                    {
                        double[] arr = xmlHelp.GetDataLineArray(child, "Row" + i.ToString());
                        if (arr != null)
                            for (int j = 0; j < arr.Length; j++)
                                FillArr[i, j] = arr[j];
                    }
                }
        }
        public void SaveToXml(XmlNode node)
        {

        }
    }

    public class LinearExpansion : List<LineExpInterval>, IXmlSaveLoad
    {
        public LinearExpansion()
        {
            PolinomKoeffsPWR = new double[9];
        }
        static double[] PolinomKoeffsPWR = new double[] { 1e-6, 1e-8, 1e-11, 1e-11, 1e-12, 1e-15, 1e-18, 1e-21, 1e-25 };

        public void LoadFromXml(XmlNode node)
        {
            Clear();
            if (node == null) return;
            foreach (XmlNode child in node)
                if (child.Name == "Interval")
                {
                    LineExpInterval ip = new LineExpInterval();
                    ip.LoadFromXml(child);
                    this.Add(ip);
                }
        }
        public void SaveToXml(XmlNode node)
        {

        }
        internal double GetNuclearDensity(double baseNuclearDensity, double temperature)
        {
            double T = temperature - 273;
            double alpha = 0;
            if (this.Count > 0)
            {
                LineExpInterval li = this[0];
                for (int i = 0; i < li.PolinomKoeffs.Length; i++)
                    alpha = alpha + Math.Pow(T, i) * li.PolinomKoeffs[i] * PolinomKoeffsPWR[i];

                return baseNuclearDensity / (1 + 3 * alpha * (T - 293));
            }
            else return baseNuclearDensity;
        }
    }

    public class LineExpInterval : IXmlSaveLoad
    {
        public double T0 = 293;
        public double T1 = 900;
        public LineExpIntervalType leiType = LineExpIntervalType.Polynomial;
        public double[] PolinomKoeffs;

        public void LoadFromXml(XmlNode node)
        {
            if (node != null)
            {
                string TypeStr = XmlHelper.GetStringFromAttributeValue(node, "Type", "None");
                if (TypeStr == "Polynomial") leiType = LineExpIntervalType.Polynomial;
                else leiType = LineExpIntervalType.None;
                T0 = XmlHelper.GetDoubleFromAttributeValue(node, "T0", 293);
                T1 = XmlHelper.GetDoubleFromAttributeValue(node, "T1", 293);
                PolinomKoeffs = XmlHelper.GetStringFromAttributeValue(node, "data", "").GetDoubleArray(';');
                if (PolinomKoeffs == null) PolinomKoeffs = new double[0];
            }
        }
        public void SaveToXml(XmlNode node)
        {

        }
    }

    public enum LineExpIntervalType
    {
        /// <summary>
        /// Не задан
        /// </summary>
        None,
        /// <summary>
        /// Форма полинома
        /// </summary>
        Polynomial,
    }




    public static class xmlHelp
    {

        public static double[,] GetMatrixFromNode(XmlNode node, int rows, int cols)
        {
            double[,] m = new double[rows, cols];
            if (node != null)
                for (int i = 0; i < rows; i++)
                {
                    double[] arr = GetDataLineArray(node, "Row" + i.ToString());
                    if (arr != null)
                        for (int j = 0; j < arr.Length; j++)
                            if (i + j < 26)
                                m[i, i + j] = arr[j];
                }
            return m;
        }

        public static void GetDataLineArray(XmlNode node, ref double[] dataDoubleArray, int len, string NodeTitle)
        {
            dataDoubleArray = GetDataLineArray(node, NodeTitle);
            if (dataDoubleArray == null)
                dataDoubleArray = new double[len];
        }

        public static double[] GetDataLineArray(XmlNode node, string NodeTitle)
        {
            System.Xml.XmlNode ch = Additionals.XmlHelper.GetChildNodeFromNode(node, NodeTitle);
            if (ch != null)
                return XmlHelper.GetStringFromAttributeValue(ch, "data", "").GetDoubleArray(';');
            else return null;
        }
    }
}
