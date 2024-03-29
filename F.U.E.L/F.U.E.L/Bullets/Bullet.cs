﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Bullet : Object
    {
        public float range { get; protected set; }
        public int damage { get; protected set; }
        public float distanceTraveled { get; protected set; }
        public Vector3 direction { get; protected set; }
        public const float width = 0.2f, height = 0.2f;
        public Boolean shotByEnemy { get; protected set; }

        public float speed = 0.3f;

        public Bullet(Game game, Model[] modelComponents, Vector3 position,
            Vector3 direction, float range, int damage, Boolean shotByEnemy)
            : base(game, modelComponents, position, new FloatRectangle(position.X, position.Z, width, height), true)
        {
            this.range = range;
            this.damage = damage;
            this.direction = direction;
            this.shotByEnemy = shotByEnemy;
            direction.Normalize();
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            position += Vector3.Multiply(direction, speed);
            world = Matrix.CreateTranslation(position);
            distanceTraveled += Vector3.Multiply(direction, speed).Length();
            if (distanceTraveled > range)
            {
                this.isAlive = false;
            }

            CheckCollisions(colliders, cameraTarget);

            this.bounds = new FloatRectangle(this.position.X, this.position.Z, width, height);
        }

        public override void Draw(Camera camera)
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

        public virtual void CheckCollisions(List<Object> colliders, Vector3 cameraTarget)
        {
            foreach (Object o in colliders)
            {
                if (bounds.FloatIntersects(o.bounds) && o.isAlive)
                {
                    if (o is Player && this.shotByEnemy)
                    {
                        this.isAlive = false;
                        Player p = (Player)o;
                        p.hp = p.hp - this.damage;
                        p.checkIfDead(cameraTarget);
                        continue;
                    }
                    if (o is Enemy && !this.shotByEnemy)
                    {
                        this.isAlive = false;
                        Enemy e = (Enemy)o;
                        e.hp = e.hp - this.damage;
                        e.checkIfDead(cameraTarget);
                        continue;
                    }
                    if (o is Tower && this.shotByEnemy)//same as player, but tower
                    {
                        this.isAlive = false;
                        Tower t = (Tower)o;
                        t.hp = t.hp - this.damage;
                        t.checkIfDead(cameraTarget);
                        continue;
                    }
                    if (o is Building)// && bounds.FloatIntersects(o.bounds))
                    {
                        this.isAlive = false;
                    }
                    if (o is Generator && this.shotByEnemy)//same as player, but generator
                    {
                        this.isAlive = false;
                        Generator g = (Generator)o;
                        g.hp = g.hp - this.damage;
                        continue;
                    }
                    
                }
            }
        }
    }
}
