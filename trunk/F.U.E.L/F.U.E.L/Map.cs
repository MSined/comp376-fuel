﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Map : GameComponent
    {
        public Model model { get; protected set; }
        protected Matrix world = Matrix.Identity;
        public List<Building> buildings { get; protected set; }
        
        public Map(Game game, Model[] modelComponents)
            : base(game)
        {
            model = modelComponents[0];
            Model[] m1 = new Model[1];
            m1[0] = modelComponents[1];

            Model[] m2 = new Model[1];
            m2[0] = modelComponents[2];

            buildings = new List<Building>();
            this.buildings.Add(new Building(game, m1, new Vector3(0, 0, 0), 0f));
            this.buildings.Add(new Generator(game, m2, new Vector3(1, 0, 0), 0f));
        }

        public void Update() { }

        public void Draw(Camera camera)
        {
            foreach (Building b in buildings)
            {
                b.Draw(camera);
            }

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
    }
}