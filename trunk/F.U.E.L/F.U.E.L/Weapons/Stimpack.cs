using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class Stimpack : Weapon
    {
        private const float RANGE = 0;
        private const int DAMAGE = 0;
        private const int FIRERATE = 10 * 10000000;

        private const float SPEEDBOOST = 0.04f;
        private const int BOOSTTIME = 3 * 10000000;
        private const int CHECKRATE = (int)(1/2.0 * 10000000);
        private long currentBoostTime = 0;
        private long lastCheck = 0;

        Player player;
        //private SoundEffect soundEffect;

        public Stimpack(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/, Player p)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {
            //soundEffect = game.Content.Load<SoundEffect>(@"Sounds/assaultrifle");
            player = p;
        }

        public override void Draw(Camera camera)
        {

        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            long nowTick = DateTime.Now.Ticks;

            if (currentBoostTime > 0 && lastCheck + CHECKRATE < nowTick)
            {
                currentBoostTime -= CHECKRATE;
                lastCheck = nowTick;
                if (currentBoostTime < 0)
                    player.speed -= SPEEDBOOST;
                //soundEffect.Play();
            }
        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastShot + fireRate < nowTick)
            {
                player.speed += SPEEDBOOST;
                currentBoostTime = BOOSTTIME;
                lastShot = nowTick;
                //soundEffect.Play();
            }
        }
    }
}
