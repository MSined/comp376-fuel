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
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D healthTexture;

        Model planeModel, towerModel, generatorModel, enemyModel, playerModel, buildingModel, treeModel, telePadModel;

        Camera camera;
        Map map;
        List<Player> players = new List<Player>();
        Enemy[] enemy = new Enemy[3];
        SpatialHashGrid grid;
        Model[] em = new Model[1];

        List<Object> removeList = new List<Object>();

        public Game1()
        {
            Components.Add(new FrameRateCounter(this));
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //graphics.IsFullScreen = true;
            // The following code removes the XNA fixed timestep (framerate limiter)
            IsFixedTimeStep = false;
            // Because the above is an artificial but necessary step, this one sets the timestep to 1ms
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 1);
            // This removes the synchronization with the screen to allow a faster framerate
            graphics.SynchronizeWithVerticalRetrace = false;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Create camera and add to components list
            camera = new Camera(this, new Vector3(0, 10, 10), Vector3.Zero, -Vector3.UnitZ);
            Components.Add(camera);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Services.AddService(typeof(SpriteBatch), spriteBatch);

            //NEED TO MAKE BLANK 1x1 TEXTURE
            healthTexture = Content.Load<Texture2D>(@"Textures\enemyTexture");

            planeModel = Content.Load<Model>(@"Models\planeModel");
            towerModel = Content.Load<Model>(@"Models\towerModel");
            generatorModel = Content.Load<Model>(@"Models\generatorModel");
            buildingModel = Content.Load<Model>(@"Models\TestBuilding");
            playerModel = Content.Load<Model>(@"Models\playerModel");
            treeModel = Content.Load<Model>(@"Models\treeModel");
            telePadModel = Content.Load<Model>(@"Models\telePadModel");

            Model[] a = new Model[6];
            a[0] = planeModel;
            a[1] = towerModel;
            a[2] = generatorModel;
            a[3] = buildingModel;
            a[4] = treeModel;
            a[5] = telePadModel;
            map = new Map(this, a, -36, 36);
            Components.Add(map);


            Model[] p = new Model[1];
            p[0] = playerModel;
            Weapon[] w = new Weapon[4];
            w[0] = new Pistol(this, p, new Vector3(0, 0, 0));
            w[1] = new FlameThrower(this, p, new Vector3(0, 0, 0));
            w[2] = new Mines(this, p, new Vector3(0, 0, 0));
            w[3] = new Grenade(this, p, new Vector3(0, 0, 0));
            players.Add(new Player(this, p, 500, 100, 0.08f, map.spawnPoints[0], w));
            foreach (Player ply in players) { Components.Add(ply); }

            // Create the grid with necessary information
            grid = new SpatialHashGrid(72, 72, 2, map.leftXPos / 2, map.bottomYPos / 2);
            for (int i = 0; i < map.buildings.Count; ++i)
                grid.insertStaticObject(map.buildings[i]);

            enemyModel = Content.Load<Model>(@"Models\alien788");
            Model[] em = new Model[1];
            em[0] = enemyModel;

            for (int i = 0; i < enemy.Length; ++i)
            {
                w = new Weapon[1];
                w[0] = new PowerFist(this, p, new Vector3(0, 0, 0));
                enemy[i] = new HunterEnemy(this, em, map.spawnPoints[1], w);
            }

            foreach (Enemy e in enemy)
            {
                Components.Add(e);
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

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
                        o.Update(gameTime, colliders);
                        colliders.Clear();
                    }
                    else
                    {
                        Components.Remove(o);
                        grid.removeDynamicObject(o);
                    }
                }
            }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            GraphicsDevice.DepthStencilState = DepthStencilState.Default;

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
            }
            base.Draw(gameTime);

            spriteBatch.Begin();
            
            List<Building> buildings = map.buildings;
            foreach (GameComponent gc in Components)
            {
                if (gc is Character && camera.onScreen((Object)gc))
                {
                    Character c = (Character)gc;
                    c.drawHealth(camera, spriteBatch, GraphicsDevice, healthTexture);
                }
                if (gc is Generator && camera.onScreen((Object)gc))
                {
                    Generator g = (Generator)gc;
                    g.drawHealth(camera, spriteBatch, GraphicsDevice, healthTexture);
                }
            }
            spriteBatch.End();
        }
    }
}
