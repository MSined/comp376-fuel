using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class AOEBullet : Bullet
    {
        private Vector3 explosionDirection = new Vector3(1, 0, 0);
        private int explosionRange = 2;

        protected SoundEffect soundEffect;

        public AOEBullet(Game game, Model[] modelComponents, Vector3 position,
            Vector3 direction, float range, int damage, Boolean shotByEnemy)
            : base(game, modelComponents, position, direction, range, damage, shotByEnemy)
        {
            soundEffect = game.Content.Load<SoundEffect>(@"Sounds/mines");
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            //position += Vector3.Multiply(direction, speed);
            world = Matrix.CreateTranslation(position);
            distanceTraveled += Vector3.Multiply(direction, speed).Length();
            if (distanceTraveled > range)
            {
                this.isAlive = false;
            }

            CheckCollisions(colliders, cameraTarget);

            this.bounds = new FloatRectangle(this.position.X, this.position.Z, width, height);
        }

        public override void CheckCollisions(List<Object> colliders, Vector3 cameraTarget)
        {
            foreach (Object o in colliders)
            {
                    if (bounds.FloatIntersects(o.bounds) && o.isAlive)
                    {
                        if (o is Player && this.shotByEnemy)
                        {
                            this.isAlive = false;
                            explode();
                            playSound(position, cameraTarget);
                            continue;
                        }
                        if (o is Enemy && !this.shotByEnemy)
                        {
                            this.isAlive = false;
                            explode();
                            playSound(position, cameraTarget);
                            continue;
                        }
                        if (o is Tower && this.shotByEnemy)//same as player, but tower
                        {
                            this.isAlive = false;
                            explode();
                            playSound(position, cameraTarget);
                            continue;
                        }
                        if (o is Building)// && bounds.FloatIntersects(o.bounds))
                        {
                            this.isAlive = false;
                        }
                        if (o is Generator && this.shotByEnemy)//same as player, but generator
                        {
                            this.isAlive = false;
                            explode();
                            playSound(position, cameraTarget);
                            continue;
                        }

                    }
            }
        }

        private void explode()
        {
            for (float i = 0; i < 2 * Math.PI; i += 0.2f)
            {
                Matrix m = Matrix.CreateRotationY(i);
                game.Components.Add(new Bullet(game, modelComponents, new Vector3(position.X, 0, position.Z), Vector3.Transform(explosionDirection, m), explosionRange, damage, shotByEnemy));
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
