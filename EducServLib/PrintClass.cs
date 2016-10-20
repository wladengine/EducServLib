using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data;

using BaseFormsLib;
using EducServLib;
using System.Diagnostics;
using OfficeOpenXml;

using System.IO;
using System.Reflection;

namespace EducServLib
{
    public static class PrintClass
    {
        public static void PrintAllToExcel(BaseList lst)
        {
            PrintAllToExcel(lst, false);
        }
        public static void PrintAllToExcel(BaseList lst, bool withId, List<string> lstFields = null)
        {
            PrintAllToExcel(lst.Dgv, withId, lst.Text, lstFields);
        }

        public static void PrintAllToExcel(DataGridView dgv)
        {
            PrintAllToExcel(dgv, false, "");
        }
        public static void PrintAllToExcel(DataGridView dgv, bool withId, string lstText, List<string> lstFields = null)
        {
            if (lstFields == null)
                lstFields = new List<string>();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Файлы Excel (.xlsx)|*.xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                PrintAllToExcel(dgv, withId, lstText, lstFields, sfd.FileName);
            }
            //На всякий случай
            sfd.Dispose();
        }
        public static void PrintAllToExcel(DataGridView dgv, bool withId, string lstText, List<string> lstFields, string fileName)
        {
            try
            {
                FileInfo newFile = new FileInfo(fileName);
                if (newFile.Exists)
                {
                    newFile.Delete();  // ensures we create a new workbook
                    newFile = new FileInfo(fileName);
                }
                using (ExcelPackage doc = new ExcelPackage(newFile))
                {
                    if (string.IsNullOrEmpty(lstText))
                        lstText = "Экспорт";
                    ExcelWorksheet ws = doc.Workbook.Worksheets.Add(lstText.Substring(0, lstText.Length < 30 ? lstText.Length - 1 : 30));

                    int i = 1;
                    int j = 1;

                    foreach (DataGridViewColumn dc in dgv.Columns)
                    {
                        if (dc.Visible || (withId && dc.Name == "Id") || lstFields.Contains(dc.Name))
                        {
                            ws.Column(j).Width = (double)dc.Width / 6.25d;
                            ws.Cells[i, j].Value = dc.HeaderText;
                            ws.Cells[i, j].Style.WrapText = true;
                            j++;
                        }
                    }

                    i++;

                    ProgressForm prog = new ProgressForm(0, dgv.Rows.Count, 1, ProgressBarStyle.Blocks, "Импорт списка");
                    prog.Show();
                    prog.SetProgressText("Импорт списка");
                    // печать из грида
                    foreach (DataGridViewRow dr in dgv.Rows)
                    {
                        j = 1;
                        foreach (DataGridViewColumn dc in dgv.Columns)
                        {
                            if (dc.Visible || (withId && dc.Name == "Id") || lstFields.Contains(dc.Name))
                            {
                                string val = dr.Cells[dc.Name].Value == null ? "" : dr.Cells[dc.Name].Value.ToString();
                                //ws.Cells[i, j].Style = new Style() { NumberFormat = new NumberFormat("@") };
                                ws.Cells[i, j].Value = val;

                                j++;
                            }
                        }

                        i++;
                        prog.PerformStep();
                    }
                    prog.Close();
                    doc.Save();
                }

                Process.Start(fileName);
            }
            catch (Exception exc)
            {
                WinFormsServ.Error(exc);
            }
        }
        
