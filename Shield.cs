using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics; //For Texture2D
using Microsoft.Xna.Framework; //for Vector2

namespace TeamProject
{
    class Shield : CombatSprites
    {
        private Texture2D texture;
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //for purple shields
        //red and blue visible and purple insivible when red and blue are not overalapping
        //red and blue not visible and purple visible when red and blue are overlapping
        public bool visible { get; set; }

        public Shield(CombatSpriteColors newColor, Directions newDirection)
        {
            SpriteColor = newColor;
            Direction = newDirection;
            //texture and position will depend on color and direction
        }

        public void MoveShield(Directions newDirection, List<Shield> otherShields)
        {
            //use input recieved in game class to move respective shield to the correct position
            //check if shields overlap and change visibilty if needed 
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
