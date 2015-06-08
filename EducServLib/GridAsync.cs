using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace EducServLib
{
    public delegate void UpdateHandler();
    public delegate void UpdateListHandler();
    public static class GridAsync
    {
        public static void UpdateGridAsync(DataGridView dgv, DoWorkEventHandler hndlr, UpdateHandler updateAfter)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += hndlr;
            bw.RunWorkerCompleted += (sender, args) =>
            {
                dgv.DataSource = args.Result;
                if (updateAfter != null)
                    updateAfter();
            };

            dgv.DataSource = null;
            try
            {
                bw.RunWorkerAsync();
            }
            catch (Exception ex)
            {
                WinFormsServ.Error(ex);
            }
        }
        public static void UpdateGridAsync(DataGridView dgv, DoWorkEventHandler hndlr, UpdateHandler updateAfter, object prm)
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += hndlr;
            bw.RunWorkerCompleted += (sender, args) =>
            {
                dgv.DataSource = args.Result;
                if (updateAfter != null)
                    updateAfter();
            };

            dgv.DataSource = null;
            try
            {
                bw.RunWorkerAsync(prm);
            }
            catch (Exception ex)
            {
                WinFormsServ.Error(ex);
            }
        }
    }
}
