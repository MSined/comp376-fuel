using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class AssaultRifle : Weapon
    {
        private const float RANGE = 5;
        private const int DAMAGE = 5;
        private const int FIRERATE = (int)(1 / 10.0 * 10000000);
        

        private SoundEffect soundEffect;

        public AssaultRifle(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {
            soundEffect = game.Content.Load<SoundEffect>(@"Sounds/assaultrifle");
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
                game.Components.Add(new Bullet(game, this.bulletModelComponents, position, direction, range, damage, shotByEnemy));
                lastShot = nowTick;
                soundEffect.Play();
            }
        }
    }
}
