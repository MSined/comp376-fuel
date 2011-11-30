using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class SuperModel
    {
        Model model;
        Texture texture;
        Effect effect;

        public SuperModel(ref Model model, Effect effect)
        {
            this.model = model;
            this.effect = effect;

            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);
            
            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (Effect e in mesh.Effects)
                {
                    if (e is BasicEffect)
                    {
                        BasicEffect be = (BasicEffect)e;
                        texture = be.Texture;
                    }
                    
                }
            }

            foreach (ModelMesh mesh in model.Meshes)
            {
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                }
            }
        }

        public void Draw(Camera camera, Matrix world)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            /* Code needed for specularity, but is not used for now
            float angle = (float)Math.Asin(lookDirection.X) + MathHelper.ToRadians(180);
            if (lookDirection.Z > 0)
            {
                angle = MathHelper.ToRadians(180) - angle;
            }
            Vector3 viewVector = Vector3.Transform(camera.cameraDirection, Matrix.CreateRotationY(MathHelper.ToRadians(angle)));
            effect.Parameters["ViewVector"].SetValue(viewVector);
            */

            foreach (ModelMesh mesh in model.Meshes)
            {
                ModelEffectCollection oldEffects = mesh.Effects;
                foreach (ModelMeshPart part in mesh.MeshParts)
                {
                    part.Effect = effect;
                    Matrix worldInverseTransposeMatrix = Matrix.Transpose(Matrix.Invert(world * mesh.ParentBone.Transform));
                    effect.Parameters["ModelTexture"].SetValue(texture);
                    effect.Parameters["WorldInverseTranspose"].SetValue(worldInverseTransposeMatrix);
                    effect.Parameters["World"].SetValue(world * mesh.ParentBone.Transform);
                    effect.Parameters["View"].SetValue(camera.view);
                    effect.Parameters["Projection"].SetValue(camera.projection);
                }
                mesh.Draw();
                mesh.Effects.Union(oldEffects);
            }
        }
    }
}
