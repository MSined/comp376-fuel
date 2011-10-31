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
        Enemy[] enemy = new Enemy[4];


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
            map = new Map(this, a);
            Components.Add(map);

            enemyModel = Content.Load<Model>(@"Models\enemyModel");
            Model[] e = new Model[1];
            e[0] = enemyModel;
            Weapon[] w = new Weapon[1];
            enemy[0] = new Enemy(this, e, new Vector3(-4f, 0, -2), 10, 10, 1, new SpawnPoint(), w);
            enemy[1] = new Enemy(this, e, new Vector3(-3, 0, -2.5f), 10, 10, 1, new SpawnPoint(), w);
            enemy[2] = new Enemy(this, e, new Vector3(-4, 0, -3), 10, 10, 1, new SpawnPoint(), w);
            enemy[3] = new Enemy(this, e, new Vector3(-3.5f, 0, -3.2f), 10, 10, 1, new SpawnPoint(), w);

            playerModel = Content.Load<Model>(@"Models\playerModel");
            Model[] p = new Model[1];
            p[0] = playerModel;

            player = new Player(this, p, new Vector3(1, 0, 0), 10, 10, 1, new SpawnPoint(), w);
            
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

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            player.Draw(camera); 
            foreach (Enemy e in enemy){
                e.Draw(camera);
            }
            map.Draw(camera);

            base.Draw(gameTime);
        }
    }
}
