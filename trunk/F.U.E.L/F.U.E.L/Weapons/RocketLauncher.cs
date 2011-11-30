using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class RocketLauncher : Weapon
    {
        private const float RANGE = 100;
        private const int DAMAGE = 35;
        private const int FIREDELAY = (int)(10 * 1000);
        private const int SPCOST = 10;

        private const int angleDiff = 25;
        private const int numBullets = 3;

        public RocketLauncher(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIREDELAY, SPCOST)
        {
            soundEffect = game.Content.Load<SoundEffect>(@"Sounds/seakingmissle");
            
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

                float stratingNum = (numBullets-1)/2f;
                for (float i = -stratingNum; i <= stratingNum; ++i)
                {
                    float a = MathHelper.ToRadians(angleDiff) * i;
                    Matrix m = Matrix.CreateRotationY(a);
                    game.Components.Add(new SeekingBullet(game, this.bulletModelComponents, position, Vector3.Transform(direction, m), range, damage, shotByEnemy));
                }

                interval = 0;
                playSound(position, cameraTarget);
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
