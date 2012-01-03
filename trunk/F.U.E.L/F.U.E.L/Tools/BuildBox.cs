using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class BuildBox : Object
    {
        Character boundCharacter;
        public Texture2D[] textures;
        public int currentTexture = 0;
        bool collided = false;

        public BuildBox(Game game, Model[] modelComponents, Vector3 position, FloatRectangle bounds, Character boundCharacter, Texture2D[] textures)
            : base(game, modelComponents, position, bounds, false)
        {
            this.boundCharacter = boundCharacter;
            this.textures = textures;
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            foreach (Object o in colliders)
            {
                if (!(o is Bullet) && this.bounds.FloatIntersects(o.bounds))
                {
                    collided = true;
                    boundCharacter.checkBoxCollision = true;
                }
            }
            if (collided)
            {
                currentTexture = 1;
                collided = false;
            }
            else
            {
                currentTexture = 0;
                boundCharacter.checkBoxCollision = false;
            }

            // Component-wise sum of two vectors to create the new position
            position = boundCharacter.position + boundCharacter.lookDirection;

            world = Matrix.CreateTranslation(position);

            // Update the bounds of the BuildBox
            bounds = new FloatRectangle(position.X, position.Z, 1, 1);

            base.Update(gameTime);
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
                    be.TextureEnabled = true;
                    be.Texture = textures[currentTexture];
                    be.SpecularPower = 10f;
                    be.Projection = camera.projection;
                    be.View = camera.view;
                    be.World = world * mesh.ParentBone.Transform;
                }
                mesh.Draw();
            }
        }
    }
}
