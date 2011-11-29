using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace F.U.E.L
{
    class Mines : Weapon
    {
        private const float RANGE = 10;
        private const int DAMAGE = 20;
        private const int FIREDELAY = 5 * 1000;
        private const int SPCOST = 10;

        public Mines(Game game, Model[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIREDELAY, SPCOST)
        {
            
        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, GameTime gameTime, Vector3 cameraTarget)
        {
            if (interval > fireDelay)
            {
                game.Components.Add(new AOEBullet(game, this.bulletModelComponents, position, new Vector3(0,0,0), range, damage, shotByEnemy));
                interval = 0;
            }
        }

        public override void Draw(Camera camera)
        {

        }
    }
}
