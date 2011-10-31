﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Map
    {
        public Model model { get; protected set; }
        protected Matrix world = Matrix.Identity;
        private Building b;
        private Building g;

        public Map(Model m, Model bmodel, Model gmodel)
        {
            model = m;
            this.b = new Building(bmodel, 0f, new Vector3(2,0,-4));
            this.g = new Generator(gmodel, 0f, new Vector3(0,0,0));
        }

        public void Update() { }

        public void Draw(Camera camera)
        {
            b.Draw(camera);
            g.Draw(camera);

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
                    be.World = GetWorld() * mesh.ParentBone.Transform;
                }

                mesh.Draw();
            }
        }

        public Matrix GetWorld() { return world; }
    }
}
