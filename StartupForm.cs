using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace FTLShipEdit
{
    public partial class StartupForm : Form
    {
        public StartupForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OptionsForm options = new OptionsForm();
            options.ShowDialog();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            SelectShipForm selectShipForm = new SelectShipForm("modeb");
            if (selectShipForm.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            return;
        }

        private void button4_Click(object sender, EventArgs e)
        {

            SelectShipForm selectShipForm = new SelectShipForm();
            if (selectShipForm.ShowDialog() != System.Windows.Forms.DialogResult.Cancel)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
                this.Close();
            }
            return;
        }

        private void StartupForm_Load(object sender, EventArgs e)
        {
            OptionsForm.dataPath = Properties.Settings1.Default.DataPath;
            OptionsForm.resPath = Properties.Settings1.Default.ResPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SelectShipForm selectShipForm = new SelectShipForm();
            selectShipForm.LoadBlueprints(OptionsForm.dataPath + "\\data\\blueprints.xml");
            selectShipForm.Dispose();
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void StartupForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.DialogResult != System.Windows.Forms.DialogResult.OK)
                this.DialogResult = System.Windows.Forms.DialogResult.Abort;
        }
    }
}
