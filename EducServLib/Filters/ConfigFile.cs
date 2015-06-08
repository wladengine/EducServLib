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

        //конструктор
        public Config(string dir, string fileName)
        {
            InitItems();

            DirectoryInfo di = new DirectoryInfo(dir);
            if (!di.Exists)
                di.Create();

            string file = string.Format(@"{0}\{1}", dir, fileName);
            _fileName = file;

            FileInfo fi = new FileInfo(file);
            
            //загрузка параметров
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
        //фильтры
        private List<SavedFilter> _savedFilterList;
        //столбцы
        private List<string> _columnListAbit;
        private List<string> _columnListPerson;
        //ключ-значение
        private SortedList<string,string> _slValues;
                
        //конструктор
        public ConfigFile(string dir, string fileName) : base(dir,fileName)
        {                   
        }

        //инициализация дополнительная
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

        //добавить пару в коллекцию
        public void AddValue(string key, string value)
        {
            if (_slValues.Keys.Contains(key))
                DeleteValue(key);
            _slValues.Add(key, value);
        }

        //удалить элемент из коллекции пар
        public void DeleteValue(string key)
        {
            _slValues.Remove(key);
        }

        //очистка списка столбцов
        public void ClearColumnListAbit()
        {
            _columnListAbit.Clear();
        }
        public void ClearColumnListPerson()
        {
            _columnListPerson.Clear();
        }

        //добавление названия столбца
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

        //добавление фильтра в список
        public void AddSavedFilter(SavedFilter sf)
        {
            _savedFilterList.Add(sf);
        }

        //удаление по имени
        public void DeleteSavedFilter(string name)
        {
            foreach (SavedFilter sf in _savedFilterList)
            {
                if (sf.Name.CompareTo(name) == 0)
                    _savedFilterList.Remove(sf);
                break;
            }
        }

        //загрузка параметров
        protected override void LoadConfig()
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(_fileName);
                XmlNodeList nodes = doc.DocumentElement.ChildNodes;
                foreach (XmlNode node in nodes)
                {
                    //ищем Filters                
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

        //записываем сохраненные фильтры
        private void LoadFilters(XmlNode node)
        {
            XmlNodeList xnl = node.ChildNodes;
            
            foreach (XmlNode xn in xnl)
                if (xn.Name == "SavedFilter")
                    AddSavedFilter(new SavedFilter(xn));
        }

        //записываем сохраненные столбцы
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

        //сохранение параметров
        public void SaveConfig()
        {
            XmlTextWriter writer = null;

            try
            {
                writer = new XmlTextWriter(_fileName, System.Text.Encoding.Unicode);

                writer.WriteStartDocument();
                writer.WriteStartElement("Priem");
                //фильтры
                writer.WriteStartElement("Filters");
                {
                    // сохраняем SavedFilter
                    foreach (SavedFilter sf in _savedFilterList)
                        sf.ToXml(writer);
                }
                writer.WriteEndElement();//Filters
                //столбцы
                writer.WriteStartElement("ColumnsAbit");
                {
                    // сохраняем Columns
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
                    // сохраняем Columns
                    foreach (string s in ColumnListPerson)
                    {
                        writer.WriteStartElement("Column");
                        writer.WriteAttributeString("Name", s);
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();//columns
                //пары
                writer.WriteStartElement("Properties");
                {
                    // сохраняем Columns
                    foreach (string s in _slValues.Keys)
                    {
                        writer.WriteStartElement("Property");
                        writer.WriteAttributeString("Name", s);
                        writer.WriteAttributeString("Value", _slValues[s]);
                        writer.WriteEndElement();
                    }
                }
                writer.WriteEndElement();//пары

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
                        //порядок столбцов
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
                    //ищем columns                
                    if (node.Name == "ColumnOrder")
                        LoadColumns(node);
                }                
            }
            catch (Exception ex)
            {
                throw ex;
            }  
        }

        //записываем сохраненные столбцы
        private void LoadColumns(XmlNode node)
        {
            GetColumnsOrder(slInputOrder, node.Attributes["DocsInput"].Value);
            GetColumnsOrder(slOutputOrder, node.Attributes["DocsOutput"].Value);
            GetColumnsOrder(slOrdersOrder, node.Attributes["Orders"].Value);
            GetColumnsOrder(slInteriorsOrder, node.Attributes["Interiors"].Value);
        }

        //парсит строку из конфига
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
                WinFormsServ.Error("Ошибка параметра конфигурации");
            }
            catch (ArgumentException)
            {
                WinFormsServ.Error("Ошибка при чтении порядка столбцов");
            }
            catch (FormatException)
            {
                WinFormsServ.Error("Ошибка при чтении порядка столбцов");
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
                    //ищем columns                
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

        //делает строку для конфига
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
