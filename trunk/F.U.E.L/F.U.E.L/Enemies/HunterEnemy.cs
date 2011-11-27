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

        // Removed position from this constructor as it will be taken from the spawnpoint when the enemy passes it to
        // the character constructor
        public HunterEnemy(Game game, Model[] modelComponents,
            SpawnPoint spawnPoint, Weapon[] weapons
            )
            : base(game, modelComponents, spawnPoint, weapons)
        {

        }

        protected override void chooseTarget(List<Building> buildings, List<Player> players, List<Tower> towers) 
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
