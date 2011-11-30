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
        SuperModel model;
        Matrix world = Matrix.Identity;
        float angle = 0;
        public float spawnTimer = 0, spawnTimeDelay = 1000;
        bool playerSpawnPoint = false;
        public int spawnLimit = 2;
        public int spawnCounter = 0;

        public SpawnPoint(SuperModel model, Vector3 position, bool isPlayersSpawn)
            : base(position)
        {
            this.model = model;
            playerSpawnPoint = isPlayersSpawn;
            world = Matrix.CreateRotationY(MathHelper.ToRadians(angle)) * Matrix.CreateTranslation(position);
        }

        public void Update(GameTime gameTime)
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

        public void Draw(Camera camera)
        {
            model.Draw(camera, world);
        }
    }
}
