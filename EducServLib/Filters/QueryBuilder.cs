using System;
using System.Collections.Generic;
using System.Text;

namespace EducServLib
{
    public class QueryItem
    {
        private string _FieldName;
        private string _TableName;
        private string _NewName;

        //�������� ��� ������� � ����
        public string FieldName
        {
            get
            {
                return _FieldName;
            }
        }

        //�������� ��� ������� � ����
        public string TableName
        {
            get
            {
                return _TableName;
            }
        }

        //�������� ��� ������� � ����
        public string NewName
        {
            get
            {
                return _NewName;
            }
        }

        //����������� 
        public QueryItem(string table,string fullfield,string newname)
        {
            _FieldName = fullfield;
            _NewName = newname;
            _TableName = table;
        }

        //����������� 
        public QueryItem(string table, string fullfield)
        {
            _FieldName = fullfield;
            _NewName = string.Empty;
            _TableName = table;
        }

        //���������� ��������
        public override string ToString()
        {
            if(_NewName.Length>0)
                return string.Format("{0} AS '{1}'",_FieldName,_NewName);
            else
                return _FieldName;
        }
    }    

    //����������� ������� ��� ListAbit
    public class QueryBuilder
    {
        private SortedList<string, QueryItem> slQueryItems;
        private SortedList<string, string> slTableJoints;

        private string _DefaultTable;

        //����������� 
        public QueryBuilder(string defaultTable)
        {
            slQueryItems = new SortedList<string, QueryItem>();
            slTableJoints = new SortedList<string, string>();
            _DefaultTable = defaultTable;
        }

        //���������� ������
        public void AddQueryItem(QueryItem qi)
        {
            slQueryItems.Add(qi.NewName, qi);
        }

        //���������� ����������
        public void AddTableJoint(string table, string joint)
        {
            slTableJoints.Add(table, joint);
        }

        //������ �����
        public string GetQuery(List<string> list, string mainTable)
        {
            return GetQuery(list, null, mainTable);
        }

        public string GetQuery(List<string> list, List<string> lTables, string mainTable)
        {
            //������ ������ ������
            List<string> lUsedTables = lTables == null ? new List<string>() : lTables;
            List<string> lFields = new List<string>();

            if (!lUsedTables.Contains("ed.Person"))
                lUsedTables.Insert(0, "ed.Person");

            if (!lUsedTables.Contains("ed.extPerson_EducationInfo_Current"))
                lUsedTables.Insert(1, "ed.extPerson_EducationInfo_Current");
            else
            {
                lUsedTables.Remove("ed.extPerson_EducationInfo_Current");
                lUsedTables.Insert(1, "ed.extPerson_EducationInfo_Current");
            }
            //�������� ���� Id 
            lFields.Add(string.Format("{0}.Id", (mainTable==null? _DefaultTable:mainTable)));

            foreach (string s in list)
            {
                try
                {
                    QueryItem qi = slQueryItems[s];
                    string table = qi.TableName;

                    lFields.Add(qi.ToString());

                    if (!lUsedTables.Contains(table))
                        lUsedTables.Add(qi.TableName);
                }
                catch (Exception) { }
            }

            if (lUsedTables.Contains(_DefaultTable))
                lUsedTables.Remove(_DefaultTable);

            string joins = _DefaultTable;
            foreach(string table in lUsedTables)
                joins = string.Format(" {0} {1} ", joins, slTableJoints[table]);

            return string.Format("SELECT DISTINCT {0} FROM {1} ", Util.BuildStringWithCollection(lFields), joins);
        }

        public static string GetBoolField(string field)
        {
            return string.Format(" case when {0} > 0 then '��' else '���' end ", field);
        }
    }
}
