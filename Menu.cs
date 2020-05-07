using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TeamProject
{   //***Sophia
    //can create menus with different options and number of options from this class
    //includes a selector that shows the user which option they currently have selected
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
            //determines HeaderPosition based on the specific header string
            HeaderPosition = new Vector2((newScreenSize.X / 2) - (header.MeasureString(newHeader).X / 2), newScreenSize.Y / 3);
            //gets the number of options from the list of options argument
            NumOptions = newOptions.Count;
            //sets the Options list to the given newOptions list
            Options = newOptions;
            //temporary variable to evenly space each of the options from the header 
            float YValue = HeaderPosition.Y + 100f;
            for (int i = 0; i < NumOptions; i++)
            {
                //adds positons to OptionPositions list, which correlates to indeces of Options list
                OptionPositions.Add(new Vector2(HeaderPosition.X, YValue));
                //adds to list of positions for the selector, which correlates with both OptionPositions and Options lists
                SelectorPositions.Add(new Vector2(HeaderPosition.X + optionFont.MeasureString(Options[i]).X + 75, YValue));
                YValue += 100f;
            }
            //set selector texture and size
            Selector = newTexture;
            SelectorSize = newSelectorSize;
            //set the current selction to the option at position 0
            Selection = 0;
            //start the selector at option with index 0
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

        /// <summary>
        /// the direction argument is determined by user input in Game1 class
        /// moves to the other side of the menu when the user tries to move it above the first option or below the last option
        /// move to the top if they try to select a locked level
        /// </summary>
        /// <param name="direction">changes selection based on the direction that the user moves it</param>
        public void ChangeSelection(CombatSprites.Directions direction)
        {
            //stores the next selection, which is altered in the following conditions
            int nextSelection;
            if(direction == CombatSprites.Directions.kTop)
            {
                //if they try to move up while at the first option
                if(Selection == 0)
                {
                    //set next selection to the bottom option
                    nextSelection = NumOptions - 1;
                    //if the bottom selection is locked, keep moving next selection up until it reaches an unlocked level
                    while (unlocked[nextSelection] == false)
                        nextSelection--;
                    //set selection to next selection, after next selection has been updated
                    Selection = nextSelection;
                }
                //if they move up while the current selection is not at option 0, the selctor is able to just move up
                else
                {
                    Selection -= 1;
                }
            }
            //player moves selector down
            else if(direction == CombatSprites.Directions.kBottom)
            {
                //if they move down from the last option, select the first option
                if(Selection == NumOptions - 1)
                {
                    Selection = 0;
                }
                //if not at the last option, move down unless the level is locked
                else
                {
                    nextSelection = Selection + 1;
                    //if it is locked, move to first option
                    while(unlocked[nextSelection] == false)
                    {
                        nextSelection++;
                        if (nextSelection == NumOptions - 1)
                            nextSelection = 0;
                    }
                    //set selection to updated nextSelection
                    Selection = nextSelection;
                }
            }

            //Set the selector positon to the correct position depnding on the updated selection using SelectorPosition list
            SelectorPosition = SelectorPositions[Selection];
        }

        //draws selector sprite
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Selector, SelectorPosition, Color.White);
        }
    }
}
