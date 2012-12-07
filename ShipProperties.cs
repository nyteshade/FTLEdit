using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
using HtmlAgilityPack;
using Ionic;
namespace FTLShipEdit
{
    public partial class ShipProperties : Form
    {

        public ShipProperties()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            OpenFileDialog openShipDia = new OpenFileDialog();
            openShipDia.DefaultExt = ".xml";
            openShipDia.Filter = "XML ship file (*.xml)|*.xml|All files|*.*";
            openShipDia.ShowDialog();

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;
            
            // filePath is a path to a file containing the html
            htmlDoc.Load(openShipDia.FileName);

            // Use:  htmlDoc.LoadXML(xmlString);  to load from a string

            // ParseErrors is an ArrayList containing any errors from the Load statement
            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required
                foreach (HtmlParseError error in htmlDoc.ParseErrors)
                {
                    Console.WriteLine(error.ToString());
                }
            }

                if (htmlDoc.DocumentNode != null)
                {
                    HtmlNodeCollection weps = htmlDoc.DocumentNode.SelectNodes("//weaponblueprint");// SelectSingleNode("//body");

                    if (weps != null)
                    {
                        // Do something with bodyNode
                        foreach (HtmlNode node in weps)
                        {
                            WeaponBlueprints tempWep = new WeaponBlueprints();
                            tempWep.name = node.GetAttributeValue("name","#NOT FOUND~");

                            if (tempWep.name == "#NOT FOUND~")
                                continue;


                            tempWep.type = node.SelectSingleNode(".//type").InnerText;//reader.ReadElementContentAsString("type", "");
                            tempWep.title = node.SelectSingleNode(".//title").InnerText;
                            if( node.SelectSingleNode(".//short") != null)
                                tempWep.shortname = node.SelectSingleNode(".//short").InnerText;

                            if (node.SelectSingleNode(".//desc") != null)
                                tempWep.desc = node.SelectSingleNode(".//desc").InnerText;
                            Game.weapons.Add(tempWep);

                        }
                    }
                }


                foreach (WeaponBlueprints wep in Game.weapons)
                {
                    wep1.Items.Add(wep);
                    wep2.Items.Add(wep);
                    wep3.Items.Add(wep);
                    wep4.Items.Add(wep);
                }

