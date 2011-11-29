using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class FlameThrower : Weapon
    {
        private const float RANGE = 5;
        private const int DAMAGE = 1;
        private const int FIREDELAY = (int)(6 * 1000);
        private const int SPCOST = 10;

        private const int NUMBULLETS = 100;
        private const int LAUNCHDELAY = (int)(5 / 1000.0 * 1000);
        private int bulletsLeft = 0;
        private float launchInterval = 0;
        private Boolean shotByEnemy;
        private Player player;

        private Random random = new Random();
        private const int maxSpread = 50;

        public FlameThrower(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/, Player p)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIREDELAY, SPCOST)
        {
            this.player = p;
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            interval += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            launchInterval += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (launchInterval > LAUNCHDELAY && bulletsLeft > 0)
            {
                float shotAngle = (float)Math.Asin(player.lookDirection.X) + MathHelper.ToRadians(180);
                if (player.lookDirection.Z > 0)
                {
                    shotAngle = MathHelper.ToRadians(180) - shotAngle;
                }

                float a = -MathHelper.ToRadians(maxSpread) / 2 + MathHelper.ToRadians(maxSpread) * (float)random.NextDouble();
                Matrix m = Matrix.CreateRotationY(a);

                float r = range / 2 + range / 2 * (float)random.NextDouble();

                game.Components.Add(new BurningBullet(game, this.bulletModelComponents, player.position, Vector3.Transform(player.lookDirection, m), r, damage, shotByEnemy));

                --bulletsLeft;
                launchInterval = 0;
            }
        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, GameTime gameTime, Vector3 cameraTarget)
        {
            if (interval > fireDelay)
            {
                //launchPosition = position;
                //this.direction = direction;
                this.shotByEnemy = shotByEnemy;
                bulletsLeft = NUMBULLETS;
                interval = 0;
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
