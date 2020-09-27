using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Additionals
{
    public class LogFile : List<string>
    {
        private string fNameTxt;
        string DTFormat = "yyyy-MM-dd HH:mm:ss.fff";
        bool isDeveloperDebugMode = true;
        public int LastStringNum { get { return Count - 1; } }

        public string FNameTxt { get => fNameTxt; set => fNameTxt = value; }

        public LogFile(string Fname, bool isDeveloperDebugMode = false)
        {
            this. isDeveloperDebugMode = isDeveloperDebugMode;
            FNameTxt = Fname.Remove(Fname.Length - 3, 3) +"log";
        }

        public int AddString(string s)
        {
            this.Add(DateTime.Now.ToString(DTFormat) + " " + s);
            return this.Count - 1;
        }

        public int AddDebugString(string s)
        {
            if (isDeveloperDebugMode) this.Add(DateTime.Now.ToString(DTFormat) + " " + s);
            return this.Count - 1;
        }

        /*public void AddSection(string header)
        {
            string s = "{СЕКЦИЯ: " + header + "}";
            this.Add(s);
        }

        public void AddOperation(int num, string OpName, string GroupName)
        {
            string s = "{ОПЕРАЦИЯ: " + num.ToString() + " " + OpName + " [" + GroupName + "]}";
            this.Add(s);
        }

        public void DeleteString(int i)
        {
            if (i < 0) return;
            if (i <= this.Count)
            {
                this.RemoveAt(i);
            }
        }*/

        public void SaveToFile()
        {
            try
            {
                if (this.Count > 0)
                {
                    using (System.IO.StreamWriter file = new System.IO.StreamWriter(FNameTxt, false, Encoding.Default))//открыли на перезапись
                    {
                        for (int i = 0; i < this.Count; i++)
                        {
                            file.WriteLine(this[i]);
                        }
                    }
                }
            }
            catch { }
        }

        public void AddWarning(string v)
        {
            AddString("Warning! " + v);
        }
    }
}
