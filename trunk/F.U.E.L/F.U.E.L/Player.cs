using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace F.U.E.L
{
    class Player : Character
    {
        
        //protected float lookAngle;
        
        public Player(Game game, Model[] modelComponents, Vector3 position,
            int topHP, int topSP, float speed, SpawnPoint spawnPoint, Weapon[] weapons
            )
            : base(game, modelComponents, position, topHP, topSP, speed, spawnPoint, weapons)
        {
            
        }

        public void playerUpdate(GamePadThumbSticks thumbSticks) 
        {
            lookDirection = new Vector3(thumbSticks.Left.X, 0, thumbSticks.Left.Y);
            velocity = new Vector3(thumbSticks.Left.X, 0, thumbSticks.Left.Y);
            /*
            if (lookDirection.X > 0)
            {
                lookAngle = (float)Math.Atan(lookDirection.Y / lookDirection.X);
            }
            else 
            {
                lookAngle = (float)Math.Atan(lookDirection.Y / lookDirection.X) + MathHelper.ToRadians(180);
            }
             * */

            position += velocity;
        }
    }
}
