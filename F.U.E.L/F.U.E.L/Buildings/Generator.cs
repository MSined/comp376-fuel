using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Generator : Building
    {
        private const int topHP = 500;
        private const int repairSpeed = 20;
        private const int repairRate = 10000000;
        private long lastRepair;

        public int hp;
        public bool functional;
 
        public Generator(Game game, Model[] modelComponents, Vector3 position,
            float angle
            )
            : base(game, modelComponents, position, 0.7f, 0.7f, angle)
        {
            this.hp = 10;
            this.functional = false;
            this.lastRepair = 0;
        }

        public override void use()
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastRepair + repairRate < nowTick)
            {
                hp += repairSpeed;
                lastRepair = nowTick;
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (hp >= topHP)
            {
                hp = topHP;
                functional = true;
            }

            if (hp <= 0) {
                hp = 0;
                functional = false;
            }
        }

        public void drawHealth(Camera camera, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Texture2D healthTexture)
        {
            int healthBarWidth = 40;
            int healthBarHeight = 5;
            Rectangle srcRect, destRect;

            Vector3 screenPos = graphicsDevice.Viewport.Project(this.position + new Vector3(0, 1.5f, 0), camera.projection, camera.view, Matrix.Identity);

            srcRect = new Rectangle(0, 0, 1, 1);
            destRect = new Rectangle((int)screenPos.X - healthBarWidth / 2, (int)screenPos.Y, healthBarWidth, healthBarHeight);
            spriteBatch.Draw(healthTexture, destRect, srcRect, Color.LightGray);

            float healthPercentage = (float)hp / (float)topHP;

            srcRect = new Rectangle(0, 0, 1, 1);
            destRect = new Rectangle((int)screenPos.X - healthBarWidth / 2, (int)screenPos.Y, (int)(healthPercentage * healthBarWidth), healthBarHeight);
            spriteBatch.Draw(healthTexture, destRect, srcRect, Color.Blue);
        }
    }
}
