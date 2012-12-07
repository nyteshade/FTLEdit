using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using HtmlAgilityPack;
using SFML.Graphics;
namespace FTLShipEdit
{
    public partial class SelectShipForm : Form
    {
        public SelectShipForm()
        {
            InitializeComponent();
        }

        class ShipInfo
        {
            public string id;
            public string name;

            public ShipInfo(string id, string name)
            {
                this.id = id;
                this.name = name;

            }

            public override string ToString()
            {
                return name.PadRight(18) + '\t' + "[" + id + "]";
            }
        }
        //OpenFileDialog openShipDia = new OpenFileDialog();
        public string lastPath;
        public void LoadShips(string file)
        {
            lastPath = file;
            /*
            openShipDia.DefaultExt = ".xml";
            openShipDia.Filter = "XML ship file (*.xml)|*.xml|All files|*.*";
            openShipDia.ShowDialog();
            */
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;

            // filePath is a path to a file containing the html
            //htmlDoc.Load(openShipDia.FileName);
            if (!System.IO.File.Exists(file))
            {
                DialogResult result1 = MessageBox.Show(file + " was not found! Please check your directory settings in the options!",
            "ERROR", MessageBoxButtons.OK);
                return;

            }

            htmlDoc.Load(file);
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
                HtmlNodeCollection weps = htmlDoc.DocumentNode.SelectNodes("//shipblueprint");// SelectSingleNode("//body");

                listBox1.Items.Clear();
                if (weps != null)
                {
                    // Do something with bodyNode
                    foreach (HtmlNode node in weps)
                    {
                        string shipID = node.GetAttributeValue("name", "#NOT FOUND~");
                        string shipName = "No name?";

                        if (node.SelectSingleNode(".//name") != null)
                            shipName = node.SelectSingleNode(".//name").InnerText;

                        listBox1.Items.Add(new ShipInfo(shipID, shipName));
                        /*WeaponBlueprints tempWep = new WeaponBlueprints();
                        tempWep.name = node.GetAttributeValue("name", "#NOT FOUND~");

                        if (tempWep.name == "#NOT FOUND~")
                            continue;


                        tempWep.type = node.SelectSingleNode(".//type").InnerText;//reader.ReadElementContentAsString("type", "");
                        tempWep.title = node.SelectSingleNode(".//title").InnerText;
                        if (node.SelectSingleNode(".//short") != null)
                            tempWep.shortname = node.SelectSingleNode(".//short").InnerText;

                        if (node.SelectSingleNode(".//desc") != null)
                            tempWep.desc = node.SelectSingleNode(".//desc").InnerText;
                        weapons.Add(tempWep);*/

                    }
                }
            }



        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void LoadBlueprints(string path)
        {

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;

            // filePath is a path to a file containing the html
            htmlDoc.Load(path);//OptionsForm.dataPath + "\\data\\blueprints.xml");

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
                        tempWep.name = node.GetAttributeValue("name", "#NOT FOUND~");

                        if (tempWep.name == "#NOT FOUND~")
                            continue;


                        tempWep.type = node.SelectSingleNode(".//type").InnerText;//reader.ReadElementContentAsString("type", "");
                        tempWep.title = node.SelectSingleNode(".//title").InnerText;
                        if (node.SelectSingleNode(".//short") != null)
                            tempWep.shortname = node.SelectSingleNode(".//short").InnerText;

                        if (node.SelectSingleNode(".//desc") != null)
                            tempWep.desc = node.SelectSingleNode(".//desc").InnerText;
                        Game.weapons.Add(tempWep);

                    }
                }
            }

            if (htmlDoc.DocumentNode != null)
            {
                HtmlNodeCollection augs = htmlDoc.DocumentNode.SelectNodes("//augblueprint");// SelectSingleNode("//body");

                if (augs != null)
                {
                    // Do something with bodyNode
                    foreach (HtmlNode node in augs)
                    {
                        AugmentBlueprint tempAug = new AugmentBlueprint();
                        tempAug.name = node.GetAttributeValue("name", "#NOT FOUND~");

                        if (tempAug.name == "#NOT FOUND~")
                            continue;

                        tempAug.title = node.SelectSingleNode(".//title").InnerText;

                        if (node.SelectSingleNode(".//desc") != null)
                            tempAug.desc = node.SelectSingleNode(".//desc").InnerText;
                        Game.augments.Add(tempAug);

                    }
                }
            }
            if (htmlDoc.DocumentNode != null)
            {
                HtmlNodeCollection drones = htmlDoc.DocumentNode.SelectNodes("//droneblueprint");// SelectSingleNode("//body");

                if (drones != null)
                {
                    // Do something with bodyNode
                    foreach (HtmlNode node in drones)
                    {
                        DroneBlueprint tempDrone = new DroneBlueprint();
                        tempDrone.name = node.GetAttributeValue("name", "#NOT FOUND~");

                        if (tempDrone.name == "#NOT FOUND~")
                            continue;


                        tempDrone.type = node.SelectSingleNode(".//type").InnerText;//reader.ReadElementContentAsString("type", "");
                        tempDrone.title = node.SelectSingleNode(".//title").InnerText;
                        if (node.SelectSingleNode(".//short") != null)
                            tempDrone.shortname = node.SelectSingleNode(".//short").InnerText;

                        if (node.SelectSingleNode(".//desc") != null)
                            tempDrone.desc = node.SelectSingleNode(".//desc").InnerText;
                        Game.drones.Add(tempDrone);

                    }
                }
            }

            foreach (AugmentBlueprint aug in Game.augments)
            {
                Game.optWnd.aug1.Items.Add(aug);
                Game.optWnd.aug2.Items.Add(aug);
                Game.optWnd.aug3.Items.Add(aug);
            }
            foreach (DroneBlueprint wep in Game.drones)
            {
                Game.optWnd.drone1.Items.Add(wep);
                Game.optWnd.drone2.Items.Add(wep);
                Game.optWnd.drone3.Items.Add(wep);
                Game.optWnd.drone4.Items.Add(wep);
            }
            foreach (WeaponBlueprints wep in Game.weapons)
            {
                Game.optWnd.wep1.Items.Add(wep);
                Game.optWnd.wep2.Items.Add(wep);
                Game.optWnd.wep3.Items.Add(wep);
                Game.optWnd.wep4.Items.Add(wep);
            }

        }
        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            LoadBlueprints(lastPath);

            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();

            // There are various options, set as needed
            htmlDoc.OptionFixNestedTags = true;

            // filePath is a path to a file containing the html
            htmlDoc.Load(lastPath);//OptionsForm.dataPath + "\\data\\blueprints.xml");

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
                HtmlNodeCollection weps = htmlDoc.DocumentNode.SelectNodes("//shipblueprint");// SelectSingleNode("//body");

                if (weps != null)
                {
                    // Do something with bodyNode
                    foreach (HtmlNode node in weps)
                    {
                        string shipID = node.GetAttributeValue("name", "#NOT FOUND~"); 
                        if (shipID == ((ShipInfo)listBox1.SelectedItem).id)
                        {
                            // This is our ship! Let's load it up~~
                            Ship ship = new Ship();
                            ship.id = shipID;
                            ship.img = node.GetAttributeValue("img", "");
                            ship.layout = node.GetAttributeValue("layout", "");
                            if (node.SelectSingleNode(".//class") != null)
                                ship.className = node.SelectSingleNode(".//class").InnerText;

                            if (node.SelectSingleNode(".//name") != null)
                                ship.name = node.SelectSingleNode(".//name").InnerText;
                            if (node.SelectSingleNode(".//desc") != null)
                                ship.desc = node.SelectSingleNode(".//desc").InnerText;

                            HtmlNode sytems = node.SelectSingleNode(".//systemlist");

                            List<string> rooms = new List<string>() { "pilot", "doors", "sensors", "oxygen", "engines", "shields", "weapons", "drones", "medbay", "teleporter", "cloaking" };


                            foreach (string currentRoom in rooms)
                            {
                                if ((sytems.SelectSingleNode(".//" + currentRoom)) != null)
                                {
                                    ship.rooms[currentRoom].power = Convert.ToInt32(sytems.SelectSingleNode(".//" + currentRoom).GetAttributeValue("power", "0"));
                                    ship.rooms[currentRoom].roomId = Convert.ToInt32(sytems.SelectSingleNode(".//" + currentRoom).GetAttributeValue("room", "0"));
                                    if (sytems.SelectSingleNode(".//" + currentRoom).GetAttributeValue("start", "false") == "true")
                                        ship.rooms[currentRoom].start = true;
                                    else
                                        ship.rooms[currentRoom].start = false;
                                }
                            }

                            if (node.SelectSingleNode(".//weaponslots") != null)
                                ship.weaponSlots = Convert.ToInt32(node.SelectSingleNode(".//weaponslots").InnerText);
                            if (node.SelectSingleNode(".//droneslots") != null)
                                ship.droneSlots = Convert.ToInt32(node.SelectSingleNode(".//droneslots").InnerText);

                            if (node.SelectSingleNode(".//health") != null)
                                ship.health = Convert.ToInt32(node.SelectSingleNode(".//health").GetAttributeValue("amount", "0"));
                            if (node.SelectSingleNode(".//maxpower") != null)
                                ship.maxPower = Convert.ToInt32(node.SelectSingleNode(".//maxpower").GetAttributeValue("amount", "0"));
                            HtmlNodeCollection crew = node.SelectNodes(".//crewcount");
                            foreach (HtmlNode crewmembers in crew)
                            {
                                switch (crewmembers.GetAttributeValue("class", "unk"))
                                {
                                    case "human":
                                        ship.crewHuman = Convert.ToInt32(node.SelectSingleNode(".//crewcount").GetAttributeValue("amount", "0"));
                                        break;
                                    case "rock":
                                        ship.crewRock = Convert.ToInt32(node.SelectSingleNode(".//crewcount").GetAttributeValue("amount", "0"));
                                        break;
                                    case "mantis":
                                        ship.crewMantis = Convert.ToInt32(node.SelectSingleNode(".//crewcount").GetAttributeValue("amount", "0"));
                                        break;
                                    case "engi":
                                        ship.crewEngi = Convert.ToInt32(node.SelectSingleNode(".//crewcount").GetAttributeValue("amount", "0"));
                                        break;
                                    case "slug":
                                        ship.crewSlug = Convert.ToInt32(node.SelectSingleNode(".//crewcount").GetAttributeValue("amount", "0"));
                                        break;
                                    case "energy":
                                        ship.crewZoltan = Convert.ToInt32(node.SelectSingleNode(".//crewcount").GetAttributeValue("amount", "0"));
                                        break;
                                    case "crystal":
                                        ship.crewCrystal = Convert.ToInt32(node.SelectSingleNode(".//crewcount").GetAttributeValue("amount", "0"));
                                        break;

                                }
                            }

                            // Load weapons
                            HtmlNode weaponList = node.SelectSingleNode(".//weaponlist");
                            if (weaponList != null)
                            {
                                int weapons = Convert.ToInt32(weaponList.GetAttributeValue("count", "0"));
                                int missiles = Convert.ToInt32(weaponList.GetAttributeValue("missiles", "0"));
                                HtmlNodeCollection actualWeapons = weaponList.SelectNodes(".//weapon");
                                ship.missiles = missiles;
                                
                                if (actualWeapons != null)
                                {
                                    for (int i = 0; i < actualWeapons.Count; i++)
                                    {
                                        string wepname = actualWeapons[i].GetAttributeValue("name", "");
                                        ship.weapons[i] = FindWep(wepname);
                                       
                                    }
                                }
                            }



                            // Load drones
                            HtmlNode droneList = node.SelectSingleNode(".//dronelist");
                            if (droneList != null)
                            {
                                int drones = Convert.ToInt32(droneList.GetAttributeValue("count", "0"));
                                int droneParts = Convert.ToInt32(droneList.GetAttributeValue("drones", "0"));
                                HtmlNodeCollection actualDrones = droneList.SelectNodes(".//drone");
                                ship.droneParts = droneParts;

                                if (actualDrones != null)
                                {
                                    for (int i = 0; i < actualDrones.Count; i++)
                                    {
                                        string dronename = actualDrones[i].GetAttributeValue("name", "");
                                        ship.drones[i] = FindDrone(dronename);

                                    }
                                }
                            }

                            // Load augments
                            HtmlNodeCollection augments = node.SelectNodes(".//aug");
                            if (augments != null)
                            {
                                for (int i = 0; i < augments.Count; i++)
                                {
                                    string augname = augments[i].GetAttributeValue("name", "");
                                    ship.augments[i] = FindAug(augname);

                                }
                            }

                            Game.ship = ship;
                            Game.optWnd.UpdateScreen();

                            // Load layout
                            Game.game.ImportShip(ship.layout,ship.img);

                            this.DialogResult = System.Windows.Forms.DialogResult.OK;
                            this.Close();
                            return;
                        }


                    }
                    /*
                    string shipName = "No name?";

                    if (node.SelectSingleNode(".//name") != null)
                        shipName = node.SelectSingleNode(".//name").InnerText;

                    listBox1.Items.Add(new ShipInfo(shipID, shipName));*/
                    /*WeaponBlueprints tempWep = new WeaponBlueprints();
                    tempWep.name = node.GetAttributeValue("name", "#NOT FOUND~");

                    if (tempWep.name == "#NOT FOUND~")
                        continue;


                    tempWep.type = node.SelectSingleNode(".//type").InnerText;//reader.ReadElementContentAsString("type", "");
                    tempWep.title = node.SelectSingleNode(".//title").InnerText;
                    if (node.SelectSingleNode(".//short") != null)
                        tempWep.shortname = node.SelectSingleNode(".//short").InnerText;

                    if (node.SelectSingleNode(".//desc") != null)
                        tempWep.desc = node.SelectSingleNode(".//desc").InnerText;
                    weapons.Add(tempWep);*/

                }
            }
        }
        WeaponBlueprints FindWep(string id)
        {
            foreach (WeaponBlueprints wepprint in Game.weapons)
            {
                if (wepprint.name == id)
                    return wepprint;

            }
            return new WeaponBlueprints();
        }

        DroneBlueprint FindDrone(string id)
        {
            foreach (DroneBlueprint droneprint in Game.drones)
            {
                if (droneprint.name == id)
                    return droneprint;

            }
            return new DroneBlueprint();
        }

        AugmentBlueprint FindAug(string id)
        {
            foreach (AugmentBlueprint augprint in Game.augments)
            {
                if (augprint.name == id)
                    return augprint;

            }
            return new AugmentBlueprint();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            LoadShips(OptionsForm.dataPath + "\\data\\blueprints.xml");
        }

        private void button1_Click(object sender, EventArgs e)
        {

            LoadShips(OptionsForm.dataPath + "\\data\\blueprints.xml");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadShips(OptionsForm.dataPath + "\\data\\autoBlueprints.xml");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            OpenFileDialog openShipDia = new OpenFileDialog();
            openShipDia.DefaultExt = ".xml";
            openShipDia.Filter = "XML ship file (*.xml)|*.xml|All files|*.*";
            
            openShipDia.ShowDialog();

            LoadShips(openShipDia.FileName);
            openShipDia.Dispose();
   
        }
    }
}

