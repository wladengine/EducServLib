using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace EducServLib
{
    public class ObjListItem
    {
        private object _key;
        private object _value;

        public ObjListItem(object key, object value)
        {
            _key = key;
            _value = value;
        }

        public object Key
        {
            get
            {
                return _key;
            }
        }

        public object Value
        {
            get
            {
                return _value;
            }
        }
    }

    //пара имя-айди
    [Serializable]
    public class ListItem
    {
        private String _id;
        private String _name;

        public ListItem()
        {            
        }

        public ListItem(string id, string name)
        {
            _id = id;
            _name = name;
        }

        public ListItem(int id, string name)
        {
            _id = id.ToString();
            _name = name;
        }

        public ListItem(XmlNode node)
        {
            _id = node.Attributes["Id"].Value;
            _name = node.Attributes["Name"].Value;
        }

        public String Id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }

        public String Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public override string ToString()
        {
            return _name;
        }

        //запись в хмл
        public void ToXml(XmlTextWriter writer)
        {
            writer.WriteStartElement("ListItem");

            writer.WriteAttributeString("Id", _id);
            writer.WriteAttributeString("Name", _name);

            writer.WriteEndElement();
        }
    }
    
}
