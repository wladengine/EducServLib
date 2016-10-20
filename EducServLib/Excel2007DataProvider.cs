using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EducServLib
{
    public class ExcelParsedCell
    {
        public int RowIndex { get; set; }
        public int ColIndex { get; set; }
        public string Value { get; set; }
    }

    public static class Excel2007DataProvider
    {
        public static List<ExcelParsedCell> GetParsedCellList(string fileName, string sheetName)
        {
            FileInfo newFile = new FileInfo(fileName);
            //if (newFile.Exists)
            //{
            //    newFile.Delete();  // ensures we create a new workbook
            //    newFile = new FileInfo(fileName);
            //}

            List<ExcelParsedCell> result = new List<ExcelParsedCell>();

            using (ExcelPackage doc = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = string.IsNullOrEmpty(sheetName) ? doc.Workbook.Worksheets[1] : doc.Workbook.Worksheets[sheetName];
                var zzz = (from c in ws.Cells
                           select new ExcelParsedCell()
                           {
                               RowIndex = new ExcelCellAddress(c.Address).Row,
                               ColIndex = new ExcelCellAddress(c.Address).Column,
                               Value = c.Value == null ? null : c.Value.ToString()
                           });

                result.AddRange(zzz);
            }

            return result;
        }
        public static List<ExcelParsedCell> GetParsedCellList(string fileName)
        {
            return GetParsedCellList(fileName, null);
        }
    }
}
