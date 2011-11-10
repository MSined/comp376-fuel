using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Generator : Building
    {
        private const int topHP = 500;
        private const int repairSpeed = 20;
        private const int repairRate = 10000000;
        private long lastRepair;

        public int hp { get; protected set; }
        private bool functional;
 
        public Generator(Game game, Model[] modelComponents, Vector3 position,
            float angle
            )
            : base(game, modelComponents, position, angle)
        {
            this.hp = 0;
            this.functional = false;
        }

        public override void use()
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastRepair + repairRate < nowTick)
            {
                hp += repairSpeed;
            }
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            CheckCollisions(colliders);

            if (hp >= topHP)
            {
                hp = topHP;
                functional = true;
            }

            if (hp <= 0) {
                hp = 0;
                functional = false;
            }
        }

        public void CheckCollisions(List<Object> colliders)
        {
            foreach (Object o in colliders)
            {
                if (bounds.FloatIntersects(o.bounds))
                {
                    if (o is Bullet)
                    {
                        Bullet b = (Bullet)o;
                        if (b.shotByEnemy)
                        {
                            o.isAlive = false;
                            this.hp = hp - b.damage;
                            continue;
                        }
                    }
                }
            }
        }
    }
}
