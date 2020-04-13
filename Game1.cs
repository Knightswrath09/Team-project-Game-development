using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio; //for sound effects and music.
using System.Collections.Generic; //for list
using System;



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


        //sound effects
        SoundEffect HullCritical;
        SoundEffect ProjectileFired;
        SoundEffect ShieldBlock;
        SoundEffect Hullhit;
        SoundEffect ShipBlowsUp;
        

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
            kVolume_Menu,
            kGame_Play
        }
        //variable to store current screenstate, start with the main menu
        ScreenState CurrentScreenState = ScreenState.kMain_Menu;

        //mostly to determine if controls or volume menus direct back to pause menu or main menu
        bool GameStarted = true;

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
            Hullhit = Content.Load<SoundEffect>("111048__cyberkineticfilms__gunshot-with-metal-hit");//Gunshot with metal hit sound effect made by user Cyberkineticfilms on FreeSound.org
            ShipBlowsUp = Content.Load<SoundEffect>("244394__werra__bang-explosion-metallic");//Bang explosion metallic sound effect made by user Werra on FreeSound.org

            CurrentLevel = new Level(1, 10, Level.ProjectileTypes.kRBP, 3, 10);

            ship = new ShipSprite("ship", Content.Load<Texture2D>("PlayerShip"), new Vector2(graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight), new Vector2(360f, 253f));

            ShipPosition = new Vector2((graphics.PreferredBackBufferWidth / 2) - (shipSize.X / 2), (graphics.PreferredBackBufferHeight / 2) - (shipSize.Y / 2));


            rShield = new Shield(CombatSprites.CombatSpriteColors.kRed, CombatSprites.Directions.kTop, 0,
                Content.Load<Texture2D>("RedShield"), new Vector2(56f, 253f), shipSize, ShipPosition);
            bShield = new Shield(CombatSprites.CombatSpriteColors.kBlue, CombatSprites.Directions.kBottom, 1,
                Content.Load<Texture2D>("BlueShield"), new Vector2(56f, 253f), shipSize, ShipPosition);
            pShield = new Shield(CombatSprites.CombatSpriteColors.kPurple, CombatSprites.Directions.kLeft, 2,
                Content.Load<Texture2D>("PurpleShield"), new Vector2(56f, 253f), shipSize, ShipPosition);

            CurrentShields.Add(rShield);
            CurrentShields.Add(bShield);
            CurrentShields.Add(pShield);

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
                if (RandomColor <= 12)
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
                else //if (RandomColor <= 13)
                    spriteColor = CombatSprites.CombatSpriteColors.kPurple;
                
                //WILL INCLUDE POWERUPS IF WE HAVE EXTRA TIME BEFORE PROTOTYPE IS DUE
                //else if (RandomColor <= 14)
                    //spriteColor = CombatSprites.CombatSpriteColors.kGreen;
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
                ProjectileFired.Play(1f, 0, 0);
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
            //arrow keys control red shield, which has index 0 in CurrentShield list
            if (keyboardState.IsKeyDown(Keys.Up))
                CurrentShields[0].MoveShield(CombatSprites.Directions.kTop, CurrentShields);
            else if (keyboardState.IsKeyDown(Keys.Right))
                CurrentShields[0].MoveShield(CombatSprites.Directions.kRight, CurrentShields);
            else if (keyboardState.IsKeyDown(Keys.Down))
                CurrentShields[0].MoveShield(CombatSprites.Directions.kBottom, CurrentShields);
            else if (keyboardState.IsKeyDown(Keys.Left))
                CurrentShields[0].MoveShield(CombatSprites.Directions.kLeft, CurrentShields);

            //WASD keys control blue shield, index 1
            if (keyboardState.IsKeyDown(Keys.W))
                CurrentShields[1].MoveShield(CombatSprites.Directions.kTop, CurrentShields);
            else if (keyboardState.IsKeyDown(Keys.D))
                CurrentShields[1].MoveShield(CombatSprites.Directions.kRight, CurrentShields);
            else if (keyboardState.IsKeyDown(Keys.S))
                CurrentShields[1].MoveShield(CombatSprites.Directions.kBottom, CurrentShields);
            else if (keyboardState.IsKeyDown(Keys.A))
                CurrentShields[1].MoveShield(CombatSprites.Directions.kLeft, CurrentShields);
                
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
        WinStatus CheckCollision()
        {
            WinStatus newWinStatus;
            //check each projectile in list with each shield in list
            //returns Winstatus inProgress if no change
            int blocked;
            for (int a = 0; a < CurrentProjectiles.Count; a++) {
                ///0 = hasn't reached shields, 1 = blocked, 2 = hit
                blocked = 0;
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
                        if ((CurrentProjectiles[a].Direction == CurrentShields[i].Direction) && (CurrentProjectiles[a].SpriteColor == CurrentShields[i].SpriteColor) && CurrentShields[i].visible)
                        {
                            blocked = 1;
                            ShieldBlock.Play(1f, 0, 0);
                        }
                        else if (blocked != 1)
                        {
                            blocked = 2;
                        }
                        
                    }
                    if(blocked == 2)
                    {
                        ship.HP--;
                        Hullhit.Play(1f, 0, 0);

                        //emergency sound effect
                        if (ship.HP == 1)
                            HullCritical.Play(1.0f, 0, 0);

                    }
                    CurrentProjectiles[a] = null;
                    CurrentProjectiles.RemoveAt(a);
                    CurrentLevel.FiredProjectiles++;
                }
                
            }
            if (ship.HP == 0)
            {
                newWinStatus = WinStatus.kLose;
            }
            else if (CurrentLevel.FiredProjectiles == CurrentLevel.TotalProjectiles)
            {
                newWinStatus = WinStatus.kWin_Level;
            }
            else
                newWinStatus = WinStatus.kLevel_In_Progress;
            //ABILITY TO WIN GAME WILL BE IMPLEMENTED AFTER PROTOTYPE PHASE
            /*
            if ((CurrentLevel.FiredProjectiles == CurrentLevel.TotalProjectiles) && (CurrentLevel == maxLevel))
            {
                CurrentWinStatus = WinStatus.kWin_Game;
            }*/
            return newWinStatus;
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
                        CreateProjectile();
                    }

                    //move current projectiles in CurrentProjectiles list
                    MoveProjectiles();

                    //allow player to move shields
                    MoveShields();


                    //check collisions and if the the winstate changes
                    //returns Winstatus inProgress if no change
                    //returns winstatus lose if hp of ship == 0
                    //returns winstatus win level if currentlevel firedprojectiles == totalprojectiles
                    //returns winstatus win game if currentlevel firedprojectiles == totalprojectiles and currentlevel == maxlevel
                    CurrentWinStatus = CheckCollision();

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
            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

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
                //draw the ship
                ship.Draw(spriteBatch);
                //draw function for each projectile in current projectiles
                for (int i = 0; i < CurrentProjectiles.Count; i++)
                    CurrentProjectiles[i].Draw(spriteBatch);
                //draw function for each shield in current shields list
                for (int j = 0; j < CurrentShields.Count; j++)
                {
                    if(CurrentShields[j].visible)   
                        CurrentShields[j].Draw(spriteBatch);
                }
            }
            else if (CurrentWinStatus == WinStatus.kLose)
            {
                //draw "you lose!" screen
            }
            else if (CurrentWinStatus == WinStatus.kWin_Level)
            {
                ship.Draw(spriteBatch);
            }
            else if (CurrentWinStatus == WinStatus.kWin_Game)
            {
                //draw win game screen
            }
            //}

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
