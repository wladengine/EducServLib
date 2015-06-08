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

                    ProgressForm prog = new ProgressForm(0, tbl.Rows.Count, 1, ProgressBarStyle.Blocks, "Импорт списка");
                    prog.Show();
                    prog.SetProgressText("Импорт списка");
                    // печать из грида
                    foreach (DataRow dr in tbl.Rows)
                    {
                        j = 1;
                        foreach (DataColumn dc in tbl.Columns)
                        {
                            string val = dr[dc.ColumnName] == null ? "" : dr[dc.ColumnName].ToString();
                            //ws.Cells[i, j].Style = new Style() { NumberFormat = new NumberFormat("@") };
                            ws.Cells[i, j].Value = val;
                            j++;
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
            DataTable tbl = new DataTable();

            FileInfo newFile = new FileInfo(fileName);
            using (ExcelPackage doc = new ExcelPackage(newFile))
            {
                ExcelWorksheet ws = string.IsNullOrEmpty(sheetName) ? doc.Workbook.Worksheets[1] : doc.Workbook.Worksheets[sheetName];
                var zzz = from c in ws.Cells select new ExcelCellAddress(c.Address);

                int firstCol = zzz.Select(x => x.Column).Min();
                int firstRow = zzz.Select(x => x.Row).Min();

                int lastCol = zzz.Select(x => x.Column).Max();
                int lastRow = zzz.Select(x => x.Row).Max();

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

                // выборка данных для таблицы
                foreach (int rowId in zzz.Where(x => x.Row > firstRow && ws.Cells[x.Row, x.Column].Value != null).Select(x => x.Row).OrderBy(x => x).Distinct())
                {
                    DataRow rw = tbl.NewRow();
                    foreach (int colId in zzz.Where(x => x.Row == rowId && x.Column <= tbl.Columns.Count).Select(x => x.Column).Distinct())
                    {
                        string rowName = bUseColNames ? ws.Cells[firstRow, colId].Value.ToString() : colId.ToString();
                        if (ws.Cells[rowId, colId] != null)
                            rw[rowName] = ws.Cells[rowId, colId].Value;
                    }

                    tbl.Rows.Add(rw);
                }
            }

            return tbl;
        }
    }
}
