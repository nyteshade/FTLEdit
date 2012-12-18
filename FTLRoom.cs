using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SFML;
using SFML.Graphics;
using SFML.Window;

namespace FTLShipEdit
{

    
   public class RoomFunc
    {
        public RoomFunc(string name, Texture image, FTLRoom location)
        {
            this.name = name;
            this.image = image;
            this.location = location;
        }

        public void Draw(RenderTarget t)
        {
            if (location == null)
                return;
            Sprite tempSpr = new Sprite(image);
            tempSpr.Position = new Vector2f(location.position.X*35 + (location.size.X*35 / 2),location.position.Y*35 + (location.size.Y*35 / 2));
        }

        public string name;
        public Texture image;
        public FTLRoom location;
        public int power;
        public int roomId;
        public bool start;

    }


    public class FTLRoom
    {
        public FTLRoom(Vector2i pos, Vector2i size)
        {
            this.position = pos;
            this.size = size;
            this.selected = false;
        }


        public Vector2i position;
        public Vector2i size;
        public bool selected;
        public int id;

        public bool WasClicked(Vector2f pos)
        {
            pos /= 35;
            return ((pos.X >= position.X && pos.X < position.X + size.X) && (pos.Y >= position.Y && pos.Y < position.Y + size.Y));
        }
        public bool IsAt(Vector2i pos)
        {
            return ((pos.X >= position.X && pos.X < position.X + size.X) && (pos.Y >= position.Y && pos.Y < position.Y + size.Y));
        }

        public RectangleShape drawer
        {
            get
            {
                RectangleShape tempShape = new RectangleShape(new Vector2f(size.X * 35 - 4, size.Y * 35 - 4));
                tempShape.Position = new Vector2f(position.X * 35 + 2, position.Y * 35 + 2) + (Game.Offset * 35);
                tempShape.OutlineThickness = 2;

                if (selected)
                {
                    tempShape.FillColor = new Color(200, 255, 200);
                    tempShape.OutlineColor = Color.Green;
                }
                else
                {
                    tempShape.FillColor = new Color(230, 226, 219);

                    tempShape.OutlineColor = Color.Black;
                }

                return tempShape;
            }
        }
    }

    class FTLDoor
    {
        public FTLDoor(Vector2i pos, bool horiz)
        {
            this.position = pos;
            //this.size = size;
            this.horiz = horiz;
            this.selected = false;
        }
        public Vector2i position;
        public Vector2i size = new Vector2i(17, 17);
        public bool selected;
        public bool horiz;
        
        
        public bool WasClicked(Vector2f pos)
        { 
            if (horiz)
            {
                return ((pos.X >= (position.X * 35 + 7) && pos.X < (position.X * 35 + 7) + size.X) && (pos.Y >= (position.Y * 35 - 2) && pos.Y < (position.Y * 35 - 2) + size.Y));
            }
            else
            {
                return ((pos.X >= (position.X * 35 - 2) && pos.X < (position.X * 35 - 2) + size.X) && (pos.Y >= (position.Y * 35 + 7) && pos.Y  < (position.Y * 35 + 7) + size.Y));
            }
        }
    
        public RectangleShape drawer
        {
            get
            {

                RectangleShape tempShape;
                if (horiz)
                {
                    tempShape = new RectangleShape(new Vector2f(19, 4));
                    tempShape.Position = new Vector2f(position.X * 35 + 7, position.Y * 35 - 2) + (Game.Offset * 35);

                }
                else
                {
                    tempShape = new RectangleShape(new Vector2f(4, 19));
                    tempShape.Position = new Vector2f(position.X * 35 - 2, position.Y * 35 + 7) + (Game.Offset * 35);
                }

                tempShape.OutlineThickness = 1;



                if (selected)
                {
                    tempShape.FillColor = new Color(200, 255, 200);
                    tempShape.OutlineColor = Color.Green;
                }
                else
                {

                    tempShape.FillColor = new Color(255, 150, 50);
                    tempShape.OutlineColor = new Color(0, 0, 0);
                }

                return tempShape;
            }
        }
    }
}
