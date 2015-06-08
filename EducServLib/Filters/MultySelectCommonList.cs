using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EducServLib
{
    public delegate void OkButtonHandler(IList<string> lst);
    public partial class MultySelectCommonList : Form
    {
        /// <summary>
        /// Событие по нажатию кнопки
        /// </summary>
        public event OkButtonHandler OnOk;

        public IList<string> SelectedList
        {
            get { return MSL.SelectedList; }
            set { MSL.SelectedList = value; }
        }

        /// <summary>
        /// Создаёт экземпляр списка множественного выбора. TypeToSelect - что именно будет написано, например, "Выберите факультет"
        /// </summary>
        /// <param name="TypeToSelect"></param>
        /// <param name="source"></param>
        public MultySelectCommonList(Form parent, string TypeToSelect, Dictionary<string, string> source)
        {
            this.MdiParent = parent;
            this.TopMost = true;
            InitializeComponent();
            label1.Text = "Выберите " + TypeToSelect;
            MSL.InitSource(source);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            IList<string> lst = MSL.SelectedList;
            if (OnOk != null)
                OnOk(lst);

            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
