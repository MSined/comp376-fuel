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
        public float damage { get; protected set; }
        public int fireRate { get; protected set; }
        public long lastShot { get; protected set; }

        public Model[] bulletModelComponents;

        public Weapon(Game game, Model[] modelComponents, Vector3 position,
            float range, float damage, int fireRate)
            : base(game, modelComponents, position)
        {
            this.range = range;
            this.damage = damage;
            this.fireRate = fireRate;
            this.lastShot = 0;

            this.bulletModelComponents = new Model[1];
            this.bulletModelComponents[0] = modelComponents[0];
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

        /*
        else//have target 
        {
            Vector2 to_target = new Vector2(target.getPosition().X - this.position.X, target.getPosition().Y - this.position.Y);
            //velocity, position, atkRange, atkTimer, atk
            if (to_target.Length() > atkRange)//target out of atkRange, move
            {
                move(to_target);
            }
            else//target in atkRange, atk
            {
                atkTimer += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
                if (atkTimer > atkInterval)
                {
                    target.setHp(target.getHp() - atk);
                    atkTimer = 0;
                }
            }
            //lookDirection, lookAngle
            lookDirection = to_target / to_target.Length();
            if (lookDirection.X > 0)
            {
                lookAngle = (float)Math.Asin(lookDirection.Y / lookDirection.Length());
            }
            else
            {
                lookAngle = (float)Math.Asin(lookDirection.Y / lookDirection.Length()) + MathHelper.ToRadians(180);
            }
        }
        * */
    }
}
