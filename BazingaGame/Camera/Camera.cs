using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BazingaGame.Prefabs;
using FarseerPhysics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace BazingaGame.GameCamera
{
    public class BazingaCamera
    {
        //public int X { get; set; }
        //public int Y { get; set; }

        public Vector2 Position;
        private static BazingaCamera _camera;
        private float Rotation = 0f;
        private float Zoom = 1f;
        public Vector2 ViewportCenter;

        private Player _playerToFollow;

        public float _followAcceleration;

        private BazingaCamera()
        {
            Position = new Vector2(0, 0);
            ViewportCenter = new Vector2(0, 0);
            _playerToFollow = null;
            _followAcceleration = 1f;
        }

        public static BazingaCamera GetCamera()
        {
            if(_camera == null)
            {
                _camera = new BazingaCamera();
            }

            return _camera;
        }

        public void SetPlayerToFollow(Player playerToFollow)
        {
            _playerToFollow = playerToFollow;
        }

        public Matrix TranslationMatrix
        {
            get
            {
                return Matrix.CreateTranslation(-(int)Position.X,
                   -(int)Position.Y, 0) *
                   Matrix.CreateRotationZ(Rotation) *
                   Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *
                   Matrix.CreateTranslation(new Vector3(ViewportCenter, 0));
            }
        }

        public void Update(GameTime gameTime, KeyboardState keyboardState)
        {
            if (_playerToFollow == null)
            {
                if (keyboardState.IsKeyDown(Keys.D))
                {
                    BazingaCamera.GetCamera().Position.X += 10;
                }

                if (keyboardState.IsKeyDown(Keys.A))
                {
                    BazingaCamera.GetCamera().Position.X -= 10;
                }

                if (keyboardState.IsKeyDown(Keys.W))
                {
                    BazingaCamera.GetCamera().Position.Y -= 10;
                }

                if (keyboardState.IsKeyDown(Keys.S))
                { 
                    BazingaCamera.GetCamera().Position.Y += 10;
                }
            }
            else
            {
                float border = 4.5f;
                bool moved = false;
                //float delta = 3f + ConvertUnits.ToSimUnits(Math.Abs(
                //    Vector2.Distance(_playerToFollow.Body.Position, ConvertUnits.ToSimUnits(BazingaCamera.GetCamera().Position.X + 1920 / 2, BazingaCamera.GetCamera().Position.X + 1080 / 2))
                //    ))*50;

                if (_playerToFollow.Body.Position.X - ConvertUnits.ToSimUnits(BazingaCamera.GetCamera().Position.X) >= ConvertUnits.ToSimUnits(1920) - border)
                {
                    BazingaCamera.GetCamera().Position.X += _followAcceleration;
                    _followAcceleration += 0.6f;
                    moved = true;
                }

                if (_playerToFollow.Body.Position.X - ConvertUnits.ToSimUnits(BazingaCamera.GetCamera().Position.X) <= border)
                {
                    BazingaCamera.GetCamera().Position.X -= _followAcceleration;
                    _followAcceleration += 0.6f;
                    moved = true;
                }

                if (_playerToFollow.Body.Position.Y - ConvertUnits.ToSimUnits(BazingaCamera.GetCamera().Position.Y) >= ConvertUnits.ToSimUnits(1080) - border)
                {
                    BazingaCamera.GetCamera().Position.Y += _followAcceleration;
                    _followAcceleration += 0.6f;
                    moved = true;
                }

                if (_playerToFollow.Body.Position.Y - ConvertUnits.ToSimUnits(BazingaCamera.GetCamera().Position.Y) <= border)
                {
                    BazingaCamera.GetCamera().Position.Y -= _followAcceleration;
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
