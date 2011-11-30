using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class Tower : Character
    {
        protected Object target = null;
        const float height = .5f;
        const float width = .5f;
        const float depth = .5f;
        // Used for offsetting the initial position of the tower so that we can rarely (never?)
        // Have two towers that overlap exactly, causing the game to act unexpectedly
        static Random rand = new Random();

        protected SoundEffect soundEffect;

        public int attackerNum = 0;
        public static int numTowers = 0, towerCost = 100;

        public Tower(Game game, SuperModel[] modelComponents,
            int topHP, int topSP, Vector3 position, SpawnPoint anySpawnPoint, Weapon[] weapons)
            : base(game, modelComponents, new Vector3(position.X + (float)(rand.NextDouble() / 6),
                                                      position.Y,
                                                      position.Z + (float)(rand.NextDouble() / 6)),
                   topHP, topSP, 0.1f, anySpawnPoint, weapons, new FloatRectangle(position.X, position.Z, width, depth), true)
        {
            towerCost = 100 + numTowers * 100;
            ++numTowers;

            soundEffect = game.Content.Load<SoundEffect>(@"Sounds/buildingcollapse");
        }

        private void chooseTarget(List<Enemy> enemies)
        {
            float distance = float.PositiveInfinity;
            foreach (Enemy e in enemies)
            {
                if ((e.position - this.position).Length() < distance)
                {
                    distance = (e.position - this.position).Length();
                    target = e;
                }
            }
            if (distance > weapons[selectedWeapon].range)//out of weapon range
            {
                target = null;
            }
        }

        public void playSound(Vector3 position, Vector3 cameraTarget)
        {
            float dist = (cameraTarget - position).LengthSquared();
            float vol = dist / 300;
            float scaledVol = (vol >= 1 ? 0 : (1 - vol));
            soundEffect.Play(scaledVol, 0.0f, 0.0f);
        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            if (target == null || !target.isAlive)//choose target if it doesn't have one or the last one is dead
            {
                List<Enemy> enemies = new List<Enemy>();
                foreach (GameComponent gc in game.Components)
                {
                    if (gc is Enemy)
                    {
                        enemies.Add((Enemy)gc);
                    }
                }
                chooseTarget(enemies);
            }
            else
            {
                float targetDist = (target.position - this.position).Length();
                if (targetDist < weapons[selectedWeapon].range)
                {
                    lookDirection = target.position - this.position;
                    weapons[selectedWeapon].shoot(this.position, lookDirection, false, gameTime, cameraTarget);
                }
                else //target went away, choose another target
                {
                    target = null;
                }
            }
            weapons[selectedWeapon].Update(gameTime, colliders, cameraTarget);
            base.Update(gameTime, colliders, cameraTarget);
        }
    }
}
