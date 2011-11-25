using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class SnakeGun : Weapon
    {
        private const float RANGE = 6;
        private const int DAMAGE = 1;
        private const int FIRERATE = (int)(4 * 10000000);

        private const int NUMBULLETS = 250;
        private const int LAUNCHSPEED = (int)(1 / 1000.0 * 10000000);
        private int bulletsLeft = 0;
        private long lastLaunch = 0;
        private Boolean shotByEnemy;
        private Player player;

        //private SoundEffect soundEffect;

        private const int maxSpread = 30;
        private int currentAngle = (int)(30/2.0f);
        private int sweepDir = 1;

        public SnakeGun(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/, Player p)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {
            this.player = p;
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastLaunch + LAUNCHSPEED < nowTick && bulletsLeft > 0)
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
                lastLaunch = nowTick;
            }
        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastShot + fireRate < nowTick)
            {
                currentAngle = (int)(30 / 2.0f);
                //launchPosition = position;
                //this.direction = direction;
                this.shotByEnemy = shotByEnemy;
                bulletsLeft = NUMBULLETS;
                lastShot = nowTick;
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
