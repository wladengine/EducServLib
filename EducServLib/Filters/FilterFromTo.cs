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
    public partial class FilterFromTo : UserControl
    {
        public FilterFromTo()
        {
            InitializeComponent();
        }       

        //проверка на непустоту
        public bool CheckValues()
        {
            if (tbFrom.Text.Trim().CompareTo(string.Empty) == 0 &&
                tbTo.Text.Trim().CompareTo(string.Empty) == 0)
            {
                epError.SetError(this.tbFrom, "Оба поля пусты.");
                return false;
            }
            else
            {
                epError.Clear();
                return true;
            }
        }

        public string FromValue
        {
            get
            {
                return tbFrom.Text.Trim();
            }
            set
            {
                tbFrom.Text = value;
            }
        }

        public string ToValue
        {
            get
            {
                return tbTo.Text.Trim();
            }
            set
            {
                tbTo.Text = value;
            }
        }

        //очистка полей
        public void Clear()
        {
            tbFrom.Text = string.Empty;
            tbTo.Text = string.Empty;
        }
    }
}
