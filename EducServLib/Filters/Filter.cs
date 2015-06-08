using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EducServLib
{
    //общий интерфейс
    public interface iFilter
    {
        void ToXml(XmlTextWriter writer);
    }

    //общий тип для видов фильтров
    public abstract class Filter: iFilter
    {
        protected int _place;
        protected FilterItem _filterItem;

        public int NumInList
        {
            get
            {
                return _place;
            }
        }

        public abstract string GetFilter();
        public abstract void ToXml(XmlTextWriter writer);

        public FilterItem GetFilterItem()
        {
            return _filterItem;
        }        
    }

    //фильтр типа просто текст
    public class TextFilter : Filter
    {
        private string _value;

        //конструктор
        public TextFilter(FilterItem fi, string sValue, int place)
        {
            _value = sValue;
            _filterItem = fi;
            _place = place;
        }

        //конструктор 2
        public TextFilter(XmlNode node)
        {
            _value = node.Attributes["Value"].Value;
            _place = int.Parse(node.Attributes["Place"].Value);
            foreach(XmlNode xn in node.ChildNodes)
                if (xn.Name=="FilterItem")
                {
                    _filterItem = new FilterItem(xn);
                    break;
                }
        }

        public string Value
        {
            get
            {
                return _value;
            }
        }

        //sql
        public override string GetFilter()
        {
            return string.Format("({0} Like '%{1}%')", _filterItem.DbName,_value);
        }

        public override string ToString()
        {
            return string.Format("{0}: {1}", _filterItem.Name, _value);
        }

        //запись в хмл
        public override void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("TextFilter");
            {
                writer.WriteAttributeString("Value", _value.ToString());
                writer.WriteAttributeString("Place", _place.ToString());

                _filterItem.ToXml(writer);
            }
            writer.WriteEndElement();
        }        
    }

    //фильтр вида ДА-НЕТ
    public class BoolFilter : Filter
    {
        private bool _value;        

        //конструктор
        public BoolFilter(FilterItem fi,bool bValue, int place)
        {
            _value = bValue;
            _filterItem = fi;
            _place = place;
        }

        //конструктор 2
        public BoolFilter(XmlNode node)
        {
            _value = bool.Parse(node.Attributes["Value"].Value);
            _place = int.Parse(node.Attributes["Place"].Value);
            foreach(XmlNode xn in node.ChildNodes)
                if (xn.Name=="FilterItem")
                {
                    _filterItem = new FilterItem(xn);
                    break;
                }
        }

        public bool Value
        {
            get
            {
                return _value;
            }
        }

        //sql формат
        public override string GetFilter()
        {
            string s = _filterItem.DbName.ToLower().Trim();
            if (_value)
                if (s.StartsWith("exists") || s.StartsWith("("))
                    return _filterItem.DbName;
                else
                    return string.Format("{0}>0", _filterItem.DbName);
            else
                if (s.StartsWith("exists") || s.StartsWith("("))
                    return " NOT " + _filterItem.DbName;
                else
                    return string.Format("{0}=0", _filterItem.DbName);
        }

        //строит строку для вывода
        public override string ToString()
        {
            string res = string.Empty;
            res = string.Format("{0}: {1}", _filterItem.Name, (_value ? "ДА" : "НЕТ"));

            return res;
        }

        //запись в хмл
        public override void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("BoolFilter");

            writer.WriteAttributeString("Value", _value.ToString());
            writer.WriteAttributeString("Place", _place.ToString());
            _filterItem.ToXml(writer);

            writer.WriteEndElement();
        }
    }

    //фильтр типа ОТ-ДО
    public class FromToFilter : Filter
    {
        private string _from;
        private string _to;

        //конструктор
        public FromToFilter(FilterItem fi, string sFrom, string sTo, int place)
        {
            _from = sFrom;
            _to = sTo;
            _filterItem = fi;
            _place = place;
        }

        //конструктор 2
        public FromToFilter(XmlNode node)
        {
            _from = node.Attributes["From"].Value;
            _to = node.Attributes["To"].Value;
            _place = int.Parse(node.Attributes["Place"].Value);
            foreach(XmlNode xn in node.ChildNodes)
                if (xn.Name=="FilterItem")
                {
                    _filterItem = new FilterItem(xn);
                    break;
                }
        }

        public override string GetFilter()
        {
            string s1 = string.Empty, s2 = string.Empty;
            bool b1, b2; 
            b1 = _from.Length > 0;
            b2 = _to.Length > 0;

            if (b1)
                s1 = string.Format("{0}>='{1}'", _filterItem.DbName,_from);

            if (b2)
                s2 = string.Format("{0}<='{1}'", _filterItem.DbName, _to);

            if(b1 && b2)
                return string.Format("({0} AND {1})", s1,s2);
            if(b1)
                return s1;
            if (b2)
                return s2;

            return "1=1";
        }

        public string FromValue
        {
            get
            {
                return _from;
            }
        }

        public string ToValue
        {
            get
            {
                return _to;
            }
        }

        //строит строку для вывода
        public override string ToString()
        {
            string res = string.Empty;
            res = string.Format("{0}: {1} {2}",
                _filterItem.Name,
                _from==string.Empty ? "" : string.Format("от {0}",_from),
                _to==string.Empty ? "" : string.Format("до {0}",_to));

            return res;
        }

        //запись в хмл
        public override void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("FromToFilter");

            writer.WriteAttributeString("From", _from);
            writer.WriteAttributeString("To", _to);
            writer.WriteAttributeString("Place", _place.ToString());
            _filterItem.ToXml(writer);

            writer.WriteEndElement();
        }
    }

    //фильтр типа от-до ДАТЫ
    public class DateFromToFilter : Filter
    {
        protected DateTime _from;
        protected DateTime _to;

        //конструктор
        public DateFromToFilter(FilterItem fi, DateTime sFrom, DateTime sTo, int place)
        {
            _from = sFrom;
            _to = sTo;
            _filterItem = fi;
            _place = place;
        }

        //конструктор 2
        public DateFromToFilter(XmlNode node)
        {
            string from, to;
            from = node.Attributes["From"].Value;
            to = node.Attributes["To"].Value;

            _from = (from == string.Empty) ? DateTime.MaxValue : DateTime.Parse(from);
            _to = (to == string.Empty) ? DateTime.MaxValue : DateTime.Parse(to);
            _place = int.Parse(node.Attributes["Place"].Value);
            foreach(XmlNode xn in node.ChildNodes)
                if (xn.Name=="FilterItem")
                {
                    _filterItem = new FilterItem(xn);
                    break;
                }
        }

        public DateTime FromValue
        {
            get
            {
                return _from;
            }
        }

        public DateTime ToValue
        {
            get
            {
                return _to;
            }
        }

        //строит строку для вывода
        public override string ToString()
        {
            string res = string.Empty;
            res = string.Format("{0}: {1} {2}",
                _filterItem.Name,
                _from == DateTime.MaxValue ? "" : string.Format("от {0}", _from.Date.ToShortDateString()),
                _to == DateTime.MaxValue ? "" : string.Format("до {0}", _to.Date.ToShortDateString()));

            return res;
        }

        public override string GetFilter()
        {
            string s1 = string.Empty, s2 = string.Empty;
            bool b1, b2;
            b1 = (_from != DateTime.MaxValue);
            b2 = (_to != DateTime.MaxValue);

            if (b1)
                s1 = string.Format("{0}>='{1}/{2}/{3}'", _filterItem.DbName, _from.Day,_from.Month,_from.Year);

            if (b2)
                s2 = string.Format(@"{0}<='{1}/{2}/{3}'", _filterItem.DbName, _to.Day,_to.Month,_to.Year);

            if (b1 && b2)
                return string.Format("({0} AND {1})", s1, s2);
            if (b1)
                return s1;
            if (b2)
                return s2;

            return "1=1";
        }

        //запись в хмл
        public override void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("DateFromToFilter");

            string from, to;

            from = (_from == DateTime.MaxValue) ? string.Empty : _from.ToString();
            to = (_to == DateTime.MaxValue) ? string.Empty : _to.ToString();

            writer.WriteAttributeString("From", from);
            writer.WriteAttributeString("To", to);
            writer.WriteAttributeString("Place", _place.ToString());
            _filterItem.ToXml(writer);

            writer.WriteEndElement();
        }
    }

    public class MultiFromToFilter : Filter
    {
        private MultiSelectFilter _msf;
        private FromToFilter _ftf;

        //constructor
        public MultiFromToFilter(FilterItem fi,MultiSelectFilter msf, FromToFilter ftf, int place)
        {
            _ftf = ftf;
            _msf = msf;
            _place = place;
            _filterItem = fi;
        }

        //constructor2
        public MultiFromToFilter(XmlNode node)
        {
            _place = int.Parse(node.Attributes["Place"].Value);


            foreach (XmlNode xn in node.ChildNodes)
                if (xn.Name == "MultiSelectFilter")
                {
                    _msf = new MultiSelectFilter(xn);
                }
                else if (xn.Name == "FromToFilter")
                {
                    _ftf = new FromToFilter(xn);
                }
                else if (xn.Name == "FilterItem")
                {
                    _filterItem = new FilterItem(xn);
                }
        }

        public MultiSelectFilter MultiSelectFilter
        {
            get
            {
                return _msf;
            }
        }

        public FromToFilter FromtoFilter
        {
            get
            {
                return _ftf;
            }
        }

        public override string GetFilter()
        {
            string res = string.Empty;
            res = string.Format(_filterItem.DbName, _msf.GetFilter(), _ftf.GetFilter());

            return res;
        }

        public override string ToString()
        {
            return string.Format("{0}, {1}", _msf.ToString(), _ftf.ToString());
        }

        public override void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("MultiFromToFilter");

            writer.WriteAttributeString("Place", _place.ToString());
            _filterItem.ToXml(writer);
            _ftf.ToXml(writer);
            _msf.ToXml(writer);
            
            writer.WriteEndElement();
        }
    }

    //фильтр типа МНОЖЕСТВЕННЫЙ ВЫБОР
    public class MultiSelectFilter : Filter
    {
        private List<ListItem> _list;

        //конструктор
        public MultiSelectFilter(FilterItem fi, List<ListItem> list, int place)
        {
            _list = list;
            _filterItem = fi;
            _place = place;
        }

        //конструктор 2
        public MultiSelectFilter(XmlNode node)
        {
            _place = int.Parse(node.Attributes["Place"].Value);

            List<ListItem> list = new List<ListItem>();

            foreach(XmlNode xn in node.ChildNodes)
                if (xn.Name == "ListItem")
                {
                    list.Add(new ListItem(xn));
                }
                else if (xn.Name=="FilterItem")
                {
                    _filterItem = new FilterItem(xn);                    
                }
            _list = list;
        }

        public List<ListItem> List
        {
            get
            {
                return _list;
            }
        }

        public override string GetFilter()
        {
            string res = string.Empty;

            foreach (ListItem li in _list)
            {
                // прописала хак, потому что профили у нас теперь guid а не int, нужны апострофы!
                if (li.Id.Contains('-'))
                    res = string.Format("{0},'{1}'", res, li.Id);
                else   
                    res = string.Format("{0},{1}", res, li.Id);
            }

            return string.Format("{0} IN ({1})", _filterItem.DbName, res.Substring(1));
        }

        //строит строку для вывода
        public override string ToString()
        {
            string res = string.Empty;

            foreach (ListItem li in _list)
                res = string.Format("{0},{1}",res, li.Name);

            return string.Format("{0}: {1}", _filterItem.Name, res.Substring(1));
        }

        //запись в хмл
        public override void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("MultiSelectFilter");

            writer.WriteAttributeString("Place", _place.ToString());
            _filterItem.ToXml(writer);

            foreach (ListItem li in _list)
                li.ToXml(writer);

            writer.WriteEndElement();
        }
    }

    public abstract class NotFilter : iFilter
    {
        protected string _name;

        //запись в хмл
        public abstract void ToXml(XmlTextWriter writer);
    }

    public class Or : NotFilter
    {
        //конструктор
        public Or()
        {
            _name = "ИЛИ";
        }

        //ту стринг
        public override string ToString()
        {
            return _name;
        }

        //запись в хмл
        public override void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("Or");
            writer.WriteEndElement();
        }
    }

    public abstract class Bracket : NotFilter
    {
        //запись в хмл
        public abstract override void ToXml(XmlTextWriter writer);
    }

    public class LeftBracket : Bracket
    {
        //конструктор
        public LeftBracket()
        {
            _name = "(";
        }

        //ту стринг
        public override string ToString()
        {
            return _name;
        }

        //запись в хмл
        public override void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("LeftBracket");
            writer.WriteEndElement();
        }
    }

    public class RightBracket : Bracket
    {
        //конструктор
        public RightBracket()
        {
            _name = ")";
        }

        //ту стринг
        public override string ToString()
        {
            return _name;
        }

        //запись в хмл
        public override void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("RightBracket");
            writer.WriteEndElement();
        }
    }

}
