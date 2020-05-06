using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TeamProject
{
    //class for the star sprites which create a dynamic background
    class Stars
    {
        //texture for star sprite
        private Texture2D Texture;
        public Texture2D texture;

        //X and Y sectors divide the screen into 100 sectors, so the staring positions can be more easily calculated
        private int ySector;
        public int YSector
        {
            get { return ySector; }
            set { ySector = value; }
        }
        //x and y sectors have positions associated with them, requires only the sector numbers in the constructors to 
        //place the stars
        private List<float> SectorPositionsX = new List<float>();
        private List<float> SectorPositionsY = new List<float>();
        //when the stars cross the screen, this list maps each sector to another, so the stars 
        //dont just start on the other side of the screen
        //gives some sense of randomness
        private List<int> SectorMap = new List<int>() { 2, 6, 5, 0, 3, 8, 1, 9, 4, 7};
        //position variables
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
        //the stars will move at differnet speeds for a more interesting and realistic look
        private float speed;
        public float Speed
        {
            get { return speed; }
        }
        public int Index;
        private Vector2 screenSize;

        public Stars(Texture2D newTexture, int newYsector, int newX, float newSpeed, int newIndex, Vector2 newScreen)
        {
            texture = newTexture;
            ySector = newYsector - 1;
            speed = newSpeed;
            Index = newIndex;
            screenSize = newScreen;
            //set sector position lists for easier calculation of initial position, and new position when the star
            //exits the screen then enters from the other side of the screen
            for(int i = 0; i < 10; i++)
            {
                SectorPositionsY.Add(67 + (i * 180));
            }
            for (int j = 0; j < 10; j++)
                SectorPositionsX.Add(137 + (j * 320));

            //sets unutal positions with sectorposition lists, removes a lot of human calculations
            position = new Vector2(SectorPositionsX[newX - 1], SectorPositionsY[newYsector - 1]);

        }

        /// <summary>
        /// move stars to the right until they hit the edge of the screen
        /// the the stars map to a new ySector depending on their current Y sector, and start
        /// at the left side of the screen in their new ySector
        /// </summary>
        public void Move()
        {
            //if the star exited the screen
            if(position.X + speed >= screenSize.X + 46)
            {
                //set position to just off the left side of the screen, in the sector that their current sector maps to
                position = new Vector2(-46f, SectorPositionsY[SectorMap[ySector]]);
                //update sector by mapping current sector to next sector with SectorMap list
                ySector = SectorMap[ySector];
            }
            //if the star will not exit the scree, move it right at its specific velocity
            else
                position += new Vector2(speed, 0);
            
        }

        //draws stars
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
