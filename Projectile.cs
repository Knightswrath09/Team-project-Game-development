﻿using System;
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
            set { screensize = new Vector2(ScreenWidth, ScreenHeight); }
        }

        private Vector2 size { get; set; }
        public Vector2 Size { get { return size; } set { size = Size; } }
        //maybe force a purple in if ot doesnt randpmly generate?
        //***DUSTIN
        public Projectile(Level currentLevel, Vector2 screens, Vector2 siz)
        {
            int RandomColor = Random(0, 14); //random for color
            int RandomDirection = Random(0, 5); //random for direction
            //color randomization
            if (currentlevel.typesOfProjectiles == Level.ProjectileTypes.kRed_Only)
            {
                if (RandomColor <= 12)
                    color = CombatSprites.CombatSpriteColors.kRed;
                if (RandomColor == 13 || RandomColor == 14)
                    color = CombatSprites.CombatSpriteColorskGreen;
            }
            else if (currentlevel.typesOfProjectiles == Level.ProjectileTypes.Red_And_Blue)
            {
                if (RandomColor <= 6)
                    color = CombatSprites.CombatSpriteColors.kRed;
                if (RandomColor > 6 && RandomColor < 13)
                    color = CombatSprites.CombatSpriteColors.kBlue;
                else if (RandomColor == 13)
                    color = CombatSprites.CombatSpriteColors.kGreen;
            }
            else if (currentlevel.typesOfProjectiles == Level.ProjectileTypes.kRBP)
            {
                if (RandomColor <= 5)
                    color = CombatSprites.CombatSpriteColors.kRed;
                if (RandomColor > 5 && RandomColor < 11)
                    color = CombatSpriteColors.kBlue;
                if (RandomColor == 11 || RandomColor == 12)
                    color = CombatSprites.CombatSpriteColors.kPurple;
                else if (RandomColor == 13)
                    color = CombatSprites.CombatSpriteColors.kGreen;
            }
            //direction randomization
            if (RandomDirection == 1)
                direction = CombatSprites.Directions.kBottom;
            else if (RandomDirection == 2)
                direction = CombatSprites.Directions.kUp;
            else if (RandomDirection == 3)
                direction = CombatSprites.Directions.kLeft;
            else if (RandomDirection == 4)
                direction = CombatSprites.Directions.kRight;
            SpriteColor = color;
            Direction = direction;
            Screensize = screens;
            Size = siz;
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
            ///If the projectile is coming from the bottom of the screen, move it up
            if(Direction == Directions.kBottom)
            {
                velocity = new Vector2(velocity.X, -velocity.Y);
            }
            ///If it's coming from the top, move it down
            if (Direction == Directions.kTop)
            {
                velocity = new Vector2(velocity.X, -velocity.Y);
            }
            ///If it's coming from the left, move it right
            if (Direction == Directions.kLeft)
            {
                velocity = new Vector2(-velocity.X, velocity.Y);
            }
            ///If it's coming from the right, move it left
            if (Direction == Directions.kRight)
            {
                velocity = new Vector2(-velocity.X, velocity.Y);
            }
            ///adds the adjusted velocity to the current position
            position += velocity;
        }
    }
}
