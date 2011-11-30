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

        public Vector3 cameraTarget;

        //Vector3 onScreenAdjust = new Vector3(0, 0, -7f);
        public float top=-11;
        public float bottom=8;
        public float left=-17;
        public float right=17;

        // Current scroll wheel value. It stores the cumulative scroll value since start of game
        // Also used to verify against new scroll values to determine if zoom in or out
        float scrollWheelValue = 0;

        private Game game;
        //private Player player = null;

        private List<Player> players = new List<Player>();

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
            if (players.Count == 0) 
            {
                foreach (GameComponent p in game.Components)
                {
                    if (p is Player)
                    {
                        players.Add((Player)p);
                    }
                }
            }
            int i = 0;//to count alive players 
            cameraTarget = Vector3.Zero;
            foreach (Player p in players) 
            {
                if (p.isAlive) 
                {
                    ++i;
                    cameraTarget += p.position;
                }
            }
            if (i != 0)
            {
                cameraTarget/=i;
            }
            else 
            {
                cameraTarget = new Vector3(-29, 0, 25);
            }
            cameraPosition = cameraTarget + (Vector3.UnitZ * 2.2f) + cameraDistFromPlayer;


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
            if (o.position.Z > cameraTarget.Z + top &&
                o.position.Z < cameraTarget.Z + bottom &&
                o.position.X > cameraTarget.X + left &&
                o.position.X < cameraTarget.X + right) { return true; }

            else { return false; }
        }
    }
}
