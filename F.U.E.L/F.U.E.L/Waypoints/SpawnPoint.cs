using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class SpawnPoint : Waypoint
    {
        Model model;
        Matrix world = Matrix.Identity;
        float angle = 0;
        public float spawnTimer = 999, spawnTimeDelay = 1000;
        bool playerSpawnPoint = false;
        public int spawnLimit = 3;
        public int spawnCounter = 0;

        public SpawnPoint(Game game, Model model, Vector3 position, bool isPlayersSpawn)
            : base(game, position)
        {
            this.model = model;
            playerSpawnPoint = isPlayersSpawn;
            world = Matrix.CreateRotationY(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(position);
        }

        public override void Update(GameTime gameTime)
        {
            spawnTimer += gameTime.ElapsedGameTime.Milliseconds;
        }

        public bool readyToSpawn()
        {
            if (!playerSpawnPoint)
            {
                if (spawnTimer >= spawnTimeDelay)
                {
                    spawnTimer = 0;
                    return true;
                }
                return false;
            }
            return false;
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
    }
}
