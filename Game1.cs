using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio; //for sound effects and music. Dustin
using System.Collections.Generic; //for list Sophia
using System;
using System.IO; //for text reader and writer Dustin



namespace TeamProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //GLOBAL VARIABLES THAT UPDATE THROUGH THE WHOLE GAME

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //***STORY
        //list for story strings between levels Dustin
        List<string> Stories = new List<string>() 
        {
            "Hello Officer Joseph Stalwart, We at the board of MEDEX would like to thank you for undertaking this mission to deliver\nhighly requested medical supplies to the blockaded world of Sedeth-18. We understand that this mission would normally\nbe reserved for employees who have demonstrated a higher score of competence on their SSATs tests, but please know,\nwe have full faith in you since you scored 677th out of all MEDEX employees, and we are 67% certain that you will succeed\nin this endeavor! We have also provided you with the Goodstar, a ship that is only a mere thrity-four years old and still\nhas a working shield generator that has as of yet to critically fail!\nWe have detected a hostile drone heading to your position, good luck!\nPress Enter on your keyboard or A on your controller to start...",
            "Hostile scouting party with energy and slug munitions detected. Secondary shield activated!\nThreat Assessment: You'll likely survive.\nPress Enter on your keyboard or A on your controller to start...",
            "Hostile fleet has commenced hostilities. Energy-charged munitions are positive. Activating trinary shielding system.\nWARNING: tertiary shielding can only be activated by suspending primary and secondary shielding. To activate tertiary\nshielding, combine primary and secondary shielding. Threat Assessment: Significant\nPress Enter on your keyboard or A on your controller to start...",
            "Approaching blockaded world. Hostile ships incoming from all directions.\nThreat Assessment: EVACUATE SHIP! THERE IS NO HOPE OF SURVIVAL!\nPress Enter on your keyboard or A on your controller to start...",
            "Officer Stalwart, judging from your recent sucess on the blockade of Sedeth-18, we have decided to task you with another\nmission of vital importance. We require you to deliver a bandage to a captain of a war vessel. This vessel is currently\nlocated in the middle of a warzone between two superpowers, but it's nothing you can't handle!\n[REDACTED MESSAGE FOR MEDEX BOARD EYES ONLY]: The Goodstar is a horribly old ship and its costing us more money to keep it\nactive than what it's bringing to us. Its lifetime warranty is about to expire in a month, so now's the time to have an \n\"unforseen desctruction of property\" so we can collect the insurance money on it.\nPress Enter on your keyboard or A on your controller to start...",
            "We are happy, Officer Stalwart, that you are above decent at your job, but if you really care about MEDEX and our values,\nwe'd like to collect that insurance money now, please.\nScoring system: Red and Blue: 100 Purple: 150 Green = -25\nPress Enter on your keyboard or A on your controller to start and eventaully blow up..."
        };
        //list for gregg voicelines accompanying each story string between levels Dustin
        List<SoundEffectInstance> GreggV = new List<SoundEffectInstance>() {};
        //bool to pause level until players read level start text.
        bool levelactive = false;

        //***LEVELS
        //level object for current level
        Level CurrentLevel;
        //integer to keep track of current level number
        int CurrentLevelNum;
        //access the current level object using CurrentLevelNum as the index
        List<Level> Levels = new List<Level>();
        //Levels
        Level Level1;
        Level Level2;
        Level Level3;
        Level Level4;
        Level Level5;
        Level Endless;
        //integer indicating highest level unlocked (1 = level 1, 2 = level 2 ... 6 = endless)
        //imported from MedExSave.txt in LoadContent
        int HighestUnlocked;
        //to make sure the level only changes once when a level is beat
        bool LevelChanged = false;

        //***SCORES
        //keep track of score for current game
        int CurrentScore;
        //reads high scores from text file in LoadContent method
        //used to compare players score with high scores, and updte them if necessary
        List<int> HighScores = new List<int>();
        //true if the player beat a high score so "New highscore!" message can be displayed in the Draw method
        bool NewHighScore = false;

        //***SOUND EFFECTS
        //genreal sound effects
        SoundEffect Hullhit; //Dustin
        SoundEffect HullCritical;//Dustin
        SoundEffect ProjectileFired;//Dustin
        SoundEffect ShieldBlock;//Dustin
        SoundEffect ShieldMove;//Dustin
        SoundEffect ShipBlowsUp;//Dustin
        SoundEffect VictoryJingle;//Dustin
        SoundEffect GameTheme;//Dustin
        SoundEffect MenuSound; //Iris
        SoundEffect MenuMusic;//Iris
        SoundEffect GreenSpawn;//Dustin
        SoundEffect Greenhit;//Dustin
        //Sound effects for GREGG, all voice acted by Dustin
        SoundEffect GREGG1;
        SoundEffect GREGG2;
        SoundEffect GREGG3;
        SoundEffect GREGG4;
        SoundEffect GREGG5;
        SoundEffect GREGGEndless;
        //sound effect instances
        SoundEffectInstance ShieldM; //sound effect instance of shieldmove soundeffect. Dustin
        SoundEffectInstance ShieldM2; //sound effect instance for blue shield. Dustin
        SoundEffectInstance Victory; //sound effect instance for victory song so it doesn't sound haunted. Dustin
        SoundEffectInstance ShipisGone; //sound effect instance for the ship blowing up Dustin
        SoundEffectInstance Hullcrit; //sound effect instance of HullCritical Dustin
        SoundEffectInstance GameTh; //sound effect instance of GameTheme Dustin
        SoundEffectInstance MenuSelect; //sound effect instance of cursor moving over menu options. Dustin
        SoundEffectInstance MenuM; //Iris
        SoundEffectInstance Greensp; //Dustin
        SoundEffectInstance Greenh;//Dustin
        //Sound effect instances for GREGG, Dustin
        SoundEffectInstance G1;
        SoundEffectInstance G2;
        SoundEffectInstance G3;
        SoundEffectInstance G4;
        SoundEffectInstance G5;
        SoundEffectInstance GE;
        //Soundeffect Loop disablers Dustin
        bool explode = true; //sound effect bool to ensure that the ShipisGone instance does not loop.
        //bool used to prevent gregg's voice from looping
        bool Greggtalk = true;

        //***FONTS
        Vector2 FontPos; //position of font Iris and Dustin
        Vector2 StoryPos; //position for story text Dustin
        //main font
        SpriteFont PixelFont; //Sophia
        //header font Sophia
        SpriteFont HeaderFont;

        //Menu objects for each menu Sophia
        Menu MainMenu;
        Menu SelectLevel;
        Menu PauseMenu;

        //mostly to determine if controls or volume menus direct back to pause menu or main menu Sophia
        bool GameStarted = false;

        string Winner; //message for winning the level Dustin
        string Loser; //message for losing the level Dustin
        
        //***SHIELDS AND PROJECTILES
        //shield objects
        Shield rShield;
        Shield bShield;
        Shield pShield;
        //list to keep track of current shields
        //0 = red, 1 = blue, 2 = purple
        List<Shield> CurrentShields = new List<Shield>();
        //list to keep track of current projectiles on screen
        List<Projectile> CurrentProjectiles = new List<Projectile>();
        //stores projectiles that have passed the shield not and were not blocked
        //keep these moving until they hit the ship then remove them from this list
        List<Projectile> unblockedProjectiles = new List<Projectile>();

        //***SHIP
        Vector2 shipSize = new Vector2(360, 253);
        Vector2 ShipPosition;
        ShipSprite ship;


        //***ENUMERATORS
        //enumerator for the possible states of the game
        enum ScreenState
        {
            kMain_Menu = 0,
            kControls,
            kPaused,
            kGame_Play,
            kHigh_Scores,
            kLevel_Select
        }
        //variable to store current screenstate, start with the main menu
        ScreenState CurrentScreenState = ScreenState.kMain_Menu;
        //enumerator to store the win status of the player
        enum WinStatus
        {
            kLevel_In_Progress = 0,
            kLose,
            kWin_Level,
            kWin_Game
        }
        //variable to store current winstatus, starts with a level in progress
        WinStatus CurrentWinStatus = WinStatus.kLevel_In_Progress;

        //list of stars in the background
        List<Stars> stars = new List<Stars>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            //screensize is 1800px by 3200px
            graphics.PreferredBackBufferHeight = 1800;
            graphics.PreferredBackBufferWidth = 3200;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            //load sound effects
            HullCritical = Content.Load<SoundEffect>("SirenandVoice"); //siren sound effect made by user Samsterbirdies on FreeSound.org Dustin
            ProjectileFired = Content.Load<SoundEffect>("Blast"); //laster-shots sound effect made by user theogobbo on FreeSound.org Dustin
            ShieldBlock = Content.Load<SoundEffect>("440783__wcoltd__pulsar");//pulsar sound effect made by user Wcoltd on FreeSound.org Dustin
            ShieldMove = Content.Load<SoundEffect>("274211__littlerobotsoundfactory__whoosh-electric-00");//Whoosh eletric sound effect made by user Littlerobotsoundfactory on Freesound.org Dustin
            Hullhit = Content.Load<SoundEffect>("111048__cyberkineticfilms__gunshot-with-metal-hit");//Gunshot with metal hit sound effect made by user Cyberkineticfilms on FreeSound.org Dustin
            ShipBlowsUp = Content.Load<SoundEffect>("244394__werra__bang-explosion-metallic");//Bang explosion metallic sound effect made by user Werra on FreeSound.org Dustin
            GameTheme = Content.Load<SoundEffect>("371516__mrthenoronha__space-game-theme-loop");//Space Game Loop sound effect by user Mrthenoronha on Freesound.org Dustin
            VictoryJingle = Content.Load<SoundEffect>("453296__xcreenplay__your-move-dream-boy-buchla-fif9th-131bpm");//Your Move Dream Boy sound effect by user Xcreenplay on Freesound.org Dustin
            MenuSound = Content.Load<SoundEffect>("menu-select");//cursor select sound effect, originally titled "cursor.mp3", made by user Loyalty_Freak_Music on Freesound.org Iris
            MenuMusic = Content.Load<SoundEffect>("menu-music");//menu background music, originally titled "Futuristic Rhythmic Game Ambience", made by user PatrickLieberkind on Freesound.org Iris
            GreenSpawn = Content.Load<SoundEffect>("55853__sergenious__teleport"); //teleport sound effect by user Sergenious on Freesound.org Dustin
            Greenhit = Content.Load<SoundEffect>("GreenHeal"); //improved healing chime sound effect by user Raclure on Freesound.org Dustin

            //load GREGG voiceLines Dustin
            GREGG1 = Content.Load<SoundEffect>("GreggLevel1");
            GREGG2 = Content.Load<SoundEffect>("VoiceGreggLevel2");
            GREGG3 = Content.Load<SoundEffect>("VoiceGregLevel3");
            GREGG4 = Content.Load<SoundEffect>("VoiceGreggLevel4");
            GREGG5 = Content.Load<SoundEffect>("VoiceGreggLevel5");
            GREGGEndless = Content.Load<SoundEffect>("VoiceGreggEndless");

            //sound effect instances
            ShieldM = ShieldMove.CreateInstance(); //Dustin
            ShieldM2 = ShieldMove.CreateInstance();//Dustin
            Victory = VictoryJingle.CreateInstance();//Dustin
            ShipisGone = ShipBlowsUp.CreateInstance();//Dustin
            Hullcrit = HullCritical.CreateInstance();//Dustin
            GameTh = GameTheme.CreateInstance();//Dustin
            MenuSelect = MenuSound.CreateInstance();//Iris
            MenuM = MenuMusic.CreateInstance();//Iris
            Greensp = GreenSpawn.CreateInstance();//Dustin
            Greenh = Greenhit.CreateInstance();//Dustin

            //sound effect instances for GREGG Dustin
            G1 = GREGG1.CreateInstance();
            G2 = GREGG2.CreateInstance();
            G3 = GREGG3.CreateInstance();
            G4 = GREGG4.CreateInstance();
            G5 = GREGG5.CreateInstance();
            GE = GREGGEndless.CreateInstance();

            //adds gregg's lines to the gregg list Dustin
            GreggV.Add(G1);
            GreggV.Add(G2);
            GreggV.Add(G3);
            GreggV.Add(G4);
            GreggV.Add(G5);
            GreggV.Add(GE);

            //load font, header font is larger Sophia
            PixelFont = Content.Load<SpriteFont>("PixelFont");
            HeaderFont = Content.Load<SpriteFont>("MenuHeader");

            //load strings for font Dustin
            Winner = "We are clear to warp! \nExcellent work, officer. \nPress Enter or A to continue.";
            Loser = "Well, that could have gone just a slight bit better.";

            //create each level Dustin
            Level1 = new Level(1, 10, Level.ProjectileTypes.kRed_Only, 3, 10);
            Level2 = new Level(2, 15, Level.ProjectileTypes.kRed_And_Blue, 2, 11);
            Level3 = new Level(3, 25, Level.ProjectileTypes.kRBP, 2, 12);
            Level4 = new Level(4, 40, Level.ProjectileTypes.kRBP, 2, 13);
            Level5 = new Level(5, 75, Level.ProjectileTypes.kRBP, 1, 15);
            Endless = new Level(6, 10, Level.ProjectileTypes.kRBP, 1, 15);
            //set current level to level 1 Sophia
            CurrentLevel = Level1;
            //this integer correlates with the Levels list Sophia
            CurrentLevelNum = 0;
            //Add each level to the Levels list, allows us to switch between levels more easily Sophia
            Levels.Add(Level1);
            Levels.Add(Level2);
            Levels.Add(Level3);
            Levels.Add(Level4);
            Levels.Add(Level5);
            Levels.Add(Endless);

            //create ship object
            ship = new ShipSprite("ship", Content.Load<Texture2D>("PlayerShip"), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), new Vector2(360f, 253f));

            //this is used in the shield contructors
            ShipPosition = new Vector2((graphics.PreferredBackBufferWidth / 2) - (shipSize.X / 2), (graphics.PreferredBackBufferHeight / 2) - (shipSize.Y / 2));
            
            //list that stores high scores, these are filler values that will be overwritten when 
            //real save values are read from MedExSave.txt
            HighScores = new List<int>() { 1000, 200, 100, 50, 50 };

            //read MedExSave.txt for HighestUnlocked and HighScores values
            //Dustin
            try
            {
                //code for reading from a text file from https://support.microsoft.com/en-us/help/816149/how-to-read-from-and-write-to-a-text-file-by-using-visual-c
                //Pass the text file to the stream reader
                StreamReader sr = new StreamReader(System.IO.Path.GetFullPath(@"..\MedExSave.txt"));

                //Read the first line of text
                string line = sr.ReadLine(); //string that holds the value of a line to pass it on to integer
                HighestUnlocked = Int32.Parse(line); //assigns level null to determine what level is unlocked
                while(line != null)
                    {
                    //reads the lines that store the five high scores
                    line = sr.ReadLine(); HighScores[0] = Int32.Parse(line);
                    line = sr.ReadLine(); HighScores[1] = Int32.Parse(line);
                    line = sr.ReadLine(); HighScores[2] = Int32.Parse(line);
                    line = sr.ReadLine(); HighScores[3] = Int32.Parse(line);
                    line = sr.ReadLine(); HighScores[4] = Int32.Parse(line);;
                    line = sr.ReadLine();//refrences a null and closes text file
                    }
                 

                //close the text file so it is avliable again when needed
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message); //exception/error message
                Exit();
            }
            finally
            {
                Console.WriteLine("Executing finally block."); //finally block
            }

            //initialize menus Sophia
            MainMenu = new Menu(HeaderFont, PixelFont, "Main Menu",
                new List<string>() { "How to Play", "Select Level", "High Scores", "Start Game" }, 
                Content.Load<Texture2D>("StarSprite"), new Vector2(56f, 56f), 
                new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), 4);
            SelectLevel = new Menu(HeaderFont, PixelFont, "Select Level", 
                new List<string>() { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Endless Mode"},
                Content.Load<Texture2D>("StarSprite"), new Vector2(56f, 56f),
                new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), HighestUnlocked);
            PauseMenu = new Menu(HeaderFont, PixelFont, "Pause Menu", 
                new List<string> { "How to Play", "Resume Game", "Return to Main Menu"}, Content.Load<Texture2D>("StarSprite"),
                new Vector2(56f, 56f),
                new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), 3);

            //Initalize the red, blue, and purple shield objects
            rShield = new Shield(CombatSprites.CombatSpriteColors.kRed, CombatSprites.Directions.kTop, 0,
                Content.Load<Texture2D>("RedShield"), new Vector2(56f, 253f), shipSize, ShipPosition);
            bShield = new Shield(CombatSprites.CombatSpriteColors.kBlue, CombatSprites.Directions.kBottom, 1,
                Content.Load<Texture2D>("BlueShield"), new Vector2(56f, 253f), shipSize, ShipPosition);
            pShield = new Shield(CombatSprites.CombatSpriteColors.kPurple, CombatSprites.Directions.kLeft, 2,
                Content.Load<Texture2D>("PurpleShield"), new Vector2(56f, 253f), shipSize, ShipPosition);

            //add shields to CurrentShields list
            //helpful when comparing projectiles and shield to determine if a projectile hits or is blocked
            CurrentShields.Add(rShield);
            CurrentShields.Add(bShield);
            CurrentShields.Add(pShield);

            //***Sophia
            //used in Stars constructor, easier than typing new Vector2(graphics.Preferred...) every time
            Vector2 screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            //initialize the 12 background stars and add to stars list
            Stars Star0 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 8, 1, 2f, 0, screenSize);
            stars.Add(Star0);
            Stars Star1 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 3, 2, 2f, 1, screenSize);
            stars.Add(Star1);
            Stars Star2 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 10, 2, 2f, 2, screenSize);
            stars.Add(Star2);
            Stars Star3 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 6, 3, 3f, 3, screenSize);
            stars.Add(Star3);
            Stars Star4 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 1, 4, 2f, 4, screenSize);
            stars.Add(Star4);
            Stars Star5 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 5, 5, 2f, 5, screenSize);
            stars.Add(Star5);
            Stars Star6 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 9, 6, 2f, 6, screenSize);
            stars.Add(Star6);
            Stars Star7 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 2, 7, 3f, 7, screenSize);
            stars.Add(Star7);
            Stars Star8 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 7, 8, 2f, 8, screenSize);
            stars.Add(Star8);
            Stars Star9 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 4, 9, 2f, 9, screenSize);
            stars.Add(Star9);
            Stars Star10 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 1, 10, 3f, 10, screenSize);
            stars.Add(Star10);
            Stars Star11 = new Stars(Content.Load<Texture2D>("StarSpriteBackground"), 10, 10, 3f, 11, screenSize);
            stars.Add(Star11);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        //
        //THESE FUNCTONS WILL BE TRIGGERED IN THE GAMEPLAY SECTION OF THE UPDATE FUNCTION
        //

        /// <summary>
        ///Create projectile with random color and direction according to the types of projectiles present in the 
        ///current level
        ///Add new projectile to the projectile list
        ///Use CurrentProjectiles.Count() to determine the index it was placed at, 
        ///Will be placed at the end of the list
        /// </summary>
        void CreateProjectile()
        {
            //***set up random number generators Dustin
            Random random = new Random();
            int RandomColor = random.Next(1, 15); //random for color
            int RandomDirection = random.Next(0, 4); //random for direction

            //stores color and direction determined from randomly generated number
            CombatSprites.CombatSpriteColors spriteColor;
            CombatSprites.Directions spriteDirection;

            //stores the new projectile created from random color and direction
            Projectile newProjectile;

            //color randomization
            if (CurrentLevel.TypesOfProjectiles == Level.ProjectileTypes.kRed_Only)
            {
                if (RandomColor <= 10)
                    spriteColor = CombatSprites.CombatSpriteColors.kRed;

                else //if (RandomColor <= 14)
                    spriteColor = CombatSprites.CombatSpriteColors.kGreen;
            }
            else if (CurrentLevel.TypesOfProjectiles == Level.ProjectileTypes.kRed_And_Blue)
            {
                if (RandomColor <= 7)
                    spriteColor = CombatSprites.CombatSpriteColors.kRed;
                else if (RandomColor <= 12)
                    spriteColor = CombatSprites.CombatSpriteColors.kBlue;
                else //if (RandomColor <= 14)
                    spriteColor = CombatSprites.CombatSpriteColors.kGreen;
            }
            else //if (CurrentLevel.TypesOfProjectiles == Level.ProjectileTypes.kRBP)
            {
                if (RandomColor <= 5)
                    spriteColor = CombatSprites.CombatSpriteColors.kRed;
                else if (RandomColor <= 11)
                    spriteColor = CombatSprites.CombatSpriteColors.kBlue;
                else if (RandomColor <= 13)
                    spriteColor = CombatSprites.CombatSpriteColors.kPurple;
                else //if (RandomColor <= 14)
                    spriteColor = CombatSprites.CombatSpriteColors.kGreen;
            }

            //direction randomization
            if (RandomDirection == 0)
            {
                spriteDirection = CombatSprites.Directions.kTop;
            }
            else if (RandomDirection == 1)
            {
                spriteDirection = CombatSprites.Directions.kRight;
            }
            else if (RandomDirection == 2)
            {
                spriteDirection = CombatSprites.Directions.kBottom;
            }
            else // if (RandomDirection == 3)
            {
                spriteDirection = CombatSprites.Directions.kLeft;
            }

            //load correct texture with the determined color
            //use determined direction in constructor
            //play ProjectileFired sound effect
            if(spriteColor == CombatSprites.CombatSpriteColors.kRed)
            {
                ProjectileFired.Play(1f, 0, 0);
                newProjectile = new Projectile(CurrentLevel, new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight),
                    new Vector2(56f, 56f), spriteDirection, spriteColor, Content.Load<Texture2D>("RedProjectile"));
            }
            else if(spriteColor == CombatSprites.CombatSpriteColors.kBlue)
            {
                ProjectileFired.Play(1f, 0, 0);
                newProjectile = new Projectile(CurrentLevel, new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight),
                    new Vector2(56f, 56f), spriteDirection, spriteColor, Content.Load<Texture2D>("BlueProjectile"));    
            }
            else if(spriteColor == CombatSprites.CombatSpriteColors.kPurple)
            {
                ProjectileFired.Play(1f, 0, 0);
                newProjectile = new Projectile(CurrentLevel, new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight),
                    new Vector2(56f, 56f), spriteDirection, spriteColor, Content.Load<Texture2D>("PurpleProjectile"));
            }
            else
            {
                GreenSpawn.Play(1f, 0, 0);
                newProjectile = new Projectile(CurrentLevel, new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight),
                    new Vector2(56f, 56f), spriteDirection, spriteColor, Content.Load<Texture2D>("GreenProjectile"));
            }

            //Add the new projectile to the list of projectiles on the screen
            CurrentProjectiles.Add(newProjectile);
            
        }

        /// <summary>
        /// Moves each of the projectiles cirrently on screen with CurrentProjectiles list and 
        /// projectile move function
        /// ***Caryln
        /// </summary>
        void MoveProjectiles()
        {

            for (int i = 0; i < CurrentProjectiles.Count; ++i)
                CurrentProjectiles[i].Move();
        }

        /// <summary>
        /// takes user input to call the shield class MoveShield method with correct direction argument
        /// ***keyboard controlls by Sophia, controller controls by Iris and Carlyn
        /// </summary>
        void MoveShields()
        {
            KeyboardState keyboardState = Keyboard.GetState();
            GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);
            //Checks if a controller is connected, and if so, implements the controller buttons for the shield
            if (gamePadState.IsConnected)
            {
                //DPad controls blue shield
                if (gamePadState.DPad.Up == ButtonState.Pressed)
                {
                    CurrentShields[1].MoveShield(CombatSprites.Directions.kTop, CurrentShields);
                    ShieldM.Play();
                }
                else if (gamePadState.DPad.Right == ButtonState.Pressed)
                {
                    CurrentShields[1].MoveShield(CombatSprites.Directions.kRight, CurrentShields);
                    ShieldM.Play();
                }
                else if (gamePadState.DPad.Down == ButtonState.Pressed)
                {
                    CurrentShields[1].MoveShield(CombatSprites.Directions.kBottom, CurrentShields);
                    ShieldM.Play();
                }
                else if (gamePadState.DPad.Left == ButtonState.Pressed)
                {
                    CurrentShields[1].MoveShield(CombatSprites.Directions.kLeft, CurrentShields);
                    ShieldM.Play();
                }

                //Buttons on the right control red shield
                if (gamePadState.Buttons.Y == ButtonState.Pressed)
                {
                    CurrentShields[0].MoveShield(CombatSprites.Directions.kTop, CurrentShields);
                    ShieldM2.Play();
                }
                else if (gamePadState.Buttons.B == ButtonState.Pressed)
                {
                    CurrentShields[0].MoveShield(CombatSprites.Directions.kRight, CurrentShields);
                    ShieldM2.Play();
                }
                else if (gamePadState.Buttons.A == ButtonState.Pressed)
                {
                    CurrentShields[0].MoveShield(CombatSprites.Directions.kBottom, CurrentShields);
                    ShieldM2.Play();
                }
                else if (gamePadState.Buttons.X == ButtonState.Pressed)
                {
                    CurrentShields[0].MoveShield(CombatSprites.Directions.kLeft, CurrentShields);
                    ShieldM2.Play();
                }
            }
            //arrow keys control red shield, which has index 0 in CurrentShield list
            if (keyboardState.IsKeyDown(Keys.Up))
               {
                CurrentShields[0].MoveShield(CombatSprites.Directions.kTop, CurrentShields);
                ShieldM.Play();
                }
            else if (keyboardState.IsKeyDown(Keys.Right))
                {
                CurrentShields[0].MoveShield(CombatSprites.Directions.kRight, CurrentShields);
                                ShieldM.Play();
                }
            else if (keyboardState.IsKeyDown(Keys.Down))
                {
                    CurrentShields[0].MoveShield(CombatSprites.Directions.kBottom, CurrentShields);
                                ShieldM.Play();
                }
            else if (keyboardState.IsKeyDown(Keys.Left))
                {
                        CurrentShields[0].MoveShield(CombatSprites.Directions.kLeft, CurrentShields);
                            ShieldM.Play();
                }

            //WASD keys control blue shield, index 1
            if (keyboardState.IsKeyDown(Keys.W))
                {
                CurrentShields[1].MoveShield(CombatSprites.Directions.kTop, CurrentShields);
                ShieldM2.Play();
                }
            else if (keyboardState.IsKeyDown(Keys.D))
               { CurrentShields[1].MoveShield(CombatSprites.Directions.kRight, CurrentShields);
                ShieldM2.Play();
                }
            else if (keyboardState.IsKeyDown(Keys.S))
               {
                CurrentShields[1].MoveShield(CombatSprites.Directions.kBottom, CurrentShields);
                ShieldM2.Play();
                }
            else if (keyboardState.IsKeyDown(Keys.A))
               { 
                CurrentShields[1].MoveShield(CombatSprites.Directions.kLeft, CurrentShields);
                ShieldM2.Play();
                }      
        }

        

        /// <summary>
        /// checks collision between projectiles, shields, and ship
        /// returns result of any collisions
        /// if it collides:
        /// -removes projectile from projectile list and set it to null
        /// -adds to number fired so far in current level object
        /// if it hits the ship:
        /// -add it to unblockedProjectiles list so it continues to move until it hits the ship
        /// -then removes it from unblockedProkectiles once it hits the ship
        /// -adds a hit to the ship if it is blue red or purple
        /// -power up if it is green
        /// ***Iris and Sophia
        /// </summary>
        void CheckCollision()
        {
            //stores the winstatus that will result from any events in this function
            WinStatus newWinStatus;
            //check each projectile in list with each shield in list
            //returns Winstatus inProgress if no change
            int blocked;
            bool green;
            //checks each projectile in CurrentProjectiles
            for (int a = 0; a < CurrentProjectiles.Count; a++) {
                //0 = hasn't reached shields, 1 = blocked, 2 = hit
                blocked = 0;
                //checks if the projectile is green, to trigger correct sound and change in HP
                if (CurrentProjectiles[a].SpriteColor == CombatSprites.CombatSpriteColors.kGreen)
                    green = true;
                else
                    green = false;

                //when the edge of the projectile passes the edge of the shield, it is either blocked or missed
                float projectileEdge;
                float shieldLine;

                //when the projectile is approaching from the top or the left, the projectile passes the shield when 
                //the projectileEdge value is greater than the shieldLine value
                //when projectile approaches from bottom or right, passes when less than.
                //this boolean should prevent from rewriting collision code
                bool greaterThan;

                //determine the shieldline, projectileedge and greaterthan bool depending on direction
                if (CurrentProjectiles[a].Direction == CombatSprites.Directions.kTop)
                {
                    projectileEdge = CurrentProjectiles[a].Position.Y + CurrentProjectiles[a].Size.Y;
                    shieldLine = ShipPosition.Y - (2 * 56);
                    greaterThan = true;
                }
                else if (CurrentProjectiles[a].Direction == CombatSprites.Directions.kRight)
                {
                    projectileEdge = CurrentProjectiles[a].Position.X;
                    shieldLine = ShipPosition.X + shipSize.X + (2 * 56);
                    greaterThan = false;
                }
                else if (CurrentProjectiles[a].Direction == CombatSprites.Directions.kBottom)
                {
                    projectileEdge = CurrentProjectiles[a].Position.Y;
                    shieldLine = ShipPosition.Y + shipSize.Y + (2 * 56);
                    greaterThan = false;
                }
                else //if (CurrentProjectiles[a].Direction == CombatSprites.Directions.kLeft)
                {
                    projectileEdge = CurrentProjectiles[a].Position.X + CurrentProjectiles[a].Size.X;
                    shieldLine = ShipPosition.X - (2 * 56);
                    greaterThan = true;
                }
                //if projectile passes the shield line, check if there is the correct shield there
                if ((greaterThan && projectileEdge >= shieldLine) || (!greaterThan && projectileEdge <= shieldLine))
                {
                    //check projectile with each shield
                    for (int i = 0; i < CurrentShields.Count; i++)
                    {
                        //blocked if projectile color and direction matches shield color and direction, and if the 
                        //shield is visible
                        //(visibility of a shield relates to the purple shield, which is insivible whenever the red and 
                        //blue shields are visible)
                        if ((CurrentProjectiles[a].Direction == CurrentShields[i].Direction) && (CurrentProjectiles[a].SpriteColor == CurrentShields[i].SpriteColor) && CurrentShields[i].visible)
                        {
                            //if the CurrentLevel is endless mode, count scores for each projectile
                            if (CurrentLevel == Endless && (CurrentProjectiles[a].SpriteColor == CombatSprites.CombatSpriteColors.kBlue ||
                                CurrentProjectiles[a].SpriteColor == CombatSprites.CombatSpriteColors.kRed))
                            {
                                CurrentScore += 100;
                            }
                            else if (CurrentLevel == Endless && CurrentProjectiles[a].SpriteColor == CombatSprites.CombatSpriteColors.kPurple)
                            {
                                CurrentScore += 150;
                            }
                            blocked = 1;
                            ShieldBlock.Play(1f, 0, 0);
                        }
                        //green will never match a shield color, we just have to check if it matches the direction of any visible shield
                        else if (green == true && (CurrentProjectiles[a].Direction == CurrentShields[i].Direction) && CurrentShields[i].visible)
                        {
                            blocked = 1;
                            ShieldBlock.Play(1f, 0, 0);
                        }
                        //hits ship if not blocked
                        if (blocked != 1)
                        {
                            blocked = 2;
                        }

                    }
                    if (blocked == 2)
                    {
                        //green is the only projectile that doesnt reduce HP
                        if (!green)
                        {
                            ship.HP--;
                        }
                        //if it is green, reduce score if in endless mode
                        else
                        {
                            if (CurrentLevel == Endless)
                            {
                                CurrentScore -= 25;
                            }
                        }

                        //emergency sound effect
                        if (ship.HP == 1)
                            Hullcrit.Play();

                        //so that unblocked projectiles dont stop before hitting the ship, but arent checked for collision with a shield
                        unblockedProjectiles.Add(CurrentProjectiles[a]);
                    }
                    
                    //remove projectile from Current projectiles list, regardless of it was a hit or miss
                    CurrentProjectiles[a] = null;
                    CurrentProjectiles.RemoveAt(a);

                    //stops level from ending if the player is playing in endless mode
                    if(CurrentLevel != Endless)
                        //level will end when FiredProjectiles == TotalProjectiles in the CurrentLevel object
                        CurrentLevel.FiredProjectiles++;
                }   
            }
            //player loses when the ships HP reaches 0
            if (ship.HP == 0)
            {
                newWinStatus = WinStatus.kLose;
            }
            //wins level when all of the projectiles of that level have been blocked or hit the ship
            else if (CurrentLevel.FiredProjectiles == CurrentLevel.TotalProjectiles)
            {
                //win the game if they beat level 5
                if (CurrentLevelNum == Levels.Count)
                    newWinStatus = WinStatus.kWin_Game;
                else
                    newWinStatus = WinStatus.kWin_Level;
                
            }
            //keep the level going if they dont lose or win the level
            else
                newWinStatus = WinStatus.kLevel_In_Progress;

            //check each projectile in unblockedProjectiles list
            for(int x = 0; x < unblockedProjectiles.Count; x++)
            {
                bool isGreen = false;
                //check if the projectile is green, again
                if (unblockedProjectiles[x].SpriteColor == CombatSprites.CombatSpriteColors.kGreen)
                    isGreen = true;
                //depending on direction, the position at which the projectile hits the ship is different
                if(unblockedProjectiles[x].Direction == CombatSprites.Directions.kTop)
                {
                    //position for direction = top
                    if(unblockedProjectiles[x].Position.Y + unblockedProjectiles[x].Velocity.Y >= 
                        ship.Position.Y + 67f - unblockedProjectiles[x].Size.Y)
                    {
                        //remove projectile from unblockedProjectiles list
                        unblockedProjectiles[x] = null;
                        unblockedProjectiles.RemoveAt(x);
                        //play hit ship noise if its not green
                        if (!isGreen)
                            Hullhit.Play();
                        //play powerup noise and increase HP if is is green
                        else
                        {
                            Greenhit.Play();
                            ship.HP++;
                        }
                    }
                    //if it doesnt reach the ship, it continues to move
                    else
                    {
                        unblockedProjectiles[x].Move();
                    }
                }
                //each other condition follows similar logic to the first
                else if(unblockedProjectiles[x].Direction == CombatSprites.Directions.kRight)
                {
                    if (unblockedProjectiles[x].Position.X + unblockedProjectiles[x].Velocity.X <=
                        ship.Position.X + ship.Size.X)
                    {
                        unblockedProjectiles[x] = null;
                        unblockedProjectiles.RemoveAt(x);
                        if(!isGreen)
                            Hullhit.Play(1f, 0, 0);
                        else
                        {
                            
                            Greenhit.Play();
                            ship.HP++;
                        }
                    }
                    else
                    {
                        unblockedProjectiles[x].Move();
                    }
                }
                else if(unblockedProjectiles[x].Direction == CombatSprites.Directions.kBottom)
                {
                    if (unblockedProjectiles[x].Position.Y + unblockedProjectiles[x].Velocity.Y <=
                        ship.Position.Y + ship.Size.Y - 67f)
                    {
                        unblockedProjectiles[x] = null;
                        unblockedProjectiles.RemoveAt(x);
                        if(!isGreen)
                            Hullhit.Play(1f, 0, 0);
                        else
                        {
                            Greenhit.Play();
                            ship.HP++;
                        }
                    }
                    else
                    {
                        unblockedProjectiles[x].Move();
                    }
                }
                else
                {
                    if (unblockedProjectiles[x].Position.X + unblockedProjectiles[x].Velocity.X >=
                        ship.Position.X - unblockedProjectiles[x].Size.X)
                    {
                        unblockedProjectiles[x] = null;
                        unblockedProjectiles.RemoveAt(x);
                        if(!isGreen)
                            Hullhit.Play(1f, 0, 0);
                        else
                        {
                            Greenhit.Play();
                            ship.HP++;
                        }
                    }
                    else
                    {
                        unblockedProjectiles[x].Move();
                    }
                }
            }
            
            //update winstatus
            CurrentWinStatus = newWinStatus;
        }


        //variables for time counting
        float countDuration = 1f; //one second
        float currentTime = 0f;
        int Counter = 0;

        //for menu selection, so clicking a button once only registers once until it is pressed again
        KeyboardState LastKeyboardState;
        GamePadState lastGamePadState;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //keyboard input used for menus
            KeyboardState CurrentKeyboardState = Keyboard.GetState();
            GamePadState CurrentGamePadState = GamePad.GetState(PlayerIndex.One);

           //***menu logic Sophia
            if (CurrentScreenState == ScreenState.kMain_Menu)
            {
                //play main menu music 
                MenuM.Play();
                //this bool dtermines whether the how to play screen returns to the pause or main menu
                GameStarted = false;
                levelactive = false;
                Greggtalk = true;
                //resets Hp to 3
                ship.HP = 3;
                //resets all levels to firedprojectiles = 0
                for (int i = 0; i < Levels.Count; i++)
                    Levels[i].FiredProjectiles = 0;
                //resets CurrentProjectiles and unblockedProjectiles to 0 elements
                for (int j = 0; j < CurrentProjectiles.Count; j++)
                {
                    CurrentProjectiles[j] = null;
                    CurrentProjectiles.RemoveAt(j);
                }
                for (int j = 0; j < unblockedProjectiles.Count; j++)
                {
                    unblockedProjectiles[j] = null;
                    unblockedProjectiles.RemoveAt(j);
                }
                //keyboard and controller input to change selection and select option
                //plays menuselect sound effect
                if ((CurrentKeyboardState.IsKeyDown(Keys.Up) && !LastKeyboardState.IsKeyDown(Keys.Up))
                     || (CurrentGamePadState.DPad.Up == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                    {
                        MainMenu.ChangeSelection(CombatSprites.Directions.kTop);
                        MenuSelect.Play();
                    }
                else if ((CurrentKeyboardState.IsKeyDown(Keys.Down) && !LastKeyboardState.IsKeyDown(Keys.Down)) 
                    || (CurrentGamePadState.DPad.Down == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                    {
                        MainMenu.ChangeSelection(CombatSprites.Directions.kBottom);
                        MenuSelect.Play();
                    }
                //uses current selection to navigate to approproate screenstate
                else if ((CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                    {
                        MenuSelect.Play();
                    {
                        if (MainMenu.Selection == 0)
                            CurrentScreenState = ScreenState.kControls;
                        else if (MainMenu.Selection == 1)
                            CurrentScreenState = ScreenState.kLevel_Select;
                        else if (MainMenu.Selection == 2)
                            CurrentScreenState = ScreenState.kHigh_Scores;
                        else if (MainMenu.Selection == 3)
                            CurrentScreenState = ScreenState.kGame_Play;
                    }
                }
                //sets last states equal to current states
                LastKeyboardState = CurrentKeyboardState;
                lastGamePadState = CurrentGamePadState;
            }

            //changes level according to inpute on level select screen
            //functions the same as the main menu, they are both objects of the Menu class
            else if (CurrentScreenState == ScreenState.kLevel_Select)
            {
                if ((CurrentKeyboardState.IsKeyDown(Keys.Up) && !LastKeyboardState.IsKeyDown(Keys.Up))
                    || (CurrentGamePadState.DPad.Up == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                {
                    MenuSelect.Play();
                    SelectLevel.ChangeSelection(CombatSprites.Directions.kTop);
                }
                else if ((CurrentKeyboardState.IsKeyDown(Keys.Down) && !LastKeyboardState.IsKeyDown(Keys.Down))
                    || (CurrentGamePadState.DPad.Down == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                {
                    SelectLevel.ChangeSelection(CombatSprites.Directions.kBottom);
                    MenuSelect.Play();
                }
                else if ((CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                {
                    MenuSelect.Play();
                    //sets appropriate level to current level, and the corresponding CurrentLevelNum
                    if (SelectLevel.Selection == 0)
                    {
                        CurrentLevel = Level1;
                        CurrentLevelNum = 0;
                    }
                    else if (SelectLevel.Selection == 1)
                    {
                        CurrentLevel = Level2;
                        CurrentLevelNum = 1;
                    }
                    else if (SelectLevel.Selection == 2)
                    {
                        CurrentLevel = Level3;
                        CurrentLevelNum = 2;
                    }
                    else if (SelectLevel.Selection == 3)
                    {
                        CurrentLevel = Level4;
                        CurrentLevelNum = 3;
                    }
                    else if (SelectLevel.Selection == 4)
                    {
                        CurrentLevel = Level5;
                        CurrentLevelNum = 4;
                    }
                    else if (SelectLevel.Selection == 5)
                    {
                        CurrentLevel = Endless;
                        CurrentLevelNum = 5;
                    }

                    //starts level after the level has been selected
                    CurrentScreenState = ScreenState.kGame_Play;

                }
                LastKeyboardState = CurrentKeyboardState;
                lastGamePadState = CurrentGamePadState;
            }

            //allows player to view saved high scores Sophia
            else if (CurrentScreenState == ScreenState.kHigh_Scores)
            {
                if ((CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                {
                    CurrentScreenState = ScreenState.kMain_Menu;
                    MenuSelect.Play();
                }
                lastGamePadState = CurrentGamePadState;
                LastKeyboardState = CurrentKeyboardState;
            }

            //***allows player to see the how to play screen Sophia
            //***how to screen designed by Sophia
            else if (CurrentScreenState == ScreenState.kControls)
            {
                //if the game has not been started, go back to main menu
                if (!GameStarted && (CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                {
                    CurrentScreenState = ScreenState.kMain_Menu;
                    MenuSelect.Play();
                }
                //***if the game has been started, go back to pause menu Sophia
                else if (GameStarted && (CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                {
                    CurrentScreenState = ScreenState.kPaused;
                    MenuSelect.Play();
                }

                LastKeyboardState = CurrentKeyboardState;
                lastGamePadState = CurrentGamePadState;
            }

            //player can pause at any point during gameplay to access pause menu
            //can see the how to play screen, resume game, or go to the main menu
            else if (CurrentScreenState == ScreenState.kPaused)
            {
                Hullcrit.Stop(); //ends siren and AI sound effect so it no longer plays when ship blows up. Dustin
                GameTh.Stop(); //ends the theme song Dustin
                Victory.Stop(); //ends victory Jingle Dustin
                if ((CurrentKeyboardState.IsKeyDown(Keys.Up) && !LastKeyboardState.IsKeyDown(Keys.Up))
                    || (CurrentGamePadState.DPad.Up == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                {
                    PauseMenu.ChangeSelection(CombatSprites.Directions.kTop);
                    MenuSelect.Play();
                }
                else if ((CurrentKeyboardState.IsKeyDown(Keys.Down) && !LastKeyboardState.IsKeyDown(Keys.Down))
                    || (CurrentGamePadState.DPad.Down == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                {
                    PauseMenu.ChangeSelection(CombatSprites.Directions.kBottom);
                    MenuSelect.Play();
                }
                else if ((CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                {
                    MenuSelect.Play();
                    {
                        if (PauseMenu.Selection == 0)
                            CurrentScreenState = ScreenState.kControls;
                        else if (PauseMenu.Selection == 1)
                            CurrentScreenState = ScreenState.kGame_Play;
                        else if (PauseMenu.Selection == 2)
                        {
                            CurrentScreenState = ScreenState.kMain_Menu;
                            CurrentLevelNum = 0;
                            CurrentLevel = Level1;
                        }
                    }
                }
                LastKeyboardState = CurrentKeyboardState;
                lastGamePadState = CurrentGamePadState;
            }

            //whenever the game is being played, ie. not in main menu, high scores, pause, how to play, or level select Sophia
            else if (CurrentScreenState == ScreenState.kGame_Play)
            {
                //***can pause with P key or right shoulder at any point Sophia
                if (CurrentKeyboardState.IsKeyDown(Keys.P) || CurrentGamePadState.Buttons.RightShoulder == ButtonState.Pressed)
                    CurrentScreenState = ScreenState.kPaused;
                //when level is in preogress, projectiles are being fired
                if (CurrentWinStatus == WinStatus.kLevel_In_Progress)
                {
                    //checks if level is active before starting
                    if (levelactive)
                    {
                        //this bool helps when the game is won, so that the level only changes once 
                        LevelChanged = false;
                        //this creates a new projectile at a frequency established in the currentlevel object
                        int Limit = CurrentLevel.FireFreq;
                        currentTime += (float)gameTime.ElapsedGameTime.TotalSeconds; //Time passed since last Update()
                        if (currentTime >= countDuration)
                        {
                            Counter++;
                            currentTime = 0f; // "use up" the time
                                                //any actions to perform
                        }
                        if (Counter >= Limit)
                        {
                            Counter = 0;//Reset the counter;
                            CreateProjectile();
                        }

                        //move current projectiles in CurrentProjectiles list with MoveProjectiles method (defined above in Game1)
                        MoveProjectiles();

                        //allow player to move shields with MoveShields method (defined above in Game1)
                        MoveShields();

                        //check collisions and changes the CurrentWinState accordingly
                        CheckCollision();
                    }
                    //***turns text off and allows gameplay Dustin
                    if(levelactive == false)
                    {
                        if (CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState || CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState)
                        {
                            levelactive = true; 
                            GameStarted = true;
                            GreggV[CurrentLevelNum].Stop();//stops gregg's voice when player begins level Dustin
                        }
                    }

                    LastKeyboardState = CurrentKeyboardState;
                    lastGamePadState = CurrentGamePadState;
                }

                //when ships HP reaches zero, they lose the game
                else if (CurrentWinStatus == WinStatus.kLose)
                {
                    //if they are in endless mode and their score has not been checked for high score yet
                    if((CurrentLevel == Endless) && !NewHighScore)
                    {
                        //check each score in high scores list to compare with current score
                        for(int i = 0; i < 5; i++)
                        {
                            //if the score is higher than the current element in HighScores
                            if(CurrentScore > HighScores[i] && !NewHighScore)
                            {
                                //move each score after and including the beaten high score down one, overwrites lowest of the high scores
                                for(int j = 3; j >= i; j--)
                                {
                                    HighScores[j + 1] = HighScores[j];
                                }
                                //inserts new high score
                                HighScores[i] = CurrentScore;
                                NewHighScore = true;

                                //code for writing to a text file is from https://support.microsoft.com/en-us/help/816149/how-to-read-from-and-write-to-a-text-file-by-using-visual-c
                                //write each element of the high score list to MedExSave.txt
                                //***Dustin
                                try{
                                //Pass the textfile path and name to the streamwriter
                                StreamWriter swhs = new StreamWriter(System.IO.Path.GetFullPath(@"..\MedExSave.txt")); //relative path to text file

                                //Writes text to text file
                                swhs.WriteLine(HighestUnlocked); //levels unlcoked
                                swhs.WriteLine(HighScores[0]);//high score 1
                                swhs.WriteLine(HighScores[1]);//high score 2
                                swhs.WriteLine(HighScores[2]);//high score 3
                                swhs.WriteLine(HighScores[3]);//high score 4
                                swhs.WriteLine(HighScores[4]);//high score 5

                                //Close the text file so it is avilable again when needed
                                swhs.Close();
                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine("Exception: " + e.Message); //exception/error message
                                }
                                finally 
                                {
                                    Console.WriteLine("Executing finally block."); //finally block
                                }
                            }
                        }
                    }
                    //can press enter or A to return to main menu after losing
                    if ((CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                    {
                        //resets all values alterd during gameplay
                        Hullcrit.Stop(); //ends siren and AI sound effect so it no longer plays when ship blows up. Dustin
                        GameTh.Stop(); //***ends the theme song Dustin
                        levelactive = false; //***Dustin
                        Greggtalk = true;//***Dustin
                        CurrentLevel = Level1;
                        CurrentLevelNum = 0;
                        CurrentScreenState = ScreenState.kMain_Menu;
                        CurrentWinStatus = WinStatus.kLevel_In_Progress;
                        ship.HP = 3;
                        NewHighScore = false;
                        CurrentScore = 0;
                    }
                    LastKeyboardState = CurrentKeyboardState;
                    lastGamePadState = CurrentGamePadState;
                }
                //***Sophia
                //advance to next level if they win the level
                else if (CurrentWinStatus == WinStatus.kWin_Level)
                {
                    //checks that the level will not be changed more than once
                    if (LevelChanged == false)
                    {
                        //empties unblockedProjectiles and CurrentProjectiles list before next level
                        for (int j = 0; j < unblockedProjectiles.Count; j++)
                        {
                            unblockedProjectiles[j] = null;
                            unblockedProjectiles.RemoveAt(j);
                        }
                        for (int j = 0; j < CurrentProjectiles.Count; j++)
                        {
                            CurrentProjectiles[j] = null;
                            CurrentProjectiles.RemoveAt(j);
                        }
                        //increases level num
                        CurrentLevelNum++;
                        //sets current level to level object at next index in Levels list
                        CurrentLevel = Levels[CurrentLevelNum];
                        //add 2 HP to the ship each time the player beats a levle
                        ship.HP += 2;
                        levelactive = false;
                        Greggtalk = true;
                        //unlock a new level if it was not unlocked already
                        if(HighestUnlocked < CurrentLevel.LevelNum)
                        {
                            HighestUnlocked = CurrentLevel.LevelNum;
                            SelectLevel = null;
                            //streamwriter opens save text file to update highest level unlocked
                            //writing to a text file code is from https://support.microsoft.com/en-us/help/816149/how-to-read-from-and-write-to-a-text-file-by-using-visual-c
                            //***Dustin
                            try{
                                //Pass the file name and path to the streamwriter
                                StreamWriter sw = new StreamWriter(System.IO.Path.GetFullPath(@"..\MedExSave.txt")); //relative path

                                //records highest unlocked level to first line of the document
                                 sw.WriteLine(HighestUnlocked); //levels unlcoked
                                sw.WriteLine(HighScores[0]);//high score 1
                                sw.WriteLine(HighScores[1]);//high score 2
                                sw.WriteLine(HighScores[2]);//high score 3
                                sw.WriteLine(HighScores[3]);//high score 4
                                sw.WriteLine(HighScores[4]);//high score 5
                                //closes the text file so it is free to use later
                                sw.Close();
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("Exception: " + e.Message); //exception/error message
                            }
                            finally 
                            {
                                Console.WriteLine("Executing finally block."); //finally block
                            }
                        }

                        //update select level menu so that the unlocked level can be selected and no longer has "(locked)" next to it Sophia
                        SelectLevel = new Menu(HeaderFont, PixelFont, "Select Level",
                            new List<string>() { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Endless Mode" },
                            Content.Load<Texture2D>("StarSprite"), new Vector2(56f, 56f),
                            new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), HighestUnlocked);
                        LevelChanged = true;
                    }
                    //go to next level when the player presses enter or A Sophia
                    if ((CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                    {
                        CurrentWinStatus = WinStatus.kLevel_In_Progress;
                        Victory.Stop();
                    }

                    LastKeyboardState = CurrentKeyboardState;
                    lastGamePadState = CurrentGamePadState;
                }
                //beating level five unlocks endless mode and wins the game, will show special win game message Sophia
                else if (CurrentWinStatus == WinStatus.kWin_Game)
                {
                    if ((CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                    {
                        Hullcrit.Stop(); //ends siren and AI sound effect so it no longer plays when ship blows up. Dustin
                        GameTh.Stop(); //***ends the theme song Dustin
                        if(HighestUnlocked != 6)
                        {
                            HighestUnlocked = 6;
                            if(HighestUnlocked < CurrentLevel.LevelNum)
                            {
                                HighestUnlocked = CurrentLevel.LevelNum;
                                //opens streamwriter to record the final level unlock to the text file
                                //code for reading from a text file from https://support.microsoft.com/en-us/help/816149/how-to-read-from-and-write-to-a-text-file-by-using-visual-c
                                //***Dustin
                                try{
                                    //Pass the text file name and path to the stream writer
                                    StreamWriter sw = new StreamWriter(System.IO.Path.GetFullPath(@"..\MedExSave.txt")); //relative path

                                    //writes integer 6 to the first line in textfile, the highest possible.
                                 sw.WriteLine(HighestUnlocked); //levels unlcoked
                                sw.WriteLine(HighScores[0]);//high score 1
                                sw.WriteLine(HighScores[1]);//high score 2
                                sw.WriteLine(HighScores[2]);//high score 3
                                sw.WriteLine(HighScores[3]);//high score 4
                                sw.WriteLine(HighScores[4]);//high score 5

                                        //Close the text file so it is avilable again when needed
                                        sw.Close();
                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine("Exception: " + e.Message);//exception/error message
                                }
                                finally 
                                {
                                    Console.WriteLine("Executing finally block."); //finally block
                                }
                            }
                            SelectLevel = null;
                            SelectLevel = new Menu(HeaderFont, PixelFont, "Select Level",
                            new List<string>() { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Endless Mode" },
                            Content.Load<Texture2D>("StarSprite"), new Vector2(56f, 56f),
                            new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), HighestUnlocked);
                        }
                        CurrentScreenState = ScreenState.kMain_Menu;
                        CurrentWinStatus = WinStatus.kLevel_In_Progress;
                        CurrentLevel = Level1;
                        CurrentLevelNum = 0;
                        ship.HP = 3;
                        for(int i = 0; i < Levels.Count; i++)
                        {
                            Levels[i].FiredProjectiles = 0;
                        }
                    }
                    LastKeyboardState = CurrentKeyboardState;
                    lastGamePadState = CurrentGamePadState;
                }
                
            }

            //move all of the stars to create synamic background
            for(int i = 0; i < stars.Count; i++)
            {
                stars[i].Move();
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            //***Sophia
            //draw the stars only if the player is in gameplay 
            if (CurrentScreenState == ScreenState.kGame_Play)
            {
                for (int i = 0; i < stars.Count; i++)
                {
                    stars[i].Draw(spriteBatch);
                }
            }

            //***Sophia handled darwing main menus
            //draw main menu
            if (CurrentScreenState == ScreenState.kMain_Menu)
            {
                //draw medex logo
                spriteBatch.Draw(Content.Load<Texture2D>("MedEx"),
                    new Vector2((graphics.PreferredBackBufferWidth / 2) - (956f / 2), 120f), Color.White);
                //draw main menu strings
                spriteBatch.DrawString(HeaderFont, MainMenu.HeaderText, MainMenu.HeaderPosition, Color.Yellow);
                for (int i = 0; i < MainMenu.NumOptions; i++)
                {
                    spriteBatch.DrawString(PixelFont, MainMenu.Options[i], MainMenu.OptionPositions[i], Color.White);
                }
                //draw mainmenu selector
                MainMenu.Draw(spriteBatch);
            }
            //draw level select menu
            else if (CurrentScreenState == ScreenState.kLevel_Select)
            {
                //draw level select strings
                spriteBatch.DrawString(HeaderFont, SelectLevel.HeaderText, SelectLevel.HeaderPosition, Color.Yellow);
                for (int i = 0; i < SelectLevel.NumOptions; i++)
                {
                    spriteBatch.DrawString(PixelFont, SelectLevel.Options[i], SelectLevel.OptionPositions[i], Color.White);
                }
                //draw selector
                SelectLevel.Draw(spriteBatch);
            }
            //draw pause menu
            else if (CurrentScreenState == ScreenState.kPaused)
            {
                //draw pause menu strings
                spriteBatch.DrawString(HeaderFont, PauseMenu.HeaderText, PauseMenu.HeaderPosition, Color.Yellow);
                for (int i = 0; i < PauseMenu.NumOptions; i++)
                {
                    spriteBatch.DrawString(PixelFont, PauseMenu.Options[i], PauseMenu.OptionPositions[i], Color.White);
                }
                //draw selector
                PauseMenu.Draw(spriteBatch);
            }
            //shows how to screen
            else if (CurrentScreenState == ScreenState.kControls)
            {
                //draw how to play bmp file
                spriteBatch.Draw(Content.Load<Texture2D>("HowToPlay"), new Vector2(0f, 0f), Color.White);
            }
            //displays high scores
            else if (CurrentScreenState == ScreenState.kHigh_Scores)
            {
                //writes high score header
                spriteBatch.DrawString(HeaderFont, "High Scores",
                    new Vector2((graphics.PreferredBackBufferWidth / 2) - (HeaderFont.MeasureString("High Scores").X / 2), 300), Color.Yellow);
                //displays each high score in the HighScores list
                for (int i = 0; i < 5; i++)
                {
                    spriteBatch.DrawString(PixelFont, HighScores[i].ToString(), 
                        new Vector2((graphics.PreferredBackBufferWidth / 2) - (PixelFont.MeasureString(HighScores[i].ToString()).X / 2), 400 + (i*100)), Color.White);
                }
            }

            //Draw logic for each winstatus during gameplay
            else if (CurrentScreenState == ScreenState.kGame_Play)
            {
                if (CurrentWinStatus == WinStatus.kLevel_In_Progress)
                {
                    //stop menu music
                    MenuM.Stop();
                    //draw the ship
                    ship.Draw(spriteBatch);
                    //play theme music
                    GameTh.Play();
                    //font and story text position
                    FontPos = new Vector2((graphics.GraphicsDevice.Viewport.Width / 15) - 150, (graphics.GraphicsDevice.Viewport.Height / 2) + 550);
                    StoryPos = new Vector2((graphics.GraphicsDevice.Viewport.Width/2 - 1600), (graphics.GraphicsDevice.Viewport.Height-1800));
                    //checks if the level is active
                    if (levelactive == false)
                    {
                        //draws story text and plays voicelines for the current level
                        spriteBatch.DrawString(PixelFont, Stories[CurrentLevelNum], StoryPos, Color.White);
                        if(Greggtalk)
                        {
                            GreggV[CurrentLevelNum].Play();
                            Greggtalk = false;
                        }
                    }

                    //draw hull status on screen
                    spriteBatch.DrawString(PixelFont, "HULL INTEGRITY: " + ship.HP, FontPos, Color.White);

                    //if not in endless mode, display current level
                    if (CurrentLevel != Endless)
                    {
                        //draw current level on screen
                        int levelDisplayed = CurrentLevel.LevelNum;
                        spriteBatch.DrawString(PixelFont, "Current Level: " + levelDisplayed, FontPos + new Vector2(0, 150), Color.White);
                    }
                    //if in endless mode, draw current score
                    else
                    {
                        spriteBatch.DrawString(PixelFont, "Current Score: " + CurrentScore, FontPos + new Vector2(0, 150), Color.White);
                    }

                    //draw function for each projectile in CurrentProjectiles and unblockedProjectiles
                    for (int i = 0; i < CurrentProjectiles.Count; i++)
                        CurrentProjectiles[i].Draw(spriteBatch);
                    for (int i = 0; i < unblockedProjectiles.Count; i++)
                        unblockedProjectiles[i].Draw(spriteBatch);
                    //draw function for each shield in current shields list
                    for (int j = 0; j < CurrentShields.Count; j++)
                    {
                        if (CurrentShields[j].visible)
                            CurrentShields[j].Draw(spriteBatch);
                    }
                }
                //when the player loses, draw lose message
                else if (CurrentWinStatus == WinStatus.kLose)
                {
                    Hullcrit.Stop(); //ends siren and AI sound effect so it no longer plays when ship blows up.
                    GameTh.Stop(); //ends the theme song
                    if (explode)
                    { ShipisGone.Play(); explode = false; } //plays instanced sound effect and disables looping.
                    //Defeat message
                    spriteBatch.DrawString(PixelFont, Loser + "\n Press enter or A to return to main menu or esc to quit", FontPos, Color.White);
                    //***Sophia
                    //if the player is in endless mode
                    //show them their final score and high scores
                    if(CurrentLevel == Endless)
                    {
                        spriteBatch.DrawString(PixelFont, "Your Score: " + CurrentScore,
                            new Vector2((graphics.PreferredBackBufferWidth / 2) - (PixelFont.MeasureString("Your Score: " + CurrentScore).X / 2), 300),
                            Color.White);
                        //if they beat the high score, notify them
                        if (NewHighScore)
                        {
                            spriteBatch.DrawString(PixelFont, "New High Score!",
                                new Vector2((graphics.PreferredBackBufferWidth / 2) - (PixelFont.MeasureString("New High Score!").X / 2), 350),
                                Color.White);
                            
                        }
                        spriteBatch.DrawString(HeaderFont, "High Scores:",
                                new Vector2((graphics.PreferredBackBufferWidth / 2) - (HeaderFont.MeasureString("High Scores:").X / 2), 450), Color.Yellow);
                        for (int i = 0; i < 5; i++)
                        {
                            spriteBatch.DrawString(PixelFont, HighScores[i].ToString(),
                                new Vector2((graphics.PreferredBackBufferWidth / 2) - (PixelFont.MeasureString(HighScores[i].ToString()).X / 2), 550 + (i * 100)), Color.White);
                        }
                    }
                    
                }

                //draw victory string
                else if (CurrentWinStatus == WinStatus.kWin_Level)
                {
                    Hullcrit.Stop(); //ends siren and AI sound effect so it no longer plays when ship is out of danger. Dustin
                    GameTh.Stop(); //***ends the theme song Dustin
                    ship.Draw(spriteBatch);
                    //***victory jingle Dustin
                    Victory.Play();
                    //***victory message Dustin
                    spriteBatch.DrawString(PixelFont, Winner, FontPos, Color.White);
                }

                else if (CurrentWinStatus == WinStatus.kWin_Game)
                {
                    spriteBatch.DrawString(PixelFont, "You Made It! Press Enter or A to return to main menu or esc to exit",
                        FontPos, Color.White);
                }
            } 
           

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
