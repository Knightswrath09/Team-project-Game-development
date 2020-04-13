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

        private Vector2 screensize { get; set; }
        public Vector2 Screensize
        {
            get { return screensize; }
            set { screensize = value; }
        }

        private Vector2 size { get; set; }
        public Vector2 Size 
        { 
            get { return size; } 
            set { size = value; } 
        }

        //maybe force a purple in if ot doesnt randpmly generate?
        //***DUSTIN
        public Projectile(Level currentLevel, Vector2 newScreensize, Vector2 newSize, Directions newDirection, CombatSpriteColors newColor, 
            Texture2D newTexture)
        {
            screensize = newScreensize;
            size = newSize;
            Direction = newDirection;
            SpriteColor = newColor;
            texture = newTexture;
            
            //set velocity and position according to direction
            if (newDirection == Directions.kTop)
            {
                velocity = new Vector2(0, currentLevel.ProjectileSpeed);
                position = new Vector2((newScreensize.X / 2) - (newSize.X / 2), 0);
            }
            else if (newDirection == Directions.kRight)
            {
                velocity = new Vector2(-currentLevel.ProjectileSpeed, 0);
                position = new Vector2(newScreensize.X, (newScreensize.Y / 2) - (newSize.Y / 2));
            }
            else if (newDirection == Directions.kBottom)
            {
                velocity = new Vector2(0, -currentLevel.ProjectileSpeed);
                position = new Vector2((newScreensize.X / 2) - (newSize.X / 2), newScreensize.Y);
            }
            else if (newDirection == Directions.kLeft)
            {
                velocity = new Vector2(currentLevel.ProjectileSpeed, 0);
                position = new Vector2(0, (newScreensize.Y / 2) - (newSize.Y / 2));
            }
            
            
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

        //*****IRIS
        public void Move()
        {
            ///adds the adjusted velocity to the current position
            position += velocity;
        }
    }
}
