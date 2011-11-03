using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Shotgun : Weapon
    {
        private const float RANGE = 4;
        private const float DAMAGE = 5;
        private const int FIRERATE = 10000000;

        private const int angleDiff = 5;
        private const int numBullets = 5;

        public Shotgun(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {
            
        }

        public override void shoot(Vector3 position, Vector3 direction)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastShot + fireRate < nowTick)
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
                    game.Components.Add(new Bullet(game, this.bulletModelComponents, position, Vector3.Transform(direction, m), range, damage));
                }
                


                lastShot = nowTick;
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
