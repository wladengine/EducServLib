using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Text;

namespace EducServLib
{
    public class FilterItem : iFilter
    {
        private string _name;
        private FilterType _type;
        private string _query;
        private string _dbName;
        private string _table;

        //конструктор 
        public FilterItem(string s, FilterType ft, string dbName, string table)
            : this(s, ft, dbName, table, null)
        {
        }

        //конструктор 
        public FilterItem(string s, FilterType ft, string dbName, string table, string query)
        {
            _name = s;
            _type = ft;
            _query = query;
            _dbName = dbName;
            _table = table;
        }

        //конструктор
        public FilterItem(XmlNode node)
        {
            _name = node.Attributes["Name"].Value;
            _type = (FilterType)int.Parse(node.Attributes["FilterType"].Value);
            _query = node.Attributes["Query"].Value;
            _dbName = node.Attributes["DbName"].Value;
            _table = node.Attributes["Table"].Value;
        }

        //запись в хмл
        public void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("FilterItem");

            writer.WriteAttributeString("Name", _name);
            writer.WriteAttributeString("FilterType", ((int)_type).ToString());
            writer.WriteAttributeString("Query", _query);
            writer.WriteAttributeString("DbName", _dbName);
            writer.WriteAttributeString("Table", _table);

            writer.WriteEndElement();
        }

        //
        //свойства доступа к полям класса
        //
        public string DbName
        {
            get
            {
                return _dbName;
            }
        }

        public FilterType Type
        {
            get
            {
                return this._type;
            }
        }

        public string Name
        {
            get
            {
                return _name;
            }
        }

        public string Table
        {
            get
            {
                return _table;
            }
        }

        public string Query
        {
            get
            {
                return this._query;
            }
        }

        //ту стринг
        public override string ToString()
        {
            return this._name;
        }
    }

    public enum FilterType : int
    {
        Bool = 0,
        FromTo = 1,
        Multi = 2,
        DateFromTo = 3,
        Text = 4,
        MultiFromTo = 5
    }
}