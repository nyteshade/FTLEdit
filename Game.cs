using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
//using System.Drawing;
using System.Windows.Forms;
using SFML;
using SFML.Graphics;
using SFML.Window;
//using Gwen;
namespace FTLShipEdit
{

    public class Gibs
    {
        Texture img;
        Vector2f pos;
    }
    public class GuiButton
    {


        public Vector2f pos;
        public Vector2u size;
        public string text;
        public Texture button;
        public Texture button_pressed;

        public GuiButton(Vector2f pos, Texture button, Texture button_pressed)
        {

            this.pos = pos;
            this.size = button.Size;
            this.button = button;
            this.button_pressed = button_pressed;
        }
        public GuiButton(Vector2f pos, Texture button)
            : this(pos, button, null)
        {
        }

        public void Draw(RenderTarget t, bool pressed)
        {
            // todo
            Sprite tempSprite;
            if (pressed)
                tempSprite = new Sprite(button_pressed);
            else
                tempSprite = new Sprite(button);

            tempSprite.Position = pos;
            t.Draw(tempSprite);
            tempSprite.Dispose();
        }
        public bool WasClicked(Vector2i mousePos)
        {
            return (mousePos.X > pos.X && mousePos.X < pos.X + size.X && mousePos.Y > pos.Y && mousePos.Y < pos.Y + size.Y);
        }


    }


    public class Ship
    {
        public Ship()
        {
            rooms = new Dictionary<string, RoomFunc>();
            rooms.Add("pilot", new RoomFunc("pilot", new Texture("img/GUI/roomfuncs/s_pilot_overlay.png"), null));
            rooms.Add("doors", new RoomFunc("doors", new Texture("img/GUI/roomfuncs/s_doors_overlay.png"), null));
            rooms.Add("sensors", new RoomFunc("sensors", new Texture("img/GUI/roomfuncs/s_sensors_overlay.png"), null));
            rooms.Add("oxygen", new RoomFunc("oxygen", new Texture("img/GUI/roomfuncs/s_oxygen_overlay.png"), null));
            rooms.Add("engines", new RoomFunc("engines", new Texture("img/GUI/roomfuncs/s_engines_overlay.png"), null));
            rooms.Add("shields", new RoomFunc("shields", new Texture("img/GUI/roomfuncs/s_shields_overlay.png"), null));
            rooms.Add("weapons", new RoomFunc("weapons", new Texture("img/GUI/roomfuncs/s_weapons_overlay.png"), null));
            rooms.Add("drones", new RoomFunc("drones", new Texture("img/GUI/roomfuncs/s_drones_overlay.png"), null));
            rooms.Add("medbay", new RoomFunc("medbay", new Texture("img/GUI/roomfuncs/s_medbay_overlay.png"), null));
            rooms.Add("teleporter", new RoomFunc("teleporter", new Texture("img/GUI/roomfuncs/s_teleporter_overlay.png"), null));
            rooms.Add("cloaking", new RoomFunc("cloaking", new Texture("img/GUI/roomfuncs/s_cloaking_overlay.png"), null));
            weapons = new WeaponBlueprints[4];
            drones = new DroneBlueprint[4];
            augments = new AugmentBlueprint[4];
        }


        public string id;
        public string img;
        public string layout;
        public string name;
        public string className;
        public string desc;
        public string unlock;
        public int weaponSlots;
        public int droneSlots;
        public int health;
        public int maxPower;
        public int missiles;
        public int droneParts;
        //public int crewCount;
        public int crewHuman;
        public int crewEngi;
        public int crewMantis;
        public int crewRock;
        public int crewZoltan;
        public int crewSlug;
        public int crewCrystal;
        public AugmentBlueprint[] augments;
        public WeaponBlueprints[] weapons;
        public DroneBlueprint[] drones;
        //public List<RoomFunc> rooms;
        public Dictionary<string, RoomFunc> rooms;
    }


    public struct DroneBlueprint
    {
        public string name;
        public string type;
        public string title;
        public string shortname;
        public string desc;

        public override string ToString()
        {
            return title + " (" + name + ")";
        }
    }

    public struct WeaponBlueprints
    {
        public string name;
        public string type;
        public string title;
        public string shortname;
        public string desc;

        public override string ToString()
        {
            return title + " (" + name + ")";
        }
    }
    public struct AugmentBlueprint
    {
        public string name;
        public string title;
        public string desc;

        public override string ToString()
        {
            return title + " (" + name + ")";
        }
    }
    enum CursorMode
    {
        None = 0,
        RoomSelected,
        DoorSelected,
        AddRoom,
        AddDoor,
        PlacingRoomFunc,
        PlaceBGCursor,
        PlacedBGCursor,
        PlaceGibsCursor,
        PlacedGibsCursor,
    }
    enum RoomID
    {
        None = 0,
        cloaking,
        doors,
        drones,
        engines,
        medbay,
        oxygen,
        pilot,
        sensors,
        shields,
        teleporter,
        weapons,

    }

