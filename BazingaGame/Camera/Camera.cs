using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BazingaGame.Prefabs;
using FarseerPhysics;
using GameInput;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BazingaGame.Display
{
    public class Camera
    {
        private Vector2 _position;
        public Vector2 Position { get { return _position; } }
        public Vector2 Origin { get; set; }
        public float Zoom { get; set; }
        public float Rotation { get; set; }
        public Vector2 ViewportCenter;
        private Viewport _viewport;

        private BazingaPlayer _playerToFollow;

        public float _followAcceleration;

        public Camera(Viewport viewport)
        {
            _position = new Vector2(0, 0);
            ViewportCenter = new Vector2(0, 0);
            _playerToFollow = null;
            _followAcceleration = 1f;
            Origin = new Vector2(viewport.Width / 2.0f, viewport.Height / 2.0f);
            Zoom = 1.0f;
            _viewport = viewport;
        }

        public Matrix GetViewMatrix(Vector2 parallax)
        {
            // To add parallax, simply multiply it by the position
            return Matrix.CreateTranslation(new Vector3(-_position * parallax, 0.0f)) *
                // The next line has a catch. See note below.
                Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
                Matrix.CreateRotationZ(Rotation) *
                Matrix.CreateScale(Zoom, Zoom, 1) *
                Matrix.CreateTranslation(new Vector3(Origin, 0.0f));
        }

        public Matrix GetProjectionMatrix()
        {
            //return Matrix.CreateOrthographicOffCenter(0f, _viewport.Width, _viewport.Height, 0f, 0f, 1f);
            return Matrix.CreateOrthographicOffCenter(
                ConvertUnits.ToSimUnits(_position.X), 
                ConvertUnits.ToSimUnits(_viewport.Width) + ConvertUnits.ToSimUnits(_position.X), 
                ConvertUnits.ToSimUnits(_viewport.Height) + ConvertUnits.ToSimUnits(_position.Y),
                ConvertUnits.ToSimUnits(_position.Y), 
                0f, 
                1f
            );
        }

        public void SetPlayerToFollow(BazingaPlayer playerToFollow)
        {
            _playerToFollow = playerToFollow;
        }

        public Matrix GetTransformMatrix()
        {
            return Matrix.CreateTranslation(-(int)_position.X,
               -(int)_position.Y, 0) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
               Matrix.CreateTranslation(new Vector3(ViewportCenter, 0))
               ;
        }

        public Matrix GetScaledTransformMatrix()
        {
            return Matrix.CreateTranslation(-ConvertUnits.ToSimUnits(_position.X),
               -ConvertUnits.ToSimUnits(_position.Y), 0) *
               Matrix.CreateRotationZ(Rotation) *
               Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
               Matrix.CreateTranslation(new Vector3(ViewportCenter, 0))
               ;
        }

        //public Matrix GetTransformMatrix()
        //{
        //    return Matrix.CreateTranslation(-ConvertUnits.ToSimUnits(_position.X), -ConvertUnits.ToSimUnits(_position.Y), 0);
        //}

		public void Update(GameTime gameTime, InputHelper gameInput)
        {
            if (_playerToFollow == null)
            {
				if (gameInput.KeyboardState.IsKeyDown(Keys.D))
                {
                    _position.X += 30;
                }

				if (gameInput.KeyboardState.IsKeyDown(Keys.A))
                {
                    _position.X -= 30;
                }

				if (gameInput.KeyboardState.IsKeyDown(Keys.W))
                {
                    _position.Y -= 30;
                }

				if (gameInput.KeyboardState.IsKeyDown(Keys.S))
                {
                    _position.Y += 30;
                }
            }
            else
            {
                float border = 4.5f;
                bool moved = false;
                //float delta = 3f + ConvertUnits.ToSimUnits(Math.Abs(
                //    Vector2.Distance(_playerToFollow.Body.Position, ConvertUnits.ToSimUnits(BazingaGame.Camera.Position.X + 1920 / 2, BazingaGame.Camera.Position.X + 1080 / 2))
                //    ))*50;

                if (_playerToFollow.Body.Position.X - ConvertUnits.ToSimUnits(_position.X) >= ConvertUnits.ToSimUnits(1920) - border)
                {
                    _position.X += _followAcceleration;
                    _followAcceleration += 0.6f;
                    moved = true;
                }

                if (_playerToFollow.Body.Position.X - ConvertUnits.ToSimUnits(_position.X) <= border)
                {
                    _position.X -= _followAcceleration;
                    _followAcceleration += 0.6f;
                    moved = true;
                }

                if (_playerToFollow.Body.Position.Y - ConvertUnits.ToSimUnits(_position.Y) >= ConvertUnits.ToSimUnits(1080) - border)
                {
                    _position.Y += _followAcceleration;
                    _followAcceleration += 0.6f;
                    moved = true;
                }

                if (_playerToFollow.Body.Position.Y - ConvertUnits.ToSimUnits(_position.Y) <= border)
                {
                    _position.Y -= _followAcceleration;
                    _followAcceleration += 0.6f;
                    moved = true;
                }

                if(!moved)
                {
                    _followAcceleration = 1f;
                }
            }
        }
    }
}
