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
    public partial class FilterBool : UserControl
    {
        public FilterBool()
        {
            InitializeComponent();
        }

        private void rbYEs_CheckedChanged(object sender, EventArgs e)
        {
            if (rbYEs.Checked)
                rbNo.Checked = false;
        }

        private void rbNo_CheckedChanged(object sender, EventArgs e)
        {
            if (rbNo.Checked)
                rbYEs.Checked = false;
        }

        //свойство доступа к значению
        public bool Value
        {
            get
            {
                if (rbYEs.Checked)
                    return true;
                else
                    return false;
            }
            set
            {
                if (value)
                    rbYEs.Checked = true;
                else
                    rbNo.Checked = true;
            }
        }

        //выставить значения по умолчанию
        public void Clear()
        {
            rbYEs.Checked = true;
        }
    }
}
