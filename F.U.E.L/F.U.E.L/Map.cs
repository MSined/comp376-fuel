using System;
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
        public float leftXPos { get; protected set; }
        public float bottomYPos { get; protected set; }
        
        public Map(Game game, Model[] modelComponents, float leftX, float bottomY)
            : base(game)
        {
            // Set model attribute
            model = modelComponents[0];

            // Each object takes an array, need to create a new array
            Model[] m1 = new Model[1];
            // Load object into array
            m1[0] = modelComponents[1];

            // Each object takes an array, need to create a new array
            Model[] m2 = new Model[1];
            // Load object into array
            m2[0] = modelComponents[2];

            // Each object takes an array, need to create a new array
            Model[] m3 = new Model[1];
            // Load object into array
            m3[0] = modelComponents[3];

            // When adding buildings, specify the width and height of the bounding box (last two constructor parameters
            // If you want to refer to the model size to determine the bounding box:
            // BB Width = Model Width, BB Height = Model Depth
            // Because remember the BB is on the XZ-plane
            buildings = new List<Building>();
            this.buildings.Add(new Factory(game, m1, new Vector3(2, 0, -4), 0f));
            this.buildings.Add(new Generator(game, m2, new Vector3(6, 0, -4), 0f));
            // Top row of buildings
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, 10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-8, 0, 10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-6, 0, 10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-4, 0, 10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-2, 0, 10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(0, 0, 10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(2, 0, 10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(4, 0, 10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(6, 0, 10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(8, 0, 10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, 10), 0f));
            // Bottom row of buildings
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-8, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-6, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-4, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-2, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(0, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(2, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(4, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(6, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(8, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, -10), 0f));
            // Left Column of buildings
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, -8), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, -6), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, -4), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, -2), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, 0), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, 2), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, 4), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, 6), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, 8), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-10, 0, 10), 0f));
            // Right Column of buildings
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, -10), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, -8), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, -6), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, -4), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, -2), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, 0), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, 2), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, 4), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, 6), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, 8), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(10, 0, 10), 0f));
            // Lower left hiding spot
            this.buildings.Add(new Building(game, m3, new Vector3(-8, 0, 4), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-6, 0, 4), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-4, 0, 4), 0f));
            this.buildings.Add(new Building(game, m3, new Vector3(-4, 0, 6), 0f));

            leftXPos = leftX;
            bottomYPos = bottomY;
        }

        public void Update(GameTime gameTime, List<Object> colliders)
        {
            
        }

        public void Draw(Camera camera)
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

            foreach (Building b in buildings)
            {
                b.Draw(camera);
            }
        }
    }
}
