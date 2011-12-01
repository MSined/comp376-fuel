using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class MiniGun : Weapon
    {
        private const float RANGE = 7;
        private const int DAMAGE = 5;
        private const int FIREDELAY = (int)(1 / 20.0 * 1000);
        private const int SPCOST = 0;

        private Random random = new Random();
        private const int maxSpread = 10;

        public MiniGun(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIREDELAY, SPCOST)
        {
            soundEffect = game.Content.Load<SoundEffect>(@"Sounds/minigun");
            
        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, GameTime gameTime, Vector3 cameraTarget)
        {
            if (interval > fireDelay)
            {
                float shotAngle = (float)Math.Asin(direction.X) + MathHelper.ToRadians(180);
                if (direction.Z > 0)
                {
                    shotAngle = MathHelper.ToRadians(180) - shotAngle;
                }

                float a = -MathHelper.ToRadians(maxSpread)/2 + MathHelper.ToRadians(maxSpread) * (float) random.NextDouble();
                Matrix m = Matrix.CreateRotationY(a);
                game.Components.Add(new Bullet(game, this.bulletModelComponents, position, Vector3.Transform(direction, m), range, damage, shotByEnemy));
                interval = 0;
                playSound(position, cameraTarget);
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
