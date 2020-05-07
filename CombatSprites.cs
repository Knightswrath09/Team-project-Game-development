using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject
{
    //***Sophia
    //this class is basically so that the color and direction of the projectiles and shields are in the same format
    //makes shields and projectiles easier to compare in CheckCollision function
    class CombatSprites
    {
        //each shield will be in one of these four directions repective to the ship
        //projectile will enter from one of the four sides of the screen
        public enum Directions
        {
            kTop = 0,
            kRight,
            kBottom,
            kLeft
        }
        //private and public direction variables
        private Directions direction;
        public Directions Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        //the shields will either be red, blue, or purple
        //projectiles will be red, blue, purple, or green
        //can easily compare shield and projectile colors to see if the shield blocks
        public enum CombatSpriteColors
        {
            kRed = 0,
            kBlue,
            kPurple,
            kGreen
        }
        //private and public color variables
        private CombatSpriteColors spriteColor;
        public CombatSpriteColors SpriteColor
        {
            get { return spriteColor; }
            set { spriteColor = value; }
        }
    }
}
