using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Character : Object
    {
        protected int topHP;
        protected int hp { get; private set; }
        protected int topSP;
        protected int sp { get; private set; }

        protected SpawnPoint spawnPoint { get; private set; }
        protected Weapon[] weapons { get; private set; }
        protected int selectedWeapon { get; private set; }
        protected int[] attributes { get; private set; }

        protected Vector3 lookDirection = new Vector3(0, 0, 0);
        protected Vector3 velocity = new Vector3(0,0,0);
        protected float speed;

        public Character(Game game, Model[] modelComponents, Vector3 position,
            int topHP, int topSP, float speed, SpawnPoint spawnPoint, Weapon[] weapons)
            : base(game, modelComponents, position)
        {
            this.topHP = topHP;
            this.hp = topHP;

            this.topSP = topSP;
            this.sp = topSP;

            this.speed = speed;

            this.spawnPoint = spawnPoint;

        }

        public void Draw(Camera camera)
        {
            Matrix[] transforms = new Matrix[modelComponents[0].Bones.Count];
            modelComponents[0].CopyAbsoluteBoneTransformsTo(transforms);

            foreach (ModelMesh mesh in modelComponents[0].Meshes)
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
    }
}
