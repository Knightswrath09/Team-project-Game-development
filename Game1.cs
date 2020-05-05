using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio; //for sound effects and music.
using System.Collections.Generic; //for list
using System;
using System.IO;



namespace TeamProject
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        //GLOBAL VARIABLES THAT UPDATE THROUGH THE WHOLE GAME

        //integer to keep track of current level number
        int CurrentLevelNum = 1;


        List<string> Stories = new List<string>() 
        {
            "Hello Officer Joseph Stalwart, We at the board of MEDEX would like to thank you for undertaking this mission to deliver\nhighly requested medical supplies to the blockaded world of Sedeth-18. We understand that this mission would normally\nbe reserved for employees who have demonstrated a higher score of competence on their SSATs tests, but please know,\nwe have full faith in you since you scored 677th out of all MEDEX employees, and we are 67% certain that you will succeed\nin this endeavor! We have also provided you with the Goodstar, a ship that is only a mere thrity-four years old and still\nhas a working shield generator that has as of yet to critically fail!\nWe have detected a hostile drone heading to your position, good luck!\nPress Enter on your keyboard or A on your controller to start...",
            "Hostile scouting party with energy and slug munitions detected. Secondary shield activated!\nThreat Assessment: You'll likely survive.\nPress Enter on your keyboard or A on your controller to start...",
            "Hostile fleet has commenced hostilities. Energy-charged munitions are positive. Activating trinary shielding system.\nWARNING: tertiary shielding can only be activated by suspending primary and secondary shielding. To activate tertiary\nshielding, combine primary and secondary shielding. Threat Assessment: Significant\nPress Enter on your keyboard or A on your controller to start...",
            "Approaching blockaded world. Hostile ships incoming from all directions.\nThreat Assessment: EVACUATE SHIP! THERE IS NO HOPE OF SURVIVAL!\nPress Enter on your keyboard or A on your controller to start...",
            "Officer Stalwart, judging from your recent sucess on the blockade of Sedeth-18, we have decided to task you with another\nmission of vital importance. We require you to deliver a bandage to a captain of a war vessel. This vessel is currently\nlocated in the middle of a warzone between two superpowers, but it's nothing you can't handle!\n[REDACTED MESSAGE FOR MEDEX BOARD EYES ONLY]: The Goodstar is a horribly old ship and its costing us more money to keep it\nactive than what it's bringing to us. Its lifetime warranty is about to expire in a month, so now's the time to have an \n\"unforseen desctruction of property\" so we can collect the insurance money on it.\nPress Enter on your keyboard or A on your controller to start...",
            "We are happy, Officer Stalwart, that you are above decent at your job, but if you really care about MEDEX and our values,\nwe'd like to collect that insurance money now, please.\nScoring system: Red and Blue: 100 Purple: 150 Green = -25\nPress Enter on your keyboard or A on your controller to start and eventaully blow up..."
        };
        List<SoundEffectInstance> GreggV = new List<SoundEffectInstance>() {};
        //level object for current level
        //im thinking a list of all of the levels might be easier to handle
        //you would access the current level object using the current level integer as the index
        Level CurrentLevel;
        List<Level> Levels = new List<Level>();
        int maxLevel;
        //Levels
        Level Level1;
        Level Level2;
        Level Level3;
        Level Level4;
        Level Level5;
        Level Endless;
        //integer indicating highest level unlocked (1 = level 1, 2 = level 2 ... 6 = endless)
        //****WE NEED TO IMPORT THE VALUE FROM A FILE AT THE BEGINNING OF THE GAME
        int HighestUnlocked;
        
        //bool to pause level until players read level start text.
        bool levelactive = false;
        //bool used to prevent gregg's voice from looping
        bool Greggtalk = true;

        //to make sure the level only changes once when a level is beat
        bool LevelChanged = false;

        //keep track of score for current game
        int CurrentScore;

        //reads high scores from text file in LoadContent method
        //used to compare players score with high scores, and updte them if necessary
        List<int> HighScores = new List<int>();

        //true if the player beat a high score so "New highscore!" message can be displayed
        bool NewHighScore = false;


        //list to keep track of current projectiles on screen
        List<Projectile> CurrentProjectiles = new List<Projectile>();


        //sound effects
        SoundEffect Hullhit;
        SoundEffect HullCritical;
        SoundEffect ProjectileFired;
        SoundEffect ShieldBlock;
        SoundEffect ShieldMove;
        SoundEffect ShipBlowsUp;
        SoundEffect VictoryJingle;
        SoundEffect GameTheme;
        SoundEffect MenuSound;
        SoundEffect MenuMusic;
        SoundEffect GreenSpawn;
        SoundEffect Greenhit;

        //Sound effects for GREGG
        SoundEffect GREGG1;
        SoundEffect GREGG2;
        SoundEffect GREGG3;
        SoundEffect GREGG4;
        SoundEffect GREGG5;
        SoundEffect GREGGEndless;
        //sound effect instances
        SoundEffectInstance ShieldM; //sound effect instance of shieldmove soundeffect.
        SoundEffectInstance ShieldM2; //sound effect instance for blue shield.
        SoundEffectInstance Victory; //sound effect instance for victory song so it doesn't sound haunted.
        SoundEffectInstance ShipisGone; //sound effect instance for the ship blowing up
        SoundEffectInstance Hullcrit; //sound effect instance of HullCritical
        SoundEffectInstance GameTh; //sound effect instance of GameTheme
        SoundEffectInstance MenuSelect; //sound effect instance of cursor moving over menu options.
        SoundEffectInstance MenuM;
        SoundEffectInstance Greensp;
        SoundEffectInstance Greenh;

        //Sound effect instances for GREGG
         SoundEffectInstance G1;
         SoundEffectInstance G2;
         SoundEffectInstance G3;
         SoundEffectInstance G4;
         SoundEffectInstance G5;
         SoundEffectInstance GE;
       //Soundeffect Loop disablers
        bool explode = true; //sound effect bool to ensure that the ShipisGone instance does not loop.

        //font
        SpriteFont Font1; //font
        Vector2 FontPos; //position of font
        Vector2 StoryPos; //position for story text
        //main font
        SpriteFont PixelFont;
        //header font
        SpriteFont HeaderFont;

        //Menu objects for each menu
        Menu MainMenu;
        Menu SelectLevel;
        Menu PauseMenu;

        //mostly to determine if controls or volume menus direct back to pause menu or main menu
        bool GameStarted = false;

        string Winner; //message for winning the level
        string Loser; //message for losing the level
        

        Shield rShield;
        Shield bShield;
        Shield pShield;

        //list to keep track of current shields
        //0 = red, 1 = blue, 2 = purple
        List<Shield> CurrentShields = new List<Shield>();

        Vector2 shipSize = new Vector2(360, 253);
        Vector2 ShipPosition;

        ShipSprite ship;

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

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //list of stars in the background
        List<Stars> stars = new List<Stars>();

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

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
            HullCritical = Content.Load<SoundEffect>("SirenandVoice"); //siren sound effect made by user Samsterbirdies on FreeSound.org
            ProjectileFired = Content.Load<SoundEffect>("Blast"); //laster-shots sound effect made by user theogobbo on FreeSound.org
            ShieldBlock = Content.Load<SoundEffect>("440783__wcoltd__pulsar");//pulsar sound effect made by user Wcoltd on FreeSound.org
            ShieldMove = Content.Load<SoundEffect>("274211__littlerobotsoundfactory__whoosh-electric-00");//Whoosh eletric sound effect made by user Littlerobotsoundfactory on Freesound.org
            Hullhit = Content.Load<SoundEffect>("111048__cyberkineticfilms__gunshot-with-metal-hit");//Gunshot with metal hit sound effect made by user Cyberkineticfilms on FreeSound.org
            ShipBlowsUp = Content.Load<SoundEffect>("244394__werra__bang-explosion-metallic");//Bang explosion metallic sound effect made by user Werra on FreeSound.org
            GameTheme = Content.Load<SoundEffect>("371516__mrthenoronha__space-game-theme-loop");//Space Game Loop sound effect by user Mrthenoronha on Freesound.org
            VictoryJingle = Content.Load<SoundEffect>("453296__xcreenplay__your-move-dream-boy-buchla-fif9th-131bpm");//Your Move Dream Boy sound effect by user Xcreenplay on Freesound.org
            MenuSound = Content.Load<SoundEffect>("menu-select");//cursor select sound effect, originally titled "cursor.mp3", made by user Loyalty_Freak_Music on Freesound.org
            MenuMusic = Content.Load<SoundEffect>("menu-music");//menu background music, originally titled "Futuristic Rhythmic Game Ambience", made by user PatrickLieberkind on Freesound.org
            GreenSpawn = Content.Load<SoundEffect>("55853__sergenious__teleport"); //teleport sound effect by user Sergenious on Freesound.org
            Greenhit = Content.Load<SoundEffect>("GreenHeal"); //improved healing chime sound effect by user Raclure on Freesound.org

            //load GREGG voiceLines
            GREGG1 = Content.Load<SoundEffect>("GreggLevel1");
            GREGG2 = Content.Load<SoundEffect>("VoiceGreggLevel2");
            GREGG3 = Content.Load<SoundEffect>("VoiceGregLevel3");
            GREGG4 = Content.Load<SoundEffect>("VoiceGreggLevel4");
            GREGG5 = Content.Load<SoundEffect>("VoiceGreggLevel5");
            GREGGEndless = Content.Load<SoundEffect>("VoiceGreggEndless");

            //sound effect instances
            ShieldM = ShieldMove.CreateInstance();
            ShieldM2 = ShieldMove.CreateInstance();
            Victory = VictoryJingle.CreateInstance();
            ShipisGone = ShipBlowsUp.CreateInstance();
            Hullcrit = HullCritical.CreateInstance();
            GameTh = GameTheme.CreateInstance();
            MenuSelect = MenuSound.CreateInstance();
            MenuM = MenuMusic.CreateInstance();
            Greensp = GreenSpawn.CreateInstance();
            Greenh = Greenhit.CreateInstance();

            //sound effect instances for GREGG
            G1 = GREGG1.CreateInstance();
            G2 = GREGG2.CreateInstance();
            G3 = GREGG3.CreateInstance();
            G4 = GREGG4.CreateInstance();
            G5 = GREGG5.CreateInstance();
            GE = GREGGEndless.CreateInstance();

            //adds gregg's lines to the gregg list
            GreggV.Add(G1);
            GreggV.Add(G2);
            GreggV.Add(G3);
            GreggV.Add(G4);
            GreggV.Add(G5);
            GreggV.Add(GE);
            //load font
            Font1 = Content.Load<SpriteFont>("Courier New");

            //load font, header font is larger
            PixelFont = Content.Load<SpriteFont>("PixelFont");
            HeaderFont = Content.Load<SpriteFont>("MenuHeader");

            //load strings for font
            Winner = "We are clear to warp! \nExcellent work, officer. \nPress Enter to continue.";
            Loser = "Well, that could have gone just a slight bit better.";

            //create each level
            Level1 = new Level(1, 10, Level.ProjectileTypes.kRed_Only, 3, 10);
            Level2 = new Level(2, 15, Level.ProjectileTypes.kRed_And_Blue, 2, 11);
            Level3 = new Level(3, 25, Level.ProjectileTypes.kRBP, 2, 12);
            Level4 = new Level(4, 40, Level.ProjectileTypes.kRBP, 2, 13);
            Level5 = new Level(5, 75, Level.ProjectileTypes.kRBP, 1, 15);
            Endless = new Level(6, 10, Level.ProjectileTypes.kRBP, 1, 15);
            CurrentLevel = Level1;
            CurrentLevelNum = 0;
            Levels.Add(Level1);
            Levels.Add(Level2);
            Levels.Add(Level3);
            Levels.Add(Level4);
            Levels.Add(Level5);
            Levels.Add(Endless);

            maxLevel = 5;

            //create ship object
            ship = new ShipSprite("ship", Content.Load<Texture2D>("PlayerShip"), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), new Vector2(360f, 253f));

            ShipPosition = new Vector2((graphics.PreferredBackBufferWidth / 2) - (shipSize.X / 2), (graphics.PreferredBackBufferHeight / 2) - (shipSize.Y / 2));
                        //READ HIGH SCORES FROM TEXT FILE INTO HIGHSCORES LIST
            //CURRENT VALUES FOR TESTING PURPOSES
            HighScores = new List<int>() { 1000, 200, 100, 50, 50 };

            //READ HIGHESTUNLOCKED FROM FILE
            //current value is for testing purposes
            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(System.IO.Path.GetFullPath(@"..\MedExSave.txt"));

                //Read the first line of text
                string line = sr.ReadLine();
                HighestUnlocked = Int32.Parse(line);
                while(line != null)
                    {
                    line = sr.ReadLine(); HighScores[0] = Int32.Parse(line);
                    line = sr.ReadLine(); HighScores[1] = Int32.Parse(line);
                    line = sr.ReadLine(); HighScores[2] = Int32.Parse(line);
                    line = sr.ReadLine(); HighScores[3] = Int32.Parse(line);
                    line = sr.ReadLine(); HighScores[4] = Int32.Parse(line);;
                    line = sr.ReadLine();//refrences a null and closes text file
                    }
                 

                //close the file
                sr.Close();
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
                Exit();
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }



            //initialize menus
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


            rShield = new Shield(CombatSprites.CombatSpriteColors.kRed, CombatSprites.Directions.kTop, 0,
                Content.Load<Texture2D>("RedShield"), new Vector2(56f, 253f), shipSize, ShipPosition);
            bShield = new Shield(CombatSprites.CombatSpriteColors.kBlue, CombatSprites.Directions.kBottom, 1,
                Content.Load<Texture2D>("BlueShield"), new Vector2(56f, 253f), shipSize, ShipPosition);
            pShield = new Shield(CombatSprites.CombatSpriteColors.kPurple, CombatSprites.Directions.kLeft, 2,
                Content.Load<Texture2D>("PurpleShield"), new Vector2(56f, 253f), shipSize, ShipPosition);

            CurrentShields.Add(rShield);
            CurrentShields.Add(bShield);
            CurrentShields.Add(pShield);

            Vector2 screenSize = new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
            //initialize stars
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

            // TODO: use this.Content to load your game content here
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

        void CreateProjectile()
        {
            //create projectile with random color and direction according to the current level
            //add new projectile to the projectile list with CurrentProjectiles.Add(newProjectile)
            //use CurrentProjectiles.Count() to determine the index it was placed at, will be placed at the end of the list
            Random random = new Random();
            int RandomColor = random.Next(1, 15); //random for color
            int RandomDirection = random.Next(0, 4); //random for direction
            CombatSprites.CombatSpriteColors spriteColor;
            CombatSprites.Directions spriteDirection;
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


            int numProjectiles = CurrentProjectiles.Count;
            CurrentProjectiles.Add(newProjectile);
            
        }

        //moves all of the projectiles currently on the screen with Move() in projectile class
        //***CARLYN
        void MoveProjectiles()
        {

            for (int i = 0; i < CurrentProjectiles.Count; ++i)
                CurrentProjectiles[i].Move();
        }

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

        List<Projectile> unblockedProjectiles = new List<Projectile>();

        /// <summary>
        /// checks collision between projectiles, shields, and ship
        /// returns result of any collisions
        /// if it collides:
        /// -removes projectile from projectile list and set it to null
        /// -adds to number fired so far in current level object
        /// if it hits the ship:
        /// -adds a hit to the ship if it is blue red or purple
        /// -power up if it is green
        /// </summary>
        //***IRIS
        void CheckCollision()
        {
            WinStatus newWinStatus;
            //check each projectile in list with each shield in list
            //returns Winstatus inProgress if no change
            int blocked;
            bool green;
            for (int a = 0; a < CurrentProjectiles.Count; a++) {
                ///0 = hasn't reached shields, 1 = blocked, 2 = hit
                blocked = 0;
                if (CurrentProjectiles[a].SpriteColor == CombatSprites.CombatSpriteColors.kGreen)
                    green = true;
                else
                    green = false;
                //when the edge of the projectile passes the edge of the shield, it is either blocked or missed
                float projectileEdge;
                float shieldLine;

                //when the projectile is approaching from the top or the left, the projectile passes the shield when 
                //the projectile edge is greater tha the shieldLine
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
                    for (int i = 0; i < CurrentShields.Count; i++)
                    {

                        //blocked
                        if ((CurrentProjectiles[a].Direction == CurrentShields[i].Direction) && (CurrentProjectiles[a].SpriteColor == CurrentShields[i].SpriteColor) && CurrentShields[i].visible)
                        {
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
                        else if (green == true && (CurrentProjectiles[a].Direction == CurrentShields[i].Direction))
                        {
                            blocked = 1;
                            ShieldBlock.Play(1f, 0, 0);
                        }
                        //hits ship
                        if (blocked != 1)
                        {
                            blocked = 2;
                        }

                    }
                    if (blocked == 2)
                    {
                        if (!green)
                        {
                            ship.HP--;
                        }
                        else
                        {
                            ship.HP++;
                            Greenhit.Play();
                            if (CurrentLevel == Endless)
                            {
                                CurrentScore -= 25;
                            }

                            //replace with powerup sound effect
                        }

                        //emergency sound effect
                        if (ship.HP == 1)
                            Hullcrit.Play();

                        //so that unblocked projectiles dont stop before hitting the ship
                        unblockedProjectiles.Add(CurrentProjectiles[a]);
                    }
                    
                    CurrentProjectiles[a] = null;
                    CurrentProjectiles.RemoveAt(a);
                    
                    
                    //stops level from ending if the player is playing in endless mode
                    if(CurrentLevel != Endless)
                        CurrentLevel.FiredProjectiles++;
                }
                
            }
            if (ship.HP == 0)
            {
                newWinStatus = WinStatus.kLose;
            }
            else if (CurrentLevel.FiredProjectiles == CurrentLevel.TotalProjectiles)
            {
                if (CurrentLevelNum == Levels.Count)
                    newWinStatus = WinStatus.kWin_Game;
                else
                    newWinStatus = WinStatus.kWin_Level;
                
            }
            else
                newWinStatus = WinStatus.kLevel_In_Progress;

            for(int x = 0; x < unblockedProjectiles.Count; x++)
            {
                if(unblockedProjectiles[x].Direction == CombatSprites.Directions.kTop)
                {
                    if(unblockedProjectiles[x].Position.Y + unblockedProjectiles[x].Velocity.Y >= 
                        ship.Position.Y + 67f - unblockedProjectiles[x].Size.Y)
                    {
                        unblockedProjectiles[x] = null;
                        unblockedProjectiles.RemoveAt(x);
                        Hullhit.Play();
                        
                    }
                    else
                    {
                        unblockedProjectiles[x].Move();
                    }
                }
                else if(unblockedProjectiles[x].Direction == CombatSprites.Directions.kRight)
                {
                    if (unblockedProjectiles[x].Position.X + unblockedProjectiles[x].Velocity.X <=
                        ship.Position.X + ship.Size.X)
                    {
                        unblockedProjectiles[x] = null;
                        unblockedProjectiles.RemoveAt(x);
                        Hullhit.Play(1f, 0, 0);
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
                        Hullhit.Play(1f, 0, 0);
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
                        Hullhit.Play(1f, 0, 0);
                    }
                    else
                    {
                        unblockedProjectiles[x].Move();
                    }
                }
            }
            
            CurrentWinStatus = newWinStatus;
        }


        //variables for time counting
        float countDuration = 1f; //one second
        float currentTime = 0f;
        int Counter = 0;

        //for menu selection
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

           //main menu logic
            if (CurrentScreenState == ScreenState.kMain_Menu)
            {
                MenuM.Play();
                GameStarted = false;
                levelactive = false;
                Greggtalk = true;
                ship.HP = 3;
                //resets all levels to firedprojectiles = 0
                for (int i = 0; i < Levels.Count; i++)
                    Levels[i].FiredProjectiles = 0;
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
                LastKeyboardState = CurrentKeyboardState;
                lastGamePadState = CurrentGamePadState;
            }

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


                    CurrentScreenState = ScreenState.kGame_Play;

                }
                LastKeyboardState = CurrentKeyboardState;
                lastGamePadState = CurrentGamePadState;
            }

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

            else if (CurrentScreenState == ScreenState.kControls)
            {
                if (!GameStarted && (CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                {
                    CurrentScreenState = ScreenState.kMain_Menu;
                    MenuSelect.Play();
                }
                else if (GameStarted && (CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                {
                    CurrentScreenState = ScreenState.kPaused;
                    MenuSelect.Play();
                }

                LastKeyboardState = CurrentKeyboardState;
                lastGamePadState = CurrentGamePadState;
            }

            else if (CurrentScreenState == ScreenState.kPaused)
            {
                Hullcrit.Stop(); //ends siren and AI sound effect so it no longer plays when ship blows up.
                GameTh.Stop(); //ends the theme song
                Victory.Stop(); //ends victory Jingle
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

            else if (CurrentScreenState == ScreenState.kGame_Play)
            {

                if (CurrentKeyboardState.IsKeyDown(Keys.P) || CurrentGamePadState.Buttons.RightShoulder == ButtonState.Pressed)
                    CurrentScreenState = ScreenState.kPaused;
                if (CurrentWinStatus == WinStatus.kLevel_In_Progress)
                {
                    if(levelactive)
                        {
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

                   //checks if level is active before starting
                    //move current projectiles in CurrentProjectiles list
                    MoveProjectiles();

                    //allow player to move shields
                    MoveShields();


                    //check collisions and if the the winstate changes
                    //returns Winstatus inProgress if no change
                    //returns winstatus lose if hp of ship == 0
                    //returns winstatus win level if currentlevel firedprojectiles == totalprojectiles
                    //returns winstatus win game if currentlevel firedprojectiles == totalprojectiles and currentlevel == maxlevel
                    CheckCollision();
                        }
                   //turns text off and allows gameplay
                    if(levelactive == false)
                    {
                     if (CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState || CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState)
                    {levelactive = true; 
                                     GameStarted = true;
                            GreggV[CurrentLevelNum].Stop();//stops gregg's voice when player begins level
                            }
                    }
                                        LastKeyboardState = CurrentKeyboardState;
                    lastGamePadState = CurrentGamePadState;
                    //allow player to pause game
                }
                else if (CurrentWinStatus == WinStatus.kLose)
                {
                    if((CurrentLevel == Endless) && !NewHighScore)
                    {
                        for(int i = 0; i < 5; i++)
                        {
                            if(CurrentScore > HighScores[i] && !NewHighScore)
                            {
                                for(int j = 3; j >= i; j--)
                                {
                                    
                                    HighScores[j + 1] = HighScores[j];

                                }
                                HighScores[i] = CurrentScore;
                                NewHighScore = true;

                                //WRITE EACH ELEMENT OF THE UPDATED HIGHSCORES LIST TO THE FILE
                                                            try{
                                //Pass the filepath and filename to the StreamWriter Constructor
                                StreamWriter swhs = new StreamWriter(System.IO.Path.GetFullPath(@"..\MedExSave.txt"));

                                //Write a line of text
                                swhs.WriteLine(HighestUnlocked);
                                    swhs.WriteLine(HighScores[0]);
                                    swhs.WriteLine(HighScores[1]);
                                    swhs.WriteLine(HighScores[2]);
                                    swhs.WriteLine(HighScores[3]);
                                    swhs.WriteLine(HighScores[4]);

                                //Close the file
                                swhs.Close();
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("Exception: " + e.Message);
                            }
                            finally 
                            {
                                Console.WriteLine("Executing finally block.");
                            }
                            }
                        }
                    }
                    if ((CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                    {
                        Hullcrit.Stop(); //ends siren and AI sound effect so it no longer plays when ship blows up.
                        GameTh.Stop(); //ends the theme song
                        levelactive = false;
                        Greggtalk = true;
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

                else if (CurrentWinStatus == WinStatus.kWin_Level)
                {
                    if (LevelChanged == false)
                    {
                        for (int j = 0; j < unblockedProjectiles.Count; j++)
                        {
                            unblockedProjectiles[j] = null;
                            unblockedProjectiles.RemoveAt(j);
                        }
                        CurrentLevelNum++;
                        CurrentLevel = Levels[CurrentLevelNum];
                        ship.HP += 2;
                        levelactive = false;
                        Greggtalk = true;
                        if(HighestUnlocked < CurrentLevel.LevelNum)
                        {
                            HighestUnlocked = CurrentLevel.LevelNum;
                            SelectLevel = null;
                            try{
                                //Pass the filepath and filename to the StreamWriter Constructor
                                StreamWriter sw = new StreamWriter(System.IO.Path.GetFullPath(@"..\MedExSave.txt"));

                                //Write a line of text
                                sw.WriteLine(HighestUnlocked);

                                //Write a second line of text

                                //Close the file
                                sw.Close();
                            }
                            catch(Exception e)
                            {
                                Console.WriteLine("Exception: " + e.Message);
                            }
                            finally 
                            {
                                Console.WriteLine("Executing finally block.");
                            }
                        }

                        SelectLevel = new Menu(HeaderFont, PixelFont, "Select Level",
                            new List<string>() { "Level 1", "Level 2", "Level 3", "Level 4", "Level 5", "Endless Mode" },
                            Content.Load<Texture2D>("StarSprite"), new Vector2(56f, 56f),
                            new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), HighestUnlocked);
                        LevelChanged = true;


                    }
                    if ((CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                    {
                        CurrentWinStatus = WinStatus.kLevel_In_Progress;
                        Victory.Stop();
                    }

                    LastKeyboardState = CurrentKeyboardState;
                    lastGamePadState = CurrentGamePadState;
                }
                else if (CurrentWinStatus == WinStatus.kWin_Game)
                {
                    if ((CurrentKeyboardState.IsKeyDown(Keys.Enter) && CurrentKeyboardState != LastKeyboardState)
                    || (CurrentGamePadState.Buttons.A == ButtonState.Pressed && CurrentGamePadState != lastGamePadState))
                    {
                        Hullcrit.Stop(); //ends siren and AI sound effect so it no longer plays when ship blows up.
                        GameTh.Stop(); //ends the theme song
                        if(HighestUnlocked != 6)
                        {
                            HighestUnlocked = 6;
                            if(HighestUnlocked < CurrentLevel.LevelNum)
                            {
                                HighestUnlocked = CurrentLevel.LevelNum;
                                SelectLevel = null;
                                try{
                                    //Pass the filepath and filename to the StreamWriter Constructor
                                    StreamWriter sw = new StreamWriter(System.IO.Path.GetFullPath(@"..\MedExSave.txt"));

                                    //Write a line of text
                                    sw.WriteLine(HighestUnlocked);

                                    //Write a second line of text

                                        //Close the file
                                        sw.Close();
                                }
                                catch(Exception e)
                                {
                                    Console.WriteLine("Exception: " + e.Message);
                                }
                                finally 
                                {
                                    Console.WriteLine("Executing finally block.");
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

            if (CurrentScreenState == ScreenState.kGame_Play)
            {
                for (int i = 0; i < stars.Count; i++)
                {
                    stars[i].Draw(spriteBatch);
                }
            }

            if (CurrentScreenState == ScreenState.kMain_Menu)
            {
                //draw medex logo
                spriteBatch.Draw(Content.Load<Texture2D>("MedEx"),
                    new Vector2((graphics.PreferredBackBufferWidth / 2) - (956f / 2), 120f), Color.White);
                //draw main menu string
                spriteBatch.DrawString(HeaderFont, MainMenu.HeaderText, MainMenu.HeaderPosition, Color.Yellow);
                for (int i = 0; i < MainMenu.NumOptions; i++)
                {
                    spriteBatch.DrawString(PixelFont, MainMenu.Options[i], MainMenu.OptionPositions[i], Color.White);
                }
                //draw mainmenu selector
                MainMenu.Draw(spriteBatch);
            }
            else if (CurrentScreenState == ScreenState.kLevel_Select)
            {
                spriteBatch.DrawString(HeaderFont, SelectLevel.HeaderText, SelectLevel.HeaderPosition, Color.Yellow);
                for (int i = 0; i < SelectLevel.NumOptions; i++)
                {
                    spriteBatch.DrawString(PixelFont, SelectLevel.Options[i], SelectLevel.OptionPositions[i], Color.White);
                }
                SelectLevel.Draw(spriteBatch);
            }
            else if (CurrentScreenState == ScreenState.kPaused)
            {
                spriteBatch.DrawString(HeaderFont, PauseMenu.HeaderText, PauseMenu.HeaderPosition, Color.Yellow);
                for (int i = 0; i < PauseMenu.NumOptions; i++)
                {
                    spriteBatch.DrawString(PixelFont, PauseMenu.Options[i], PauseMenu.OptionPositions[i], Color.White);
                }
                PauseMenu.Draw(spriteBatch);
            }
            else if (CurrentScreenState == ScreenState.kControls)
            {
                //draw how to play bmp file
                spriteBatch.Draw(Content.Load<Texture2D>("HowToPlay"), new Vector2(0f, 0f), Color.White);
            }
            else if (CurrentScreenState == ScreenState.kHigh_Scores)
            {
                //show high scores
                spriteBatch.DrawString(HeaderFont, "High Scores",
                    new Vector2((graphics.PreferredBackBufferWidth / 2) - (HeaderFont.MeasureString("High Scores").X / 2), 300), Color.Yellow);
                for (int i = 0; i < 5; i++)
                {
                    spriteBatch.DrawString(PixelFont, HighScores[i].ToString(), 
                        new Vector2((graphics.PreferredBackBufferWidth / 2) - (PixelFont.MeasureString(HighScores[i].ToString()).X / 2), 400 + (i*100)), Color.White);
                }
            }
            else if (CurrentScreenState == ScreenState.kGame_Play)
            {
                if (CurrentWinStatus == WinStatus.kLevel_In_Progress)
                {
                    MenuM.Stop();
                    //draw the ship
                    ship.Draw(spriteBatch);
                    //play theme music
                    GameTh.Play();
                    //font position
                    FontPos = new Vector2((graphics.GraphicsDevice.Viewport.Width / 15) - 150, (graphics.GraphicsDevice.Viewport.Height / 2) + 550);
                    StoryPos = new Vector2((graphics.GraphicsDevice.Viewport.Width/2 - 1600), (graphics.GraphicsDevice.Viewport.Height-1800));
                    if (levelactive == false)
                        {
                        spriteBatch.DrawString(PixelFont, Stories[CurrentLevelNum], StoryPos, Color.White);
                        if(Greggtalk)
                        {
                        GreggV[CurrentLevelNum].Play();
                            Greggtalk = false;
                            }
                        }
                    //draw hull status on screen
                    spriteBatch.DrawString(PixelFont, "HULL INTEGRITY: " + ship.HP, FontPos, Color.White);

                    if (CurrentLevel != Endless)
                    {
                        //draw current level on screen
                        int levelDisplayed = CurrentLevel.LevelNum;
                        spriteBatch.DrawString(PixelFont, "Current Level: " + levelDisplayed, FontPos + new Vector2(0, 150), Color.White);
                    }
                    else
                    {
                        spriteBatch.DrawString(PixelFont, "Current Score: " + CurrentScore, FontPos + new Vector2(0, 150), Color.White);
                    }

                    //draw function for each projectile in current projectiles
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
                else if (CurrentWinStatus == WinStatus.kLose)
                {
                    Hullcrit.Stop(); //ends siren and AI sound effect so it no longer plays when ship blows up.
                    GameTh.Stop(); //ends the theme song
                    if (explode)
                    { ShipisGone.Play(); explode = false; } //plays instanced sound effect and disables looping.
                    //Defeat message
                    spriteBatch.DrawString(PixelFont, Loser + "\n Press enter to return to main menu or esc to quit", FontPos, Color.White);
                    //if the player is in endless mode
                    //show them their final score and high scores
                    //if they beat the high score, notify them
                    if(CurrentLevel == Endless)
                    {
                        spriteBatch.DrawString(PixelFont, "Your Score: " + CurrentScore,
                            new Vector2((graphics.PreferredBackBufferWidth / 2) - (PixelFont.MeasureString("Your Score: " + CurrentScore).X / 2), 300),
                            Color.White);
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
                else if (CurrentWinStatus == WinStatus.kWin_Level)
                {
                    Hullcrit.Stop(); //ends siren and AI sound effect so it no longer plays when ship is out of danger.
                    GameTh.Stop(); //ends the theme song
                    ship.Draw(spriteBatch);
                    //victory jingle
                    Victory.Play();
                    //victory message
                    spriteBatch.DrawString(PixelFont, Winner, FontPos, Color.White);
                }
                else if (CurrentWinStatus == WinStatus.kWin_Game)
                {
                    spriteBatch.DrawString(PixelFont, "You win! Press Enter to return to main menu or esc to exit",
                        FontPos, Color.White);
                }
            } 
            //}

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
