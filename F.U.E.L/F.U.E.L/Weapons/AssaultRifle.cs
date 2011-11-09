using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class AssaultRifle : Weapon
    {
        private const float RANGE = 5;
        private const float DAMAGE = 5;
        private const int FIRERATE = 1000000;

        public AssaultRifle(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIRERATE)
        {
            
        }

        public override void Draw(Camera camera)
        {

        }

        public override void Update(GameTime gameTime, List<Object> colliders)
        {

        }
    }
}