    class Game
    {
        //Sprite AddRoomBtn_n;
        //Sprite AddRoomBtn_s;
        //Sprite AddDoorBtn_n;
        //Sprite AddDoorBtn_s;
        //Sprite LoadBtn_n;
        //Sprite LoadBtn_s;
        public Sprite BaseTex;
        public Sprite FloorTex;
        public CursorMode cursorMode;
        public static Game game;
        public static Ship ship;
        Sprite ViewOffsetter;
        List<RoomFunc> roomFuncs;
        public static Vector2f Offset;
        Vector2f bgOffset = new Vector2f(0, 0);
        Vector2f bgSize = new Vector2f(0, 0);
        int GUIStartX = 670;
        List<FTLRoom> ShipRooms = new List<FTLRoom>();
        List<FTLDoor> ShipDoors = new List<FTLDoor>();
        FTLRoom selectedRoom;
        FTLDoor selectedDoor;
        // bool addRoom = false;
        bool DoorHorizontal = true;
        //  bool addDoor = false;
        //   bool resizeMode = false;
        bool lastLMBState = false;
        RenderStates rs = RenderStates.Default;
        public static ShipProperties optWnd;
        public static List<WeaponBlueprints> weapons = new List<WeaponBlueprints>();
        public static List<AugmentBlueprint> augments = new List<AugmentBlueprint>();
        public static List<DroneBlueprint> drones = new List<DroneBlueprint>();
        public Sprite[] gibs;
        public RoomID setRoom = RoomID.None;
        GuiButton btnAddRoom;
        GuiButton btnAddDoor;
        //GuiButton btnLoad;
        GuiButton btnLoadBase;
        GuiButton btnLoadFloor;
        GuiButton btnOptions;
        GuiButton btnToggleBase;
        GuiButton btnMoveBG;
        GuiButton btnMoveGibs;
        GuiButton btnToggleFloor;
        GuiButton placeRoomCloaking;
        GuiButton placeRoomDoors;
        GuiButton placeRoomDrones;
        GuiButton placeRoomEngines;
        GuiButton placeRoomMedbay;
        GuiButton placeRoomOxygen;
        GuiButton placeRoomPilot;
        GuiButton placeRoomSensors;
        GuiButton placeRoomShields;
        GuiButton placeRoomTeleporter;
        GuiButton placeRoomWeapons;
        bool drawBase;
        bool drawFloor;
        //GuiButton placeRoomRepair;
        //GuiButton placeRoomCombat;
        public void Start()
        {
            game = this;
            ship = new Ship();
            optWnd = new ShipProperties();
            gibs = new Sprite[6];
            //ShipRooms.Add(new FTLRoom(new Vector2i(0, 0), new Vector2i(1, 1)));
            //ShipRooms.Add(new FTLRoom(new Vector2i(2, 2), new Vector2i(1, 1)));
            //ShipRooms.Add(new FTLRoom(new Vector2i(3, 3), new Vector2i(1, 1)));
            //AddRoomBtn_n = new Sprite(new Texture("img/GUI/btns/addroom_n.png"));
            btnAddRoom = new GuiButton(new Vector2f(GUIStartX, 0), new Texture("img/GUI/btns/addroom_n.png"), new Texture("img/GUI/btns/addroom_s.png"));
            btnAddDoor = new GuiButton(new Vector2f(GUIStartX, 37), new Texture("img/GUI/btns/adddoor_n.png"), new Texture("img/GUI/btns/adddoor_s.png"));

            placeRoomCloaking = new GuiButton(new Vector2f(GUIStartX + 5, 65), new Texture("img/GUI/roomfuncs/s_cloaking_overlay.png"));
            placeRoomDoors = new GuiButton(new Vector2f(GUIStartX + 35, 65), new Texture("img/GUI/roomfuncs/s_doors_overlay.png"));
            placeRoomDrones = new GuiButton(new Vector2f(GUIStartX + 65, 65), new Texture("img/GUI/roomfuncs/s_drones_overlay.png"));
            placeRoomEngines = new GuiButton(new Vector2f(GUIStartX + 95, 65), new Texture("img/GUI/roomfuncs/s_engines_overlay.png"));
            placeRoomMedbay = new GuiButton(new Vector2f(GUIStartX + 5, 95), new Texture("img/GUI/roomfuncs/s_medbay_overlay.png"));
            placeRoomOxygen = new GuiButton(new Vector2f(GUIStartX + 35, 95), new Texture("img/GUI/roomfuncs/s_oxygen_overlay.png"));
            placeRoomPilot = new GuiButton(new Vector2f(GUIStartX + 65, 95), new Texture("img/GUI/roomfuncs/s_pilot_overlay.png"));
            placeRoomSensors = new GuiButton(new Vector2f(GUIStartX + 95, 95), new Texture("img/GUI/roomfuncs/s_sensors_overlay.png"));
            placeRoomShields = new GuiButton(new Vector2f(GUIStartX + 5, 125), new Texture("img/GUI/roomfuncs/s_shields_overlay.png"));
            placeRoomTeleporter = new GuiButton(new Vector2f(GUIStartX + 35, 125), new Texture("img/GUI/roomfuncs/s_teleporter_overlay.png"));
            placeRoomWeapons = new GuiButton(new Vector2f(GUIStartX + 65, 125), new Texture("img/GUI/roomfuncs/s_weapons_overlay.png"));
            //AddRoomBtn_s = new Sprite(new Texture("img/GUI/btns/addroom_s.png"));
            //AddDoorBtn_n = new Sprite(new Texture("img/GUI/btns/adddoor_n.png"));
            //AddDoorBtn_s = new Sprite(new Texture("img/GUI/btns/adddoor_s.png"));

            btnOptions = new GuiButton(new Vector2f(GUIStartX, 560), new Texture("img/GUI/btns/options.png"));
            //btnLoad = new GuiButton(new Vector2f(GUIStartX, 150), new Texture("img/GUI/btns/load_n.png"), new Texture("img/GUI/btns/load_s.png"));
            btnLoadFloor = new GuiButton(new Vector2f(GUIStartX, 250), new Texture("img/GUI/btns/loadfloor_n.png"), new Texture("img/GUI/btns/loadfloor_s.png"));
            btnLoadBase = new GuiButton(new Vector2f(GUIStartX, 250 + 37), new Texture("img/GUI/btns/loadbase_n.png"), new Texture("img/GUI/btns/loadbase_s.png"));
            btnToggleFloor = new GuiButton(new Vector2f(GUIStartX, 350), new Texture("img/GUI/btns/showfloor_n.png"), new Texture("img/GUI/btns/showfloor_s.png"));
            btnToggleBase = new GuiButton(new Vector2f(GUIStartX, 350 + 37), new Texture("img/GUI/btns/showbase_n.png"), new Texture("img/GUI/btns/showbase_s.png"));

            btnMoveBG = new GuiButton(new Vector2f(GUIStartX, 400 + 37), new Texture("img/GUI/btns/movebg_n.png"), new Texture("img/GUI/btns/movebg_s.png"));
            btnMoveGibs = new GuiButton(new Vector2f(GUIStartX, 400 + (37 * 2)), new Texture("img/GUI/btns/movegibs_n.png"), new Texture("img/GUI/btns/movegibs_s.png"));
            //LoadBtn_n = new Sprite(new Texture("img/GUI/btns/load_n.png"));
            //LoadBtn_s = new Sprite(new Texture("img/GUI/btns/load_s.png"));
            ViewOffsetter = new Sprite(new Texture("img/GUI/btns/viewshift.png"));
            ViewOffsetter.Position = new Vector2f(GUIStartX + 40, 200);
            //AddRoomBtn_n.Position = new Vector2f(GUIStartX, 0);
            //AddRoomBtn_s.Position = new Vector2f(GUIStartX + 2, 2);
            //AddDoorBtn_n.Position = new Vector2f(GUIStartX, 37);
            //AddDoorBtn_s.Position = new Vector2f(GUIStartX + 2, 37 + 2);
            //LoadBtn_n.Position = new Vector2f(GUIStartX, 140);

            verts[0] = new Vertex();
            verts[1] = new Vertex();
            verts[0].Color = new Color(100, 100, 100, 100);
            verts[1].Color = new Color(100, 100, 100, 100);

            roomFuncs = new List<RoomFunc>();
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
        }
        Vertex[] verts = new Vertex[2];
        public void Draw(RenderTarget t)
        {
            t.Clear(new Color(65, 64, 60));



            btnAddRoom.Draw(t, cursorMode == CursorMode.AddRoom);
            btnAddDoor.Draw(t, cursorMode == CursorMode.AddDoor);
            //btnLoad.Draw(t, false);
            btnLoadBase.Draw(t, false);
            btnLoadFloor.Draw(t, false);
            btnOptions.Draw(t, false);

            if (BaseTex != null)
                btnToggleBase.Draw(t, drawBase);
            if (FloorTex != null)
                btnToggleFloor.Draw(t, drawFloor);

            if (BaseTex != null || FloorTex != null)
                btnMoveBG.Draw(t, cursorMode == CursorMode.PlaceBGCursor || cursorMode == CursorMode.PlacedBGCursor);

            btnMoveGibs.Draw(t, cursorMode == CursorMode.PlaceGibsCursor || cursorMode == CursorMode.PlacedGibsCursor);

            placeRoomCloaking.Draw(t, false);
            placeRoomDoors.Draw(t, false);
            placeRoomDrones.Draw(t, false);
            placeRoomEngines.Draw(t, false);
            placeRoomMedbay.Draw(t, false);
            placeRoomOxygen.Draw(t, false);
            placeRoomPilot.Draw(t, false);
            placeRoomSensors.Draw(t, false);
            placeRoomShields.Draw(t, false);
            placeRoomTeleporter.Draw(t, false);
            placeRoomWeapons.Draw(t, false);
            if (BaseTex != null && drawBase)
            {
                BaseTex.Position = bgOffset + (Offset * 35);
                BaseTex.Draw(t, RenderStates.Default);
            }
            if (FloorTex != null && drawFloor)
            {
                FloorTex.Position = bgOffset + (Offset * 35);
                FloorTex.Draw(t, RenderStates.Default);
            }

            // Draw grid
            foreach (FTLRoom room in ShipRooms)
                t.Draw(room.drawer);
            foreach (FTLDoor door in ShipDoors)
                t.Draw(door.drawer);


            for (int currentY = 1; currentY < 600; currentY += 35)
            {
                verts[0].Position = new Vector2f(0, currentY);
                verts[1].Position = new Vector2f(GUIStartX, currentY);
                t.Draw(verts, PrimitiveType.Lines);
            }
            for (int currentX = 0; currentX < GUIStartX; currentX += 35)
            {
                verts[0].Position = new Vector2f(currentX, 0);
                verts[1].Position = new Vector2f(currentX, 600);
                t.Draw(verts, PrimitiveType.Lines);
            }

            if (ship != null)
                foreach (RoomFunc rf in ship.rooms.Values)
                {
                    if (rf == null || rf.location == null)
                        continue;
                    Sprite temp = new Sprite(rf.image);
                    temp.Position = new Vector2f((rf.location.position.X + (((float)rf.location.size.X - 1) / 2)) * 35, (rf.location.position.Y + (((float)rf.location.size.Y - 1) / 2)) * 35);
                    temp.Position += (Game.Offset * 35);
                    t.Draw(temp);
                    temp.Dispose();
                }

            ViewOffsetter.Draw(t, rs);

            if (cursorMode == CursorMode.AddDoor)
            {
                RectangleShape curDoor;
                if (DoorHorizontal)
                {
                    curDoor = new RectangleShape(new Vector2f(19, 4));
                    curDoor.Position = new Vector2f((SFML.Window.Mouse.GetPosition(Program.app).X / 35) * 35 + 7, (SFML.Window.Mouse.GetPosition(Program.app).Y / 35) * 35 - 2);

                }
                else
                {
                    curDoor = new RectangleShape(new Vector2f(4, 19));
                    curDoor.Position = new Vector2f((SFML.Window.Mouse.GetPosition(Program.app).X / 35) * 35 - 2, (SFML.Window.Mouse.GetPosition(Program.app).Y / 35) * 35 + 7);
                }
                curDoor.FillColor = new Color(255, 150, 50);
                curDoor.OutlineColor = new Color(0, 0, 0);
                curDoor.OutlineThickness = 1;
                t.Draw(curDoor);
                curDoor.Dispose();



            }


            if (cursorMode == CursorMode.AddRoom)
            {
                RectangleShape currentroom = new RectangleShape(new Vector2f(35, 35));
                currentroom.Position = new Vector2f((SFML.Window.Mouse.GetPosition(Program.app).X / 35) * 35, (SFML.Window.Mouse.GetPosition(Program.app).Y / 35) * 35);
                currentroom.FillColor = new Color(0, 0, 0);
                t.Draw(currentroom);
                currentroom = new RectangleShape(new Vector2f(31, 31));
                currentroom.Position = new Vector2f((SFML.Window.Mouse.GetPosition(Program.app).X / 35) * 35 + 2, (SFML.Window.Mouse.GetPosition(Program.app).Y / 35) * 35 + 2);
                currentroom.FillColor = new Color(230, 226, 219);
                t.Draw(currentroom);
                currentroom.Dispose();



            }


            Sprite roomSpr = null;
            switch (setRoom)
            {
                case RoomID.None:
                    break;
                case RoomID.cloaking:
                    roomSpr = new Sprite(placeRoomCloaking.button);
                    break;
                case RoomID.doors:
                    roomSpr = new Sprite(placeRoomDoors.button);
                    break;
                case RoomID.drones:
                    roomSpr = new Sprite(placeRoomDrones.button);
                    break;
                case RoomID.engines:
                    roomSpr = new Sprite(placeRoomEngines.button);
                    break;
                case RoomID.medbay:
                    roomSpr = new Sprite(placeRoomMedbay.button);
                    break;
                case RoomID.oxygen:
                    roomSpr = new Sprite(placeRoomOxygen.button);
                    break;
                case RoomID.pilot:
                    roomSpr = new Sprite(placeRoomPilot.button);
                    break;
                case RoomID.sensors:
                    roomSpr = new Sprite(placeRoomSensors.button);
                    break;
                case RoomID.shields:
                    roomSpr = new Sprite(placeRoomShields.button);
                    break;
                case RoomID.teleporter:
                    roomSpr = new Sprite(placeRoomTeleporter.button);
                    break;
                case RoomID.weapons:
                    roomSpr = new Sprite(placeRoomWeapons.button);
                    break;
                default:
                    break;
            }
            if (roomSpr != null)
            {
                roomSpr.Position = new Vector2f(Mouse.GetPosition(Program.app).X, Mouse.GetPosition(Program.app).Y);
                t.Draw(roomSpr);
                roomSpr.Dispose();
            }

        }
        FTLRoom GetRoomAt(Vector2i pos)
        {
            foreach (FTLRoom room in ShipRooms)
            {
                if (room.WasClicked(new Vector2f(pos.X * 35, pos.Y * 35)))
                    return room;
            }

            return null;
        }



