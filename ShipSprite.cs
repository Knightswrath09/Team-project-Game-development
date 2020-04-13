using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics; //For Texture2D
using Microsoft.Xna.Framework; //for Vector2

namespace TeamProject
{
    class ShipSprite
    {
        private string name;
        public string Name
        {
            get { return name; }
        }
        private Texture2D texture; //ship texture
        private Vector2 position; //ship position
        private int hp; //field for hit point
        public int HP //public proprty for hit points, can be adjusted by power-ups, level-ups, and hits
        {
            get { return hp; }
            set { hp = value; }
        }
        private Vector2 screenSize;
        private Vector2 size;
            
        ///***CARLYN
        //ShipSprite constructor
        public ShipSprite(string newName, Texture2D newTexture, Vector2 newScreenSize, Vector2 newSize)
       {
          name = newName;
          texture = newTexture;
          screenSize = newScreenSize;
          size = newSize;
          hp = 3;
          position = newVector2((screenSize.X / 2) - (size.X / 2), (screenSize.Y / 2) - (size.Y / 2));
        }
        
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
   
}
