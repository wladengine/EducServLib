using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;

namespace EducServLib
{
    public class Config
    {
        protected string _fileName;

        //�����������
        public Config(string dir, string fileName)
        {
            InitItems();

            DirectoryInfo di = new DirectoryInfo(dir);
            if (!di.Exists)
                di.Create();

            string file = string.Format(@"{0}\{1}", dir, fileName);
            _fileName = file;

            FileInfo fi = new FileInfo(file);
            
            //�������� ����������
            if (fi.Exists)
            {
                LoadConfig();
            }
            else
            {
                CreateConfigFile();
            }
        }

        protected virtual void CreateConfigFile()
        {
        }

        protected virtual void InitItems()
        {
        }

        protected virtual void LoadConfig()
        {
        }
    }

    public class ConfigFile: Config
    {        
        //�������
        private List<SavedFilter> _savedFilterList;
        //�������
        private List<string> _columnListAbit;
        private List<string> _columnListPerson;
        //����-��������
        private SortedList<string,string> _slValues;
                
        //�����������
        public ConfigFile(string dir, string fileName) : base(dir,fileName)
        {                   
        }

        //������������� ��������������
        protected override void InitItems()
        {
            _savedFilterList = new List<SavedFilter>();
            _columnListAbit = new List<string>();
            _columnListPerson = new List<string>();
            _slValues = new SortedList<string, string>();
        }

        public List<SavedFilter> SavedFilters
        {
            get
            {
                return _savedFilterList;
            }
        }

        public List<string> ColumnListAbit
        {
            get
            {
                return _columnListAbit;
            }
        }

        public List<string> ColumnListPerson
        {
            get
            {
                return _columnListPerson;
            }
        }

        public SortedList<string, string> ValuesList
        {
            get
            {
                return _slValues;
            }
        }

        //�������� ���� � ���������
        public void AddValue(string key, string value)
        {
            if (_slValues.Keys.Contains(key))
                DeleteValue(key);
            _slValues.Add(key, value);
        }

        //������� ������� �� ��������� ���
        public void DeleteValue(string key)
        {
            _slValues.Remove(key);
        }

        //������� ������ ��������
        public void ClearColumnListAbit()
        {
            _columnListAbit.Clear();
        }
        public void ClearColumnListPerson()
        {
            _columnListPerson.Clear();
        }

        //���������� �������� �������
        public void AddColumnNameAbit(string name)
        {
            if(!_columnListAbit.Contains(name))
                _columnListAbit.Add(name);
        }
        public void AddColumnNamePerson(string name)
        {
            if (!_columnListPerson.Contains(name))
                _columnListPerson.Add(name);
        }

        //���������� ������� � ������
        public void AddSavedFilter(SavedFilter sf)
        {
            _savedFilterList.Add(sf);
        }

        //�������� �� �����
        public void DeleteSavedFilter(string name)
        {
            foreach (SavedFilter sf in _savedFilterList)
            {
                if (sf.Name.CompareTo(name) == 0)
                    _savedFilterList.Remove(sf);
                break;
            }
        }