        public void ImportShip(string layout, string img)
        {
            string shipPath = Path.Combine(OptionsForm.resPath, "img\\ship\\");
            ShipDoors.Clear();
            ShipRooms.Clear();
            if (BaseTex != null)
                BaseTex.Dispose();
            if (FloorTex != null)
                FloorTex.Dispose();
            BaseTex = null;
            FloorTex = null;

            if (img != "")
            {
                if (File.Exists(Path.Combine(shipPath, img + "_base.png")))
                {
                    BaseTex = new Sprite(new Texture(Path.Combine(shipPath, img + "_base.png")));
                    drawBase = true;
                }
                if (File.Exists(Path.Combine(shipPath, img + "_floor.png")))
                {
                    FloorTex = new Sprite(new Texture(Path.Combine(shipPath, img + "_floor.png")));
                    drawFloor = true;
                }
            }

            DialogResult error;
            string XMLfilePath = Path.Combine(OptionsForm.dataPath, "data\\" + layout + ".xml");
            string LayoutfilePath = Path.Combine(OptionsForm.dataPath, "data\\" + layout + ".txt");
            do
            {
                if (!File.Exists(XMLfilePath))
                {
                    error = MessageBox.Show(XMLfilePath + " was not found! \nWould you like to manually find it?", "Error", MessageBoxButtons.YesNo);
                    if (error == DialogResult.Yes)
                    {
                        // Locate layout txt
                        OpenFileDialog openShipDia = new OpenFileDialog();
                        openShipDia.DefaultExt = ".xml";
                        openShipDia.Filter = "XML ship file (*.xml)|*.xml|All files|*.*";
                        openShipDia.ShowDialog();
                        XMLfilePath = openShipDia.FileName;
                    }
                    else
                        return;
                }
                else
                    error = DialogResult.OK;
            }
            while (!File.Exists(XMLfilePath) || error == DialogResult.Retry);
            do
            {
                if (!File.Exists(LayoutfilePath))
                {
                    error = MessageBox.Show(LayoutfilePath + " was not found! \nWould you like to manually find it?", "Error", MessageBoxButtons.YesNo);
                    if (error == DialogResult.Yes)
                    {
                        // Locate layout txt
                        OpenFileDialog openShipDia = new OpenFileDialog();
                        openShipDia.DefaultExt = ".txt";
                        openShipDia.Filter = ".TXT ship layout file (*.txt)|*.txt|All files|*.*";
                        openShipDia.ShowDialog();
                        LayoutfilePath = openShipDia.FileName;
                    }
                    else
                        return;
                }
                else
                    error = DialogResult.OK;
            }
            while (!File.Exists(LayoutfilePath) || error == DialogResult.Retry);

            ImportLayout(LayoutfilePath);
            ImportXML(XMLfilePath);



            foreach (RoomFunc room in ship.rooms.Values)
            {
                foreach (FTLRoom ftlroom in ShipRooms)
                {
                    if (room.roomId == ftlroom.id)
                    {
                        room.location = ftlroom;
                        continue;
                    }
                }
            }

        }
        public void ImportXML(string path)
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.ValidationType = ValidationType.None;




