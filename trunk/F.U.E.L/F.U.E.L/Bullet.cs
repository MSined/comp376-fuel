using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Bullet : Object
    {
        float range, damage;
        float distanceTraveled;
        Vector3 direction;
        static float width = 0.5f, height = 0.5f;

        private const float speed = 0.1f;

        public Bullet(Game game, Model[] modelComponents, Vector3 position,
            Vector3 direction, float range, float damage)
            : base(game, modelComponents, position, new FloatRectangle(position.X, position.Y, width, height), true)
        {
            this.range = range;
            this.damage = damage;
            this.direction = direction;
            direction.Normalize();
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            position += Vector3.Multiply(direction, speed);
            world = Matrix.CreateTranslation(position);
            distanceTraveled += Vector3.Multiply(direction, speed).Length();
            if (distanceTraveled > range)
            {
                this.isAlive = false;
            }
            this.bounds = new FloatRectangle(this.position.X, this.position.Z, width, height);
        }

        public override void Draw(Camera camera)
        {
            Matrix[] transforms = new Matrix[modelComponents[0].Bones.Count];
            modelComponents[0].CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in modelComponents[0].Meshes)
            {
                foreach (BasicEffect be in mesh.Effects)
                {
                    be.EnableDefaultLighting();
                    be.Projection = camera.projection;
                    be.View = camera.view;
                    be.World = world * mesh.ParentBone.Transform;
                }

                mesh.Draw();
            }
        }
    }
}
