using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class Generator : Building
    {
        public static int functionalGeneratorNum = 0; 

        private const int topHP = 500;
        private const int repairSpeed = 20;
        private const int repairRate = 10000000;
        public long lastRepair;

        protected SoundEffect soundEffectPowerUp;
        protected SoundEffect soundEffectPowerDown;

        public int hp;
        public bool functional;
 
        public Generator(Game game, Model[] modelComponents, Vector3 position,
            float angle
            )
            : base(game, modelComponents, position, 0.7f, 0.7f, angle, false)
        {
            this.hp = 50;
            this.functional = false;
            this.lastRepair = 0;

            soundEffectPowerUp = game.Content.Load<SoundEffect>(@"Sounds/powerup");
            soundEffectPowerDown = game.Content.Load<SoundEffect>(@"Sounds/powerdown");
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

        public void Update(GameTime gameTime, Vector3 cameraTarget)
        {
            if (hp >= topHP && !functional)
            {
                hp = topHP;
                functional = true;
                ++functionalGeneratorNum;
                playSoundAlive(position, cameraTarget);
            }

            if (hp <= 0 && functional) {
                hp = 0;
                functional = false;
                --functionalGeneratorNum;
                playSoundDies(position, cameraTarget);
            }
        }

        public void playSoundAlive(Vector3 position, Vector3 cameraTarget)
        {
            float dist = (cameraTarget - position).LengthSquared();
            float vol = dist / 300;
            float scaledVol = (vol >= 1 ? 0 : (1 - vol));
            soundEffectPowerUp.Play(scaledVol, 0.0f, 0.0f);
        }

        public void playSoundDies(Vector3 position, Vector3 cameraTarget)
        {
            float dist = (cameraTarget - position).LengthSquared();
            float vol = dist / 300;
            float scaledVol = (vol >= 1 ? 0 : (1 - vol));
            soundEffectPowerDown.Play(scaledVol, 0.0f, 0.0f);
        }

        public override void drawHealth(Camera camera, SpriteBatch spriteBatch, GraphicsDevice graphicsDevice, Texture2D healthTexture)
        {
            int healthBarWidth = 40;
            int healthBarHeight = 5;
            Rectangle srcRect, destRect;

            Vector3 screenPos = graphicsDevice.Viewport.Project(this.position + new Vector3(0, 1.5f, 0), camera.projection, camera.view, Matrix.Identity);

            srcRect = new Rectangle(0, 0, 1, 1);
            destRect = new Rectangle((int)screenPos.X - healthBarWidth / 2, (int)screenPos.Y, healthBarWidth, healthBarHeight);
            spriteBatch.Draw(healthTexture, destRect, srcRect, Color.LightGray, 0f, Vector2.Zero, SpriteEffects.None, 0.91f);

            float healthPercentage = (float)hp / (float)topHP;

            srcRect = new Rectangle(0, 0, 1, 1);
            destRect = new Rectangle((int)screenPos.X - healthBarWidth / 2, (int)screenPos.Y, (int)(healthPercentage * healthBarWidth), healthBarHeight);
            spriteBatch.Draw(healthTexture, destRect, srcRect, Color.Blue, 0f, Vector2.Zero, SpriteEffects.None, 0.9f);
        }
    }
}