            using (XmlReader reader = XmlReader.Create(path, settings))
            {
                while (reader.Read())
                {
                    reader.ReadToFollowing("img");
                    int x = Convert.ToInt32(reader.GetAttribute("x"));
                    int y = Convert.ToInt32(reader.GetAttribute("y"));
                    int w = Convert.ToInt32(reader.GetAttribute("w"));
                    int h = Convert.ToInt32(reader.GetAttribute("h"));
                    bgOffset = new Vector2f(x, y);
                    bgOffset = new Vector2f(x, y);
                    bgSize = new Vector2f(w, h);


                    //if (img != "")
                    //{
                    if (BaseTex != null)
                        BaseTex.Position = bgOffset;
                    if (FloorTex != null)
                        FloorTex.Position = bgOffset;
                    // }
                    break;
                }
            }
        }
        public void ImportLayout(string path)
        {
            // create a reader and open the file
            if (path == "")
                return;

            TextReader tr = new StreamReader(path);

            string line;

            while ((line = tr.ReadLine()) != null)
            {
                if (line == "X_OFFSET")
                    Offset.X = Convert.ToInt32(tr.ReadLine());
                if (line == "Y_OFFSET")
                    Offset.Y = Convert.ToInt32(tr.ReadLine());

                if (line == "ROOM")
                {
                    int id = Convert.ToInt32(tr.ReadLine());

                    Vector2i pos = new Vector2i(Convert.ToInt32(tr.ReadLine()), Convert.ToInt32(tr.ReadLine()));
                    Vector2i size = new Vector2i(Convert.ToInt32(tr.ReadLine()), Convert.ToInt32(tr.ReadLine()));
                    FTLRoom room = new FTLRoom(pos, size);
                    room.id = id;
                    ShipRooms.Add(room);
                }
                else if (line == "DOOR")
                {
                    Vector2i pos = new Vector2i(Convert.ToInt32(tr.ReadLine()), Convert.ToInt32(tr.ReadLine()));
                    tr.ReadLine(); // room A
                    tr.ReadLine(); // room B
                    bool horiz = !Convert.ToBoolean(Convert.ToInt32(tr.ReadLine()));
                    FTLDoor door = new FTLDoor(pos, horiz);
                    ShipDoors.Add(door);
                }
            }

        }


        public void ImportShip()
        {

            OpenFileDialog openTexDia = new OpenFileDialog();

            openTexDia.DefaultExt = ".png";
            openTexDia.Filter = "Base texture png (*.png)|*.png|All files|*.*";
            openTexDia.ShowDialog();
            BaseTex = new Sprite(new Texture(openTexDia.FileName));

            openTexDia.Filter = "Floor texture png (*.png)|*.png|All files|*.*";
            openTexDia.ShowDialog();
            FloorTex = new Sprite(new Texture(openTexDia.FileName));



            OpenFileDialog openShipDia = new OpenFileDialog();
            openShipDia.DefaultExt = ".xml";
            openShipDia.Filter = "XML ship file (*.xml)|*.xml|All files|*.*";
            openShipDia.ShowDialog();

            openShipDia = new OpenFileDialog();
            openShipDia.DefaultExt = ".txt";
            openShipDia.Filter = "Text files (*.txt)|*.txt|All files|*.*";
            openShipDia.ShowDialog();

        }

        public void ExportLayoutXML(string path)
        {

            if (path == "")
                return;
            // create a writer and open the file
            TextWriter tw = new StreamWriter(path);
            string imgLine;
            if (BaseTex != null)
                imgLine = "<img x=\"" + bgOffset.X + "\" y=\"" + bgOffset.Y + "\" w=\"" + bgSize.X + "\" h=\"" + bgSize.Y + "\"/>";
            else
                imgLine = "<img x=\"" + bgOffset.X + "\" y=\"" + bgOffset.Y + "\" w=\"" + bgSize.X + "\" h=\"" + bgSize.Y + "\"/>";


            tw.WriteLine(imgLine);

            tw.WriteLine("<weaponMounts>");
            tw.WriteLine("</weaponMounts>");
            tw.WriteLine("<explosion>");
            tw.WriteLine("</explosion>");

            tw.WriteLine();
            // close the stream
            tw.Close();
            tw.Dispose();
        }

