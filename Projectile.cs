using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics; //For Texture2D
using Microsoft.Xna.Framework; //for Vector2

namespace TeamProject
{
    class Projectile : CombatSprites
    {
        private Texture2D texture;
        //keeps track of the projectiles index in the list
        public int indexInList { get; set; }

        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        private Vector2 velocity;
        public Vector2 Velocity
        {
            get { return velocity; }
            set { velocity = value; }
        }

        public Projectile(CombatSpriteColors newColor, Directions newDirection, int currentLevel)
        {
            SpriteColor = newColor;
            Direction = newDirection;
            //texture and position will depend on color and direction
            //velocity will depnd on the current level (if we decide that hte projectiles will move faster as the player advances levels)
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        /// <summary>
        /// moves projectile towards ship at velocity according to level
        /// </summary>
        public void Move()
        {
            if(Direction == Directions.kBottom)
            {
                //move projectile up
            }
            //if statements and logic for remaiing directions
        }
    }
}
