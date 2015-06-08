using System;
using System.Data;
using System.Reflection;
using System.Reflection.Emit;
using System.ComponentModel;

namespace EducServLib
{
    /// <summary>
    /// Преобразователь объектов в DataTable.
    /// </summary>
    public static class Converter
    {
        /// <summary>
        /// Преобразует объект в DataTable
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable(Object o)
        {
            PropertyInfo[] properties = o.GetType().GetProperties();
            DataTable dt = CreateDataTable(properties);
            FillData(properties, dt, o);
            return dt;
        }

        /// <summary>
        /// Преобразует массив объектов в DataTable
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        public static DataTable ConvertToDataTable(Object[] array)
        {
            PropertyInfo[] properties = array.GetType().GetElementType().GetProperties();
            DataTable dt = CreateDataTable(properties);

            if (array.Length != 0)
            {
                foreach (object o in array)
                    FillData(properties, dt, o);

            }

            return dt;
        }

        private static DataTable CreateDataTable(PropertyInfo[] properties)
        {
            DataTable dt = new DataTable();
            DataColumn dc = null;
            NullableConverter nc;

            foreach (PropertyInfo pi in properties)
            {
                Type theType;

                if (IsNullableType(pi.PropertyType))
                {
                    nc = new NullableConverter(pi.PropertyType);
                    theType = nc.UnderlyingType;
                    nc = null;
                }
                else
                {
                    theType = pi.PropertyType;
                }

                dc = new DataColumn();
                dc.ColumnName = pi.Name;
                dc.DataType = theType;

                dt.Columns.Add(dc);
            }

            return dt;
        }

        private static void FillData(PropertyInfo[] properties, DataTable dt, Object o)
        {
            DataRow dr = dt.NewRow();

            foreach (PropertyInfo pi in properties)
                dr[pi.Name] = pi.GetValue(o, null) != null ? pi.GetValue(o, null) : DBNull.Value;

            dt.Rows.Add(dr);
        }

        private static bool IsNullableType(Type theType)
        {
            return (theType.IsGenericType && theType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)));
        }
    }
}


 