        public void ExportLayoutTxt(string path)
        {
            if (path == "")
                return;
            // create a writer and open the file
            TextWriter tw = new StreamWriter(path);

            // write a line of text to the file
            tw.WriteLine("X_OFFSET");
            tw.WriteLine(Offset.X.ToString());
            tw.WriteLine("Y_OFFSET");
            tw.WriteLine(Offset.Y.ToString());
            tw.WriteLine("VERTICAL");
            tw.WriteLine("0");
            tw.WriteLine("ELLIPSE");
            tw.WriteLine("200");
            tw.WriteLine("200");
            tw.WriteLine("0");
            tw.WriteLine("0");

            int roomCount = 0;
            foreach (FTLRoom room in ShipRooms)
            {
                room.id = roomCount;
                tw.WriteLine("ROOM");
                tw.WriteLine(roomCount.ToString());
                tw.WriteLine(room.position.X.ToString());
                tw.WriteLine(room.position.Y.ToString());
                tw.WriteLine(room.size.X.ToString());
                tw.WriteLine(room.size.Y.ToString());
                roomCount++;
            }

            foreach (FTLDoor door in ShipDoors)
            {

                tw.WriteLine("DOOR");
                tw.WriteLine(door.position.X.ToString());
                tw.WriteLine(door.position.Y.ToString());

                // Calculate what rooms it's connecting
                if (door.horiz)
                {
                    int idL = -1;
                    int idR = -1;
                    if (GetRoomAt(door.position + new Vector2i(0, -1)) != null)
                        idL = GetRoomAt(door.position + new Vector2i(0, -1)).id;

                    if (GetRoomAt(door.position) != null)
                        idR = GetRoomAt(door.position).id;

                    if (idL == -1 || idR == -1)
                    {
                        if (idL != -1)
                        {

                            tw.WriteLine(idL);
                            tw.WriteLine(idR);
                        }
                        else
                        {
                            tw.WriteLine(idR);
                            tw.WriteLine(idL);
                        }
                    }
                    else
                    {
                        tw.WriteLine(idL);
                        tw.WriteLine(idR);
                    }

                }
                else
                {
                    int idT = -1;
                    int idB = -1;
                    if (GetRoomAt(door.position + new Vector2i(-1, 0)) != null)
                        idT = GetRoomAt(door.position + new Vector2i(-1, 0)).id;

                    if (GetRoomAt(door.position) != null)
                        idB = GetRoomAt(door.position).id;

                    if (idT == -1 || idB == -1)
                    {
                        if (idT != -1)
                        {

                            tw.WriteLine(idT);
                            tw.WriteLine(idB);
                        }
                        else
                        {
                            tw.WriteLine(idB);
                            tw.WriteLine(idT);
                        }
                    }
                    else
                    {
                        tw.WriteLine(idT);
                        tw.WriteLine(idB);
                    }


                }

                if (door.horiz)
                    tw.WriteLine("0");
                else
                    tw.WriteLine("1");

                roomCount++;
            }
            tw.WriteLine();
            // close the stream
            tw.Close();
            tw.Dispose();
        }

        public void ExportBlueprintXML(string path, bool append)
        {

            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = "  ";
            settings.NewLineChars = "\r\n";
            settings.Encoding = new ASCIIEncoding();
            settings.NewLineHandling = NewLineHandling.Replace;

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("shipBlueprint"); // <-- Important root element
                writer.WriteAttributeString("name", ship.id);
                writer.WriteAttributeString("img", ship.img);
                writer.WriteAttributeString("layout", ship.layout);
                writer.WriteElementString("class", ship.className);
                writer.WriteElementString("name", ship.name);
                writer.WriteElementString("desc", ship.desc);
                writer.WriteStartElement("systemList");
                if (ship.rooms["pilot"].location != null)
                {
                    writer.WriteStartElement("pilot");
                    writer.WriteAttributeString("power", ship.rooms["pilot"].power.ToString());
                    writer.WriteAttributeString("room", ship.rooms["pilot"].location.id.ToString());
                    writer.WriteAttributeString("start", ship.rooms["pilot"].start.ToString().ToLower());
                    writer.WriteAttributeString("img", "room_pilot");
                    writer.WriteEndElement();
                }

                if (ship.rooms["doors"].location != null)
                {
                    writer.WriteStartElement("doors");
                    writer.WriteAttributeString("power", ship.rooms["doors"].power.ToString());
                    writer.WriteAttributeString("room", ship.rooms["doors"].location.id.ToString());
                    writer.WriteAttributeString("start", ship.rooms["doors"].start.ToString().ToLower());
                    writer.WriteAttributeString("img", "room_doors");
                    writer.WriteEndElement();
                }
                if (ship.rooms["sensors"].location != null)
                {
                    writer.WriteStartElement("sensors");
                    writer.WriteAttributeString("power", ship.rooms["sensors"].power.ToString());
                    writer.WriteAttributeString("room", ship.rooms["sensors"].location.id.ToString());
                    writer.WriteAttributeString("start", ship.rooms["sensors"].start.ToString().ToLower());
                    writer.WriteAttributeString("img", "room_sensors");
                    writer.WriteEndElement();
                }
                if (ship.rooms["medbay"].location != null)
                {
                    writer.WriteStartElement("medbay");
                    writer.WriteAttributeString("power", ship.rooms["medbay"].power.ToString());
                    writer.WriteAttributeString("room", ship.rooms["medbay"].location.id.ToString());
                    writer.WriteAttributeString("start", ship.rooms["medbay"].start.ToString().ToLower());
                    writer.WriteAttributeString("img", "room_medbay");
                    writer.WriteEndElement();
                }
                if (ship.rooms["oxygen"].location != null)
                {
                    writer.WriteStartElement("oxygen");
                    writer.WriteAttributeString("power", ship.rooms["oxygen"].power.ToString());
                    writer.WriteAttributeString("room", ship.rooms["oxygen"].location.id.ToString());
                    writer.WriteAttributeString("start", ship.rooms["oxygen"].start.ToString().ToLower());
                    writer.WriteAttributeString("img", "room_oxygen");
                    writer.WriteEndElement();
                }
                if (ship.rooms["shields"].location != null)
                {
                    writer.WriteStartElement("shields");
                    writer.WriteAttributeString("power", ship.rooms["shields"].power.ToString());
                    writer.WriteAttributeString("room", ship.rooms["shields"].location.id.ToString());
                    writer.WriteAttributeString("start", ship.rooms["shields"].start.ToString().ToLower());
                    writer.WriteAttributeString("img", "room_shields");
                    writer.WriteEndElement();
                }
                if (ship.rooms["engines"].location != null)
                {
                    writer.WriteStartElement("engines");
                    writer.WriteAttributeString("power", ship.rooms["engines"].power.ToString());
                    writer.WriteAttributeString("room", ship.rooms["engines"].location.id.ToString());
                    writer.WriteAttributeString("start", ship.rooms["engines"].start.ToString().ToLower());
                    writer.WriteAttributeString("img", "room_engines");
                    writer.WriteEndElement();
                }
                if (ship.rooms["weapons"].location != null)
                {
                    writer.WriteStartElement("weapons");
                    writer.WriteAttributeString("power", ship.rooms["weapons"].power.ToString());
                    writer.WriteAttributeString("room", ship.rooms["weapons"].location.id.ToString());
                    writer.WriteAttributeString("start", ship.rooms["weapons"].start.ToString().ToLower());
                    writer.WriteAttributeString("img", "room_weapons");
                    writer.WriteEndElement();
                }
                if (ship.rooms["drones"].location != null)
                {
                    writer.WriteStartElement("drones");
                    writer.WriteAttributeString("power", ship.rooms["drones"].power.ToString());
                    writer.WriteAttributeString("room", ship.rooms["drones"].location.id.ToString());
                    writer.WriteAttributeString("start", ship.rooms["drones"].start.ToString().ToLower());
                    writer.WriteAttributeString("img", "room_drones");
                    writer.WriteEndElement();
                }
                if (ship.rooms["teleporter"].location != null)
                {
                    writer.WriteStartElement("teleporter");
                    writer.WriteAttributeString("power", ship.rooms["teleporter"].power.ToString());
                    writer.WriteAttributeString("room", ship.rooms["teleporter"].location.id.ToString());
                    writer.WriteAttributeString("start", ship.rooms["teleporter"].start.ToString().ToLower());
                    writer.WriteAttributeString("img", "room_teleporter");
                    writer.WriteEndElement();
                }
                if (ship.rooms["cloaking"].location != null)
                {
                    writer.WriteStartElement("cloaking");
                    writer.WriteAttributeString("power", ship.rooms["cloaking"].power.ToString());
                    writer.WriteAttributeString("room", ship.rooms["cloaking"].location.id.ToString());
                    writer.WriteAttributeString("start", ship.rooms["cloaking"].start.ToString().ToLower());
                    writer.WriteAttributeString("img", "room_cloaking");
                    writer.WriteEndElement();
                }
                writer.WriteEndElement();      // End systems list    

                writer.WriteElementString("weaponSlots", ship.weaponSlots.ToString());
                writer.WriteElementString("droneSlots", ship.droneSlots.ToString());

                writer.WriteStartElement("weaponList");
                writer.WriteAttributeString("count", ship.weapons.Count(x => x.name != null).ToString());
                writer.WriteAttributeString("missiles", ship.missiles.ToString());

                for (int i = 0; i < 4; i++)
                {
                    if (ship.weapons[i].name == null)
                        continue;
                    writer.WriteStartElement("weapon");
                    writer.WriteAttributeString("name", ship.weapons[i].name);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); // End weaponList



                writer.WriteStartElement("droneList");
                writer.WriteAttributeString("count", ship.drones.Count(x => x.name != null).ToString());
                writer.WriteAttributeString("drones", ship.droneParts.ToString());

                for (int i = 0; i < 4; i++)
                {
                    if (ship.drones[i].name == null)
                        continue;
                    writer.WriteStartElement("drone");
                    writer.WriteAttributeString("name", ship.drones[i].name);
                    writer.WriteEndElement();
                }
                writer.WriteEndElement(); // End droneList


                writer.WriteStartElement("health");
                writer.WriteAttributeString("amount", ship.health.ToString());
                writer.WriteEndElement();
                writer.WriteStartElement("maxPower");
                writer.WriteAttributeString("amount", ship.maxPower.ToString());
                writer.WriteEndElement();

                // Crew:
                // Human:
                if (ship.crewHuman > 0)
                {

                    writer.WriteStartElement("crewCount");
                    writer.WriteAttributeString("amount", ship.crewHuman.ToString());
                    writer.WriteAttributeString("class", "human");
                    writer.WriteEndElement();
                }
                // Rock:
                if (ship.crewRock > 0)
                {

                    writer.WriteStartElement("crewCount");
                    writer.WriteAttributeString("amount", ship.crewRock.ToString());
                    writer.WriteAttributeString("class", "rock");
                    writer.WriteEndElement();
                }
                // Mantis:
                if (ship.crewMantis > 0)
                {

                    writer.WriteStartElement("crewCount");
                    writer.WriteAttributeString("amount", ship.crewMantis.ToString());
                    writer.WriteAttributeString("class", "mantis");
                    writer.WriteEndElement();
                }
                // Engi:
                if (ship.crewEngi > 0)
                {

                    writer.WriteStartElement("crewCount");
                    writer.WriteAttributeString("amount", ship.crewEngi.ToString());
                    writer.WriteAttributeString("class", "engi");
                    writer.WriteEndElement();
                }
                // Slug:
                if (ship.crewHuman > 0)
                {

                    writer.WriteStartElement("crewCount");
                    writer.WriteAttributeString("amount", ship.crewSlug.ToString());
                    writer.WriteAttributeString("class", "slug");
                    writer.WriteEndElement();
                }
                // Zoltan:
                if (ship.crewZoltan > 0)
                {

                    writer.WriteStartElement("crewCount");
                    writer.WriteAttributeString("amount", ship.crewZoltan.ToString());
                    writer.WriteAttributeString("class", "energy");
                    writer.WriteEndElement();
                }
                // Crystal:
                if (ship.crewCrystal > 0)
                {

                    writer.WriteStartElement("crewCount");
                    writer.WriteAttributeString("amount", ship.crewCrystal.ToString());
                    writer.WriteAttributeString("class", "crystal");
                    writer.WriteEndElement();
                }
                for (int i = 0; i < 3; i++)
                {
                    if (ship.augments[i].name == null)
                        continue;
                    writer.WriteStartElement("aug");
                    writer.WriteAttributeString("name", ship.augments[i].name);
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();              // <-- Closes it
                writer.WriteEndDocument();
            }
        }

