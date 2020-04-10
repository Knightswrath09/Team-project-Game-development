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
        public Vector2  screenSize
        {
            get(return screenSize);
        }
        private Vector2 size;
        public Vector2 size
        {
            get(return size);
        }
        
            
        ///***CARLYN
        //ShipSprite constructor
        ShipSprite ship1 = new ShipSprite("GoodStar");
        
        ship1.name = "GoodStar";
       
        
    }
}
