using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EducServLib
{
    public partial class FilterMultySelect : UserControl
    {
        //конструктор
        public FilterMultySelect()
        {
            InitializeComponent();
        }

        public int YesCount
        {
            get
            {
                return lbYes.Items.Count;
            }
        }

        //функции переноса строк
        private void btnLeft_Click(object sender, EventArgs e)
        {
            MoveRows(lbNo, lbYes, false);
        }

        //
        private void btnRight_Click(object sender, EventArgs e)
        {
            MoveRows(lbYes, lbNo, false);
        }

        //
        private void btnLeftAll_Click(object sender, EventArgs e)
        {
            MoveRows(lbNo, lbYes, true);
        }

        //
        private void btnRightAll_Click(object sender, EventArgs e)
        {
            MoveRows(lbYes,lbNo,true);
        }

        //перенос строк
        private void MoveRows(ListBox from, ListBox to, bool isAll)
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

        //заполнение правого или левого листбокса
        public void FillList(List<ListItem> list, bool isRight)
        {
            try
            {
                if(isRight)
                    foreach (ListItem li in list)
                        lbNo.Items.Add(li);
                else
                    foreach (ListItem li in list)
                        lbYes.Items.Add(li);
            }
            catch (Exception ex)
            {
                WinFormsServ.Error("Ошибка при заполнении листа: ", ex);
            }
        }

        //очистка списков
        public void ClearLists()
        {
            this.lbNo.Items.Clear();
            this.lbYes.Items.Clear();
        }

        //возвращает список выбранных объектов
        public List<ListItem> GetSelectedItems()
        {
            List<ListItem> list = new List<ListItem>();

            foreach (ListItem li in lbYes.Items)
                list.Add(li);

            return list;
        }

        //возвращает список НЕвыбранных объектов
        public List<ListItem> GetNotSelectedItems()
        {
            List<ListItem> list = new List<ListItem>();

            foreach (ListItem li in lbNo.Items)
                list.Add(li);

            return list;
        }

        //удаление по имени
        public void RemoveAtRight(string name)
        {
            for (int i = 0; i < lbNo.Items.Count; i++)
                if ((lbNo.Items[i] as ListItem).Name.CompareTo(name) == 0)
                {
                    lbNo.Items.RemoveAt(i);
                    break;
                }            
        }

        public void AddInLeft(ListItem li)
        {
            lbYes.Items.Add(li);
        }

        public void AddInRight(ListItem li)
        {
            lbNo.Items.Add(li);
        }
    }    
}
