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

        Model planeModel, towerModel, generatorModel, enemyModel, playerModel;

        Camera camera;
        Map map;
        Player player;
        Enemy[] enemy = new Enemy[12];
        SpatialHashGrid grid;
        Model[] em = new Model[1];

        List<Object> removeList = new List<Object>();

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

            planeModel = Content.Load<Model>(@"Models\planeModel");
            towerModel = Content.Load<Model>(@"Models\towerModel");
            generatorModel = Content.Load<Model>(@"Models\generatorModel");

            Model[] a = new Model[3];
            a[0] = planeModel;
            a[1] = towerModel;
            a[2] = generatorModel;
            map = new Map(this, a, -10, 10);
			Components.Add(map);

            // Create the grid with necessary information
            grid = new SpatialHashGrid(20, 20, 2, map.leftXPos/2, map.bottomYPos/2);
            for(int i = 0; i < map.buildings.Count; ++i)
                grid.insertStaticObject(map.buildings[i]);

            playerModel = Content.Load<Model>(@"Models\playerModel");
            Model[] p = new Model[1];
            p[0] = playerModel;
            Weapon[] w = new Weapon[4];
            w[0] = new Pistol(this, p, new Vector3(0, 0, 0));
            w[1] = new Shotgun(this, p, new Vector3(0, 0, 0));
            w[2] = new Mines(this, p, new Vector3(0, 0, 0));
            w[3] = new Grenade(this, p, new Vector3(0, 0, 0));
            player = new Player(this, p, new Vector3(5, 0, 5), 500, 100, 0.08f, new SpawnPoint(), w);
            Components.Add(player);

            enemyModel = Content.Load<Model>(@"Models\enemyModel");
            Model[] em = new Model[1];
            em[0] = enemyModel;

            for (int i = 0; i < enemy.Length; ++i)
            {
                w = new Weapon[1];
                w[0] = new PowerFist(this, p, new Vector3(0, 0, 0));
                enemy[i] = new HunterEnemy(this, em, new Vector3(i, 0, 0), new SpawnPoint(), w);
            }

            foreach (Enemy e in enemy)
            {
                Components.Add(e);
            }

            // Remove whatever is in the removeList, from the grid and the components list
            /*
            foreach (Object o in removeList)
            {
                grid.removeDynamicObject(o);
                Components.Remove(o);
            }
             * */
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
            Components.CopyTo(gcc,0);
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

            foreach (GameComponent gc in Components)
            {
                if (gc is Object)
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
        }
    }
}