        public void ExportShip()
        {
            ExportLayoutTxt("ship.txt");
            ExportLayoutXML("ship.xml");
            ExportBlueprintXML("blueprint.xml", append: false);
        }


        public void OnKeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            if (e.Code == Keyboard.Key.F1)
                cursorMode = CursorMode.AddRoom;
            if (e.Code == Keyboard.Key.F2)
                cursorMode = CursorMode.AddDoor;

            if (e.Code == Keyboard.Key.Return)
            {
                ExportShip();
            }

            if (e.Code == Keyboard.Key.F12)
            {
                //ImportShip();
            }

            if (e.Code == Keyboard.Key.Escape)
            {
                cursorMode = CursorMode.None;
            }

            if (e.Code == Keyboard.Key.Space)
            {

                if (selectedDoor != null)
                    selectedDoor.horiz = !selectedDoor.horiz;
                else
                    DoorHorizontal = !DoorHorizontal;

            }

            if (cursorMode == CursorMode.PlacedBGCursor)
            {
                if (e.Code == Keyboard.Key.Left)
                {
                    bgOffset += new Vector2f(-1, 0);
                }
                if (e.Code == Keyboard.Key.Right)
                {
                    bgOffset += new Vector2f(1, 0);
                }
                if (e.Code == Keyboard.Key.Up)
                {
                    bgOffset += new Vector2f(0, -1);
                }
                if (e.Code == Keyboard.Key.Down)
                {
                    bgOffset += new Vector2f(0, 1);
                }
            }

            if (e.Code == Keyboard.Key.Delete)
            {
                switch (cursorMode)
                {
                    case CursorMode.None:
                        break;
                    case CursorMode.RoomSelected:
                        foreach (RoomFunc roomfunc in ship.rooms.Values)
                        {
                            if (roomfunc.location == selectedRoom)
                                roomfunc.location = null;
                        }

                        ShipRooms.Remove(selectedRoom);
                        selectedRoom = null;
                        cursorMode = CursorMode.None;
                        break;
                    case CursorMode.DoorSelected:
                        ShipDoors.Remove(selectedDoor);
                        selectedDoor = null;
                        cursorMode = CursorMode.None;
                        break;
                    case CursorMode.AddRoom:
                        break;
                    case CursorMode.AddDoor:
                        break;
                    case CursorMode.PlacingRoomFunc:
                        break;
                    default:
                        break;
                }
            }

