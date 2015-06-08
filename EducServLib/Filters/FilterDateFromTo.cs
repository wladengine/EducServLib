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
    public partial class FilterDateFromTo : UserControl
    {
        public FilterDateFromTo()
        {
            InitializeComponent();
        }

        //проверка на непустоту
        public bool CheckValues()
        {
            if (! dtpFrom.Checked && ! dtpTo.Checked)
            {
                epError.SetError(this.dtpFrom, "Оба поля пусты.");
                return false;
            }
            else
            {
                epError.Clear();
                return true;
            }
        }

        public DateTime FromValue
        {
            get
            {
                if (dtpFrom.Checked)
                    return dtpFrom.Value.Date;
                else
                    return DateTime.MaxValue;
            }
            set
            {
                if (value != DateTime.MaxValue)
                {
                    dtpFrom.Value = value;
                    dtpFrom.Checked = true;
                }
            }
        }

        public DateTime ToValue
        {
            get
            {
                if (dtpTo.Checked)
                    return dtpTo.Value.Date;
                else
                    return DateTime.MaxValue;
            }
            set
            {
                if (value != DateTime.MaxValue)
                {
                    dtpTo.Value = value;
                    dtpTo.Checked = true;
                }
            }
        }

        //очистка полей
        public void Clear()
        {
            dtpFrom.Value = DateTime.Now;
            dtpTo.Value = DateTime.Now;
            dtpFrom.Checked = false;
            dtpTo.Checked = false;
        }
    }
}
