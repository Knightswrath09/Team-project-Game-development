using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TeamProject
{
    class Stars
    {
        private Texture2D Texture;
        public Texture2D texture;

        //y sectors divide the screen, so that the stars stay spread out 
        private int ySector;
        public int YSector
        {
            get { return ySector; }
            set { ySector = value; }
        }
        private List<float> SectorPositionsX = new List<float>();
        private List<float> SectorPositionsY = new List<float>();
        //maps each sector to another, so the stars dont just start on the other side of the screen
        //gives some sense of randomness
        private List<int> SectorMap = new List<int>() { 2, 6, 5, 0, 3, 8, 1, 9, 4, 7};
        private Vector2 position;
        public Vector2 Position
        {
            get { return position; }
            set { position = value; }
        }
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
            for(int i = 0; i < 10; i++)
            {
                SectorPositionsY.Add(67 + (i * 180));
            }
            for (int j = 0; j < 10; j++)
                SectorPositionsX.Add(137 + (j * 320));

            position = new Vector2(SectorPositionsX[newX - 1], SectorPositionsY[newYsector - 1]);

        }

        public void Move()
        {
            if(position.X + speed >= screenSize.X + 46)
            {
                position = new Vector2(-46f, SectorPositionsY[SectorMap[ySector]]);
                ySector = SectorMap[ySector];
            }
            else
                position += new Vector2(speed, 0);
            
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }
    }
}
