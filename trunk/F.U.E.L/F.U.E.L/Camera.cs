using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace F.U.E.L
{
    class Camera : Microsoft.Xna.Framework.GameComponent
    {
        // View and projection matrices for camera
        public Matrix view { get; protected set; }
        public Matrix projection { get; protected set; }
        // Camera attributes for constructor
        public Vector3 cameraPosition { get; protected set; }
        public Vector3 cameraDistFromPlayer;
        Vector3 cameraDirection;
        Vector3 cameraUp;

        //Vector3 onScreenAdjust = new Vector3(0, 0, -7f);
        public float top=-12;
        public float bottom=7;
        public float left=-15;
        public float right=15;

        // Current scroll wheel value. It stores the cumulative scroll value since start of game
        // Also used to verify against new scroll values to determine if zoom in or out
        float scrollWheelValue = 0;

        private Game game;
        private Player player = null;

        public Camera(Game game, Vector3 pos /* in respect to player*/, Vector3 target, Vector3 up)
            : base(game)
        {
            this.game = game;
            // Set values and create required matrices
            cameraPosition = pos;
            cameraDistFromPlayer = pos;
            cameraDirection = target - pos;
            cameraDirection.Normalize();
            cameraUp = up;
            CreateLookAt();
            projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4, 
                                                             (float)Game.Window.ClientBounds.Width / 
                                                             (float)Game.Window.ClientBounds.Height, 
                                                             1, 1000);
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (player == null)
            {
                foreach (GameComponent p in game.Components)
                {
                    if (p is Player)
                    {
                        player = (Player)p;
                    }

                }
            }
            cameraPosition = player.position + cameraDistFromPlayer;
            
            // Check for scroll wheel zooming
            // Camera moves along its direction matrix (where it is looking)
            if (Mouse.GetState().ScrollWheelValue < scrollWheelValue)
            {
                cameraDistFromPlayer -= cameraDirection * 1f;
                scrollWheelValue = Mouse.GetState().ScrollWheelValue;
            }
            else if (Mouse.GetState().ScrollWheelValue > scrollWheelValue)
            {
                cameraDistFromPlayer += cameraDirection * 1f;
                scrollWheelValue = Mouse.GetState().ScrollWheelValue;
            }

            // Recreate the lookat matrix to update camera
            CreateLookAt();

            base.Update(gameTime);
        }

        private void CreateLookAt()
        {
            view = Matrix.CreateLookAt(cameraPosition, cameraPosition + cameraDirection, cameraUp);
        }

        public bool onScreen(Object o) 
        {
            //if (Math.Abs((o.position - (player.position+onScreenAdjust)).Length()) < 17) { return true; }
            if (o.position.Z > player.position.Z + top &&
                o.position.Z < player.position.Z + bottom &&
                o.position.X > player.position.X + left &&
                o.position.X < player.position.X + right) { return true; }
            
            else { return false; }
            //return true;
        }
    }
}
