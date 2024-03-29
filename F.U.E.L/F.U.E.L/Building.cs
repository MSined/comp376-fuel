﻿using System;
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
        
        public Building(Game game, Model[] modelComponents, Vector3 position,
            float width, float depth, float angle)
            : base(game, modelComponents, position, new FloatRectangle(position.X, position.Z, width, depth), true)
        {
            model = modelComponents[0];
            world = Matrix.CreateRotationY(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(position);
            this.position = position;
        }

        public override void Update(GameTime gameTime, List<Object> colliders) 
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