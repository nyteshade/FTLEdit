using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
namespace FTLShipEdit
{
    public partial class ExportForm : Form
    {
        public ExportForm()
        {
            InitializeComponent();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            System.Diagnostics.Process.Start("http://www.ftlgame.com/forum/viewtopic.php?f=12&t=2464");
        }


        private void button4_Click(object sender, EventArgs e)
        {

            SaveFileDialog dia = new SaveFileDialog();
            dia.Filter = "Layout XML file (*.xml)|*.xml";
            dia.DefaultExt = ".xml";
            dia.FileName = "shipname.xml";
            dia.Title = "Save blueprint xml as...";
            dia.ShowDialog();

            Game.game.ExportBlueprintXML(dia.FileName, append: false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Game.ship.id == null)
            {
                throw new InvalidDataException("**** Error! ****\n You have not set a ship ID! Please set one before exporting!!");
            }
            string outputFolder = Path.Combine(Application.StartupPath, Game.ship.id);
            Directory.CreateDirectory(outputFolder);
            Directory.CreateDirectory(Path.Combine(outputFolder, "img"));
            Directory.CreateDirectory(Path.Combine(outputFolder, "img\\ship"));
            Directory.CreateDirectory(Path.Combine(outputFolder, "data"));

            Game.game.ExportLayoutTxt(Path.Combine(outputFolder, "data\\" + Game.ship.layout + ".txt"));
            Game.game.ExportBlueprintXML(Path.Combine(outputFolder, "data\\" + "blueprints.xml.append"), append: true);

            Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile(Path.Combine(outputFolder, Game.ship.name + string.Format("-{0:yyyy-MM-dd_hh-mm-ss}", DateTime.Now) + ".ftl"));
            if (Directory.Exists(Path.Combine(outputFolder, "img")))
                zip.AddDirectory(Path.Combine(outputFolder, "img"), "\\img");
            if (Directory.Exists(Path.Combine(outputFolder, "data")))
                zip.AddDirectory(Path.Combine(outputFolder, "data"), "\\data");

            if (Directory.Exists(Path.Combine(outputFolder, "audio")))
                zip.AddDirectory(Path.Combine(outputFolder, "audio"), "\\audio");

            zip.Save();

            System.Diagnostics.Process.Start(outputFolder);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            SaveFileDialog dia = new SaveFileDialog();
            dia.Filter = "Layout txt file (*.txt)|*.txt";
            dia.DefaultExt = ".txt";
            dia.Title = "Save layout txt as...";
            dia.ShowDialog();

            Game.game.ExportLayoutTxt(dia.FileName);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            GC.Collect();
            Game.game.ExportShip();

            SaveFileDialog dia = new SaveFileDialog();
            dia.Filter = "Blueprint XML file (*.xml)|*.xml";
            dia.DefaultExt = ".xml";
            dia.Title = "Save blueprint xml as...";
            dia.ShowDialog();

            Game.game.ExportBlueprintXML(dia.FileName, append: false);
        }



    }
}
