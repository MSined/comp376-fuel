using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace F.U.E.L
{
    class Player : Character
    {
        
        //protected float lookAngle;
        
        public Player(Game game, Model[] modelComponents, Vector3 position,
            int topHP, int topSP, float speed, SpawnPoint spawnPoint, Weapon[] weapons
            )
            : base(game, modelComponents, position, topHP, topSP, speed, spawnPoint, weapons)
        {
            
        }

        public override void Update(GameTime gameTime) 
        {
            //Hack to get it working on a computer
            KeyboardState k = Keyboard.GetState();
            if (k.IsKeyDown(Keys.Up) || k.IsKeyDown(Keys.Down) || k.IsKeyDown(Keys.Left) || k.IsKeyDown(Keys.Right))
            {
                lookDirection = new Vector3(0, 0, 0);
                if (k.IsKeyDown(Keys.Up))
                    lookDirection += new Vector3(0, 0, -1);

                if (k.IsKeyDown(Keys.Down))
                    lookDirection += new Vector3(0, 0, 1);

                if (k.IsKeyDown(Keys.Left))
                    lookDirection += new Vector3(-1, 0, 0);

                if (k.IsKeyDown(Keys.Right))
                    lookDirection += new Vector3(1, 0, 0);
            }

            velocity = new Vector3(0, 0, 0);
            if (k.IsKeyDown(Keys.W) || k.IsKeyDown(Keys.S) || k.IsKeyDown(Keys.A) || k.IsKeyDown(Keys.D))
            {
                
                if (k.IsKeyDown(Keys.W))
                    velocity += new Vector3(0, 0, -1);

                if (k.IsKeyDown(Keys.S))
                    velocity += new Vector3(0, 0, 1);

                if (k.IsKeyDown(Keys.A))
                    velocity += new Vector3(-1, 0, 0);

                if (k.IsKeyDown(Keys.D))
                    velocity += new Vector3(1, 0, 0);
            }

            //Gamepad Support
            /*
            GamePadState gp = GamePad.GetState(PlayerIndex.One);
            lookDirection = new Vector3(gp.ThumbSticks.Right.X, 0, -gp.ThumbSticks.Right.Y);

            velocity = new Vector3(gp.ThumbSticks.Left.X, 0, -gp.ThumbSticks.Left.Y);
            */
             
            base.Update(gameTime);
        }

    }
}
