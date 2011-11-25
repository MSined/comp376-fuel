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
    class PoisonRing : Weapon
    {
        private const float RANGE = 3;
        private const int DAMAGE = 5;
        private const int FIRERATE = (int)(10 * 10000000);

        //private SoundEffect soundEffect;

        public PoisonRing(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {

        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastShot + fireRate < nowTick)
            {
                for (float i = 0; i < 2*Math.PI; i += 0.2f)
                {
                    Matrix m = Matrix.CreateRotationY(i);
                    game.Components.Add(new PoisonBullet(game, this.bulletModelComponents, position, Vector3.Transform(direction, m), range, damage, shotByEnemy));
                }
                lastShot = nowTick;
            }
        }

        public override void Draw(Camera camera)
        {

        }
        public override void Update(GameTime gameTime, List<Object> colliders)
        {

        }
    }
}
