using System;
using System.Collections.Generic;
using System.Linq;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace F.U.E.L
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        SpriteFont spriteFont;

        MenuManager menuManager;

        Texture2D healthTexture, UITexture, minimapTexture, unitsTexture, iconsTexture, buttonTexture;

        Model planeModel, towerModel, generatorModel, enemyModel, playerModel, bulletModel, fireBulletModel, poisonBulletModel, bigBulletModel, mineBulletModel, buildingModel, treeModel, playerSpawnModel, checkBoxModel, enemySpawnModel;

        Camera camera;
        Map map;
        List<Player> players = new List<Player>();
        //Enemy[] enemy = new Enemy[10];
        List<Enemy> enemyList = new List<Enemy>();
        SpatialHashGrid grid;
        Model[] em = new Model[1];
        Model[] p;
        Model[] t;

        MouseState mouse;
        KeyboardState keyboard;
        GamePadState gamepad1, gamepad2, gamepad3, gamepad4;

        UI userInterface;

        List<Object> removeList = new List<Object>();

        public Effect redEffect, greenEffect;

        FrameRateCounter fpsCounter;

        MainMenu pauseMenu;
        MainMenu mainMenu;
        MainMenu characterMenu;
        MainMenu winMenu;
        MainMenu loseMenu;

        protected Song bgm;

        public static bool fpsCounterOn = false;

        private bool playing = false;

        private bool EscapeKeyDown = false;
        private bool EnterKeyDown = false;

        private bool BackButtonDown1 = false;
        private bool StartButtonDown1 = false;
        private bool AButtonDown1 = false;

        private bool BackButtonDown2 = false;
        private bool StartButtonDown2 = false;
        private bool AButtonDown2 = false;

        private bool BackButtonDown3 = false;
        private bool StartButtonDown3 = false;
        private bool AButtonDown3 = false;

        private bool BackButtonDown4 = false;
        private bool StartButtonDown4 = false;
        private bool AButtonDown4 = false;

        private bool inPauseMenu = false;
        private bool inGame = false;
        private bool inMainMenu = false;
        private bool inCharacterMenu = false;
        private bool inWinMenu = false;
        private bool inLoseMenu = false;
        private bool enterWinMenu = false;
        private bool enterLoseMenu = false;

        public Vector3 cameraTarget { get; private set; }

        List<GameComponent> removeFromComponents = new List<GameComponent>();

        public Game1()
        {
            fpsCounter = new FrameRateCounter(this);
            Components.Add(fpsCounter);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;
            // The following code removes the XNA fixed timestep (framerate limiter)
            //IsFixedTimeStep = false;
            //// Because the above is an artificial but necessary step, this one sets the timestep to 1ms
            //TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1);
            //// This removes the synchronization with the screen to allow a faster framerate
            //graphics.SynchronizeWithVerticalRetrace = false;

            //***************Multiple Resolutions for debugging purposes************************
            //graphics.PreferredBackBufferWidth = 1024;
            //graphics.PreferredBackBufferHeight = 768;

            // We will most probably use this resolution (1680*1050) for the demo as it seems that there is little to NO FPS change when changing resolution
            // Also it is the native resolution of the gaming room monitors
            // At lower resolutions our models and graphics become REALLY UGLY

            //graphics.PreferredBackBufferWidth = 1680;
            //graphics.PreferredBackBufferHeight = 1050;
            //graphics.ToggleFullScreen();

            graphics.PreferredBackBufferWidth = 1280; 
            graphics.PreferredBackBufferHeight = 720;

            //graphics.PreferredBackBufferWidth = 800;
            //graphics.PreferredBackBufferHeight = 480;

            //graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            // Create camera and add to components list
            camera = new Camera(this, new Vector3(0, 12, 9), Vector3.Zero, -Vector3.UnitZ);
            Components.Add(camera);

            mouse = Mouse.GetState();
            keyboard = Keyboard.GetState();
            gamepad1 = GamePad.GetState(PlayerIndex.One);
            gamepad2 = GamePad.GetState(PlayerIndex.Two);
            gamepad3 = GamePad.GetState(PlayerIndex.Three);
            gamepad4 = GamePad.GetState(PlayerIndex.Four);

            menuManager = new MenuManager(mouse, ref keyboard);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            spriteFont = Content.Load<SpriteFont>(@"FPSFont");

            //NEED TO MAKE BLANK 1x1 TEXTURE
            healthTexture = Content.Load<Texture2D>(@"UITextures\unitsTexture");
            UITexture = Content.Load<Texture2D>(@"UITextures\UI");
            minimapTexture = Content.Load<Texture2D>(@"UITextures\minimap");
            unitsTexture = Content.Load<Texture2D>(@"UITextures\unitsTexture");
            iconsTexture = Content.Load<Texture2D>(@"UITextures\icons");

            planeModel = Content.Load<Model>(@"Models\floorModel");
            bulletModel = Content.Load<Model>(@"Models\bulletModel");
            fireBulletModel = Content.Load<Model>(@"Models\fireBulletModel");
            poisonBulletModel = Content.Load<Model>(@"Models\poisonBulletModel");
            bigBulletModel = Content.Load<Model>(@"Models\bigBulletModel");
            mineBulletModel = Content.Load<Model>(@"Models\mineModel");
            towerModel = Content.Load<Model>(@"Models\towerModel");
            generatorModel = Content.Load<Model>(@"Models\generatorModel");
            buildingModel = Content.Load<Model>(@"Models\buildingModel");
            playerModel = Content.Load<Model>(@"Models\NewPlayer1");
            treeModel = Content.Load<Model>(@"Models\treeModel");
            playerSpawnModel = Content.Load<Model>(@"Models\playerSpawn");
            enemySpawnModel = Content.Load<Model>(@"Models\enemySpawn");
            checkBoxModel = Content.Load<Model>(@"Models\checkBoxModel");

            bgm = Content.Load<Song>(@"Sounds\bgm");

            string menuBG = @"ScreenManagerAssets\Textures\MainMenuBG";
            string menuBGSound = @"ScreenManagerAssets\Sounds\MainMenuBGM";
            string menuOpenPath =  @"ScreenManagerAssets\Sounds\MenuOpen";
            string menuClosePath = @"ScreenManagerAssets\Sounds\MenuClose";

            pauseMenu = new MainMenu("Pause Menu"); // pause menu
            pauseMenu.Load(Content, menuBG, menuBGSound, menuOpenPath, menuClosePath);
            pauseMenu.LoadButtons(Content,
                new int[] { 1, 2 },
                new List<Rectangle>() { new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 - 30, 150, 50), 
                                        new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 + 30, 150, 50) },
                new List<string>() { "Continue", "Quit" }
                );

            winMenu = new MainMenu("Win!"); // pause menu
            winMenu.Load(Content, menuBG, menuBGSound, menuOpenPath, menuClosePath);
            winMenu.LoadButtons(Content,
                new int[] { 1 },
                new List<Rectangle>() { new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 , 150, 50), 
                                        new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 , 150, 50) },
                new List<string>() { "Okay" }
                );

            loseMenu = new MainMenu("Lose!"); // pause menu
            loseMenu.Load(Content, menuBG, menuBGSound, menuOpenPath, menuClosePath);
            loseMenu.LoadButtons(Content,
                new int[] { 1 },
                new List<Rectangle>() { new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 , 150, 50), 
                                        new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 , 150, 50) },
                new List<string>() { "Okay" }
                );

            mainMenu = new MainMenu("Main Menu"); // main menu
            mainMenu.Load(Content, menuBG, menuBGSound, menuOpenPath, menuClosePath);
            mainMenu.LoadButtons(Content,
                new int[] { 1, 2 },
                new List<Rectangle>() { new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 - 30, 150, 50), 
                                        new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 + 30, 150, 50) },
                new List<string>() { "New Game", "Quit" }
                );

            characterMenu = new MainMenu("Character Menu"); // main menu
            characterMenu.Load(Content, menuBG, menuBGSound, menuOpenPath, menuClosePath);
            characterMenu.LoadButtons(Content,
                        new int[] {  0,  1,  2,  3,  4, 
                                     5,  6,  7,  8,  9, 
                                    10, 11, 12, 13, 14,
                                    15, 16, 17, 18, 19 },
                        new List<Rectangle>() { new Rectangle(graphics.PreferredBackBufferWidth / 5, graphics.PreferredBackBufferHeight / 2 - 90, 150, 50),
                                                new Rectangle(graphics.PreferredBackBufferWidth / 5, graphics.PreferredBackBufferHeight / 2 - 30, 150, 50), 
                                                new Rectangle(graphics.PreferredBackBufferWidth / 5, graphics.PreferredBackBufferHeight / 2 + 30, 150, 50), 
                                                new Rectangle(graphics.PreferredBackBufferWidth / 5, graphics.PreferredBackBufferHeight / 2 + 90, 150, 50), 
                                                new Rectangle(graphics.PreferredBackBufferWidth / 5, graphics.PreferredBackBufferHeight / 2 + 150, 150, 50),

                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 2, graphics.PreferredBackBufferHeight / 2 - 90, 150, 50),
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 2, graphics.PreferredBackBufferHeight / 2 - 30, 150, 50), 
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 2, graphics.PreferredBackBufferHeight / 2 + 30, 150, 50), 
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 2, graphics.PreferredBackBufferHeight / 2 + 90, 150, 50), 
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 2, graphics.PreferredBackBufferHeight / 2 + 150, 150, 50),
                                                                                                    
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 3, graphics.PreferredBackBufferHeight / 2 - 90, 150, 50),
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 3, graphics.PreferredBackBufferHeight / 2 - 30, 150, 50), 
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 3, graphics.PreferredBackBufferHeight / 2 + 30, 150, 50), 
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 3, graphics.PreferredBackBufferHeight / 2 + 90, 150, 50), 
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 3, graphics.PreferredBackBufferHeight / 2 + 150, 150, 50),
                                                                                                    
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 4, graphics.PreferredBackBufferHeight / 2 - 90, 150, 50),
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 4, graphics.PreferredBackBufferHeight / 2 - 30, 150, 50), 
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 4, graphics.PreferredBackBufferHeight / 2 + 30, 150, 50), 
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 4, graphics.PreferredBackBufferHeight / 2 + 90, 150, 50), 
                                                new Rectangle((graphics.PreferredBackBufferWidth / 5) * 4, graphics.PreferredBackBufferHeight / 2 + 150, 150, 50)},
                        new List<string>() { "None", "Gunner", "Alchemist", "Sniper", "Tank", 
                                             "None", "Gunner", "Alchemist", "Sniper", "Tank", 
                                             "None", "Gunner", "Alchemist", "Sniper", "Tank", 
                                             "None", "Gunner", "Alchemist", "Sniper", "Tank" }
                        );

            characterMenu.buttons[0].setSelected(true);
            characterMenu.buttons[5].setSelected(true);
            characterMenu.buttons[10].setSelected(true);
            characterMenu.buttons[15].setSelected(true);

            // Initiate menus
            menuManager.AddMenu("Main Menu", mainMenu);
            menuManager.AddMenu("Character Menu", characterMenu);
            menuManager.AddMenu("Pause Menu", pauseMenu);
            menuManager.AddMenu("Win!", winMenu);
            menuManager.AddMenu("Lose!", loseMenu);

            userInterface = new UI(spriteBatch, GraphicsDevice, UITexture, healthTexture, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth, minimapTexture, unitsTexture);

            //redEffect = Content.Load<Effect>(@"Effects\Red");
            //greenEffect = Content.Load<Effect>(@"Effects\Green");

            Model[] a = new Model[7];
            a[0] = planeModel;
            a[1] = towerModel;
            a[2] = generatorModel;
            a[3] = buildingModel;
            a[4] = treeModel;
            a[5] = playerSpawnModel;
            a[6] = enemySpawnModel;
            map = new Map(this, a, -36, 36, GraphicsDevice);
            Components.Add(map);

            // The first model will be of the player
            // The LAST model (always last) will be the checkBox model
            p = new Model[7];
            p[0] = playerModel;
            p[1] = towerModel;
            p[2] = bulletModel;
            p[3] = fireBulletModel;
            p[4] = poisonBulletModel;
            p[5] = bigBulletModel;
            p[6] = mineBulletModel;
            //p[2] = bulletModel;
            t = new Model[4];
            t[0] = checkBoxModel;

            // Create the grid with necessary information
            grid = new SpatialHashGrid(72, 72, 2, map.leftXPos / 2, map.bottomYPos / 2);
            for (int i = 0; i < map.buildings.Count; ++i)
                grid.insertStaticObject(map.buildings[i]);

            for (int i = 0; i < map.usableBuildings.Count; ++i)//for collisions
                grid.insertStaticObject(map.usableBuildings[i]);

            enemyModel = Content.Load<Model>(@"Models\alien788_60");
            Model[] em = new Model[1];
            em[0] = enemyModel;
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        private void resetGame() 
        {
            players.Clear();
            enemyList.Clear();
            foreach (GameComponent gc in Components)
            {
                if (gc is Tower) 
                {
                    Tower t = (Tower)gc;
                    t.isAlive = false;
                }
                else if (gc is Character || gc is Tower)
                    removeFromComponents.Add(gc);
            }

            foreach (GameComponent c in removeFromComponents)
                Components.Remove(c);

            foreach (Generator g in map.usableBuildings) 
            {
                g.functional = false;
                g.hp = 50;
                g.lastRepair = 0;
            }

            Player.credit = 500;
            Player.respawnCost = 500;

            mainMenu.upButtonDown1 = mainMenu.upButtonDown2 = mainMenu.upButtonDown3 = mainMenu.upButtonDown4 = mainMenu.currentUpButtonDown =false;
            mainMenu.downButtonDown1 = mainMenu.downButtonDown2 = mainMenu.downButtonDown3 = mainMenu.downButtonDown4 = mainMenu.currentDownButtonDown =false;

            pauseMenu.upButtonDown1 = pauseMenu.upButtonDown2 = pauseMenu.upButtonDown3 = pauseMenu.upButtonDown4 = pauseMenu.currentUpButtonDown =false;
            pauseMenu.downButtonDown1 = pauseMenu.downButtonDown2 = pauseMenu.downButtonDown3 = pauseMenu.downButtonDown4 = pauseMenu.currentDownButtonDown =false;

            characterMenu.upButtonDown1 = characterMenu.upButtonDown2 = characterMenu.upButtonDown3 = characterMenu.upButtonDown4 = characterMenu.currentUpButtonDown =false;
            characterMenu.downButtonDown1 = characterMenu.downButtonDown2 = characterMenu.downButtonDown3 = characterMenu.downButtonDown4 = characterMenu.currentDownButtonDown =false;

            characterMenu.player1Chosen = characterMenu.player2Chosen = characterMenu.player3Chosen = characterMenu.player4Chosen = false;
            characterMenu.allPlayersChose = false;

            playing = false;

            EscapeKeyDown = false;
            EnterKeyDown = false;

            BackButtonDown1 = false;
            StartButtonDown1 = false;
            AButtonDown1 = false;

            BackButtonDown2 = false;
            StartButtonDown2 = false;
            AButtonDown2 = false;

            BackButtonDown3 = false;
            StartButtonDown3 = false;
            AButtonDown3 = false;

            BackButtonDown4 = false;
            StartButtonDown4 = false;
            AButtonDown4 = false;

            inPauseMenu = false;
            inGame = false;
            inMainMenu = false;
            inCharacterMenu = false;
        }

        protected override void Update(GameTime gameTime)
        {
            cameraTarget = camera.cameraTarget;

            keyboard = Keyboard.GetState();

            #region Update the menus for each player

            if (gamepad1.IsConnected)
            {
                gamepad1 = GamePad.GetState(PlayerIndex.One);
                //menuManager.Update(keyboard, gamepad1, 1);
                if (inPauseMenu)
                    pauseMenu.Update(keyboard, gamepad1, 1);
                if (inMainMenu)
                    mainMenu.Update(keyboard, gamepad1, 1);
                if (inCharacterMenu)
                    characterMenu.Update(keyboard, gamepad1, 1);
            }
            else
                characterMenu.player1Chosen = true;

            if (gamepad2.IsConnected)
            {
                gamepad2 = GamePad.GetState(PlayerIndex.Two);
                //menuManager.Update(keyboard, gamepad2, 2);
                if (inPauseMenu)
                    pauseMenu.Update(keyboard, gamepad2, 2);
                if (inCharacterMenu)
                    characterMenu.Update(keyboard, gamepad2, 2);
            }
            else
                characterMenu.player2Chosen = true;

            if (gamepad3.IsConnected)
            {
                gamepad3 = GamePad.GetState(PlayerIndex.Three);
                //menuManager.Update(keyboard, gamepad3, 3);
                if (inPauseMenu)
                    pauseMenu.Update(keyboard, gamepad3, 3);
                if (inCharacterMenu)
                    characterMenu.Update(keyboard, gamepad3, 3);
            }
            else
                characterMenu.player3Chosen = true;

            if (gamepad4.IsConnected)
            {
                gamepad4 = GamePad.GetState(PlayerIndex.Four);
                //menuManager.Update(keyboard, gamepad4, 4);
                if (inPauseMenu)
                    pauseMenu.Update(keyboard, gamepad4, 4);
                if (inCharacterMenu)
                    characterMenu.Update(keyboard, gamepad4, 4);
            }
            else
                characterMenu.player4Chosen = true;

            #endregion

            #region Create Players
            if (characterMenu.allPlayersChose)
            {
                int k = 0;
                // Just as a default player index, changes in each one
                PlayerIndex current = PlayerIndex.One;
                for (int i = 0; i < characterMenu.buttons.Count; ++i)
                {
                    if (characterMenu.buttons[i].getSelected() && characterMenu.buttons[i].getText().Equals("Gunner"))
                    {
                        if (i >= 0 && i < 5)
                        {
                            current = PlayerIndex.One;
                            characterMenu.player1Chosen = false;
                        }
                        else if (i >= 5 && i < 10)
                        {
                            current = PlayerIndex.Two;
                            characterMenu.player2Chosen = false;
                        }
                        else if (i >= 10 && i < 15)
                        {
                            current = PlayerIndex.Three;
                            characterMenu.player3Chosen = false;
                        }
                        else if (i >= 15 && i < 20)
                        {
                            current = PlayerIndex.Four;
                            characterMenu.player4Chosen = false;
                        }

                        p[1] = towerModel;
                        players.Add(new Player(this, p, map.spawnPoints[k], Player.Class.Gunner, current));

                        players[k].checkBox = new BuildBox(this, t, players[k].position,
                                                            new FloatRectangle((players[k].position + players[k].lookDirection).X, (players[k].position + players[k].lookDirection).Z, 1, 1),
                                                            players[k]);
                        ++k;
                    }
                    else if (characterMenu.buttons[i].getSelected() && characterMenu.buttons[i].getText().Equals("Alchemist"))
                    {
                        if (i >= 0 && i < 5)
                        {
                            current = PlayerIndex.One;
                            characterMenu.player1Chosen = false;
                        }
                        else if (i >= 5 && i < 10)
                        {
                            current = PlayerIndex.Two;
                            characterMenu.player2Chosen = false;
                        }
                        else if (i >= 10 && i < 15)
                        {
                            current = PlayerIndex.Three;
                            characterMenu.player3Chosen = false;
                        }
                        else if (i >= 15 && i < 20)
                        {
                            current = PlayerIndex.Four;
                            characterMenu.player4Chosen = false;
                        }

                        players.Add(new Player(this, p, map.spawnPoints[k], Player.Class.Alchemist, current));

                        players[k].checkBox = new BuildBox(this, t, players[k].position,
                                                            new FloatRectangle((players[k].position + players[k].lookDirection).X, (players[k].position + players[k].lookDirection).Z, 1, 1),
                                                            players[k]);
                        ++k;
                    }
                    else if (characterMenu.buttons[i].getSelected() && characterMenu.buttons[i].getText().Equals("Sniper"))
                    {
                        if (i >= 0 && i < 5)
                        {
                            current = PlayerIndex.One;
                            characterMenu.player1Chosen = false;
                        }
                        else if (i >= 5 && i < 10)
                        {
                            current = PlayerIndex.Two;
                            characterMenu.player2Chosen = false;
                        }
                        else if (i >= 10 && i < 15)
                        {
                            current = PlayerIndex.Three;
                            characterMenu.player3Chosen = false;
                        }
                        else if (i >= 15 && i < 20)
                        {
                            current = PlayerIndex.Four;
                            characterMenu.player4Chosen = false;
                        }

                        players.Add(new Player(this, p, map.spawnPoints[k], Player.Class.Sniper, current));

                        players[k].checkBox = new BuildBox(this, t, players[k].position,
                                                            new FloatRectangle((players[k].position + players[k].lookDirection).X, (players[k].position + players[k].lookDirection).Z, 1, 1),
                                                            players[k]);
                        ++k;
                    }
                    else if (characterMenu.buttons[i].getSelected() && characterMenu.buttons[i].getText().Equals("Tank"))
                    {
                        if (i >= 0 && i < 5)
                        {
                            current = PlayerIndex.One;
                            characterMenu.player1Chosen = false;
                        }
                        else if (i >= 5 && i < 10)
                        {
                            current = PlayerIndex.Two;
                            characterMenu.player2Chosen = false;
                        }
                        else if (i >= 10 && i < 15)
                        {
                            current = PlayerIndex.Three;
                            characterMenu.player3Chosen = false;
                        }
                        else if (i >= 15 && i < 20)
                        {
                            current = PlayerIndex.Four;
                            characterMenu.player4Chosen = false;
                        }

                        players.Add(new Player(this, p, map.spawnPoints[k], Player.Class.Tank, current));

                        players[k].checkBox = new BuildBox(this, t, players[k].position,
                                                            new FloatRectangle((players[k].position + players[k].lookDirection).X, (players[k].position + players[k].lookDirection).Z, 1, 1),
                                                            players[k]);
                        ++k;
                    }
                    else if (characterMenu.buttons[i].getSelected() && characterMenu.buttons[i].getText().Equals("None")) 
                    {
                        //++k;
                    }
                }
                foreach (Player ply in players) { Components.Add(ply); }
                characterMenu.allPlayersChose = false;
                inCharacterMenu = false;
                inGame = true;
                inPauseMenu = false;
                inMainMenu = false;
            }
            #endregion

            #region Update Menus
            if (menuManager != null)
            {
                switch (menuManager.MenuEvent)
                {
                    case MenuManager.MenuEvents.None:
                        break;
                    case MenuManager.MenuEvents.Exit:
                        this.Exit();
                        break;
                }
            }

            if (menuManager.ActiveMenu != null)
            {
                playing = false;
                fpsCounterOn = false;
            }

            if (!inGame && !inMainMenu && !inCharacterMenu && !inPauseMenu)
            {
                menuManager.Show("Main Menu");
                inMainMenu = true;
            }

            if (inGame && !inMainMenu && !inCharacterMenu && !inPauseMenu && enterWinMenu)
            {
                menuManager.Show("Win!");
                enterWinMenu = false;
                inWinMenu = true;
            }

            if (inGame && !inMainMenu && !inCharacterMenu && !inPauseMenu && enterLoseMenu)
            {
                menuManager.Show("Lose!");
                enterLoseMenu = false;
                inLoseMenu = true;
            }
            #endregion

            #region gamepad controls for menu

            #region GamePad 1 Controls
            if (gamepad1.IsButtonDown(Buttons.A) && gamepad1.IsConnected)
            {
                if (!AButtonDown1 && menuManager.ActiveMenu != null && inMainMenu && (mainMenu.selected() == "New Game"))
                {
                    resetGame();//RESET GAME
                    menuManager.Exit();
                    menuManager.Show("Character Menu");
                    AButtonDown1 = true;
                    inCharacterMenu = true;
                    inMainMenu = false;
                    inPauseMenu = false;
                }

                if (!AButtonDown1 && menuManager.ActiveMenu != null && inMainMenu && !inPauseMenu && (mainMenu.selected() == "Quit"))
                {
                    menuManager.Exit();
                    this.Exit();
                }

                if (!AButtonDown1 && menuManager.ActiveMenu != null && !inMainMenu && inPauseMenu && (pauseMenu.selected() == "Continue"))
                {
                    menuManager.Exit();
                    AButtonDown1 = true;
                    inGame = true;
                }

                if (!AButtonDown1 && menuManager.ActiveMenu != null && !inMainMenu && inPauseMenu && (pauseMenu.selected() == "Quit"))
                {
                    menuManager.Exit();
                    menuManager.Show("Main Menu");
                    AButtonDown1 = true;
                    inGame = false;
                    inMainMenu = true;
                    inPauseMenu = false;
                }

                if ((inWinMenu || inLoseMenu))
                {
                    AButtonDown1 = true;
                    inGame = false;
                    inMainMenu = true;
                    inCharacterMenu = false;
                    inPauseMenu = false;
                    inWinMenu = false;
                    inLoseMenu = false;
                }
                
                if (AButtonDown1)
                    AButtonDown1 = false;
            }

            if(gamepad1.IsButtonDown(Buttons.Back))
            {
                if (!BackButtonDown1 && menuManager.ActiveMenu != null && inMainMenu && !inPauseMenu && !inCharacterMenu)
                {
                    menuManager.Exit();
                    this.Exit();
                }

                if (!BackButtonDown1 && menuManager.ActiveMenu != null && inCharacterMenu && !inPauseMenu && !inMainMenu)
                {
                    menuManager.Exit();
                    menuManager.Show("Main Menu");
                    BackButtonDown1 = true;
                    inCharacterMenu = false;
                    inMainMenu = true;
                    inPauseMenu = false;
                }
                
                if (BackButtonDown1)
                    BackButtonDown1 = false;
            }

            if (gamepad1.IsButtonDown(Buttons.Start))
            {
                if (!StartButtonDown1 && menuManager.ActiveMenu == null)
                {
                    menuManager.Show("Pause Menu");
                    StartButtonDown1 = true;
                    inPauseMenu = true;
                }
                if (!StartButtonDown1 && menuManager.ActiveMenu != null && !inCharacterMenu && !inMainMenu)
                {
                    menuManager.Exit();
                    StartButtonDown1 = true;
                }
            }
            else if (gamepad1.IsButtonUp(Buttons.Start))
                StartButtonDown1 = false;

            #endregion

            #region GamePad 2 Controls
            if (gamepad2.IsButtonDown(Buttons.A) && gamepad2.IsConnected)
            {
                if (!AButtonDown2 && menuManager.ActiveMenu != null && inMainMenu && (mainMenu.selected() == "New Game"))
                {
                    menuManager.Exit();
                    menuManager.Show("Character Menu");
                    AButtonDown2 = true;
                    inCharacterMenu = true;
                    inMainMenu = false;
                    inPauseMenu = false;
                }

                if (!AButtonDown2 && menuManager.ActiveMenu != null && inMainMenu && !inPauseMenu && (mainMenu.selected() == "Quit"))
                {
                    menuManager.Exit();
                    this.Exit();
                }

                if (!AButtonDown2 && menuManager.ActiveMenu != null && !inMainMenu && inPauseMenu && (pauseMenu.selected() == "Continue"))
                {
                    menuManager.Exit();
                    AButtonDown2 = true;
                    inGame = true;
                }

                if (!AButtonDown2 && menuManager.ActiveMenu != null && !inMainMenu && inPauseMenu && (pauseMenu.selected() == "Quit"))
                {
                    menuManager.Exit();
                    menuManager.Show("Main Menu");
                    AButtonDown2 = true;
                    inGame = false;
                    inMainMenu = true;
                    inPauseMenu = false;
                }

                if ((inWinMenu || inLoseMenu))
                {
                    AButtonDown2 = true;
                    inGame = false;
                    inMainMenu = true;
                    inCharacterMenu = false;
                    inPauseMenu = false;
                    inWinMenu = false;
                    inLoseMenu = false;
                }

                if (AButtonDown2)
                    AButtonDown2 = false;
            }

            if (gamepad2.IsButtonDown(Buttons.Back))
            {
                if (!BackButtonDown2 && menuManager.ActiveMenu != null && inMainMenu && !inPauseMenu && !inCharacterMenu)
                {
                    menuManager.Exit();
                    this.Exit();
                }

                if (!BackButtonDown2 && menuManager.ActiveMenu != null && inCharacterMenu && !inPauseMenu && !inMainMenu)
                {
                    menuManager.Exit();
                    menuManager.Show("Main Menu");
                    BackButtonDown2 = true;
                    inCharacterMenu = false;
                    inMainMenu = true;
                    inPauseMenu = false;
                }

                if (BackButtonDown2)
                    BackButtonDown2 = false;
            }

            if (gamepad2.IsButtonDown(Buttons.Start))
            {
                if (!StartButtonDown2 && menuManager.ActiveMenu == null && !inMainMenu && !inCharacterMenu)
                {
                    menuManager.Show("Pause Menu");
                    StartButtonDown2 = true;
                    inPauseMenu = true;
                }
                if (!StartButtonDown2 && menuManager.ActiveMenu != null && !inCharacterMenu)
                {
                    menuManager.Exit();
                    StartButtonDown2 = true;
                }
            }
            else if (gamepad2.IsButtonUp(Buttons.Start))
                StartButtonDown2 = false;

            #endregion

            #region GamePad 3 Controls
            if (gamepad3.IsButtonDown(Buttons.A) && gamepad3.IsConnected)
            {
                if (!AButtonDown3 && menuManager.ActiveMenu != null && inMainMenu && (mainMenu.selected() == "New Game"))
                {
                    menuManager.Exit();
                    menuManager.Show("Character Menu");
                    AButtonDown3 = true;
                    inCharacterMenu = true;
                    inMainMenu = false;
                    inPauseMenu = false;
                }

                if (!AButtonDown3 && menuManager.ActiveMenu != null && inMainMenu && !inPauseMenu && (mainMenu.selected() == "Quit"))
                {
                    menuManager.Exit();
                    this.Exit();
                }

                if (!AButtonDown3 && menuManager.ActiveMenu != null && !inMainMenu && inPauseMenu && (pauseMenu.selected() == "Continue"))
                {
                    menuManager.Exit();
                    AButtonDown3 = true;
                    inGame = true;
                }

                if (!AButtonDown3 && menuManager.ActiveMenu != null && !inMainMenu && inPauseMenu && (pauseMenu.selected() == "Quit"))
                {
                    menuManager.Exit();
                    menuManager.Show("Main Menu");
                    AButtonDown3 = true;
                    inGame = false;
                    inMainMenu = true;
                    inPauseMenu = false;
                }

                if ((inWinMenu || inLoseMenu))
                {
                    AButtonDown3 = true;
                    inGame = false;
                    inMainMenu = true;
                    inCharacterMenu = false;
                    inPauseMenu = false;
                    inWinMenu = false;
                    inLoseMenu = false;
                }

                if (AButtonDown3)
                    AButtonDown3 = false;
            }

            if (gamepad3.IsButtonDown(Buttons.Back))
            {
                if (!BackButtonDown3 && menuManager.ActiveMenu != null && inMainMenu && !inPauseMenu && !inCharacterMenu)
                {
                    menuManager.Exit();
                    this.Exit();
                }

                if (!BackButtonDown3 && menuManager.ActiveMenu != null && inCharacterMenu && !inPauseMenu && !inMainMenu)
                {
                    menuManager.Exit();
                    menuManager.Show("Main Menu");
                    BackButtonDown3 = true;
                    inCharacterMenu = false;
                    inMainMenu = true;
                    inPauseMenu = false;
                }

                if (BackButtonDown3)
                    BackButtonDown3 = false;
            }

            if (gamepad3.IsButtonDown(Buttons.Start))
            {
                if (!StartButtonDown3 && menuManager.ActiveMenu == null && !inMainMenu && !inCharacterMenu)
                {
                    menuManager.Show("Pause Menu");
                    StartButtonDown3 = true;
                    inPauseMenu = true;
                }
                if (!StartButtonDown3 && menuManager.ActiveMenu != null && !inCharacterMenu)
                {
                    menuManager.Exit();
                    StartButtonDown3 = true;
                }
            }

            else if (gamepad3.IsButtonUp(Buttons.Start))
                StartButtonDown3 = false;
            #endregion

            #region GamePad 4 Controls
            if (gamepad4.IsButtonDown(Buttons.A) && gamepad4.IsConnected)
            {
                if (!AButtonDown4 && menuManager.ActiveMenu != null && inMainMenu && (mainMenu.selected() == "New Game"))
                {
                    menuManager.Exit();
                    menuManager.Show("Character Menu");
                    AButtonDown4 = true;
                    inCharacterMenu = true;
                    inMainMenu = false;
                    inPauseMenu = false;
                }

                if (!AButtonDown4 && menuManager.ActiveMenu != null && inMainMenu && !inPauseMenu && (mainMenu.selected() == "Quit"))
                {
                    menuManager.Exit();
                    this.Exit();
                }

                if (!AButtonDown4 && menuManager.ActiveMenu != null && !inMainMenu && inPauseMenu && (pauseMenu.selected() == "Continue"))
                {
                    menuManager.Exit();
                    AButtonDown4 = true;
                    inGame = true;
                }

                if (!AButtonDown4 && menuManager.ActiveMenu != null && !inMainMenu && inPauseMenu && (pauseMenu.selected() == "Quit"))
                {
                    menuManager.Exit();
                    menuManager.Show("Main Menu");
                    AButtonDown4 = true;
                    inGame = false;
                    inMainMenu = true;
                    inPauseMenu = false;
                }

                if ((inWinMenu || inLoseMenu))
                {
                    AButtonDown4 = true;
                    inGame = false;
                    inMainMenu = true;
                    inCharacterMenu = false;
                    inPauseMenu = false;
                    inWinMenu = false;
                    inLoseMenu = false;
                }

                if (AButtonDown4)
                    AButtonDown4 = false;
            }

            if (gamepad4.IsButtonDown(Buttons.Back))
            {
                if (!BackButtonDown4 && menuManager.ActiveMenu != null && inMainMenu && !inPauseMenu && !inCharacterMenu)
                {
                    menuManager.Exit();
                    this.Exit();
                }

                if (!BackButtonDown4 && menuManager.ActiveMenu != null && inCharacterMenu && !inPauseMenu && !inMainMenu)
                {
                    menuManager.Exit();
                    menuManager.Show("Main Menu");
                    BackButtonDown4 = true;
                    inCharacterMenu = false;
                    inMainMenu = true;
                    inPauseMenu = false;
                }

                if (BackButtonDown4)
                    BackButtonDown4 = false;
            }

            if (gamepad4.IsButtonDown(Buttons.Start))
            {
                if (!StartButtonDown4 && menuManager.ActiveMenu == null && !inMainMenu && !inCharacterMenu)
                {
                    menuManager.Show("Pause Menu");
                    StartButtonDown4 = true;
                    inPauseMenu = true;
                }
                if (!StartButtonDown4 && menuManager.ActiveMenu != null && !inCharacterMenu)
                {
                    menuManager.Exit();
                    StartButtonDown4 = true;
                }
            }

            else if (gamepad4.IsButtonUp(Buttons.Start))
                StartButtonDown4 = false;
            #endregion

            #endregion

            if (menuManager.ActiveMenu == null) //Encapsulation to "Pause" game
            {
                fpsCounterOn = true;
                int aliveCount=0;
                foreach(Player p in players)
                {
                    if (p.isAlive) { ++aliveCount; }
                }
                if (aliveCount == 0 && Player.credit < Player.respawnCost) 
                {
                    enterLoseMenu = true;
                }

                if (Generator.functionalGeneratorNum == 5) 
                {
                    enterWinMenu = true;
                }

                #region Update Game Components
                // Background music

                if (!playing)
                {
                    MediaPlayer.IsRepeating = true;
                    MediaPlayer.Play(bgm);
                    playing = true;
                }

                List<Object> colliders = new List<Object>();
                GameComponent[] gcc = new GameComponent[Components.Count];
                Components.CopyTo(gcc, 0);
                foreach (GameComponent gc in gcc)
                {
                    if (!(gc is Object))
                    {
                        gc.Update(gameTime);
                    }
                    else
                    {
                        Object o = (Object)gc;
                        // Only update if the object is alive
                        if (o.isAlive)
                        {
                            colliders = grid.getPotentialColliders(o);
                            o.Update(gameTime, colliders, cameraTarget);
                            colliders.Clear();
                        }
                        else
                        {
                            if (o is Enemy)
                            {
                                Enemy e = (Enemy)o;
                                --e.spawnPoint.spawnCounter;
                                enemyList.Remove(e);
                            }
                            if (o is Player)
                            {
                                o.Update(gameTime, colliders, cameraTarget);
                            }
                            else
                            {
                                Components.Remove(o);
                                grid.removeDynamicObject(o);
                            }
                        }
                    }
                }
                #endregion

                #region Update The SpawnPoints
                // Spawn enemies when the spawnPoints respawn rate is reached
                foreach (SpawnPoint s in map.spawnPoints)
                {
                    s.Update(gameTime);
                    if (s.readyToSpawn() && s.spawnCounter < s.spawnLimit)
                    {
                        /*
                        bool skip = false;
                        int i = 0;
                        while (enemy[i] != null)
                        {
                            ++i;
                            if (i >= enemy.Length)
                            {
                                skip = true;
                                break;
                            }
                        }

                        if (skip)
                            continue;
                        */

                        Weapon[] w = new Weapon[1];
                        Model[] shotModel = new Model[1];
                        shotModel[0] = playerModel;
                        w[0] = new PowerFist(this, shotModel, Vector3.Zero);
                        Model[] em = new Model[1];
                        em[0] = enemyModel;

                        enemyList.Add(new Enemy(this, em, s, w));
                        Components.Add(enemyList[enemyList.Count - 1]);// Add the newest enemy in the enemyList to Components (last indexed enemy)
                        ++s.spawnCounter;
                    }
                }

                foreach (Building b in map.usableBuildings)
                {
                    if (b is Generator)
                    {
                        Generator g = (Generator)b;
                        g.Update(gameTime, cameraTarget);
                    }
                }

                #endregion
            }
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;
            spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);

            if (menuManager.ActiveMenu == null)
            {
                //List<Building> buildings = map.buildings;
                foreach (GameComponent gc in Components)
                {
                    if (gc is Object && camera.onScreen((Object)gc) && !(gc is FrameRateCounter))
                    {
                        Object o = (Object)gc;
                        o.Draw(camera);
                    }

                    if (gc is Map)
                    {
                        Map m = (Map)gc;
                        m.Draw(camera);
                    }
                
                    #region drawHealth
                    if (gc is Character && camera.onScreen((Object)gc))
                    {
                        Character c = (Character)gc;
                        if (!(c is Player))
                            c.drawHealth(camera, spriteBatch, GraphicsDevice, healthTexture);
                        else
                            c.drawHealth(camera, spriteBatch, GraphicsDevice, healthTexture, graphics.PreferredBackBufferWidth, graphics.PreferredBackBufferHeight);
                    }
                    else if (gc is Map)
                    {
                        Map m = (Map)gc;
                        foreach (Building b in m.usableBuildings)
                        {
                            if (b is Generator && camera.onScreen((Object)b))
                            {
                                Generator g = (Generator)b;
                                g.drawHealth(camera, spriteBatch, GraphicsDevice, healthTexture);
                            }
                        }
                    }
                    #endregion

                    #region weapons/skills
                    if (gc is Player)
                    {
                        Player p = (Player)gc;
                        //String s = "";
                        //long nowTick = DateTime.Now.Ticks;
                        int abilityNum = 0;
                        foreach (Weapon w in p.weapons)
                        {
                            float t = w.fireDelay - w.interval;
                            /*if (t < 0)
                            {
                                s += "0 ";
                            }
                            else
                            {
                                s += t + "s ";
                                userInterface.drawCooldowns(healthTexture, w.fireDelay, w.interval, abilityNum);
                            }*/
                            if (t >= 0) 
                            {
                                userInterface.drawCooldowns(healthTexture, w.fireDelay, w.interval, abilityNum, p.playerID);
                            }
                            ++abilityNum;
                        }
                        userInterface.drawSelectedWeapon(iconsTexture, p);//draw selected weapon frame
                        userInterface.drawSkills(iconsTexture, p);//draw icons
                        //spriteBatch.DrawString(spriteFont, s, new Vector2(33, 73), Color.Black, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0.01f);
                        //spriteBatch.DrawString(spriteFont, s, new Vector2(32, 72), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
                    }
                    //END TEST
                    #endregion
                }
                userInterface.drawUserInterface(players, enemyList, map.usableBuildings);

            }
            menuManager.Draw(spriteBatch, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
