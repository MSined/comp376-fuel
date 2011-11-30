using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Map : Microsoft.Xna.Framework.GameComponent
    {
        public Model model { get; protected set; }
        protected Matrix world = Matrix.Identity;
        public List<Building> buildings { get; protected set; }
        public float leftXPos { get; protected set; }
        public float bottomYPos { get; protected set; }
        // Map has list of spawnpoints first in the list is the player spawnpoint
        public List<SpawnPoint> spawnPoints { get; protected set; }
        public List<Building> usableBuildings { get; protected set; }

        public Map(Game game, Model[] modelComponents, float leftX, float bottomY)
            : base(game)
        {
            // Set model attribute
            model = modelComponents[0];

            // Each object takes an array, need to create a new array
            Model[] tower = new Model[1];
            // Load object into array
            tower[0] = modelComponents[1];

            Model[] generator = new Model[1];
            generator[0] = modelComponents[2];

            Model[] building = new Model[1];
            building[0] = modelComponents[3];

            Model[] trees = new Model[1];
            trees[0] = modelComponents[4];

            Model[] telePad = new Model[1];
            telePad[0] = modelComponents[5];

            // When adding buildings, specify the width and height of the bounding box (last two constructor parameters
            // If you want to refer to the model size to determine the bounding box:
            // BB Width = Model Width, BB Height = Model Depth
            // Because remember the BB is on the XZ-plane
            buildings = new List<Building>();
            spawnPoints = new List<SpawnPoint>();
            usableBuildings = new List<Building>();

            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(-30, 0, 24), true));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(-28, 0, 24), true));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(-30, 0, 26), true));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(-28, 0, 26), true));

            //spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(24, 0, -26), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(24, 0, -34), false));
            //spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(32, 0, -26), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(32, 0, -34), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(-26, 0, -18), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(-16, 0, -22), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(-8, 0, -14), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(-2, 0, -18), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(4, 0, -16), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(12, 0, -12), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(-22, 0, -6), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(-12, 0, 2), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(12, 0, 2), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(-6, 0, 18), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(8, 0, 16), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(16, 0, 16), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(16, 0, 22), false));
            spawnPoints.Add(new SpawnPoint(modelComponents[5], new Vector3(18, 0, 28), false));

            Model[] m = new Model[1];
            m[0] = modelComponents[2];

            usableBuildings.Add(new Generator(game, m, new Vector3(-26, 0, 24), 0f));
            usableBuildings.Add(new Generator(game, m, new Vector3(28, 0, 28), 0f));
            usableBuildings.Add(new Generator(game, m, new Vector3(-2, 0, 2), 0f));
            usableBuildings.Add(new Generator(game, m, new Vector3(-30, 0, -30), 0f));
            usableBuildings.Add(new Generator(game, m, new Vector3(28, 0, -30), 0f));

            addTrees(game, trees, buildings);
            addBuildings(game, building, buildings);
            leftXPos = leftX;
            bottomYPos = bottomY;
        }

        private void addBuildings(Game game, Model[] model, List<Building> buildings) {
            int[,] coordinate = { //[36][37]/ tile number
                                  { 0, 1, 2, 3, 4, 5, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//0
                                  { 0, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//1
                                  { 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//2
                                  { 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//3
                                  { 0,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//4
                                  { 0, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//5
                                  { 0, 1, 2, 3, 5, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//6
                                  { 3, 5, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//7
                                  { 3, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//8
                                  { 2, 3,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//9
                                  { 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//10
                                  { 2, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//11
                                  { 2, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//12
                                  { 2, 5,17,/*18,*/19,20,21,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//13
                                  { 2, 5,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//14
                                  { 2, 5,22,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//15
                                  { 2,15,22,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//16
                                  { 2,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//17
                                  { 2, 3, 5, 6,15,22,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//18
                                  { 3,15,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//19
                                  { 3, 6,15,16,19,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//20
                                  { 2, 3, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//21
                                  { 2, 6,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//22
                                  { 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//23
                                  { 2, 4, 5, 6, 7, 8, 9,10,11,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//24
                                  { 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//25
                                  { 2,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//26
                                  { 2,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//27
                                  { 2,12,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//28
                                  { 0, 1, 2, 4, 5, 6,12,14,15,16,/*18,*/31,34,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//29
                                  { 0, 6,12,14,16,/*18,*/35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//30
                                  { 0, 6,12,14,16,21,22,23,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//31
                                  { 0,12,14,15,16,20,21,23,26,27,28,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//32
                                  { 0, 6,14,15,20,21,22,23,26,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//33
                                  { 0, 6,12,17,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//34
                                  { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,-1}};//35
            for (int i = 0; i < 36 /* coordinate.Length*/; i++)
            {
                for (int j = 0; coordinate[i, j] != -1; j++)
                {
                    this.buildings.Add(new Building(game, model, new Vector3((i * 2) - 36, 0, (2 * (coordinate[i, j])) - 36), 2f, 2f, 0f));
                }
            }
        }

        private void addTrees(Game game, Model[] model, List<Building> buildings)
        {
            int[,] coordinate = { //[29][27]
                                  {10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,25,26,27,28,29,30,31,32,33,34,35,-1},//0
                                  { 7, 8, 9,10,24,25,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//1
                                  {35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//2
                                  { 7, 8, 9,10,12,13,14,20,22,25,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//3
                                  { 8, 9,10,12,14,15,17,/*18,*/19,20,25,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//4
                                  {10,12,13,14,15,/*18,*/20,21,23,25,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//5
                                  { 9,10,13,14,/*18,*/19,21,23,25,34,35,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//6
                                  {10,13,14,19,20,21,23,25,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//7
                                  {10,13,14,15,16,17,20,21,23,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//8
                                  { 7, 8,13,16,17,/*18,*/21,23,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//9
                                  { 8,13,14,16,19,27,28,29,30,31,33,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//10
                                  { 7, 8,14,15,16,21,24,25,33,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//11
                                  { 7, 8,15,20,21,23,24,25,28,29,33,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//12
                                  { 8, 9,11,12,23,24,25,28,29,33,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//13
                                  { 8, 9,12,13,27,28,29,30,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//14
                                  {11,12,13,14,25,28,29,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//15
                                  { 9,10,11,14,24,25,26,27,28,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//16
                                  {10,25,27,30,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//17
                                  { 8, 9,10,11,23,25,26,27,29,30,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//18
                                  {11,23,25,26,29,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//19
                                  { 7,11,23,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//20
                                  { 7, 9,10,11,14,15,/*18,*/19,25,26,27,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//21
                                  { 7, 9,10,11,12,13,14,17,/*18,*/19,20,25,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//22
                                  {12,13,16,17,/*18,*/19,20,25,26,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//23
                                  {13,15,16,17,20,29,31,32,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//24
                                  {15,16,17,/*18,*/25,26,28,29,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//25
                                  {15,16,17,/*18,*/19,25,28,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//26
                                  {15,/*18,*/19,22,23,26,28,29,31,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1},//27
                                  {22,23,28,29,31,32,34,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1,-1}};//28
            for (int i = 0; i < 29 /* coordinate.Length*/; i++)
            {
                for (int j = 0; coordinate[i, j] != -1; j++)
                {
                    // Changed hit box area for trees, they were too small for debugging, tree texture was building texture
                    this.buildings.Add(new Building(game, model, new Vector3((i * 2) - 36, 0, (2 * (coordinate[i, j])) - 36), 2f, 2f, 0f));
                }
            }
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
                if (camera.onScreen((Object)b))
                {
                    b.Draw(camera);
                }
            }

            foreach (SpawnPoint s in spawnPoints)
            {
                //if (camera.onScreen((Object)s))
                //{
                    s.Draw(camera);
                //}
            }

            foreach (Building b in usableBuildings)
            {
                if (camera.onScreen((Object)b))
                {
                    b.Draw(camera);
                }
            }
        }
    }
}
