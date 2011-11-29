using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class SnakeGun : Weapon
    {
        private const float RANGE = 6;
        private const int DAMAGE = 1;
        private const int FIREDELAY = (int)(4 * 1000);
        private const int SPCOST = 10;

        private const int NUMBULLETS = 250;
        private const int LAUNCHDELAY = (int)(1 / 1000.0 * 1000);
        private int bulletsLeft = 0;
        private float launchInterval = 0;
        private Boolean shotByEnemy;
        private Player player;

        private const int maxSpread = 30;
        private int currentAngle = (int)(30/2.0f);
        private int sweepDir = 1;

        public SnakeGun(Game game, Model[] modelComponents, Vector3 position/*,
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

                float a = MathHelper.ToRadians(currentAngle);
                Matrix m = Matrix.CreateRotationY(a);

                currentAngle += sweepDir;
                if ((currentAngle >= maxSpread / 2.0f && sweepDir == 1) || (currentAngle <= -maxSpread / 2.0f && sweepDir == -1)) sweepDir = -sweepDir;

                game.Components.Add(new Bullet(game, this.bulletModelComponents, player.position, Vector3.Transform(player.lookDirection, m), range, damage, shotByEnemy));

                --bulletsLeft;
                launchInterval = 0;
            }
        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, GameTime gameTime, Vector3 cameraTarget)
        {
            if (interval > fireDelay)
            {
                currentAngle = (int)(30 / 2.0f);
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
