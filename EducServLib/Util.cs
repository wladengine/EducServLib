using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Linq.Expressions;

namespace EducServLib
{
    public static class Util
    {
        static char[] eng = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public static string BuildStringWithCollection(List<string> list)
        {
            if (list.Count == 0)
                return string.Empty;

            return string.Join(", ", list.ToArray());
        }

        public static string BuildStringWithCollection(IList<int> list)
        {
            if (list.Count == 0)
                return string.Empty;

            string res = string.Empty;
            foreach (object s in list)
                if (s!= null && s.ToString() != string.Empty)
                    res += ", " + s.ToString();

            return res.Substring(1);
        }

        public static string BuildStringWithCollectionWithApps(IList<string> list)
        {
            if (list.Count == 0)
                return string.Empty;

            string res = string.Empty;
            foreach (string s in list)
                if (s != string.Empty)
                    res += ", " + "'" + s + "'";

            return res.Substring(1);
        }

        public static string BuildStringWithCollection(IList<string> list)
        {
            if (list.Count == 0)
                return string.Empty;

            string res = string.Empty;
            foreach (string s in list)
                if (s != string.Empty)
                    res += ", " + s;

            return res.Substring(1);
        }

        public static string BuildStringWithCollection(List<ListItem> list, bool ids)
        {
            if (list.Count == 0)
                return string.Empty;

            string res = string.Empty;
            foreach (ListItem li in list)
                res += ", " + (ids ? li.Id : li.Name);

            return res.Substring(1);
        } 

        public static string GetFIO(string surname, string name, string lastname)
        {
            surname = surname.Trim();
            name = name.Trim();
            lastname = lastname.Trim();
            if (name != "")
                name = name.Substring(0, 1);
            if (lastname != "")
                lastname = lastname.Substring(0, 1);
            return surname + " " + (name == "" ? "" : name + ". ") + (lastname == "" ? "" : lastname + ".");
        }

        //возвращает строку число - длинный русский месяц
        public static string GetDayStringNow()
        {
            return GetDateString(DateTime.Now, false, false);
        }

        //возвращает дату прописью
        public static string GetDateStringNow()
        {
            return GetDateString(DateTime.Now, true, false);
        }

        public static string GetDateString(DateTime dt, bool year, bool skobki)
        {
            string day = dt.Day.ToString();
            int month = dt.Month;
            string sYear = dt.Year.ToString();
            string m = string.Empty;

            string res = string.Empty;

            if (skobki)
                day = string.Format("«{0}»", day);

            switch (month)
            {
                case 1:
                    m = "января"; break;
                case 2:
                    m = "февраля"; break;
                case 3:
                    m = "марта"; break;
                case 4:
                    m = "апреля"; break;
                case 5:
                    m = "мая"; break;
                case 6:
                    m = "июня"; break;
                case 7:
                    m = "июля"; break;
                case 8:
                    m = "августа"; break;
                case 9:
                    m = "сентября"; break;
                case 10:
                    m = "октября"; break;
                case 11:
                    m = "ноября"; break;
                case 12:
                    m = "декабря"; break;
            }
            res = string.Format("{0} {1}", day, m);
            if (year)
                res = string.Format("{0} {1}", res, sYear);

            return res;
        }

        public static bool ListCompare(List<string> l1, List<string> l2)
        {
            if (l1 == null)
                return l2 == null ? true : false;
            else if (l2 == null)
                return false;

            if (l1.Count != l2.Count)
                return false;

            for (int i = 0; i < l1.Count; i++)
                if (l1[i].CompareTo(l2[i]) != 0)
                    return false;

            return true;
        }

        public static string ToStr(object obj)
        {
            if (obj == null)
                return string.Empty;
            else
                return obj.ToString();
        }

        public static object ToNullObject(object val)
        {
            if (Util.ToStr(val) == string.Empty)
                return null;
            else
                return val;
        }

        public static bool ToBool(object obj)
        {
            if (obj == null || string.IsNullOrEmpty(obj.ToString()))
                return false;
            else
                return (bool)obj;
        }

        public static bool IsRussianString(string s)
        {
            if (string.IsNullOrEmpty(s))
                return true;

            if (s.ToUpper().IndexOfAny(eng) >= 0)
                return false;
            else
                return true;
        }

        public static string DateToRus(string date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return dt.ToString(@"dd MMMM yyyy", CultureInfo.GetCultureInfo("ru-RU")) + " года";
            }
            catch
            {
                return "";
            }
        }

        public static string DateToEng(string date)
        {
            try
            {
                DateTime dt = DateTime.Parse(date);
                return dt.ToString(@"MMMM dd, yyyy", CultureInfo.GetCultureInfo("en-US"));
            }
            catch
            {
                return "";
            }
        }

        public static IList<int> GetListInt(IList<string> lst)
        {
            List<int> lstInts = new List<int>();
            foreach (string val in lst)
            {
                lstInts.Add(int.Parse(val));
            }

            return lstInts;
        }

        public static IList<string> GetListString(IList<int> lst)
        {
            List<string> lstStrings = new List<string>();
            foreach (int val in lst)
            {
                lstStrings.Add(val.ToString());
            }

            return lstStrings;
        }        
    }
}
