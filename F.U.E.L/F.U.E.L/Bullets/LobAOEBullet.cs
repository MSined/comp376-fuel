using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class LobAOEBullet : Bullet
    {
        private Vector3 impulse = new Vector3(0, 2, 0);
        private Vector3 gravity = new Vector3(0, -0.2f, 0);
        private float hitHeight = 0.5f;
        private int explosionRange = 2;

        public LobAOEBullet(Game game, Model[] modelComponents, Vector3 position,
            Vector3 direction, float range, int damage, Boolean shotByEnemy)
            : base(game, modelComponents, position, direction, range, damage, shotByEnemy)
        {
            this.direction.Normalize();
            this.direction += impulse;
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            position += Vector3.Multiply(direction, speed);
            direction += gravity;
            world = Matrix.CreateTranslation(position);
            distanceTraveled += Vector3.Multiply(direction, speed).Length();
            if (distanceTraveled > range)
            {
                this.isAlive = false;
            }

            if (position.Y < hitHeight)
                CheckCollisions(colliders, cameraTarget);

            if (isAlive && position.Y <= 0)
            {
                this.isAlive = false;
                explode();
            }

            this.bounds = new FloatRectangle(this.position.X, this.position.Z, width, height);
        }

        public override void CheckCollisions(List<Object> colliders, Vector3 cameraTarget)
        {
            foreach (Object o in colliders)
            {
                if (bounds.FloatIntersects(o.bounds))
                {
                    this.isAlive = false;
                    explode();
                }
            }
        }

        private void explode()
        {
            for (float i = 0; i < 2 * Math.PI; i += 0.2f)
            {
                Matrix m = Matrix.CreateRotationY(i);
                game.Components.Add(new Bullet(game, modelComponents, new Vector3(position.X, hitHeight, position.Z), Vector3.Transform(direction, m), explosionRange, damage, shotByEnemy));
            }
        }
    }
}
