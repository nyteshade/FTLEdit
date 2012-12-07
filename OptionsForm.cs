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
    public partial class OptionsForm : Form
    {
        public static string dataPath;
        public static string resPath;

        public OptionsForm()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://westerbaan.name/~bas/ftldat/ftldat-latest.exe");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            OpenFileDialog fileDia = new OpenFileDialog();
            fileDia.Title = "Please select the data.dat file to extract.";
            fileDia.Filter = "data.dat|*.dat";
            fileDia.DefaultExt = "*.dat";
            fileDia.ShowDialog();

            string dataPath = fileDia.FileName;

            fileDia.Title = "Please select the resource.dat file to extract.";
            fileDia.Filter = "resource.dat|*.dat";
            fileDia.ShowDialog();

            string resourcesPath = fileDia.FileName;

            string targetPath = Application.StartupPath;
            //System.Diagnostics.ProcessStartInfo procStartInfo = new System.Diagnostics.ProcessStartInfo("cmd", "/c " + "ftldat unpack \"" + dataPath + "\" \"" + targetPath + "\"\\data\\");

            System.Diagnostics.Process.Start("cmd", "/c " + "ftldat unpack \"" + dataPath + "\" \"" + targetPath + "\\data\"");
            System.Diagnostics.Process.Start("cmd", "/c " + "ftldat unpack \"" + resourcesPath + "\" \"" + targetPath + "\\resource\"");
            tbPathData.Text = targetPath + "\\data\\";
            dataPath = targetPath + "\\data\\";
            tbPathResources.Text = targetPath + "\\resource\\";
            resPath = targetPath + "\\resource\\";
            UpdatePaths();
            fileDia.Dispose();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDia = new FolderBrowserDialog();
            folderDia.Description = "Please select the folder where you have extracted data.dat to";
            folderDia.SelectedPath = Application.StartupPath;
            folderDia.ShowDialog();
            tbPathData.Text = folderDia.SelectedPath;
            dataPath = folderDia.SelectedPath;
            UpdatePaths();
            folderDia.Dispose();
        }

        private void OptionsForm_Load(object sender, EventArgs e)
        {
            tbPathData.Text = Properties.Settings1.Default.DataPath;
            tbPathResources.Text = Properties.Settings1.Default.ResPath;
            UpdatePaths();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDia = new FolderBrowserDialog();
            folderDia.Description = "Please select the folder where you have extracted resource.dat to";
            folderDia.SelectedPath = Application.StartupPath;
            folderDia.ShowDialog();
            tbPathResources.Text = folderDia.SelectedPath;
            resPath = folderDia.SelectedPath;
            UpdatePaths();
            folderDia.Dispose();
        }

        public void UpdatePaths()
        {

            dataPath = tbPathData.Text;
            resPath = tbPathResources.Text;
            Properties.Settings1.Default.DataPath = dataPath;
            Properties.Settings1.Default.ResPath = resPath;
            Properties.Settings1.Default.Save();

        }

        private void tbPathData_TextChanged(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start("http://www.ftlgame.com/forum/viewtopic.php?f=4&t=2788");
        }
    }
}
