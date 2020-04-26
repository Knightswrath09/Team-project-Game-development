using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeamProject
{
    class Menu
    {
        //string and positon of the menu header
        public string HeaderText { get; set; }
        public Vector2 HeaderPosition { get; set; }
        //number of options for certain menu
        public int NumOptions;
        //lists of options and positions for each option
        public List<string> Options = new List<string>();
        public List<Vector2> OptionPositions = new List<Vector2>();
        //sprite to show the user which option they current have selected
        public Texture2D Selector;
        public Vector2 SelectorSize;
        //indexes correspons with option positions
        public List<Vector2> SelectorPositions = new List<Vector2>();
        //corresponds with SelectorPositions
        public int Selection;
        public Vector2 SelectorPosition;

        public Menu(SpriteFont header, SpriteFont optionFont, string newHeader, List<string> newOptions, Texture2D newTexture,
            Vector2 newSelectorSize, Vector2 newScreenSize)
        {
            HeaderText = newHeader;
            HeaderPosition = new Vector2((newScreenSize.X / 2) - (header.MeasureString(newHeader).X / 2), newScreenSize.Y / 5);
            NumOptions = newOptions.Count;
            Options = newOptions;
            float YValue = HeaderPosition.Y + 100f;
            for (int i = 0; i < NumOptions; i++)
            {
                OptionPositions.Add(new Vector2(HeaderPosition.X, YValue));
                SelectorPositions.Add(new Vector2(HeaderPosition.X + optionFont.MeasureString(Options[i]).X + 75, YValue));
                YValue += 100f;
            }
            Selector = newTexture;
            SelectorSize = newSelectorSize;
            Selection = 0;
            SelectorPosition = SelectorPositions[0];
        }

        public void ChangeSelection(CombatSprites.Directions direction)
        {
            if(direction == CombatSprites.Directions.kTop)
            {
                if(Selection == 0)
                {
                    Selection = NumOptions - 1;
                }
                else
                {
                    Selection -= 1;
                }
            }
            else if(direction == CombatSprites.Directions.kBottom)
            {
                if(Selection == NumOptions - 1)
                {
                    Selection = 0;
                }
                else
                {
                    Selection += 1;
                }
            }
            SelectorPosition = SelectorPositions[Selection];
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Selector, SelectorPosition, Color.White);
        }
    }
}
