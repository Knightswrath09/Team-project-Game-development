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
        //list of bools indicating which options are unlocked
        List<bool> unlocked = new List<bool>();

        //constructor for menu class
        public Menu(SpriteFont header, SpriteFont optionFont, string newHeader, List<string> newOptions, Texture2D newTexture,
            Vector2 newSelectorSize, Vector2 newScreenSize, int maxUnlocked)
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

            //adjust unlocked list for the highest level unlocked, this will only end up applying
            //to the level select menu
            for(int i = 0; i < maxUnlocked; i++)
            {
                unlocked.Add(true);
            }
            for(int j = maxUnlocked; j < NumOptions; j++)
            {
                unlocked.Add(false);
            }
            //add locked to the end of each level that is still locked
            for(int k = 0; k < NumOptions; k++)
            {
                if(unlocked[k] == false)
                {
                    Options[k] = Options[k] + " (locked)";
                }
            }
        }

        public void ChangeSelection(CombatSprites.Directions direction)
        {
            int nextSelection;
            if(direction == CombatSprites.Directions.kTop)
            {
                if(Selection == 0)
                {
                    nextSelection = NumOptions - 1;
                    while (unlocked[nextSelection] == false)
                        nextSelection--;
                    Selection = nextSelection;
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
                    nextSelection = Selection + 1;
                    while(unlocked[nextSelection] == false)
                    {
                        nextSelection++;
                        if (nextSelection == NumOptions - 1)
                            nextSelection = 0;
                    }
                    Selection = nextSelection;
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
