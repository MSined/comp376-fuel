using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Generator : Building
    {
        private int topHP;
        private int hp;
        private bool functional;
        public float repairRange;
        public List<Player> players;
 
        public Generator(Game game, Model[] modelComponents, Vector3 position,
            float angle,
            int topHP, int hp, bool functional, float repairRange, List<Player> players)
            : base(game, modelComponents, position, angle)
        {
            this.topHP = topHP;
            this.hp = hp;
            this.functional = functional;
            this.repairRange = repairRange;
            this.players = players;
        }

        private void repair(List<Player> players)
        {
            foreach (Player p in players) {
                if ((p.position - this.position).Length() <= repairRange && p.repairing) {
                    hp += p.repairSpeed;
                    if (hp >= topHP) {
                        hp = topHP;
                        functional = true;
                    }
                }
            }
        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {
            repair(players);
            if (hp <= 0) {
                hp = 0;
                functional = false;
            }
        }
    }
}
