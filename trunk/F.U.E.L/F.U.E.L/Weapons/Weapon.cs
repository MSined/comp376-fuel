using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    abstract class Weapon : Object
    {
        public float range { get; protected set; }
        public int damage { get; protected set; }
        public int fireDelay { get; protected set; }
        public float interval { get; protected set; }

        protected SoundEffect soundEffect;

        public Model[] bulletModelComponents;

        public Weapon(Game game, Model[] modelComponents, Vector3 position,
                      float range, int damage, int fireDelay)
               : base(game, modelComponents, position, new FloatRectangle(position.X, position.Z, 0,0), true)
        {
            this.range = range;
            this.damage = damage;
            this.fireDelay = fireDelay;
            this.interval = fireDelay;

            this.bulletModelComponents = new Model[1];
            this.bulletModelComponents[0] = modelComponents[0];
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            interval += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
        }

        public virtual void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, GameTime gameTime, Vector3 cameraTarget)
        {
            if (interval > fireDelay)
            {
                game.Components.Add(new Bullet(game, this.bulletModelComponents, position, direction, range, damage, shotByEnemy));
                interval = 0;
            }
        }

        protected void playSound(Vector3 position, Vector3 cameraTarget)
        {
            float dist = (cameraTarget - position).LengthSquared();
            float vol = dist / 300;
            float scaledVol = (vol >= 1 ? 0 : (1 - vol));
            soundEffect.Play(scaledVol, 0.0f, 0.0f);
        }
    }
}
