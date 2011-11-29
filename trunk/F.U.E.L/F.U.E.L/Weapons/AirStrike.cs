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
        private const float RANGE = 10;
        private const int DAMAGE = 8;
        private const int FIREDELAY = (int)(15 * 1000);
        private const int SPCOST = 10;

        private const int NUMEXPLOSIONS = 8;
        private const float EXPLOSIONRANGE = 2;
        private const int LAUNCHDELAY = (int)(1 / 10.0 * 1000);
        private int bulletsLeft = 0;
        private float launchInterval = 0;
        private Boolean shotByEnemy;
        private Vector3 direction;
        private Vector3 launchPosition;

        public AirStrike(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIREDELAY, SPCOST)
        {

        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            interval += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            launchInterval += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (launchInterval > LAUNCHDELAY && bulletsLeft > 0)
            {
                for (float i = 0; i < 2 * Math.PI; i += 0.2f)
                {
                    Matrix m = Matrix.CreateRotationY(i);
                    game.Components.Add(new Bullet(game, this.bulletModelComponents, launchPosition + direction * (NUMEXPLOSIONS - bulletsLeft + 1) * (range / NUMEXPLOSIONS), Vector3.Transform(direction, m), EXPLOSIONRANGE, damage, shotByEnemy));
                }

                --bulletsLeft;
                launchInterval = 0;
            }
        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, GameTime gameTime, Vector3 cameraTarget)
        {
            if (interval > fireDelay)
            {
                launchPosition = position;
                this.direction = direction;
                direction.Normalize();
                this.shotByEnemy = shotByEnemy;
                bulletsLeft = NUMEXPLOSIONS;
                interval = 0;
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
