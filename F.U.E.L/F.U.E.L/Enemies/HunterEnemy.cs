using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class HunterEnemy : Enemy
    {
        private const float SPEED = 0.05f;
        private const int TOPHP = 50;
        private const int TOPSP = 50;
       
        public HunterEnemy(Game game, Model[] modelComponents, Vector3 position,
            SpawnPoint spawnPoint, Weapon[] weapons
            )
            : base(game, modelComponents, position, spawnPoint, weapons, TOPHP, TOPSP, SPEED)
        {

        }

        protected override void chooseTarget(List<Building> buildings, List<Player> players) 
        {
            float distance = float.PositiveInfinity;
            foreach (Player p in players)
            {
                if ((p.position - this.position).Length() < distance)
                {
                    distance = (p.position - this.position).Length();
                    target = p;
                }
            }
        }
    }
}
