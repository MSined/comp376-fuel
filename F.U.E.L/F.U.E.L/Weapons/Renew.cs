using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class Renew : Weapon
    {
        private const float RANGE = 0;
        private const int DAMAGE = 0;
        private const int FIREDELAY = 3 * 1000;
        private const int SPCOST = 10;

        private const int HEALING = 5;
        private const int HEALINGTIME = 10 * 1000;
        private const int HEALINGRATE = (int)(1/2.0 * 1000);
        private float currentHealInterval = 0;
        private float healInterval = 0;
        //private SoundEffect soundEffect;

        public Renew(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIREDELAY, SPCOST)
        {
            //soundEffect = game.Content.Load<SoundEffect>(@"Sounds/assaultrifle");
        }

        public override void Draw(Camera camera)
        {

        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            interval += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            healInterval += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (currentHealInterval > 0 && healInterval > HEALINGRATE)
            {
                foreach (GameComponent gc in game.Components)
                {
                    if (gc is Player)
                    {
                        Player p = (Player)gc;
                        p.hp += HEALING;
                        if (p.hp > p.topHP) p.hp = p.topHP;
                    }
                }
                currentHealInterval -= HEALINGRATE;
                healInterval = 0;
                //soundEffect.Play();
            }
        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, GameTime gameTime, Vector3 cameraTarget)
        {
            if (interval > fireDelay)
            {
                currentHealInterval = HEALINGTIME;
                interval = 0;
                //soundEffect.Play();
            }
        }
    }
}
