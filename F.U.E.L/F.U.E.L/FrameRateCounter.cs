using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace F.U.E.L
{
    public class FrameRateCounter : DrawableGameComponent
    {
        ContentManager content;
        SpriteBatch spriteBatch;
        SpriteFont spriteFont;

        int frameRate = 0;
        int frameCounter = 0;

        TimeSpan elapsedTime = TimeSpan.Zero;

        public FrameRateCounter(Game game)
            : base(game)
        {
            content = new ContentManager(game.Services);
            content.RootDirectory = "Content";
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            spriteFont = content.Load<SpriteFont>(@"FPSFont");
        }

        protected override void UnloadContent()
        {
            content.Unload();
        }

        public override void Update(GameTime gameTime)
        {
            elapsedTime += gameTime.ElapsedGameTime;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            frameCounter++;

            string fps = string.Format("fps: {0}", frameRate);

            spriteBatch.Begin();

            // Draw framerate
            if (Game1.fpsCounterOn)
            {
                string credits = string.Format("Credits: " + Player.credit.ToString());
                string towerCostString = string.Format("Tower Cost: " + Tower.towerCost);
                string reviveCostString = string.Format("Revive: " + Player.respawnCost);
                spriteBatch.DrawString(spriteFont, fps, new Vector2(30, 102), Color.Black);
                spriteBatch.DrawString(spriteFont, fps, new Vector2(28, 100), Color.White);
                spriteBatch.DrawString(spriteFont, credits, new Vector2(30, 30), Color.Black);
                spriteBatch.DrawString(spriteFont, credits, new Vector2(28, 28), Color.White);
                spriteBatch.DrawString(spriteFont, towerCostString, new Vector2(30, 54), Color.Black);
                spriteBatch.DrawString(spriteFont, towerCostString, new Vector2(28, 52), Color.White);
                spriteBatch.DrawString(spriteFont, reviveCostString, new Vector2(30, 78), Color.Black);
                spriteBatch.DrawString(spriteFont, reviveCostString, new Vector2(28, 76), Color.White);
            }



            spriteBatch.End();
        }
    }
}