            /*
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.ValidationType = ValidationType.None;
            settings.ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags.None;
            
            settings.IgnoreComments = true;
            settings.IgnoreWhitespace = true;
            string xmlFilePath = openShipDia.FileName;
            var lines = new List<string>();

            using (var fileStream = File.Open(xmlFilePath, FileMode.Open, FileAccess.Read))
            {

                using (var reader = new StreamReader(fileStream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                        lines.Add(line);
                }

                var i = lines.FindIndex(s => s.StartsWith("<?xml"));
                var xmlLine = lines[i];
                lines.RemoveAt(i);
                lines.Insert(0, xmlLine);
                lines.Insert(1, "<data>");
                lines.Insert(lines.Count, "</data>");

                for(int s=0; s < lines.Count; s++)
                {
                    lines[s] = lines[s].Replace("<title>", "<type>");
                    lines[s] = lines[s].Replace("</title>", "</type>");
                    lines[s] = lines[s].Replace("</title>", "</type>");
                    lines[s] = lines[s].Replace("-->", "#-[]X>");
                    lines[s] = lines[s].Replace("--", "");
                    lines[s] = lines[s].Replace("<!", "<!--");
                    lines[s] = lines[s].Replace("#-[]X>","-->");
                }
            }

            
            //using (var fileStream = File.Open(xmlFilePath, FileMode.Truncate, FileAccess.Write))
            //{

            MemoryStream fileStreamy = new MemoryStream();
                var writer = new StreamWriter(fileStreamy);
               // {
                    foreach (var line in lines)
                        writer.Write(line);

                    writer.Flush();
               // }
            //}
                    fileStreamy.Position = 0;

                    XmlDocument doc = new XmlDocument();
                    doc.Load(fileStreamy);
                    var test = doc.GetElementsByTagName("weaponBlueprint");

                    test = test;

            /*


                using (XmlReader reader = XmlReader.Create(fileStreamy, settings))
            {
                while (reader.Read())
                {
                    while (reader.MoveToElement("weaponBlueprint"))
                    {
                        WeaponBlueprints tempWep = new WeaponBlueprints();
                        tempWep.name = (reader.GetAttribute("name"));
                        tempWep.type = reader.ReadElementContentAsString("type", "");
                        tempWep.title = reader.ReadElementContentAsString("title", "");
                        tempWep.shortname = reader.ReadElementContentAsString("short", "");
                        tempWep.desc = reader.ReadElementContentAsString("desc", "");
                        weapons.Add(tempWep);
                    }


                }
            }*/

            
            
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void Options_Load(object sender, EventArgs e)
        {

        }
        /*
            roomFuncs.Add(new RoomFunc("pilot", new Texture("img/GUI/roomfuncs/s_pilot_overlay.png"), null));
            roomFuncs.Add(new RoomFunc("doors", new Texture("img/GUI/roomfuncs/s_doors_overlay.png"), null));
            roomFuncs.Add(new RoomFunc("sensors", new Texture("img/GUI/roomfuncs/s_sensors_overlay.png"), null));
            roomFuncs.Add(new RoomFunc("oxygen", new Texture("img/GUI/roomfuncs/s_oxygen_overlay.png"), null));
            roomFuncs.Add(new RoomFunc("engines", new Texture("img/GUI/roomfuncs/s_engines_overlay.png"), null));
            roomFuncs.Add(new RoomFunc("shields", new Texture("img/GUI/roomfuncs/s_shields_overlay.png"), null));
            roomFuncs.Add(new RoomFunc("weapons", new Texture("img/GUI/roomfuncs/s_weapons_overlay.png"), null));
            roomFuncs.Add(new RoomFunc("drones", new Texture("img/GUI/roomfuncs/s_drones_overlay.png"), null));
            roomFuncs.Add(new RoomFunc("medbay", new Texture("img/GUI/roomfuncs/s_medbay_overlay.png"), null));
            roomFuncs.Add(new RoomFunc("teleporter", new Texture("img/GUI/roomfuncs/s_teleporter_overlay.png"), null));
            roomFuncs.Add(new RoomFunc("cloaking", new Texture("img/GUI/roomfuncs/s_cloaking_overlay.png"), null));
        */
        public void UpdateScreen()
        {
            tbClass.Text = Game.ship.className;
            tbDesc.Text = Game.ship.desc;
            tbDrones.Text = Game.ship.droneSlots.ToString();
            tbShipID.Text = Game.ship.id;
            tbShipImg.Text = Game.ship.img;
            tbShipLayout.Text = Game.ship.layout;


            crewCrystal.Text = Game.ship.crewCrystal.ToString();
            crewHuman.Text = Game.ship.crewHuman.ToString();
            crewRock.Text = Game.ship.crewRock.ToString();
            crewEngi.Text = Game.ship.crewEngi.ToString();
            crewZoltan.Text = Game.ship.crewZoltan.ToString();
            crewSlug.Text = Game.ship.crewSlug.ToString();
            crewMantis.Text = Game.ship.crewMantis.ToString();
            

            //tbMissiles.Text = Game.ship.missle
            tbName.Text = Game.ship.name;
            tbMissiles.Text = Game.ship.missiles.ToString();
            tbDroneParts.Text = Game.ship.droneParts.ToString();
            tbWeps.Text = Game.ship.weaponSlots.ToString();
            sysCloakAct.Checked = Game.ship.rooms["cloaking"].start;
            sysCloakPwr.Text = Game.ship.rooms["cloaking"].power.ToString();
            sysDoorAct.Checked = Game.ship.rooms["doors"].start;
            sysDoorPwr.Text = Game.ship.rooms["doors"].power.ToString();
            sysDroneAct.Checked = Game.ship.rooms["drones"].start;
            sysDronePwr.Text = Game.ship.rooms["drones"].power.ToString();
            SysEngAct.Checked = Game.ship.rooms["engines"].start;
            sysEngPwr.Text = Game.ship.rooms["engines"].power.ToString();
            sysMedbayAct.Checked = Game.ship.rooms["medbay"].start;
            sysMedbayPwr.Text = Game.ship.rooms["medbay"].power.ToString();
            sysOxyAct.Checked = Game.ship.rooms["oxygen"].start;
            sysOxyPwr.Text = Game.ship.rooms["oxygen"].power.ToString();
            sysPilotAct.Checked = Game.ship.rooms["pilot"].start;
            sysPilotPwr.Text = Game.ship.rooms["pilot"].power.ToString();
            sysSensAct.Checked = Game.ship.rooms["sensors"].start;
            sysSensPwr.Text = Game.ship.rooms["sensors"].power.ToString();
            sysShieldAct.Checked = Game.ship.rooms["shields"].start;
            sysShieldPwr.Text = Game.ship.rooms["shields"].power.ToString();
            sysTeleAct.Checked = Game.ship.rooms["teleporter"].start;
            sysTelePwr.Text = Game.ship.rooms["teleporter"].power.ToString();
            sysWepAct.Checked = Game.ship.rooms["weapons"].start;
            sysWepPwr.Text = Game.ship.rooms["weapons"].power.ToString();
            if (Game.ship.weapons[0].name != null)
                Game.optWnd.wep1.SelectedItem = Game.ship.weapons[0];
            else
                Game.optWnd.wep1.Text = "";
            if (Game.ship.weapons[1].name != null)
                Game.optWnd.wep2.SelectedItem = Game.ship.weapons[1];
            else
                Game.optWnd.wep2.Text = "";
            if (Game.ship.weapons[2].name != null)
                Game.optWnd.wep3.SelectedItem = Game.ship.weapons[2];
            else
                Game.optWnd.wep3.Text = "";
            if (Game.ship.weapons[3].name != null)
                Game.optWnd.wep4.SelectedItem = Game.ship.weapons[3];
            else
                Game.optWnd.wep4.Text = "";

            if (Game.ship.drones[0].name != null)
                Game.optWnd.drone1.SelectedItem = Game.ship.drones[0];
            else
                Game.optWnd.drone1.Text = "";
            if (Game.ship.drones[1].name != null)
                Game.optWnd.drone2.SelectedItem = Game.ship.drones[1];
            else
                Game.optWnd.drone2.Text = "";
            if (Game.ship.drones[2].name != null)
                Game.optWnd.drone3.SelectedItem = Game.ship.drones[2];
            else
                Game.optWnd.drone3.Text = "";
            if (Game.ship.drones[3].name != null)
                Game.optWnd.drone4.SelectedItem = Game.ship.drones[3];
            else
                Game.optWnd.drone4.Text = "";

            if (Game.ship.augments[0].name != null)
                Game.optWnd.aug1.SelectedItem = Game.ship.augments[0];
            else
                Game.optWnd.aug1.Text = "";
            if (Game.ship.augments[1].name != null)
                Game.optWnd.aug2.SelectedItem = Game.ship.augments[1];
            else
                Game.optWnd.aug2.Text = "";
            if (Game.ship.augments[2].name != null)
                Game.optWnd.aug3.SelectedItem = Game.ship.augments[2];
            else
                Game.optWnd.aug3.Text = "";
            //Game.optWnd.wep2.SelectedItem = Game.ship.weapons[1];
            //Game.optWnd.wep3.SelectedItem = Game.ship.weapons[2];
            //Game.optWnd.wep4.SelectedItem = Game.ship.weapons[3];
  
            //.Checked = Game.ship.rooms["cloaking"].start;

        }


