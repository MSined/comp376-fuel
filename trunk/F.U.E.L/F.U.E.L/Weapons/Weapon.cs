using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    abstract class Weapon : Object
    {
        public float range { get; protected set; }
        public int damage { get; protected set; }
        public int fireRate { get; protected set; }
        public long lastShot { get; protected set; }

        public Model[] bulletModelComponents;

        public Weapon(Game game, Model[] modelComponents, Vector3 position,
                      float range, int damage, int fireRate)
               : base(game, modelComponents, position, new FloatRectangle(position.X, position.Z, 0,0), true)
        {
            this.range = range;
            this.damage = damage;
            this.fireRate = fireRate;
            this.lastShot = 0;

            this.bulletModelComponents = new Model[1];
            this.bulletModelComponents[0] = modelComponents[0];
        }

        public virtual void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastShot + fireRate < nowTick)
            {
                game.Components.Add(new Bullet(game, this.bulletModelComponents, position, direction, range, damage, shotByEnemy));
                lastShot = nowTick;
            }
        }

    }
}
