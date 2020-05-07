using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics; //For Texture2D
using Microsoft.Xna.Framework; //for Vector2

namespace TeamProject
{
    //***Made by Sophia
    //class for the shield objects that the player will control
    //derived from CombatSPrites, so each shield will have a SpriteColor and Direction
    class Shield : CombatSprites
    {
        //red shield 0, blue 1, purple 2
        private int indexInList;
        private Texture2D texture;
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //0 = top, 1 = right, 2 = bottom, 3 = left (can use direction enumerator as index)
        //stores positions for each direction that the shield could be in
        //saves time because we dont have to use if states and repeat math to determine position
        private List<Vector2> PositionsByDirection = new List<Vector2>();
        
        private Vector2 size;

        //for rotation
        private Rectangle sourceRectangle;
        private Vector2 origin;
        private float angle;
        public float Angle
        {
            get { return angle; }
            set { angle = value; }
        }
        
        //for purple shields
        //red and blue visible and purple insivible when red and blue are not overalapping
        //red and blue not visible and purple visible when red and blue are overlapping
        public bool visible { get; set; }

        //***SOPHIE
        public Shield(CombatSpriteColors newColor, Directions newDirection, int newIndex, Texture2D newTexture, 
            Vector2 newSize, Vector2 newShipSize, Vector2 newShipPos)
        {
            SpriteColor = newColor;
            Direction = newDirection;
            indexInList = newIndex;
            texture = newTexture;
            size = newSize;

            //position for when the shield is at the top, index 0, note that the integer value for Directions.kTop is 0. This 
            //same concept is true for all of the elemments in PositionsByDirection
            PositionsByDirection.Add(new Vector2(newShipPos.X + (newShipSize.X / 2), 
                newShipPos.Y - newSize.X - (newSize.X / 2)));
            //position for shield on right, index 1
            PositionsByDirection.Add(new Vector2(newShipPos.X + newShipSize.X + (newSize.X * 1.5f), newShipPos.Y + (newShipSize.Y / 2)));
            //position for shield on bottom, index 2
            PositionsByDirection.Add(new Vector2(newShipPos.X + (newShipSize.X / 2), 
                newShipPos.Y + newShipSize.Y + newSize.X + (newSize.X / 2)));
            //position for shield on left, index 3
            PositionsByDirection.Add(new Vector2(newShipPos.X - (1.5f * newSize.X), newShipPos.Y + (newShipSize.Y / 2)));

            //rotate shield and change position according to direction
            if (Direction == Directions.kTop)
            {
                position = PositionsByDirection[0];
                angle = MathHelper.PiOver2;
            }
            else if (Direction == Directions.kRight)
            {
                position = PositionsByDirection[1];
                angle = 0f;
            }
            else if (Direction == Directions.kBottom)
            {
                position = PositionsByDirection[2];
                angle = MathHelper.PiOver2;
            }
            else if (Direction == Directions.kLeft)
            {
                position = PositionsByDirection[3];
                angle = 0f;
            }

            //set the purple shield to invisible when it is first created
            if (newColor == CombatSpriteColors.kPurple)
                visible = false;
            else
                visible = true;

            //for rotation
            sourceRectangle = new Rectangle(0, 0, (int)size.X, (int)size.Y);
            origin = new Vector2(size.X / 2, size.Y / 2);

        }

        /// <summary>
        /// changes direction of the shield
        /// checks if the blue and red overlap, and changes visibility of each the 3 shields accordingly
        /// </summary>
        /// <param name="newDirection">taken from use input in Game1. The shield will move to this direction</param>
        /// <param name="otherShields">used for purple shields and to change visbility</param>
        public void MoveShield(Directions newDirection, List<Shield> otherShields)
        {
            //sets direction of the shield to the direction determined by input from the Game1 class
            Direction = newDirection;
            //uses PositionsByDirection list to set the position with the integer value of the direction enumerator
            position = PositionsByDirection[(int)newDirection];
            //float to store new angle of the shield(s)
            float newAngle;
            //change newAngle according to new direction, shields on top and bottom are rotated, shields on left and right are not
            if (newDirection == Directions.kLeft || newDirection == Directions.kRight)
                newAngle = 0f;
            else
                newAngle = MathHelper.PiOver2;

            //sets the angle of the moved sprite to newAngle
            angle = newAngle;

            //this part checks if the two shields overlap after the shield is moved, and activates the purple shield if they are
            //if the red shield is being controlled
            if (indexInList == 0)
            {
                //if the blue shield is in the direction that the red shield is being moved to 
                if (otherShields[1].Direction == newDirection)
                {
                    //move the purple to the direction/position that the red shield is being moved to
                    otherShields[2].Direction = newDirection;
                    otherShields[2].Position = PositionsByDirection[(int)newDirection];
                    otherShields[2].Angle = newAngle;
                    //make red and blue shields invisible
                    this.visible = false;
                    otherShields[1].visible = false;
                    //make the purple shield invisible
                    otherShields[2].visible = true;
                    
                }
                //if the red and blue shields do not overlap with this move
                else
                {
                    //make red and blue visible, but purple not visible
                    this.visible = true;
                    otherShields[1].visible = true;
                    otherShields[2].visible = false;
                }
            }

            //same logic but for the blue shield
            else if (indexInList == 1)
            {
                if(otherShields[0].Direction == newDirection)
                {
                    otherShields[2].Direction = newDirection;
                    otherShields[2].Position = PositionsByDirection[(int)newDirection];
                    otherShields[2].Angle = newAngle;
                    this.visible = false;
                    otherShields[0].visible = false;
                    otherShields[2].visible = true;
                    
                }
                else
                {
                    this.visible = true;
                    otherShields[0].visible = true;
                    otherShields[2].visible = false;
                }
            }

            
        }

        //draws shield
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 1.0f);
        }
    }
}
