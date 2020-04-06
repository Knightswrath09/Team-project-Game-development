using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamProject
{
    //this class is basically so that the color and direction of the projectiles and shields are in the same format; easier to compare
    class CombatSprites
    {
        public enum Directions
        {
            kTop = 0,
            kRight,
            kBottom,
            kLeft
        }
        private Directions direction;
        public Directions Direction
        {
            get { return direction; }
            set { direction = value; }
        }

        public enum CombatSpriteColors
        {
            kRed = 0,
            kBlue,
            kPurple,
            kGreen
        }
        private CombatSpriteColors spriteColor;
        public CombatSpriteColors SpriteColor
        {
            get { return spriteColor; }
            set { spriteColor = value; }
        }
    }
}
