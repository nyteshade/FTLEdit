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
            // Would you like to load a custom blueprints.xml, to load weapons etc from? (Not your ship)

            //DialogResult result1 = MessageBox.Show("Would you like to load a custom blueprints.xml to load weapons, etc from, or load the default one?",
           // "Load custom blueprints.xml",MessageBoxButtons.YesNo);
            SelectShipForm selectShipForm = new SelectShipForm();

            // Select a layout txt


            OpenFileDialog openShipDia = new OpenFileDialog();


            openShipDia = new OpenFileDialog();
            openShipDia.DefaultExt = ".txt";
            openShipDia.Filter = "Text files (*.txt)|*.txt|All files|*.*";
            openShipDia.ShowDialog();

            Game.game.ImportLayout(openShipDia.FileName);

            openShipDia.DefaultExt = ".xml";
            openShipDia.Filter = "XML ship file (*.xml)|*.xml|All files|*.*";
            openShipDia.ShowDialog();
            Game.game.ImportXML(openShipDia.FileName);

            /*if (result1 == System.Windows.Forms.DialogResult.Yes)
            {
                // Open file dialog

            }
            else
            {
                // Load default blueprints file
                //Game.game.ImportShip
                //selectShipForm.LoadShips(OptionsForm.dataPath + "\\data\\blueprints.xml");
            }*/

            // Please select the xml file with your ship in
            /*
            OpenFileDialog openShipDia = new OpenFileDialog();
            openShipDia.DefaultExt = ".xml";
            openShipDia.Filter = "XML ship file (*.xml)|*.xml|All files|*.*";

            openShipDia.ShowDialog();
            selectShipForm.LoadShips(openShipDia.FileName);
            selectShipForm.lastPath = OptionsForm.dataPath + "\\data\\blueprints.xml";
            openShipDia.Dispose();


            FolderBrowserDialog selectDataFolder = new FolderBrowserDialog();
            selectDataFolder.Description = "Please select the data folder for the custom ship";
            selectDataFolder.ShowDialog();
            OptionsForm.dataPath = selectDataFolder.SelectedPath;
            selectDataFolder.Description = "Please select the resources folder for the custom ship";
            selectDataFolder.ShowDialog();
            OptionsForm.resPath = selectDataFolder.SelectedPath;
            selectShipForm.Show();
             * */
            /*
            OpenFileDialog openShipDia = new OpenFileDialog();
            openShipDia.DefaultExt = ".xml";
            openShipDia.Filter = "XML ship file (*.xml)|*.xml|All files|*.*";
            openShipDia.ShowDialog();

            
            openShipDia = new OpenFileDialog();
            openShipDia.DefaultExt = ".txt";
            openShipDia.Filter = "Text files (*.txt)|*.txt|All files|*.*";
            openShipDia.ShowDialog();
            */
            // Please select your ship

            // 

            

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