        //�������� ����������
        protected override void LoadConfig()
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(_fileName);
                XmlNodeList nodes = doc.DocumentElement.ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    //���� Filters                
                    if (node.Name == "Filters")
                        LoadFilters(node);
                    else if (node.Name == "ColumnsAbit")
                        LoadColumnsAbit(node);
                    else if (node.Name == "ColumnsPerson")
                        LoadColumnsPerson(node);
                    else if (node.Name == "Properties")
                        LoadProperties(node);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }            
        }

        //���������� ����������� �������
        private void LoadFilters(XmlNode node)
        {
            XmlNodeList xnl = node.ChildNodes;
            
            foreach (XmlNode xn in xnl)
                if (xn.Name == "SavedFilter")
                    AddSavedFilter(new SavedFilter(xn));
        }

        //���������� ����������� �������
        private void LoadColumnsAbit(XmlNode node)
        {
            XmlNodeList xnl = node.ChildNodes;
            
            foreach (XmlNode xn in xnl)
                if (xn.Name == "Column")
                    AddColumnNameAbit(xn.Attributes["Name"].Value);
        }
        private void LoadColumnsPerson(XmlNode node)
        {
            XmlNodeList xnl = node.ChildNodes;

            foreach (XmlNode xn in xnl)
                if (xn.Name == "Column")
                    AddColumnNamePerson(xn.Attributes["Name"].Value);
        }

        private void LoadProperties(XmlNode node)
        {
            XmlNodeList xnl = node.ChildNodes;

            foreach (XmlNode xn in xnl)
                if (xn.Name == "Property")
                    AddValue(xn.Attributes["Name"].Value, xn.Attributes["Value"].Value);
        }

        //���������� ����������
        public void SaveConfig()
        {
            XmlTextWriter writer = null;

            try
            {
                writer = new XmlTextWriter(_fileName, System.Text.Encoding.Unicode);

                writer.WriteStartDocument();
                writer.WriteStartElement("Priem");
                //�������
                writer.WriteStartElement("Filters");
                {
                    // ��������� SavedFilter
                    foreach (SavedFilter sf in _savedFilterList)
                        sf.ToXml(writer);
                }
                writer.WriteEndElement();//Filters
                //�������
                writer.WriteStartElement("ColumnsAbit");
                {
                    // ��������� Columns
                    foreach (string s in ColumnListAbit)
                    {
                        writer.WriteStartElement("Column");
                        writer.WriteAttributeString("Name",s);
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();//columns
                writer.WriteStartElement("ColumnsPerson");
                {
                    // ��������� Columns
                    foreach (string s in ColumnListPerson)
                    {
                        writer.WriteStartElement("Column");
                        writer.WriteAttributeString("Name", s);
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();//columns
                //����
                writer.WriteStartElement("Properties");
                {
                    // ��������� Columns
                    foreach (string s in _slValues.Keys)
                    {
                        writer.WriteStartElement("Property");
                        writer.WriteAttributeString("Name", s);
                        writer.WriteAttributeString("Value", _slValues[s]);
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();//����

                writer.WriteEndElement();//priem
                writer.WriteEndDocument();//XML
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }
    }

    public class DocsUsitConfigFile : Config
    {
        private SortedList<string, int> slInputOrder;
        private SortedList<string, int> slOutputOrder;
        private SortedList<string, int> slOrdersOrder;
        private SortedList<string, int> slInteriorsOrder;

        public DocsUsitConfigFile(string dir, string fileName)
            : base(dir, fileName)
        {
            
        }

        protected override void CreateConfigFile()
        {
            XmlTextWriter writer = null;

            try
            {
                writer = new XmlTextWriter(_fileName, System.Text.Encoding.Unicode);

                writer.WriteStartDocument();
                    writer.WriteStartElement("DocsUsit");
                        //������� ��������
                        writer.WriteStartElement("ColumnOrder");
                            writer.WriteAttributeString("DocsInput",string.Empty);
                            writer.WriteAttributeString("DocsOutput", string.Empty);
                            writer.WriteAttributeString("Orders", string.Empty);
                            writer.WriteAttributeString("Interiors", string.Empty);
                        writer.WriteEndElement();//columns

                    writer.WriteEndElement();//usit
                writer.WriteEndDocument();//XML
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (writer != null)
                    writer.Close();
            }        
        }

        protected override void InitItems()
        {
            slInputOrder = new SortedList<string, int>();
            slInteriorsOrder = new SortedList<string, int>();
            slOrdersOrder = new SortedList<string, int>();
            slOutputOrder = new SortedList<string, int>();
        }

        protected override void LoadConfig()
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(_fileName);
                XmlNodeList nodes = doc.DocumentElement.ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    //���� columns                
                    if (node.Name == "ColumnOrder")
                        LoadColumns(node);
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }  
        }

        //���������� ����������� �������
        private void LoadColumns(XmlNode node)
        {
            GetColumnsOrder(slInputOrder, node.Attributes["DocsInput"].Value);
            GetColumnsOrder(slOutputOrder, node.Attributes["DocsOutput"].Value);
            GetColumnsOrder(slOrdersOrder, node.Attributes["Orders"].Value);
            GetColumnsOrder(slInteriorsOrder, node.Attributes["Interiors"].Value);
        }

        //������ ������ �� �������
        private void GetColumnsOrder(SortedList<string, int> slColumnsOrder, string s)
        {
            try
            {
                if (s == string.Empty)
                    return;

                string[] strings = s.Split(':');

                for (int i = 0; i < strings.Length / 2; i++)
                {
                    slColumnsOrder.Add(strings[i * 2], int.Parse(strings[i * 2 + 1]));
                }
            }
            catch (NotSupportedException)
            {
                WinFormsServ.Error("������ ��������� ������������");
            }
            catch (ArgumentException)
            {
                WinFormsServ.Error("������ ��� ������ ������� ��������");
            }
            catch (FormatException)
            {
                WinFormsServ.Error("������ ��� ������ ������� ��������");
            }
        }        

        private void SaveColumnOrder(string paramName, string sValue)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(_fileName);
                XmlNodeList nodes = doc.DocumentElement.ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    //���� columns                
                    if (node.Name == "ColumnOrder")
                        node.Attributes[paramName].Value = sValue;
                }
                doc.Save(_fileName);
            }
            catch (Exception ex)
            {
                throw ex;
            }  
        }

        public void SaveColumnOrderIn(SortedList<string, int> slColumnsOrder)
        {
            slInputOrder = slColumnsOrder;
            SaveColumnOrder("DocsInput", GetParamStringFromCollection(slColumnsOrder));
        }
        public void SaveColumnOrderOut(SortedList<string, int> slColumnsOrder)
        {
            slOutputOrder = slColumnsOrder;
            SaveColumnOrder("DocsOutput", GetParamStringFromCollection(slColumnsOrder));
        }
        public void SaveColumnOrderOrder(SortedList<string, int> slColumnsOrder)
        {
            slOrdersOrder = slColumnsOrder;
            SaveColumnOrder("Orders", GetParamStringFromCollection(slColumnsOrder));
        }
        public void SaveColumnOrderInteriors(SortedList<string, int> slColumnsOrder)
        {
            slInteriorsOrder = slColumnsOrder;
            SaveColumnOrder("Interiors", GetParamStringFromCollection(slColumnsOrder));
        }

        public SortedList<string, int> GetOrderInput()
        {
            return slInputOrder;
        }
        public SortedList<string, int> GetOrderOutput()
        {
            return slOutputOrder;
        }
        public SortedList<string, int> GetOrderOrders()
        {
            return slOrdersOrder;
        }
        public SortedList<string, int> GetOrderInteriors()
        {
            return slInteriorsOrder;
        }

        //������ ������ ��� �������
        private string GetParamStringFromCollection(SortedList<string, int> slColumnsOrder)
        {
            string s = "";
            
            foreach (string key in slColumnsOrder.Keys)
            {
                s += ":" + key + ":" + slColumnsOrder[key];                
            }
            return s.Substring(1);
        }
    }
}