        private void tbClass_TextChanged(object sender, EventArgs e)
        {
            Game.ship.className = tbClass.Text;
        }

        private void tbName_TextChanged(object sender, EventArgs e)
        {
            Game.ship.name = tbName.Text;
        }

        private void tbDesc_TextChanged(object sender, EventArgs e)
        {
            Game.ship.desc = tbDesc.Text;
        }

        private void tbWeps_TextChanged(object sender, EventArgs e)
        {
            Game.ship.weaponSlots = Convert.ToInt32(tbWeps.Text);
        }

        private void tbMissiles_TextChanged(object sender, EventArgs e)
        {
            Game.ship.missiles = Convert.ToInt32(tbMissiles.Text);
        }

        private void tbDrones_TextChanged(object sender, EventArgs e)
        {
            Game.ship.droneSlots = Convert.ToInt32(tbDrones.Text);
        }

        private void wep1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game.ship.weapons[0] = (WeaponBlueprints)wep1.SelectedItem;
        }

        private void wep2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game.ship.weapons[1] = (WeaponBlueprints)wep2.SelectedItem;
        }

        private void wep3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game.ship.weapons[2] = (WeaponBlueprints)wep3.SelectedItem;
        }

        private void wep4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game.ship.weapons[3] = (WeaponBlueprints)wep4.SelectedItem;
        }

        private void sysPilotAct_CheckedChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["pilot"].start = sysPilotAct.Checked;
        }

        private void sysDoorAct_CheckedChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["doors"].start = sysDoorAct.Checked;
        }

        private void sysSensAct_CheckedChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["sensors"].start = sysSensAct.Checked;
        }

        private void sysOxyAct_CheckedChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["oxygen"].start = sysOxyAct.Checked;
        }

        private void SysEngAct_CheckedChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["engines"].start = SysEngAct.Checked;
        }

        private void sysShieldAct_CheckedChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["shields"].start = sysShieldAct.Checked;
        }

        private void sysWepAct_CheckedChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["weapons"].start = sysWepAct.Checked;
        }

        private void sysDroneAct_CheckedChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["drones"].start = sysDroneAct.Checked;
        }

        private void sysMedbayAct_CheckedChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["medbay"].start = sysMedbayAct.Checked;
        }

        private void sysTeleAct_CheckedChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["teleporter"].start = sysTeleAct.Checked;
        }

        private void sysCloakAct_CheckedChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["cloaking"].start = sysCloakAct.Checked;
        }

        private void sysPilotPwr_TextChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["pilot"].power = Convert.ToInt32(sysPilotPwr.Text);
        }

        private void sysDoorPwr_TextChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["doors"].power = Convert.ToInt32(sysDoorPwr.Text);
        }

        private void sysSensPwr_TextChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["sensors"].power = Convert.ToInt32(sysSensPwr.Text);
        }

        private void sysOxyPwr_TextChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["oxygen"].power = Convert.ToInt32(sysOxyPwr.Text);
        }

        private void sysEngPwr_TextChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["engines"].power = Convert.ToInt32(sysEngPwr.Text);
        }

        private void sysShieldPwr_TextChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["shields"].power = Convert.ToInt32(sysShieldPwr.Text);
        }

        private void sysWepPwr_TextChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["weapons"].power = Convert.ToInt32(sysWepPwr.Text);
        }

        private void sysDronePwr_TextChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["drones"].power = Convert.ToInt32(sysDronePwr.Text);
        }

        private void sysMedbayPwr_TextChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["medbay"].power = Convert.ToInt32(sysMedbayPwr.Text);
        }

        private void sysTelePwr_TextChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["teleporter"].power = Convert.ToInt32(sysTelePwr.Text);
        }

        private void sysCloakPwr_TextChanged(object sender, EventArgs e)
        {
            Game.ship.rooms["cloaking"].power = Convert.ToInt32(sysCloakPwr.Text);
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void droneParts_TextChanged(object sender, EventArgs e)
        {
            Game.ship.droneParts = Convert.ToInt32(tbDroneParts.Text);
        }

        private void tbShipID_TextChanged(object sender, EventArgs e)
        {
            Game.ship.id = tbShipID.Text;
        }

        private void tbShipLayout_TextChanged(object sender, EventArgs e)
        {
            Game.ship.layout = tbShipLayout.Text;
        }

        private void tbShipImg_TextChanged(object sender, EventArgs e)
        {
            Game.ship.img = tbShipImg.Text;
        }

        private void drone1_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game.ship.drones[0] = (DroneBlueprint)drone1.SelectedItem;
        }

        private void drone2_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game.ship.drones[1] = (DroneBlueprint)drone2.SelectedItem;
        }

        private void drone3_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game.ship.drones[2] = (DroneBlueprint)drone3.SelectedItem;
        }

        private void drone4_SelectedIndexChanged(object sender, EventArgs e)
        {
            Game.ship.drones[3] = (DroneBlueprint)drone4.SelectedItem;
        }

        private void aug1_SelectedIndexChanged(object sender, EventArgs e)
        {

            Game.ship.augments[0] = (AugmentBlueprint)aug1.SelectedItem;
        }

        private void aug2_SelectedIndexChanged(object sender, EventArgs e)
        {

            Game.ship.augments[1] = (AugmentBlueprint)aug2.SelectedItem;
        }

        private void aug3_SelectedIndexChanged(object sender, EventArgs e)
        {

            Game.ship.augments[2] = (AugmentBlueprint)aug3.SelectedItem;
        }

        private void crewHuman_TextChanged(object sender, EventArgs e)
        {
            Game.ship.crewHuman = Convert.ToInt32(crewHuman.Text);
        }

        private void crewEngi_TextChanged(object sender, EventArgs e)
        {
            Game.ship.crewEngi = Convert.ToInt32(crewEngi.Text);

        }

        private void crewMantis_TextChanged(object sender, EventArgs e)
        {
            Game.ship.crewMantis = Convert.ToInt32(crewMantis.Text);
        }

        private void crewRock_TextChanged(object sender, EventArgs e)
        {
            Game.ship.crewRock = Convert.ToInt32(crewRock.Text);
        }

        private void crewZoltan_TextChanged(object sender, EventArgs e)
        {
            Game.ship.crewZoltan = Convert.ToInt32(crewZoltan.Text);
        }

        private void crewSlug_TextChanged(object sender, EventArgs e)
        {
            Game.ship.crewSlug = Convert.ToInt32(crewSlug.Text);
        }

        private void crewCrystal_TextChanged(object sender, EventArgs e)
        {
            Game.ship.crewCrystal = Convert.ToInt32(crewCrystal.Text);
        }

        private void ShipProperties_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void ShipProperties_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            ExportForm export = new ExportForm();
            export.ShowDialog();
        }
    }
}
