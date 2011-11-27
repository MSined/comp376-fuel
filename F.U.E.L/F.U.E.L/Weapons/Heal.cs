using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class Heal : Weapon
    {
        private const float RANGE = 0;
        private const int DAMAGE = 0;
        private const int HEALING = 100;
        private const int FIRERATE = 12 * 10000000;

        //private SoundEffect soundEffect;

        public Heal(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {
            //soundEffect = game.Content.Load<SoundEffect>(@"Sounds/assaultrifle");
        }

        public override void Draw(Camera camera)
        {

        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {

        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastShot + fireRate < nowTick)
            {
                //game.Components.Add(new Bullet(game, this.bulletModelComponents, position, direction, range, damage, shotByEnemy));

                foreach (GameComponent gc in game.Components)
                {
                    if (gc is Player)
                    {
                        Player p = (Player)gc;
                        p.hp += HEALING;
                        if (p.hp > p.topHP) p.hp = p.topHP;
                    }
                }
                lastShot = nowTick;
                //soundEffect.Play();
            }
        }
    }
}
