using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Building : Object
    {
        public Model model { get; protected set; }
        public bool isTree;
        
        public Building(Game game, Model[] modelComponents, Vector3 position,
            float width, float depth, float angle, bool isTree)
            : base(game, modelComponents, position, new FloatRectangle(position.X, position.Z, width, depth), true)
        {
            model = modelComponents[0];
            world = Matrix.CreateRotationY(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(position);
            this.position = position;
            this.isTree = isTree;
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget) 
        {
		
		}

        public override void Draw(Camera camera)
        {
            Matrix[] transforms = new Matrix[model.Bones.Count];
            model.CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in model.Meshes)
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

        public virtual void use()
        {

        }
    }
}