using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class PushBackBullet : Bullet
    {
        private float pushBackDist = 0.6f;

        public PushBackBullet(Game game, Model[] modelComponents, Vector3 position,
            Vector3 direction, float range, int damage, Boolean shotByEnemy)
            : base(game, modelComponents, position, direction, range, damage, shotByEnemy)
        {
            
        }

        public override void CheckCollisions(List<Object> colliders)
        {
            foreach (Object o in colliders)
            {
                if (bounds.FloatIntersects(o.bounds))
                {
                    if (o is Player && this.shotByEnemy)
                    {
                        this.isAlive = false;
                        Player p = (Player)o;
                        p.hp = p.hp - this.damage;
                        p.position += direction * pushBackDist;
                        if (p.hp < 0) p.isAlive = false;
                        continue;
                    }
                    if (o is Enemy && !this.shotByEnemy)
                    {
                        this.isAlive = false;
                        Enemy e = (Enemy)o;
                        e.hp = e.hp - this.damage;
                        e.position += direction * pushBackDist;
                        if (e.hp < 0) e.isAlive = false;
                        continue;
                    }
                    if (o is Tower && this.shotByEnemy)//same as player, but tower
                    {
                        this.isAlive = false;
                        Tower t = (Tower)o;
                        t.hp = t.hp - this.damage;
                        if (t.hp < 0) t.isAlive = false;
                        continue;
                    }
                    if (o is Building)// && bounds.FloatIntersects(o.bounds))
                    {
                        this.isAlive = false;
                    }
                    if (o is Generator && this.shotByEnemy)//same as player, but generator
                    {
                        this.isAlive = false;
                        Generator g = (Generator)o;
                        g.hp = g.hp - this.damage;
                        continue;
                    }

                }
            }
        }

    }
}
