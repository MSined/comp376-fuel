using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Punch : Weapon
    {
        private const float RANGE = 1f;
        private const int DAMAGE = 25;
        private const int FIREDELAY = (int)(1 / 2.0 * 1000);

        public Punch(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIREDELAY)
        {
            
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