        public static void PrintAllToExcel2007(DataTable tbl, string sheetName)
        {
            List<string> lstFields = new List<string>();

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "Файлы Excel (.xlsx)|*.xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                PrintAllToExcel2007(tbl, sheetName, sfd.FileName);
            }
            //На всякий случай
            sfd.Dispose();
        }
        public static void PrintAllToExcel2007(DataTable tbl, string sheetName, string fileName)
        {
            ProgressForm prog = new ProgressForm(0, tbl.Rows.Count, 1, ProgressBarStyle.Blocks, "Импорт списка");
            try
            {
                FileInfo newFile = new FileInfo(fileName);
                if (newFile.Exists)
                {
                    newFile.Delete();  // ensures we create a new workbook
                    newFile = new FileInfo(fileName);
                }
                using (ExcelPackage doc = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = doc.Workbook.Worksheets.Add(sheetName.Substring(0, sheetName.Length < 30 ? sheetName.Length - 1 : 30));
                    int i = 1;
                    int j = 1;

                    foreach (DataColumn dc in tbl.Columns)
                    {
                        ws.Cells[i, j].Value = dc.Caption;
                        j++;
                    }

                    i++;
                    
                    prog.Show();
                    prog.SetProgressText("Импорт списка");
                    // печать из грида
                    foreach (DataRow dr in tbl.Rows)
                    {
                        j = 1;
                        foreach (DataColumn dc in tbl.Columns)
                        {
                            string val = dr[dc.ColumnName] == null ? "" : dr[dc.ColumnName].ToString();
                            ws.Cells[i, j].Value = val;
                            j++;
                        }

                        i++;
                        prog.PerformStep();
                    }

                    prog.SetProgressText("Сохранение...");
                    doc.Save();
                }
                prog.SetProgressText("Открытие файла...");
                Process.Start(fileName);
            }
            catch (Exception exc)
            {
                WinFormsServ.Error(exc);
            }
            finally
            {
                prog.Close();
            }
        }
        public static void PrintAllToExcel2007Colors(DataGridView tbl, string sheetName, string fileName)
        {
            try
            {
                FileInfo newFile = new FileInfo(fileName);
                if (newFile.Exists)
                {
                    newFile.Delete();  // ensures we create a new workbook
                    newFile = new FileInfo(fileName);
                }
                using (ExcelPackage doc = new ExcelPackage(newFile))
                {
                    ExcelWorksheet ws = doc.Workbook.Worksheets.Add(sheetName.Substring(0, sheetName.Length < 30 ? sheetName.Length - 1 : 30));
                    int i = 1;
                    int j = 1;

                    ProgressForm prog = new ProgressForm(0, tbl.Rows.Count, 1, ProgressBarStyle.Blocks, "Импорт списка");
                    prog.Show();
                    prog.SetProgressText("Импорт списка");
                    // печать из грида
                    foreach (DataGridViewColumn dc in tbl.Columns)
                    {
                        if (!dc.Visible)
                            continue;
                        if (String.IsNullOrEmpty(tbl.Rows[0].Cells[dc.Name].Value.ToString()))
                            continue;
 
                        i = 1;

                        foreach (DataGridViewRow dr in tbl.Rows)
                        {
                            if (!dr.Visible)
                                continue;

                            string val = dr.Cells[dc.Name].Value == null ? "" : dr.Cells[dc.Name].Value.ToString();
                            ws.Cells[i, j].Value = val;
                            if (dr.Cells[dc.Name].Style.BackColor != System.Drawing.Color.Empty)
                            {
                                ws.Cells[i, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[i, j].Style.Fill.BackgroundColor.SetColor(dr.Cells[dc.Name].Style.BackColor);
                            }
                            i++;
                        }

                        j++;
                        prog.PerformStep();
                    }
                    prog.Close();
                    doc.Save();
                }

                Process.Start(fileName);
            }
            catch (Exception exc)
            {
                WinFormsServ.Error(exc);
            }
        }
        public static DataTable GetDataTableFromExcel(string sheetName)
        {
            DataTable tbl = new DataTable();
            tbl.Columns.Add("Surname");
            tbl.Columns.Add("Name");
            tbl.Columns.Add("SecondName");
            tbl.Columns.Add("Degree");
            tbl.Columns.Add("AcademicTitle");
            tbl.Columns.Add("MainWorkName");
            tbl.Columns.Add("Position");
            tbl.Columns.Add("Division");
            tbl.Columns.Add("Chair");
            tbl.Columns.Add("EmploymentType");
            tbl.Columns.Add("Employment");

            OpenFileDialog sfd = new OpenFileDialog();
            sfd.Filter = "Файлы Excel (.xlsx)|*.xlsx";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                GetDataTableFromExcel2007(sfd.FileName, sheetName, false);
            }

            return tbl;
        }

        public static DataTable GetDataTableFromExcel2007(string fileName)
        {
            return GetDataTableFromExcel2007(fileName, null, false);
        }
        public static DataTable GetDataTableFromExcel2007(string fileName, string sheetName, bool bUseColNamesFromFile)
        {
            ProgressForm pf = new ProgressForm();
            pf.Show();
            pf.SetProgressText("Инициализация файла...");

            DataTable tbl = new DataTable();

            try
            {
                FileInfo newFile = new FileInfo(fileName);
                using (ExcelPackage doc = new ExcelPackage(newFile))
                {
                    pf.SetProgressText("Анализ структуры данных файла...");
                    ExcelWorksheet ws = string.IsNullOrEmpty(sheetName) ? doc.Workbook.Worksheets[1] : doc.Workbook.Worksheets[sheetName];
                    var zzz =
                        (from c in ws.Cells
                         select new { Column = new ExcelCellAddress(c.Address).Column, Row = new ExcelCellAddress(c.Address).Row }).ToList();

                    int firstCol = ws.Dimension.Start.Column; //zzz.Select(x => x.Column).Min();
                    int firstRow = ws.Dimension.Start.Row; //zzz.Select(x => x.Row).Min();

                    //int lastCol = zzz.Select(x => x.Column).Max();
                    //int lastRow = zzz.Select(x => x.Row).Max();

                    bool bUseColNames = bUseColNamesFromFile;
                    //проверка, чтобы все столбцы были уникальны
                    var lstColNames = zzz.Where(x => x.Row == firstRow && ws.Cells[firstRow, x.Column].Value != null).Select(x => x.Column).Select(x => ws.Cells[firstRow, x].Value.ToString()).ToList();
                    //если число уникальных столбцов не совпадает с общим числом столбцов, то использовать просто номера столбцов
                    if (bUseColNames && lstColNames.Distinct().Count() != lstColNames.Count)
                        bUseColNames = false;
                    
                    // выборка столбцов для будущей таблицы
                    foreach (int colId in zzz.Where(x => x.Row == firstRow && ws.Cells[firstRow, x.Column].Value != null).Select(x => x.Column))
                    {
                        if (bUseColNames)
                            tbl.Columns.Add(ws.Cells[firstRow, colId].Value.ToString());
                        else
                            tbl.Columns.Add(colId.ToString());
                    }

                    List<int> lstZZZ = zzz.Where(x => x.Row > firstRow && ws.Cells[x.Row, x.Column].Value != null).Select(x => x.Row).OrderBy(x => x).Distinct().ToList();
                    pf.SetProgressText("Импорт данных из файла...");
                    pf.MaxPrBarValue = lstZZZ.Count;
                    // выборка данных для таблицы
                    foreach (int rowId in lstZZZ)
                    {
                        pf.PerformStep();
                        DataRow rw = tbl.NewRow();
                        
                        foreach (int colId in zzz.Where(x => x.Row == rowId && x.Column <= tbl.Columns.Count).Select(x => x.Column).Distinct())
                        {
                            string colName = bUseColNames ? ws.Cells[firstRow, colId].Value.ToString() : colId.ToString();
                            if (ws.Cells[rowId, colId] != null)
                                rw[colName] = ws.Cells[rowId, colId].Text;
                        }

                        tbl.Rows.Add(rw);
                    }
                }
            }
            catch { }
            finally { pf.Close(); }

            return tbl;
        }
        public static List<T> GetClassFromExcel<T>(string path, int fromRow, int fromColumn, int toColumn = 0)
        {
            List<T> retList = new List<T>();
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                //Retrieve first Worksheet
                var ws = pck.Workbook.Worksheets.First();
                //If the to column is empty or 0, then make the tocolumn to the count of the properties
                //Of the class object inserted
                toColumn = toColumn == 0 ? typeof(T).GetProperties().Count() : toColumn;

                //Read the first Row for the column names and place into a list so that
                //it can be used as reference to properties
                List<string> columnNames = new List<string>();
                // wsRow = ws.Row(0);
                foreach (var cell in ws.Cells[1, 1, 1, ws.Cells.Count()])
                {
                    columnNames.Add(cell.Value.ToString());
                }
                //Loop through the rows of the excel sheet
                for (var rowNum = fromRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    //create a instance of T
                    T objT = Activator.CreateInstance<T>();
                    //Retrieve the type of T
                    Type myType = typeof(T);
                    //Get all the properties associated with T
                    PropertyInfo[] myProp = myType.GetProperties();

                    var wsRow = ws.Cells[rowNum, fromColumn, rowNum, ws.Cells.Count()];

                    foreach (var propertyInfo in myProp)
                    {
                        if (columnNames.Contains(propertyInfo.Name))
                        {
                            int position = columnNames.IndexOf(propertyInfo.Name);
                            //To prevent an exception cast the value to the type of the property.
                            propertyInfo.SetValue(objT, Convert.ChangeType(wsRow[rowNum, position + 1].Value, propertyInfo.PropertyType), null);
                        }
                    }

                    retList.Add(objT);
                }

            }
            return retList;
        }
        public static DataTable GetDataTableFromExcel2007_New(string path, bool hasHeader = true)
        {
            ProgressForm pf = new ProgressForm();
            pf.Show();
            pf.SetProgressText("Инициализация файла...");

            DataTable tbl = new DataTable();

            try
            {
                var pck = new OfficeOpenXml.ExcelPackage();
                pck.Load(File.OpenRead(path));
                var ws = pck.Workbook.Worksheets.First();

                pf.SetProgressText("Анализ структуры данных файла...");
                Dictionary<int, string> dicColNames = new Dictionary<int, string>();
                var dim = ws.Cells[1, 1, 1, ws.Dimension.End.Column];
                foreach (var firstRowCell in dim)
                {
                    //dicColNames.Add(firstRowCell.Start.Column, hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                    tbl.Columns.Add(hasHeader ? firstRowCell.Text : string.Format("Column {0}", firstRowCell.Start.Column));
                }

                //foreach (var kvp in dicColNames)
                //{
                //    if (!string.IsNullOrEmpty(kvp.Value) || (string.IsNullOrEmpty(kvp.Value) && dicColNames.Where(x => x.Key > kvp.Key && !string.IsNullOrEmpty(x.Value)).Count() > 0))
                //    {
                //        tbl.Columns.Add(kvp.Value);
                //    }
                //}

                var startRow = hasHeader ? 2 : 1;
                pf.SetProgressText("Импорт данных из файла...");
                pf.MaxPrBarValue = ws.Dimension.End.Row;
                for (var rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    pf.PerformStep();
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    var row = tbl.NewRow();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                    tbl.Rows.Add(row);
                }

                pck.Dispose();
            }
            catch (Exception ex) { WinFormsServ.Error(ex); }
            finally { pf.Close(); }
            return tbl;
        }
    }
}
