using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics; //For Texture2D
using Microsoft.Xna.Framework; //for Vector2

namespace TeamProject
{
    //class for the ship, which tracks HP and is displayed in the middle of the screen for each level
    class ShipSprite // Made by Carlyn Solomon
    {
        //name of the ship
        private string name;
        public string Name
        {
            get { return name; }
        }
        private Texture2D texture; //ship texture
        private Vector2 position; //ship position
        public Vector2 Position
        {
            get { return position; }
        }
        private int hp; //field for hit point
        public int HP //public proprty for hit points, can be adjusted by power-ups, level-ups, and hits. loses when HP == 0
        {
            get { return hp; }
            set { hp = value; }
        }
        //to determine positioning of the ship
        private Vector2 screenSize;
        //ship size
        private Vector2 size;
        public Vector2 Size
        {
            get { return size; }
        }
        
        //constructor
        public ShipSprite(string newName, Texture2D newTexture, Vector2 newScreenSize, Vector2 newSize)
        {
            name = newName;
            texture = newTexture;
            screenSize = newScreenSize;
            size = newSize;
            hp = 3;
            position = new Vector2((screenSize.X / 2) - (size.X / 2), (screenSize.Y / 2) - (size.Y / 2));
        }

        //draws ship
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

    }
}