            if (cursorMode == CursorMode.RoomSelected)
            {
                if (selectedRoom == null)
                {
                    cursorMode = CursorMode.None;
                    return;
                }
                if (e.Shift == true)
                {
                    // Shrink mode
                    if (selectedRoom.size.X * selectedRoom.size.Y <= 1)
                        return;

                    if (selectedRoom.size.Y > 1)
                    {

                        if (e.Code == Keyboard.Key.Up)
                        {
                            selectedRoom.size += new Vector2i(0, -1);
                        }

                        if (e.Code == Keyboard.Key.Down)
                        {
                            selectedRoom.position += new Vector2i(0, 1);
                            selectedRoom.size += new Vector2i(0, -1);
                        }
                    }


                    if (selectedRoom.size.X > 1)
                    {
                        if (e.Code == Keyboard.Key.Right)
                        {
                            selectedRoom.position += new Vector2i(1, 0);
                            selectedRoom.size += new Vector2i(-1, 0);
                        }
                        if (e.Code == Keyboard.Key.Left)
                        {
                            selectedRoom.size += new Vector2i(-1, 0);
                        }

                    }
                }
                else
                {
                    if (e.Code == Keyboard.Key.Up)
                    {
                        selectedRoom.position += new Vector2i(0, -1);
                        selectedRoom.size += new Vector2i(0, 1);
                    }
                    if (e.Code == Keyboard.Key.Right)
                    {
                        selectedRoom.size += new Vector2i(1, 0);
                    }
                    if (e.Code == Keyboard.Key.Down)
                    {
                        selectedRoom.size += new Vector2i(0, 1);
                    }
                    if (e.Code == Keyboard.Key.Left)
                    {
                        selectedRoom.position += new Vector2i(-1, 0);
                        selectedRoom.size += new Vector2i(1, 0);
                    }


                }
            }


        }
        Random r = new Random();

        public void GUIClick(Vector2i mouseScreenPos)
        {
            // Single click on the GUI!
            if (btnAddRoom.WasClicked(mouseScreenPos))
            {
                cursorMode = CursorMode.AddRoom;
            }
            if (btnAddDoor.WasClicked(mouseScreenPos))
            {
                cursorMode = CursorMode.AddDoor;
            }

            if (btnLoadBase.WasClicked(mouseScreenPos))
            {

                OpenFileDialog openTexDia = new OpenFileDialog();

                openTexDia.DefaultExt = ".png";
                openTexDia.Filter = "Base texture png (*.png)|*.png|All files|*.*";
                openTexDia.ShowDialog();
                if (openTexDia.FileName != "")
                {
                    BaseTex = new Sprite(new Texture(openTexDia.FileName));
                    bgSize = new Vector2f(BaseTex.TextureRect.Width, BaseTex.TextureRect.Height);
                    drawBase = true;
                }
                openTexDia.Dispose();

            }
            if (btnOptions.WasClicked(mouseScreenPos))
                optWnd.Show();
            if (btnLoadFloor.WasClicked(mouseScreenPos))
            {

                OpenFileDialog openTexDia = new OpenFileDialog();

                openTexDia.DefaultExt = ".png";
                openTexDia.Filter = "Base texture png (*.png)|*.png|All files|*.*";
                openTexDia.ShowDialog();
                if (openTexDia.FileName != "")
                {
                    FloorTex = new Sprite(new Texture(openTexDia.FileName));
                    drawFloor = true;
                }
                openTexDia.Dispose();
            }

            if (FloorTex != null || BaseTex != null)
                if (btnMoveBG.WasClicked(mouseScreenPos))
                {
                    if (cursorMode == CursorMode.PlacedBGCursor || cursorMode == CursorMode.PlaceBGCursor)
                    {
                        cursorMode = CursorMode.None;
                    }
                    else
                    {
                        cursorMode = CursorMode.PlaceBGCursor;
                    }
                }
            if (BaseTex != null)
                if (btnToggleBase.WasClicked(mouseScreenPos))
                {
                    drawBase = !drawBase;
                }
            if (FloorTex != null)
                if (btnToggleFloor.WasClicked(mouseScreenPos))
                {
                    drawFloor = !drawFloor;
                }

            if (placeRoomCloaking.WasClicked(mouseScreenPos))
                setRoom = RoomID.cloaking;
            if (placeRoomDoors.WasClicked(mouseScreenPos))
                setRoom = RoomID.doors;
            if (placeRoomDrones.WasClicked(mouseScreenPos))
                setRoom = RoomID.drones;
            if (placeRoomEngines.WasClicked(mouseScreenPos))
                setRoom = RoomID.engines;
            if (placeRoomMedbay.WasClicked(mouseScreenPos))
                setRoom = RoomID.medbay;
            if (placeRoomOxygen.WasClicked(mouseScreenPos))
                setRoom = RoomID.oxygen;
            if (placeRoomPilot.WasClicked(mouseScreenPos))
                setRoom = RoomID.pilot;
            if (placeRoomSensors.WasClicked(mouseScreenPos))
                setRoom = RoomID.sensors;
            if (placeRoomShields.WasClicked(mouseScreenPos))
                setRoom = RoomID.shields;
            if (placeRoomTeleporter.WasClicked(mouseScreenPos))
                setRoom = RoomID.teleporter;
            if (placeRoomWeapons.WasClicked(mouseScreenPos))
                setRoom = RoomID.weapons;

            if (setRoom != RoomID.None)
                cursorMode = CursorMode.PlacingRoomFunc;


            if (new FloatRect(ViewOffsetter.Position.X, ViewOffsetter.Position.Y, ViewOffsetter.TextureRect.Width, ViewOffsetter.TextureRect.Height).Contains(Mouse.GetPosition(Program.app).X, Mouse.GetPosition(Program.app).Y))
            {
                float Xdif = ViewOffsetter.Position.X + (ViewOffsetter.TextureRect.Width / 2) - Mouse.GetPosition(Program.app).X;
                float Ydif = ViewOffsetter.Position.Y + (ViewOffsetter.TextureRect.Width / 2) - Mouse.GetPosition(Program.app).Y;

                if (Math.Abs(Xdif) + Math.Abs(Ydif) < 10)
                {
                    Offset = new Vector2f(0, 0);
                }
                else
                    if (Math.Abs(Xdif) > Math.Abs(Ydif))
                    {
                        if (Xdif > 0)
                            Offset -= new Vector2f(1, 0);
                        else
                            Offset += new Vector2f(1, 0);
                    }
                    else
                    {
                        if (Ydif > 0)
                            Offset -= new Vector2f(0, 1);
                        else
                            Offset += new Vector2f(0, 1);
                    }

            }
        }

        public void Update()
        {
            Vector2i mouseScreenPos = Mouse.GetPosition(Program.app);
            Vector2i mouseActualPos = Mouse.GetPosition(Program.app) - new Vector2i((int)Game.Offset.X * 35, (int)Game.Offset.Y * 35);

            switch (cursorMode)
            {
                case CursorMode.None:
                    if (Mouse.IsButtonPressed(Mouse.Button.Left) && lastLMBState == false)
                    {
                        // Single click somewhere!
                        if (Mouse.GetPosition(Program.app).X >= GUIStartX)
                        {
                            GUIClick(mouseScreenPos);

                        }

                        else
                        {
                            // Single click on the screen!
                            bool clickedADoor = false;
                            foreach (FTLDoor tempDoor in ShipDoors)
                            {
                                if (tempDoor.WasClicked(new Vector2f(mouseActualPos.X, mouseActualPos.Y)))
                                {
                                    clickedADoor = true;
                                    selectedDoor = tempDoor;
                                    cursorMode = CursorMode.DoorSelected;
                                    selectedDoor.selected = true;
                                    break;

                                }
                            }

                            if (clickedADoor == false)
                            {
                                bool clickedARoom = false;
                                foreach (FTLRoom tempRoom in ShipRooms)
                                {

                                    if (tempRoom.WasClicked(new Vector2f(mouseActualPos.X, mouseActualPos.Y)))
                                    {
                                        selectedRoom = tempRoom;
                                        cursorMode = CursorMode.RoomSelected;
                                        selectedRoom.selected = true;
                                        clickedARoom = true;
                                        break;

                                    }
                                }
                                if (clickedARoom == false)
                                {
                                    if (selectedRoom != null)
                                    {
                                        selectedRoom.selected = false;
                                        selectedRoom = null;
                                        cursorMode = CursorMode.None;
                                    }
                                }
                            }
                        }

                    }
                    break;
                case CursorMode.RoomSelected:
                    if (Mouse.IsButtonPressed(Mouse.Button.Right))
                    {
                        // Work out which tile we clicked

                        Vector2i tileClicked = new Vector2i((mouseActualPos.X / 35), (mouseActualPos.Y / 35));
                        Vector2i diff = tileClicked - selectedRoom.position + new Vector2i(1, 1);
                        if (diff.X > 0 && diff.Y > 0)
                        {
                            selectedRoom.size += diff - selectedRoom.size;
                        }
                        else
                        {
                            if (diff.X < 0)
                            {
                                selectedRoom.position.X += diff.X;
                                selectedRoom.size.X -= diff.X;
                            }
                            if (diff.Y < 0)
                            {
                                selectedRoom.position.Y += diff.Y;
                                selectedRoom.size.Y -= diff.Y;
                            }
                        }

                    }


                    if (lastLMBState == true && Mouse.IsButtonPressed(Mouse.Button.Left))
                    {
                        selectedRoom.position = new Vector2i((mouseActualPos.X / 35), (mouseActualPos.Y / 35));

                    }
                    else if (lastLMBState == false && Mouse.IsButtonPressed(Mouse.Button.Left))
                    {
                        // Somewhere was clicked whilst a room was selected


                        // Did we click the current room?
                        if (selectedRoom.WasClicked(new Vector2f(mouseActualPos.X, mouseActualPos.Y)))
                        {
                            cursorMode = CursorMode.RoomSelected;
                        }
                        else
                        {

                            selectedRoom.selected = false;
                            selectedRoom = null;
                            cursorMode = CursorMode.None;
                        }

                    }
                    break;
                case CursorMode.DoorSelected:
                    if (lastLMBState == true && Mouse.IsButtonPressed(Mouse.Button.Left))
                    {
                        selectedDoor.position = new Vector2i(mouseActualPos.X / 35, mouseActualPos.Y / 35);

                    }
                    else if (lastLMBState == false && Mouse.IsButtonPressed(Mouse.Button.Left))
                    {
                        // Somewhere was clicked whilst a room was selected


                        // Did we click the current room?
                        if (selectedDoor.WasClicked(new Vector2f(mouseActualPos.X, mouseActualPos.Y)))
                        {
                            cursorMode = CursorMode.DoorSelected;
                        }
                        else
                        {

                            selectedDoor.selected = false;
                            selectedDoor = null;
                            cursorMode = CursorMode.None;
                        }

                    }
                    break;
                case CursorMode.AddRoom:
                    if (Mouse.IsButtonPressed(Mouse.Button.Left) && lastLMBState == false)
                    {
                        cursorMode = CursorMode.None;
                        FTLRoom newRoom = new FTLRoom(new Vector2i(mouseActualPos.X / 35, mouseActualPos.Y / 35), new Vector2i(1, 1));
                        ShipRooms.Add(newRoom);
                        newRoom.selected = true;
                        if (selectedRoom != null)
                            selectedRoom.selected = false;
                        selectedRoom = newRoom;
                        cursorMode = CursorMode.RoomSelected;

                        if (Mouse.IsButtonPressed(Mouse.Button.Right))
                        {
                            if (selectedRoom != null)
                                selectedRoom.selected = false;
                            selectedRoom = null;
                            cursorMode = CursorMode.None;
                        }
                    }
                    break;
                case CursorMode.AddDoor:

                    if (Mouse.IsButtonPressed(Mouse.Button.Left) && lastLMBState == false)
                    {
                        cursorMode = CursorMode.None;
                        FTLDoor newDoor = new FTLDoor(new Vector2i(mouseActualPos.X / 35, mouseActualPos.Y / 35), DoorHorizontal);
                        ShipDoors.Add(newDoor);
                    }

                    if (Mouse.IsButtonPressed(Mouse.Button.Right))
                    {
                        cursorMode = CursorMode.None;
                    }
                    break;
                case CursorMode.PlacingRoomFunc:

                    if (setRoom == RoomID.None || Mouse.IsButtonPressed(Mouse.Button.Right))
                    {
                        cursorMode = CursorMode.None;
                        break;
                    }
                    if (Mouse.IsButtonPressed(Mouse.Button.Left) && lastLMBState == false)
                    {
                        foreach (FTLRoom tempRoom in ShipRooms)
                        {

                            if (tempRoom.WasClicked(new Vector2f(mouseActualPos.X, mouseActualPos.Y)))
                            {
                                // Are we placing a room?
                                if (setRoom != RoomID.None)
                                {
                                    ship.rooms[Enum.GetName(typeof(RoomID), setRoom)].location = tempRoom;
                                    setRoom = RoomID.None;
                                    break;
                                }
                            }
                        }
                    }
                    break;
                case CursorMode.PlaceBGCursor:
                    bgOffset = new Vector2f((int)mouseScreenPos.X - Offset.X * 35, (int)mouseScreenPos.Y - Offset.Y * 35);
                    bgOffset -= new Vector2f(BaseTex.TextureRect.Width / 2, BaseTex.TextureRect.Height / 2);
                    if (Mouse.IsButtonPressed(Mouse.Button.Left) && lastLMBState == false)
                    {
                        cursorMode = CursorMode.PlacedBGCursor;
                    }
                    break;
                case CursorMode.PlacedBGCursor:
                    if (Mouse.IsButtonPressed(Mouse.Button.Left) && lastLMBState == false)
                    {
                        cursorMode = CursorMode.None;
                    }
                    if (Keyboard.IsKeyPressed(Keyboard.Key.Return))
                    {
                        cursorMode = CursorMode.None;
                    }
                    break;
                default:
                    break;
            }


            lastLMBState = Mouse.IsButtonPressed(Mouse.Button.Left);
        }

    }
}
