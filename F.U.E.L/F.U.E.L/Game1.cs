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

        Texture2D healthTexture, UITexture, minimapTexture, unitsTexture, iconsTexture;

        Model planeModel, towerModel, generatorModel, enemyModel, playerModel, buildingModel, treeModel, telePadModel, checkBoxModel;

        Camera camera;
        Map map;
        List<Player> players = new List<Player>();
        //Enemy[] enemy = new Enemy[10];
        List<Enemy> enemyList = new List<Enemy>();
        SpatialHashGrid grid;
        SuperModel[] em = new SuperModel[1];
        SuperModel[] p;
        SuperModel[] t;

        MouseState mouse;
        KeyboardState keyboard;
        GamePadState gamepad1;

        UI userInterface;

        List<Object> removeList = new List<Object>();

        public Effect redEffect, greenEffect, shadingEffect;

        FrameRateCounter fpsCounter;

        MainMenu pauseMenu;
        MainMenu mainMenu;
        MainMenu characterMenu;

        protected Song bgm;

        private bool playing = false;

        private bool EscapeKeyDown = false;
        private bool EnterKeyDown = false;

        private bool BackButtonDown = false;
        private bool StartButtonDown = false;
        private bool AButtonDown = false;

        private bool inGame = false;
        private bool inMainMenu = false;
        private bool inCharacterMenu = false;

        public Vector3 cameraTarget { get; private set; }

        public Game1()
        {
            fpsCounter = new FrameRateCounter(this);
            Components.Add(fpsCounter);
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;
            // The following code removes the XNA fixed timestep (framerate limiter)
            IsFixedTimeStep = false;
            // Because the above is an artificial but necessary step, this one sets the timestep to 1ms
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1);
            // This removes the synchronization with the screen to allow a faster framerate
            graphics.SynchronizeWithVerticalRetrace = false;

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

            planeModel = Content.Load<Model>(@"Models\planeModel");
            towerModel = Content.Load<Model>(@"Models\towerModel");
            generatorModel = Content.Load<Model>(@"Models\generatorModel");
            buildingModel = Content.Load<Model>(@"Models\TestBuilding");
            playerModel = Content.Load<Model>(@"Models\playerModel");
            treeModel = Content.Load<Model>(@"Models\treeModel");
            telePadModel = Content.Load<Model>(@"Models\telePadModel");
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
                new List<Rectangle>() { new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 - 30, 150, 50), new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 + 30, 150, 50) },
                new List<string>() { "Continue", "Quit" }
                );
            mainMenu = new MainMenu("Main Menu"); // main menu
            mainMenu.Load(Content, menuBG, menuBGSound, menuOpenPath, menuClosePath);
            mainMenu.LoadButtons(Content,
                new int[] { 1, 2 },
                new List<Rectangle>() { new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 - 30, 150, 50), new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 + 30, 150, 50) },
                new List<string>() { "New Game", "Quit" }
                );
            characterMenu = new MainMenu("Character Menu"); // main menu
            characterMenu.Load(Content, menuBG, menuBGSound, menuOpenPath, menuClosePath);
            characterMenu.LoadButtons(Content,
                new int[] { 1, 2, 3, 4 },
                new List<Rectangle>() { new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 - 90, 150, 50), new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 - 30, 150, 50), new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 + 30, 150, 50), new Rectangle(graphics.PreferredBackBufferWidth / 2, graphics.PreferredBackBufferHeight / 2 + 90, 150, 50) },
                new List<string>() { "Gunner", "Alchemist", "Sniper", "Tank" }
                );

            // Initiate menus
            menuManager.AddMenu("Main Menu", mainMenu);
            menuManager.AddMenu("Character Menu", characterMenu);
            menuManager.AddMenu("Pause Menu", pauseMenu);

            userInterface = new UI(spriteBatch, GraphicsDevice, UITexture, healthTexture, graphics.PreferredBackBufferHeight, graphics.PreferredBackBufferWidth, minimapTexture, unitsTexture);

            //redEffect = Content.Load<Effect>(@"Effects\Red");
            //greenEffect = Content.Load<Effect>(@"Effects\Green");
            shadingEffect = Content.Load<Effect>(@"Effects\Shader");

            SuperModel[] a = new SuperModel[6];
            a[0] = new SuperModel(ref planeModel, shadingEffect);
            a[1] = new SuperModel(ref towerModel, shadingEffect);
            a[2] = new SuperModel(ref generatorModel, shadingEffect);
            a[3] = new SuperModel(ref buildingModel, shadingEffect);
            a[4] = new SuperModel(ref treeModel, shadingEffect);
            a[5] = new SuperModel(ref telePadModel, shadingEffect);
            map = new Map(this, a, -36, 36);
            Components.Add(map);

            // The first model will be of the player
            // The LAST model (always last) will be the checkBox model
            p = new SuperModel[1];
            p[0] = new SuperModel(ref playerModel, shadingEffect);
            t = new SuperModel[1];
            t[0] = new SuperModel(ref checkBoxModel, shadingEffect);
            players.Add(new Player(this, p, map.spawnPoints[0], Player.Class.Tank, PlayerIndex.One));
            /*players.Add(new Player(this, p, map.spawnPoints[1], Player.Class.Sniper, PlayerIndex.Two));
            players.Add(new Player(this, p, map.spawnPoints[2], Player.Class.Gunner, PlayerIndex.Three));
            players.Add(new Player(this, p, map.spawnPoints[3], Player.Class.Alchemist, PlayerIndex.Four));
            */
			/*players.Add(new Player(this, p, map.spawnPoints[1], Player.Class.Sniper, PlayerIndex.Two));
            players.Add(new Player(this, p, map.spawnPoints[2], Player.Class.Gunner, PlayerIndex.Three));
            players.Add(new Player(this, p, map.spawnPoints[3], Player.Class.Alchemist, PlayerIndex.Four));
            */
            players[0].checkBox = new BuildBox(this, t, players[0].position,
                                                new FloatRectangle((players[0].position + players[0].lookDirection).X, (players[0].position + players[0].lookDirection).Z, 1, 1),
                                                players[0]);
            /*players[1].checkBox = new BuildBox(this, t, players[1].position,
                                                new FloatRectangle((players[1].position + players[1].lookDirection).X, (players[1].position + players[1].lookDirection).Z, 1, 1),
                                                players[1]);
            players[2].checkBox = new BuildBox(this, t, players[2].position,
                                                new FloatRectangle((players[2].position + players[2].lookDirection).X, (players[2].position + players[2].lookDirection).Z, 1, 1),
                                                players[2]);
            players[3].checkBox = new BuildBox(this, t, players[3].position,
                                                new FloatRectangle((players[3].position + players[3].lookDirection).X, (players[3].position + players[3].lookDirection).Z, 1, 1),
                                                players[3]);
            */
            foreach (Player ply in players) { Components.Add(ply); }

            // Create the grid with necessary information
            grid = new SpatialHashGrid(72, 72, 2, map.leftXPos / 2, map.bottomYPos / 2);
            for (int i = 0; i < map.buildings.Count; ++i)
                grid.insertStaticObject(map.buildings[i]);

            for (int i = 0; i < map.usableBuildings.Count; ++i)//for collisions
                grid.insertStaticObject(map.usableBuildings[i]);

            enemyModel = Content.Load<Model>(@"Models\alien788_60");
            Model[] em = new Model[1];
            em[0] = enemyModel;
            /*
            for (int i = 0; i < enemy.Length; ++i)
            {
                w = new Weapon[1];
                w[0] = new PowerFist(this, p, new Vector3(0, 0, 0));
                enemy[i] = new HunterEnemy(this, em, map.spawnPoints[1], w);
            }
            */
            /*
            foreach (Enemy e in enemy)
            {
                Components.Add(e);
            }*/
        }

        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            cameraTarget = camera.cameraTarget;

            keyboard = Keyboard.GetState();
            gamepad1 = GamePad.GetState(PlayerIndex.One);

            menuManager.Update(keyboard, gamepad1);
            pauseMenu.Update(keyboard, gamepad1);
            mainMenu.Update(keyboard, gamepad1);

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

            if (!inGame && !inMainMenu && !inCharacterMenu)
            {
                menuManager.Show("Main Menu");
                inMainMenu = true;
            }

            #region Keyboard Controls for menu
            // Enter in main
            if (keyboard.IsKeyDown(Keys.Enter) &&
                !EnterKeyDown &&
                menuManager.ActiveMenu != null &&
                inMainMenu &&
                (mainMenu.selected() == "New Game"))
            {
                menuManager.Exit();
                menuManager.Show("Character Menu");
                EnterKeyDown = true;
                inCharacterMenu = true;
                inMainMenu = false;
                //inGame = true;
            }

            if (keyboard.IsKeyDown(Keys.Enter) &&
                !EnterKeyDown &&
                menuManager.ActiveMenu != null &&
                inMainMenu &&
                (mainMenu.selected() == "Quit"))
            {
                menuManager.Exit();
                this.Exit();
            }

            //if (keyboard.IsKeyDown(Keys.Enter) && !EnterKeyDown && menuManager.ActiveMenu != null && !inMainMenu && inCharacterMenu && (characterMenu.selected() == "Gunner"))
            //{
            //    //players.Add(new Player(this, p, map.spawnPoints[0], Player.Class.Tank, PlayerIndex.One));
            //    inGame = true;
            //    inCharacterMenu = false;
            //}

            if (keyboard.IsKeyDown(Keys.Enter) && !EnterKeyDown && menuManager.ActiveMenu != null && !inMainMenu && (pauseMenu.selected() == "Continue"))
            {
                menuManager.Exit();
                EnterKeyDown = true;
                inGame = true;
            }

            if (keyboard.IsKeyDown(Keys.Enter) && !EnterKeyDown && menuManager.ActiveMenu != null && !inMainMenu && (pauseMenu.selected() == "Quit"))
            {
                menuManager.Exit();
                menuManager.Show("Main Menu");
                EnterKeyDown = true;
                inGame = false;
                inMainMenu = true;
            }

            if (keyboard.IsKeyDown(Keys.Escape) && !EscapeKeyDown && menuManager.ActiveMenu != null && inMainMenu)
            {
                menuManager.Exit();
                this.Exit();
            }

            if ((keyboard.IsKeyDown(Keys.Escape) && !EscapeKeyDown && menuManager.ActiveMenu == null && !inMainMenu))
            {
                menuManager.Show("Pause Menu");
                EscapeKeyDown = true;
            }
            if (keyboard.IsKeyDown(Keys.Escape) && !EscapeKeyDown && menuManager.ActiveMenu != null)
            {
                menuManager.Exit();
                EscapeKeyDown = true;
            }

            if ((keyboard.IsKeyUp(Keys.Escape) && EscapeKeyDown))
                EscapeKeyDown = false;

            if (keyboard.IsKeyUp(Keys.Enter) && EnterKeyDown)
                EnterKeyDown = false;
            #endregion

            #region gamepad controls for menu
            if (gamepad1.IsButtonDown(Buttons.A) && !AButtonDown && menuManager.ActiveMenu != null && inMainMenu && (mainMenu.selected() == "New Game"))
            {
                menuManager.Exit();
                AButtonDown = true;
                inMainMenu = false;
                inGame = true;
            }

            if (gamepad1.IsButtonDown(Buttons.A) && !AButtonDown && menuManager.ActiveMenu != null && inMainMenu && (mainMenu.selected() == "Quit"))
            {
                menuManager.Exit();
                this.Exit();
            }

            if (gamepad1.IsButtonDown(Buttons.A) && !AButtonDown && menuManager.ActiveMenu != null && !inMainMenu && (pauseMenu.selected() == "Continue"))
            {
                menuManager.Exit();
                AButtonDown = true;
                inGame = true;
            }

            if (gamepad1.IsButtonDown(Buttons.A) && !AButtonDown && menuManager.ActiveMenu != null && !inMainMenu && (pauseMenu.selected() == "Quit"))
            {
                menuManager.Exit();
                menuManager.Show("Main Menu");
                AButtonDown = true;
                inGame = false;
                inMainMenu = true;
            }

            if (gamepad1.IsButtonDown(Buttons.Back) && !BackButtonDown && menuManager.ActiveMenu != null && inMainMenu)
            {
                menuManager.Exit();
                this.Exit();
            }


            if ((gamepad1.IsButtonDown(Buttons.Start) && !StartButtonDown && menuManager.ActiveMenu == null && !inMainMenu))
            {
                menuManager.Show("Pause Menu");
                StartButtonDown = true;
            }
            if (gamepad1.IsButtonDown(Buttons.Start) && !StartButtonDown && menuManager.ActiveMenu != null)
            {
                menuManager.Exit();
                StartButtonDown = true;
            }

            if (gamepad1.IsButtonUp(Buttons.Start) && StartButtonDown)
                StartButtonDown = false;

            if (gamepad1.IsButtonUp(Buttons.Back) && BackButtonDown)
                BackButtonDown = false;

            if (gamepad1.IsButtonUp(Buttons.A) && AButtonDown)
                AButtonDown = false;
            #endregion

            if (menuManager.ActiveMenu == null) //Encapsulation to "Pause" game
            {
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
                                /*int i = 0;
                                while (enemy[i] != null && enemy[i].objectID != o.objectID)
                                {
                                    ++i;
                                }
                                enemy[i] = null;*/
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
                        SuperModel[] shotModel = new SuperModel[1];
                        shotModel[0] = new SuperModel(ref playerModel, shadingEffect);
                        w[0] = new PowerFist(this, shotModel, Vector3.Zero);
                        SuperModel[] em = new SuperModel[1];
                        em[0] = new SuperModel(ref enemyModel, shadingEffect);

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
                    if (gc is Object && camera.onScreen((Object)gc))
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
