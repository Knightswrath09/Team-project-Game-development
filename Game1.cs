using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic; //for list

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

        //level object for current level
        //im thinking a list of all of the levels might be easier to handle
        //you would access the current level object using the current level integer as the index
        Level CurrentLevel;

        //list to keep track of current projectiles on screen
        List<Projectile> CurrentProjectiles = new List<Projectile>();

        //list to keep track of current shields
        List<Shield> CurrentShields = new List<Shield>();

        //enumerator for the possible states of the game
        enum ScreenState
        {
            kMain_Menu = 0,
            kControls,
            kPaused,
            kVolume_Menu,
            kGame_Play
        }
        //variable to store current screenstate, start with the main menu
        ScreenState CurrentScreenState = ScreenState.kMain_Menu;

        //mostly to determine if controls or volume menus direct back to pause menu or main menu
        bool GameStarted = false;

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

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
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

            CurrentLevel = new Level(1, 10, Level.ProjectileTypes.kRed_Only, 3);

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

        void CreateProjectile(Level currLevel)
        {
            //create projectile with random color and direction according to the current level
            //add new projectile to the projectile list with CurrentProjectiles.Add(newProjectile)
            //use CurrentProjectiles.Count() to determine the index it was placed at, will be placed at the end of the list
            Projectile newProjectile = new Projectile(currLevel);
            CurrentProjectiles.Add(newProjectile);
            CurrentProjectiles.Count();
        }

        //moves all of the projectiles currently on the screen with Move() in projectile class
        //***CARLYN
        void MoveProjectiles(List<Projectile> projectiles)
        {
            
            for (int i = 0; i < projectiles.Count; ++i)
                projectiles[i].Move();
        }
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
        /// <summary>
        /// check each projectile in list with each shield in list
        ///returns Winstatus inProgress if no change
        ///returns winstatus lose if hp of ship == 0
        ///returns winstatus win level if currentlevel firedprojectiles == totalprojectiles
        ///returns winstatus win game if currentlevel firedprojectiles == totalprojectiles and currentlevel == maxlevel
        /// </summary>
        WinStatus CheckCollision()
            //check each projectile in list with each shield in list
            //returns Winstatus inProgress if no change
            for (int a = 0; a <= CurrentProjectiles.Count; a++) {
                if (//the projectile is past the shield line)
                {
                    int blocked;
                    for (int i = 0; i < CurrentShields.Count; i++)
                    {
                        if ((projectiles[i].Direction == shields[i].direction) && (projectiles[i].SpriteColor == shields[i].SpriteColor))
                        {
                            ///0 = hasn't reached shields, 1 = blocked, 2 = hit
                            if (blocked == 0)
                            {
                                CurrentWinStatus = WinStatus.kLevel_In_Progress;
                            }
                            if (blocked == 1)
                            {
                                CurrentWinStatus = WinStatus.kLevel_In_Progress;
                                CurrentLevel.FiredProjectiles++;
                            }
                            if (blocked == 2)
                            {
                                CurrentWinStatus = WinStatus.kLevel_In_Progress;
                                CurrentLevel.FiredProjectiles++;
                                ShipSprite.HP--;
                            }
                        }
                    }
                }
            if (ShipSprite.HP == 0) {
                CurrentWinStatus = WinStatus.kLose;
            }
            if (CurrentLevel.FiredProjectiles == CurrentLevel.TotalProjectiles) {
                CurrentWinStatus = WinStatus.kWin_Level;
            }
            if ((CurrentLevel.FiredProjectiles == CurrentLevel.TotalProjectiles) && (CurrentLevel == maxLevel)) {
                CurrentWinStatus = WinStatus.kWin_Game;
            }
        }

        //variables for time counting
        float countDuration = 1f; //one second
        float currentTime = 0f;
        int Counter = 0;

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            //***MENU WILL BE IMPLEMENTED AFTER THE PROTOTYPE PHASE
            /*if(CurrentScreenState == ScreenState.kMain_Menu)
            {
                //main menu logic
            }

            else if(CurrentScreenState == ScreenState.kControls)
            {
                //display controls
                //logic to navigate back to main menu, where CurrentScreenState would be set back to kMain_Menu
                //or, if GameStarted == true, naviagte back to pause menu and CurrentScreenState set back to kPaused
            }

            else if(CurrentScreenState == ScreenState.kPaused)
            {
                //display pause menu: resume, controls, or volume options
                //resume, set CurrentScreenState to kGame_Play
                //controls, set CurrentScreenState to kControls
                //volume options, set CurrentScreenState to kVolume
            }

            else if(CurrentScreenState == ScreenState.kVolume_Menu)
            {
                //display volume menu and allow player to change volume of SFX or music
                //logic to navigate back to main menu, where CurrentScreenState would be set back to kMain_Menu
                //or, if GameStarted == true, naviagte back to pause menu and CurrentScreenState set back to kPaused
            }

            else if (CurrentScreenState == ScreenState.kGame_Play)
            {*/
                if (!GameStarted)
                {
                    //have player name their ship
                    //set gamestarted = true
                }

                else
                {
                    if (CurrentWinStatus == WinStatus.kLevel_In_Progress)
                    {
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
                            CreateProjectile(CurrentLevel);
                        }

                        //move current projectiles in CurrentProjectiles list
                        MoveProjectiles(CurrentProjectiles);

                        //allow player to move shields
                        

                        //check collisions and if the the winstate changes
                        //returns Winstatus inProgress if no change
                        //returns winstatus lose if hp of ship == 0
                        //returns winstatus win level if currentlevel firedprojectiles == totalprojectiles
                        //returns winstatus win game if currentlevel firedprojectiles == totalprojectiles and currentlevel == maxlevel
                        CurrentWinStatus = CheckCollision(CurrentProjectiles, CurrentShields);

                        //allow player to pause game
                    }
                    else if (CurrentWinStatus == WinStatus.kLose)
                    {

                    }
                    /*else if (CurrentWinStatus == WinStatus.kWin_Level)
                    {
                        //increase current level
                        //change shield list if necessary
                        
                    }*/
                    else if (CurrentWinStatus == WinStatus.kWin_Game)
                    {

                    }
                }
            //}
        
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            //MENUS WILL BE IMPLEMENTED AFTER PROTOTYPE PHASE
            /*if(CurrentScreenState == ScreenState.kMain_Menu)
            {
                //draw main menu
            }
            else if(CurrentScreenState == ScreenState.kControls)
            {
                //draw controls
            }
            else if(CurrentScreenState == ScreenState.kPaused)
            {
                //draw pause menu
            }
            else if(CurrentScreenState == ScreenState.kGame_Play)
            {*/
                if (CurrentWinStatus == WinStatus.kLevel_In_Progress)
                {
                    //draw all of the gameplay sprites and stats
                }
                else if (CurrentWinStatus == WinStatus.kLose)
                {
                    //draw "you lose!" screen
                }
                /*else if (CurrentWinStatus == WinStatus.kWin_Level)
                {
                    //draw win level screen
                }*/
                else if(CurrentWinStatus == WinStatus.kWin_Game)
                {
                    //draw win game screen
                }
            //}

            base.Draw(gameTime);
        }
    }
}
