using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class PowerFist : Weapon
    {
        private const float RANGE = 0.5f;
        private const float DAMAGE = 15;
        private const int FIRERATE = 10000000;

        public PowerFist(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {
            
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
