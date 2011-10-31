﻿using System;
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
        Vector3 cameraDirection;
        Vector3 cameraUp;
        // Current scroll wheel value. It stores the cumulative scroll value since start of game
        // Also used to verify against new scroll values to determine if zoom in or out
        float scrollWheelValue = 0;

        public Camera(Game game, Vector3 pos, Vector3 target, Vector3 up)
            : base(game)
        {
            // Set values and create required matrices
            cameraPosition = pos;
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
            // Simple keyboard controls that move along the world axis
            if (Keyboard.GetState().IsKeyDown(Keys.W))
                cameraPosition -= new Vector3(0, 0, 0.1f);
            if (Keyboard.GetState().IsKeyDown(Keys.S))
                cameraPosition += new Vector3(0, 0, 0.1f);
            if (Keyboard.GetState().IsKeyDown(Keys.A))
                cameraPosition -= new Vector3(0.1f, 0, 0);
            if (Keyboard.GetState().IsKeyDown(Keys.D))
                cameraPosition += new Vector3(0.1f, 0, 0);

            // Check for scroll wheel zooming
            // Camera moves along its direction matrix (where it is looking)
            if (Mouse.GetState().ScrollWheelValue < scrollWheelValue)
            {
                cameraPosition -= cameraDirection * 1f;
                scrollWheelValue = Mouse.GetState().ScrollWheelValue;
            }
            else if (Mouse.GetState().ScrollWheelValue > scrollWheelValue)
            {
                cameraPosition += cameraDirection * 1f;
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
    }
}
