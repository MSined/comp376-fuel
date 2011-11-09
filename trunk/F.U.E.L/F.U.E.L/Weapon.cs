using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    abstract class Weapon : Object
    {
        public float range { get; protected set; }
        public float damage { get; protected set; }
        public int fireRate { get; protected set; }
        public long lastShot { get; protected set; }

        public Model[] bulletModelComponents;

        public Weapon(Game game, Model[] modelComponents, Vector3 position,
                      float range, float damage, int fireRate)
               : base(game, modelComponents, position, new FloatRectangle(position.X, position.Z, 0,0), true)
        {

        }

        public virtual void shoot(Vector3 position, Vector3 direction)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastShot + fireRate < nowTick)
            {
                game.Components.Add(new Bullet(game, this.bulletModelComponents, position, direction, range, damage));
                lastShot = nowTick;
            }
        }
