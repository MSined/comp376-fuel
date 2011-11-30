using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class AOEBullet : Bullet
    {
        private Vector3 explosionDirection = new Vector3(1, 0, 0);
        private int explosionRange = 2;

        public AOEBullet(Game game, Model[] modelComponents, Vector3 position,
            Vector3 direction, float range, int damage, Boolean shotByEnemy)
            : base(game, modelComponents, position, direction, range, damage, shotByEnemy)
        {
            
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
                            continue;
                        }
                        if (o is Enemy && !this.shotByEnemy)
                        {
                            this.isAlive = false;
                            explode();
                            continue;
                        }
                        if (o is Tower && this.shotByEnemy)//same as player, but tower
                        {
                            this.isAlive = false;
                            explode();
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
    }
}
