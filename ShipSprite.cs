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
        private Vector2 ScreenSize;
        public Vector2  ScreenSize
        {
            get(return ScreenSize);
        }
        private Vector2 Size;
        public Vector2 Size
        {
            get(return Size);
        }
        private Vector2 Position;
        public Vector2 Position
        {
            get(return Position);
        }
            
        ///***CARLYN
        //ShipSprite constructor
        public ShipSprite(Vector2 newScreenSize, Vector2 newSize, Vector2 newPosition, string newName)
        {
      
      screenSize = newScreenSize;
      size = newSize;
      position = newPosition;  
        ShipSprite = new ShipSprite("GoodStar");
        
       
    }
}
