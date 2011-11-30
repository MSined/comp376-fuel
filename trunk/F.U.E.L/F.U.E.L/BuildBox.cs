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
        Player boundPlayer;

        public BuildBox(Game game, Model[] modelComponents, Vector3 position, FloatRectangle bounds, Player boundPlayer)
            : base(game, modelComponents, position, bounds, false)
        {
            this.boundPlayer = boundPlayer;
        }

        public override void Update(GameTime gameTime)
        {
            // Component-wise sum of two vectors to create the new position
            position = boundPlayer.position + boundPlayer.lookDirection;

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
