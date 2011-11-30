using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace F.U.E.L
{
    class SpeedBoost : Weapon
    {
        private const float RANGE = 0;
        private const int DAMAGE = 0;
        private const int FIREDELAY = 10 * 1000;
        private const int SPCOST = 10;

        private const float SPEEDBOOST = 0.04f;
        private const int BOOSTTIME = 3 * 1000;
        private const int CHECKDELAY = (int)(1 / 2.0 * 1000);
        private float currentBoostInterval = 0;
        private float checkInterval = 0;

        Player player;

        public SpeedBoost(Game game, SuperModel[] modelComponents, Vector3 position/*,
            ALREADY SET -> int range, int damage, int fireRate*/, Player p)
            : base(game, modelComponents, position, RANGE, DAMAGE, FIREDELAY, SPCOST)
        {
            soundEffect = game.Content.Load<SoundEffect>(@"Sounds/speedup");
            player = p;
        }

        public override void Draw(Camera camera)
        {

        }

        public override void Update(GameTime gameTime, List<Object> colliders, Vector3 cameraTarget)
        {
            interval += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            checkInterval += (float)gameTime.ElapsedGameTime.TotalMilliseconds;
            if (currentBoostInterval > 0 && checkInterval > CHECKDELAY)
            {
                currentBoostInterval -= CHECKDELAY;
                checkInterval = 0;
                if (currentBoostInterval < 0)
                    player.speed -= SPEEDBOOST;
                
            }
        }

        public override void shoot(Vector3 position, Vector3 direction, Boolean shotByEnemy, GameTime gameTime, Vector3 cameraTarget)
        {
            if (interval > fireDelay)
            {
                player.speed += SPEEDBOOST;
                currentBoostInterval = BOOSTTIME;
                interval = 0;
                playSound(position, cameraTarget);
            }
        }
    }
}
