using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ComponentModel;
using System.Data;
using System.Globalization;
using System.Data.Objects;

namespace EducServLib
{
    public class WinFormsServ
    {  
        // функция обработки ошибок
        public static void Error(string msg)
        {
            MessageBox.Show(msg, "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        public static void Error(Exception ex)
        {
            MessageBox.Show(ex.Message + (ex.InnerException == null ? "" : "\nВнутреннее исключение: " + ex.InnerException.Message), "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Error(string StartMessage, Exception ex)
        {
            MessageBox.Show(StartMessage + "\n" + ex.Message + (ex.InnerException == null ? "" : "\nВнутреннее исключение: " + ex.InnerException.Message), "Ошибка!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void Search(DataGridView dgv, string sColumnName, string sPattern)
        {
            try
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    object cellValue = dgv.Rows[i].Cells[sColumnName].Value;
                    // Если ячейка грида соответствует полю таблицы имеющему значение NULL,
                    // то значение ячейки (объект "Value") становится null,
                    // чтобы избежать "null reference exception" в момент вызова метода ToString(),
                    // присваиваем Value объект string.Empty
                    cellValue = (cellValue == null ? string.Empty : cellValue);

                    if (cellValue.ToString().StartsWith(sPattern, true, System.Globalization.CultureInfo.CurrentCulture))
                    {
                        //dgv.FirstDisplayedScrollingRowIndex = i;
                        //dgv.Rows[i].Selected = true;
                        dgv.CurrentCell = dgv[sColumnName, i];
                        break;
                    }
                }
            }
            catch { }
        }

        public static void SearchInsideValue(DataGridView dgv, string sColumnName, string sPattern)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                object cellValue = dgv.Rows[i].Cells[sColumnName].Value;
                // Если ячейка грида соответствует полю таблицы имеющему значение NULL,
                // то значение ячейки (объект "Value") становится null,
                // чтобы избежать "null reference exception" в момент вызова метода ToString(),
                // присваиваем Value объект string.Empty
                cellValue = (cellValue == null ? string.Empty : cellValue);

                if (cellValue.ToString().Contains(sPattern))
                {
                    //dgv.FirstDisplayedScrollingRowIndex = i;
                    //dgv.Rows[i].Selected = true;
                    dgv.CurrentCell = dgv[sColumnName, i];
                    break;
                }
            }
        }

        /// <summary>
        /// column with index 1 must be visible
        /// </summary>
        /// <param name="dgv"></param>
        /// <param name="id"></param>
        public static void SearchById(DataGridView dgv, string id)
        {
            for (int i = 0; i < dgv.Rows.Count; i++)
            {
                object cellValue = dgv.Rows[i].Cells["Id"].Value;
                // Если ячейка грида соответствует полю таблицы имеющему значение NULL,
                // то значение ячейки (объект "Value") становится null,
                // чтобы избежать "null reference exception" в момент вызова метода ToString(),
                // присваиваем Value объект string.Empty
                cellValue = (cellValue == null ? string.Empty : cellValue);

                if (cellValue.ToString() == id)
                {
                    dgv.CurrentCell = dgv[1, i];
                    break;
                }
            }
        }

        //перенос строк
        public static void MoveRows(ListBox from, ListBox to, bool isAll)
        {
            for (int i = from.Items.Count - 1; i >= 0; i--)// 
            {
                if (isAll || from.SelectedIndices.Contains(i))
                {
                    to.Items.Add(from.Items[i]);
                    from.Items.RemoveAt(i);
                }
            }
        }

        public static void SetEng(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("en"));
        }

        public static void SetRus(object sender, EventArgs e)
        {
            InputLanguage.CurrentInputLanguage = InputLanguage.FromCulture(CultureInfo.GetCultureInfo("ru"));
        }

        public static void SetSubControlsEnabled(Control ctr, bool isEnabled)
        {
            if (ctr.Controls.Count == 0)
                ctr.Enabled = isEnabled;
            else
            {
                ctr.Enabled = true;
                foreach (Control c in ctr.Controls)
                {
                    SetSubControlsEnabled(c, isEnabled);                    
                }
            }
        }

        public static int FilterGrid(ref DataGridView _dgv, Dictionary<string, string> dicFilters)
        {
            int iRowsCount = 0;

            BindingContext BindingContect = _dgv.BindingContext;
            CurrencyManager currencyManager1 = (CurrencyManager)BindingContect[_dgv.DataSource];
            currencyManager1.SuspendBinding();

            //если все фильтры пустые
            if (dicFilters.Where(x => string.IsNullOrEmpty(x.Value)).Count() != dicFilters.Count)
            {
                for (int i = 0; i < _dgv.Rows.Count; i++)
                {
                    bool bVisible = true;
                    foreach (var kvp in dicFilters)
                    {
                        string sColumnName = kvp.Key;
                        string sFilterValue = kvp.Value;
                        if (_dgv.Rows[i].Cells[sColumnName].Value.ToString().IndexOf(sFilterValue, StringComparison.OrdinalIgnoreCase) < 0)
                            bVisible = false;
                    }
                    if (bVisible)
                        iRowsCount++;

                    _dgv.Rows[i].Visible = bVisible;
                }
            }
            else
            {
                for (int i = 0; i < _dgv.Rows.Count; i++)
                {
                    _dgv.Rows[i].Visible = true;
                }
                iRowsCount = _dgv.Rows.Count;
            }

            return iRowsCount;
        }
    }
}