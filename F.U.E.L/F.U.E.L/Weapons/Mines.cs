using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Mines : Weapon
    {
        private const float RANGE = 10;
        private const float DAMAGE = 100;
        private const int FIRERATE = 10000000;

        public Mines(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {
            
        }

        public override void shoot(Vector3 position, Vector3 direction)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastShot + fireRate < nowTick)
            {
                game.Components.Add(new AOEBullet(game, this.bulletModelComponents, position, new Vector3(0,0,0), range, damage));
                lastShot = nowTick;
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
