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
        //range = 1.75/ enemy size is 1.5
        //range changed to 1/ enemy size is .7
        private const float RANGE = 1f;
        private const int DAMAGE = 25;
        private const int FIRERATE = (int)(1/2.0 * 10000000);

        public Punch(Game game, Model[] modelComponents, Vector3 position/*,
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
