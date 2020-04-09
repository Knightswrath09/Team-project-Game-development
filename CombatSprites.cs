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
    public combatsprites(int color, int direction)
    {
        int RandomColor = Random(0, 14);
        int RandomDirection = Random(0, 5);
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
                color= CombatSpriteColors.kBlue;
            if (RandomColor == 11 || RandomColor == 12)
                color = CombatSprites.CombatSpriteColors.kPurple;
            else if (RandomColor == 13)
                color = CombatSprites.CombatSpriteColors.kGreen;
        }
        //directions
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
    }
}
