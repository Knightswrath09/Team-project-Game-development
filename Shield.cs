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
        //red shield 0, blue 1, purple 2
        private int indexInList;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }

        //0 = top, 1 = right, 2 = bottom, 3 = left (can use direction enumerator as index)
        //stores positions for each direction that the shield could be in
        //saves time because we dont have to use if states and repeat math to determine position
        private List<Vector2> PositionsByDirection;
        
        private Vector2 size;
        
        //for purple shields
        //red and blue visible and purple insivible when red and blue are not overalapping
        //red and blue not visible and purple visible when red and blue are overlapping
        public bool visible { get; set; }

        //***SOPHIE
        public Shield(CombatSpriteColors newColor, Directions newDirection, int newIndex, Texture2D newTexture, 
            Vector2 newSize, ShipSprite theShip)
        {
            SpriteColor = newColor;
            Direction = newDirection;
            indexInList = newIndex;
            texture = newTexture;
            size = newSize;

            PositionsByDirection[0] = new Vector2((theShip.Position.X + (theShip.Size.X / 2)) - (size.X / 2),
                theShip.Position.Y - (2 * size.Y));
            PositionsByDirection[1] = new Vector2(theShip.Position.X + theShip.Size.X + (2 * size.Y), )
            
            //texture and position will depend on color and direction respectively
        }

        //***SOPHIE
        public void MoveShield(List<Shield> otherShields)
        {
            //use input recieved in the game class to move respective shield to the correct position
            //check if shields overlap and change visibilty if needed 
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
