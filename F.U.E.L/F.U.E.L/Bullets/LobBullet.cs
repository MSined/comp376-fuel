using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class LobBullet : Bullet
    {
        private Vector3 impulse = new Vector3(0, 3, 0);
        private Vector3 gravity = new Vector3(0, -0.2f, 0);

        public LobBullet(Game game, Model[] modelComponents, Vector3 position,
            Vector3 direction, float range, int damage, Boolean shotByEnemy)
            : base(game, modelComponents, position, direction, range, damage, shotByEnemy)
        {
            this.direction.Normalize();
            this.direction += impulse;
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            position += Vector3.Multiply(direction, speed);
            direction += gravity;
            world = Matrix.CreateTranslation(position);
            distanceTraveled += Vector3.Multiply(direction, speed).Length();
            if (distanceTraveled > range)
            {
                this.isAlive = false;
            }

            CheckCollisions(colliders);
            this.bounds = new FloatRectangle(this.position.X, this.position.Z, width, height);
        }

    }
}
