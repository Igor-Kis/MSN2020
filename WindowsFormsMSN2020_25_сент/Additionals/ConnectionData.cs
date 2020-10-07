using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace Additionals
{
    public class ConnectionData : IXmlSaveLoad
    {
        string _server = "127.0.0.1";
        string _dbName = "kcmodgir";
        string _login = "ScatisUser";
        string _password = "userscatis";
        string _ArxServer = "";
        private string _filename = "";
        string _svcfile = "";
        int _days = 2;
        bool _FromDate = false;
        bool _isDebugMode = false;

        //int _hours;
        DateTime _StartDateTime;


        public bool isChanged = false;

        public ConnectionData(string name)
        {
            //System.AppDomain.CurrentDomain.BaseDirectory;
            _filename = name;
            isDebugMode = false;
            Load();
            isChanged = false;
            _StartDateTime = DateTime.Now.AddDays(-this._days);
            _StartDateTime = _StartDateTime.AddMilliseconds(-_StartDateTime.Millisecond);
            _StartDateTime = _StartDateTime.AddSeconds(-_StartDateTime.Second);
        }
        public ConnectionData(XmlNode node)
        {
            //System.AppDomain.CurrentDomain.BaseDirectory;
            _filename = "";

            LoadFromXml(node);

            _StartDateTime = DateTime.Now.AddDays(-this._days);
            _StartDateTime = _StartDateTime.AddMilliseconds(-_StartDateTime.Millisecond);
            _StartDateTime = _StartDateTime.AddSeconds(-_StartDateTime.Second);
        }

        public string Server { get { return _server; } set { isChanged = isChanged || _server != value; _server = value; } }
        public string ArxServer { get { return _ArxServer; } set { isChanged = isChanged || _ArxServer != value; _ArxServer = value; } }
        public string DBName { get { return _dbName; } set { isChanged = isChanged || _dbName != value; _dbName = value; } }
        public string Login { get { return _login; } set { isChanged = isChanged || _login != value; _login = value; } }
        public string Password { get { return _password; } set { isChanged = isChanged || _password != value; _password = value; } }
        public int Days { get { return _days; } set { isChanged = isChanged || _days != value; _days = value; } }
        public string SVCfile { get { return _svcfile; } set { _svcfile = value; } }
        public bool FromDate { get { return _FromDate; } set { isChanged = isChanged || _FromDate != value; _FromDate = value; } }
        public bool isDebugMode { get { return _isDebugMode; } set { _isDebugMode = value; } }
        public DateTime StartDateTime { get { return _StartDateTime; } set { isChanged = isChanged || !DateTime.Equals(_StartDateTime, value); _StartDateTime = value; } }


        #region IXmlSaveLoad Members
        public void SaveToXml(XmlNode node)
        {
            XmlHelper.SetAttributeValueForNode(node, "Server", Server);
            if (Server != ArxServer)
                XmlHelper.SetAttributeValueForNode(node, "ArxServer", ArxServer);
            XmlHelper.SetAttributeValueForNode(node, "DBName", DBName);
            XmlHelper.SetAttributeValueForNode(node, "Login", Login);
            XmlHelper.SetAttributeValueForNode(node, "Password", "");
            XmlHelper.SetAttributeValueForNode(node, "Days", Days.ToString());
            XmlHelper.SetAttributeValueForNode(node, "Debug", isDebugMode.ToString());
            XmlHelper.SetAttributeValueForNode(node, "SVCfile", SVCfile);
        }

        public void LoadFromXml(XmlNode node)
        {
            if (node == null) return;
            Server = XmlHelper.GetStringFromAttributeValue(node, "Server", _server);
            ArxServer = XmlHelper.GetStringFromAttributeValue(node, "ArxServer", _ArxServer);
            if (ArxServer == "")
                ArxServer = Server;
            DBName = XmlHelper.GetStringFromAttributeValue(node, "DBName", _dbName);
            Login = XmlHelper.GetStringFromAttributeValue(node, "Login", _login);
            //Password = XmlHelper.GetStringFromAttributeValue(node, "Password", _password);
            Days = XmlHelper.GetIntFromAttributeValue(node, "Days", _days);
            isDebugMode = XmlHelper.GetBoolFromAttributeValue(node, "Debug", false);
            SVCfile = XmlHelper.GetStringFromAttributeValue(node, "SVCfile", _svcfile);
        }
        #endregion

        public void Save()
        {
            XmlDocument doc = new XmlDocument();
            XmlNode root = doc.CreateElement("ConnectionSettings");
            doc.AppendChild(root);
            SaveToXml(root);
            try
            {
                doc.Save(_filename);
            }
            catch (Exception e)
            {
                System.Windows.Forms.MessageBox.Show("Сохранение параметров подключения! " + e.Message);
            }
        }
        public void Load()
        {
            if (!File.Exists(_filename)) return;
            XmlDocument main = new XmlDocument();
            main.Load(_filename);
            System.Xml.XmlNode root0 = main.DocumentElement;
            LoadFromXml(root0);
            isChanged = false;
        }
    }
    /*
     * 
     *             for (int i = 0; i < dt.Columns.Count; i++)
            {
                DataColumn dc = dt.Columns[i];
                string name = dc.Caption;
                if (name.Contains("/")) dc.ColumnName = name.Replace("/", "DEL");
                if (name.Contains(".")) dc.ColumnName = name.Replace(".", "PNT");
            }
     * 
                     if (name.Contains("DEL")) item.Header = name.Replace("DEL", "/");
                    if (name.Contains("PNT")) item.Header = name.Replace("PNT", ".");

         */
}
