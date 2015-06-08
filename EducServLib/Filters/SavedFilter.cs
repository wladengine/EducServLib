using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace EducServLib
{
    public class SavedFilter
    {
        private string _name;
        private List<iFilter> _filterList;

        //конструктор
        public SavedFilter(string name, List<iFilter> filterList)
        {
            _filterList = filterList;
            _name = name;
        }

        //конструктор 2
        public SavedFilter(XmlNode node)
        {
            string name = node.Attributes["name"].Value;
            List<iFilter> list = new List<iFilter>();

            foreach (XmlNode xn in node.ChildNodes)
            {
                string s = xn.Name;
                iFilter filter=null;

                if (s == "BoolFilter")
                    filter = new BoolFilter(xn);
                else if (s == "TextFilter")
                    filter = new TextFilter(xn);
                else if (s == "DateFromToFilter")
                    filter = new DateFromToFilter(xn);
                else if (s == "FromToFilter")
                    filter = new FromToFilter(xn);
                else if (s == "MultiSelectFilter")
                    filter = new MultiSelectFilter(xn);
                else if (s == "MultiFromToFilter")
                    filter = new MultiFromToFilter(xn);
                else if (s == "LeftBracket")
                    filter = new LeftBracket();
                else if (s == "RightBracket")
                    filter = new RightBracket();
                else if (s == "Or")
                    filter = new Or();
                
                list.Add(filter);
            }
            
            _name = name;
            _filterList = list;
        }

        //свойство доступа к имени
        public string Name
        {
            get
            {
                return _name;
            }
        }

        public List<iFilter> FilterList
        {
            get
            {
                return _filterList;
            }
        }

        //количество фильтров
        public int FiltersCount
        {
            get
            {
                return _filterList.Count;
            }
        }

        //доступ к фильтрам
        public iFilter this[int i]
        {
            get
            {
                if (i < _filterList.Count)
                    return _filterList[i];
                else
                    return null;
            }
        }

        //ту стринг
        public override string ToString()
        {
            return _name;
        }

        //запись в ХМЛ
        public void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("SavedFilter");
            {
                writer.WriteAttributeString("name", _name);

                //записываем каждый фильтр из листа
                foreach (iFilter filter in _filterList)
                {
                    filter.ToXml(writer);
                }
            }
            writer.WriteEndElement();
        }
    }
}
