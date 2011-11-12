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
            
            //addTrees(game, m3, buildings);
            //addBuildings(game, m3, buildings);
            leftXPos = leftX;
            bottomYPos = bottomY;
        }

        private void addBuildings(Game game, Model[] model, List<Building> buildings) {
            int[,] coordinate = { //[36][37]/ tile number
                                  { 0, 1, 2, 3, 4, 5, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0, 1, 2, 3, 5, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 3, 5, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 3, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2, 3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2, 5,17,18,19,20,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2, 5,22,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2,15,22,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2, 3, 5, 6,15,22,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 3,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 3, 6,15,16,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2, 3, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2, 4, 5, 6, 7, 8, 9,10,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 2,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0, 1, 2, 4, 5, 6,12,14,15,16,18,31,34,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0, 6,12,14,16,18,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0, 6,12,14,16,21,22,23,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0,12,14,15,16,20,21,23,26,27,28,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0, 6,14,15,20,21,22,23,26,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0, 6,12,17,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,-1}};
            for (int i = 0; i < 36; i++) {
                for (int j = 0; coordinate[i, j] != -1; j++)
                {
                    this.buildings.Add(new Building(game, model, new Vector3( i*2, 0, 2 * (coordinate[i, j])), 0f));
                }
            }
        }

        private void addTrees(Game game, Model[] model, List<Building> buildings)
        {
            int[,] coordinate = { //[28][27]
                                  {10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,-1},
                                  { 7, 8, 9,10,24,25,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 7, 8, 9,10,12,13,14,20,22,25,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 8, 9,10,12,14,15,17,18,19,20,25,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {10,12,13,14,15,18,20,21,23,25,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 9,10,13,14,18,19,21,23,25,34,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {10,13,14,19,20,21,23,25,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {10,13,14,15,16,17,20,21,23,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 7, 8,13,16,17,18,21,23,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 8,13,14,16,19,27,28,29,30,31,33,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 7, 8,14,15,16,21,24,25,33,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 7, 8,15,20,21,23,24,25,28,29,33,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 8, 9,11,12,23,24,25,28,29,33,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 8, 9,12,13,27,28,29,30,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {11,12,13,14,25,28,29,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 9,10,11,14,24,25,26,27,28,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {10,25,27,30,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 8, 9,10,11,23,25,26,27,29,30,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {11,23,25,26,29,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 7,11,23,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 7, 9,10,11,14,15,18,19,25,26,27,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  { 7, 9,10,11,12,13,14,17,18,19,20,25,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {12,13,16,17,18,19,20,25,26,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {13,15,16,17,20,29,31,32,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {15,16,17,18,25,26,28,29,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {15,16,17,18,19,25,28,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {15,18,19,22,23,26,28,29,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},
                                  {22,23,28,29,31,32,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}};
            for (int i = 0; i < 28; i++)
            {
                for (int j = 0; coordinate[i, j] != -1; j++)
                {
                    this.buildings.Add(new Building(game, model, new Vector3(i * 2, 0, 2 * (coordinate[i, j])), 0f));
                }
            }
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
