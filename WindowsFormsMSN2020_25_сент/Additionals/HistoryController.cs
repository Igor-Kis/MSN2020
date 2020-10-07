using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Xml;

namespace Additionals
{
    public class HistoryEvent
    {
        public XmlDocument doc;
        public string name { get; set; }
        public object tag { get; set; }
        DateTime _time = DateTime.Now;
        public DateTime time { get { return _time; } }

        public HistoryEvent(string name, XmlDocument doc)
        {
            this.name = name;
            this.doc = doc;
        }

        public void Restore(IXmlSaveLoad obj)
        {
            obj.LoadFromXml(doc.DocumentElement);
        }
    }

    public class HistoryController
    {
        int _maxEventCount = 50;
        public int maxEventCount
        {
            get { return _maxEventCount; }
            set { _maxEventCount = Math.Max(1, value); }
        }
        public ObservableCollection<HistoryEvent> events { get; set; }

        void AddEvent(string name, XmlDocument doc)
        {
            if (events.Count > 0)
            {
                if (events.Last().doc.InnerXml.Equals(doc.InnerXml))
                    return;
            }

            events.Add(new HistoryEvent(name, doc));
            while (events.Count > maxEventCount)
            {
                events.RemoveAt(0);
            }
        }

        public void AddEvent(string name, IXmlSaveLoad obj)
        {
            var doc = new XmlDocument();
            doc.LoadXml("<ObjectData />");

            obj.SaveToXml(doc.DocumentElement);

            AddEvent(name, doc);
        }

        public void RestoreFromEvent(HistoryEvent evt, IXmlSaveLoad obj)
        {
            if (events.Contains(evt) == false) throw new Exception("Событие не принадлежит ленте истории.");
            var index = events.IndexOf(evt);
            while (events.Count - 1 > index)
                events.RemoveAt(events.Count - 1);
            evt.Restore(obj);
        }

        #region .   ctor   .

        public HistoryController()
        {
            events = new ObservableCollection<HistoryEvent>();
        }

        #endregion
    }
}
