using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class AirStrike : Weapon
    {
        private const float RANGE = 6;
        private const int DAMAGE = 10;
        private const int FIRERATE = (int)(15 * 10000000);

        private const int NUMEXPLOSIONS = 5;
        private const float EXPLOSIONRANGE = 2;
        private const int LAUNCHSPEED = (int)(1 / 7.0 * 10000000);
        private int bulletsLeft = 0;
        private long lastLaunch = 0;
        private Boolean shotByEnemy;
        private Vector3 direction;
        private Vector3 launchPosition;

        //private SoundEffect soundEffect;
        //private const int maxSpread = 50;

        public AirStrike(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {

        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastLaunch + LAUNCHSPEED < nowTick && bulletsLeft > 0)
            {
                /*float shotAngle = (float)Math.Asin(direction.X) + MathHelper.ToRadians(180);
                if (direction.Z > 0)
                {
                    shotAngle = MathHelper.ToRadians(180) - shotAngle;
                }*/

                //float a = -MathHelper.ToRadians(maxSpread) / 2 + MathHelper.ToRadians(maxSpread) * (float)random.NextDouble();
                //Matrix m = Matrix.CreateRotationY(a);

                //float r = range / 2 + range / 2 * (float)random.NextDouble();

                for (float i = 0; i < 2 * Math.PI; i += 0.2f)
                {
                    Matrix m = Matrix.CreateRotationY(i);
                    game.Components.Add(new Bullet(game, this.bulletModelComponents, launchPosition + direction * (NUMEXPLOSIONS - bulletsLeft + 1) * (range / NUMEXPLOSIONS), Vector3.Transform(direction, m), EXPLOSIONRANGE, damage, shotByEnemy));
                }

                
                --bulletsLeft;
                lastLaunch = nowTick;
            }
        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, Vector3 cameraTarget)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastShot + fireRate < nowTick)
            {
                launchPosition = position;
                this.direction = direction;
                direction.Normalize();
                this.shotByEnemy = shotByEnemy;
                bulletsLeft = NUMEXPLOSIONS;
                lastShot = nowTick;
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
