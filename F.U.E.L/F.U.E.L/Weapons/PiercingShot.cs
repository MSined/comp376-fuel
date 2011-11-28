using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class PiercingShot : Weapon
    {
        private const float RANGE = 15;
        private const int DAMAGE = 20;
        private const int FIRERATE = (int)(8f * 10000000);

        private SoundEffect soundEffect;

        public PiercingShot(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {
            soundEffect = game.Content.Load<SoundEffect>(@"Sounds/pistol");

        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, Vector3 cameraTarget)
        {
            long nowTick = DateTime.Now.Ticks;

            if (lastShot + fireRate < nowTick)
            {
                game.Components.Add(new PiercingBullet(game, this.bulletModelComponents, position, direction, range, damage, shotByEnemy));
                lastShot = nowTick;
                soundEffect.Play();
            }

        }

        public override void Draw(Camera camera)
        {

        }
        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {

        }
    }
}
