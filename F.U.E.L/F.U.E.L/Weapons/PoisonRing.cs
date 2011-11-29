using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class PoisonRing : Weapon
    {
        private const float RANGE = 3;
        private const int DAMAGE = 5;
        private const int FIREDELAY = (int)(10 * 1000);
        private const int SPCOST = 10;

        //private SoundEffect soundEffect;

        public PoisonRing(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIREDELAY, SPCOST)
        {

        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, GameTime gameTime, Vector3 cameraTarget)
        {
            if (interval > fireDelay)
            {
                for (float i = 0; i < 2*Math.PI; i += 0.2f)
                {
                    Matrix m = Matrix.CreateRotationY(i);
                    game.Components.Add(new PoisonBullet(game, this.bulletModelComponents, position, Vector3.Transform(direction, m), range, damage, shotByEnemy));
                }
                interval = 0;
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
