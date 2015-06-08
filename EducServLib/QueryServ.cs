using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace EducServLib
{
    public class QueryServ
    { 
        public static bool BoolParseFromString(string value)
        {
            if (value == "0" || value == "" || value == "false")
                return false;
            else
                return true;
        }

        public static string StringParseFromBool(bool value)
        {
            if (value)
                return "1";
            else
                return "0";
        }

        public static string QueryForBool(string field)
        {
            return (field == "True" || field == "true" ? "1" : "0");
        }

        public static object ToNullDBEmpty(object value)
        {
            if (value == null || value.ToString() == string.Empty)
                return DBNull.Value;
            else
                return value;
        }

        public static object ToNullDB(object value)
        {
            if (value == null)
                return DBNull.Value;
            else
                return value;
        }

        public static string ToNullString(object val)
        {
            if (Util.ToStr(val) == string.Empty)
                return "null";
            else
                return val.ToString();
        }
     
        public static string ToIsNull(object val)
        {
            if (Util.ToStr(val) == string.Empty)
                return "is null";
            else
                return " = " + val.ToString();
        }

        public static bool ToBoolValue(object val)
        {
            if (val.GetType() == typeof(bool))
                return (bool)val;
            else if (val.GetType() == typeof(int))
            {
                if ((int)val == 0)
                    return false;
                else
                    return true;
            }
            else return BoolParse(val.ToString());
        }

        public static DateTime? ToNullDateTimeValue(object val)
        {
            if (val.GetType() == typeof(DateTime))
                return (DateTime)val;
            else
                return null;             
        }

        public static bool BoolParse(string value)
        {
            if (value == "0" || value == "false" || value == "")
                return false;
            else
                return true;
        }      
    }
}
