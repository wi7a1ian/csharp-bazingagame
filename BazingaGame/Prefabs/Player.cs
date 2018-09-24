using BazingaGame.Animations;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BazingaGame.GameCamera;

namespace BazingaGame.Prefabs
{
    public class Player : DrawableGameComponent
    {
        SpriteBatch _spriteBatch;
        private AnimatedSprite _animatedSprite;
        private float _initialX;
        private float _initialY;

        public Body Body { get; private set; }
        public Vector2 Origin { get; private set; }

        private TimeSpan _lastJumpTime;
        private TimeSpan _lastSlideTime;

        public enum PlayerState : int
        {
            Dead = 0,
            Idle = 1,
            Jump = 2,
            Melee = 3,
            Run = 4,
            Shoot = 5,
            Slide = 6
        }

	    public enum Orientation
	    {
		    Left = -1,
			Right = 1
	    }


        public Player(Game game, float initialX, float initialY)
            : base(game)
        {
            _initialX = initialX;
            _initialY = initialY;
            DrawOrder = 100;

        }

        public override void Initialize()
        {
            Texture2D texture = this.Game.Content.Load<Texture2D>("PlayerSprite");

            var spriteMap = new List<int>
            {
                10, 10, 10, 7, 8, 3, 5
            };

            _animatedSprite = new AnimatedSprite(texture, spriteMap);
            _animatedSprite.CurrentRow = (int)PlayerState.Idle;

            Origin = new Vector2(_animatedSprite.FrameWidth / 2f, _animatedSprite.FrameHeight / 2f);

            Body = BodyFactory.CreateRectangle((Game as BazingaGame).World, ConvertUnits.ToSimUnits(_animatedSprite.FrameWidth), ConvertUnits.ToSimUnits(_animatedSprite.FrameHeight), 1f);
            Body.BodyType = BodyType.Dynamic;
            Body.Position = ConvertUnits.ToSimUnits(_initialX, _initialY);
            Body.FixedRotation = true;
            Body.Friction = 1;
            Body.Restitution = 0.2f;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            float xForce = 0;
            float yForce = 0;
            bool isDirectionKeyDown = false;

            if (_animatedSprite.CurrentRow != (int)PlayerState.Dead)
            {
                if (kState.IsKeyDown(Keys.Right))
                {
                    if (Body.LinearVelocity.X < 5)
                    {
                        xForce = 50;
                    }

                    _animatedSprite.Effect = SpriteEffects.None;
                    _animatedSprite.Repeat = true;
                    _animatedSprite.CurrentRow = ((int)PlayerState.Run);
                    isDirectionKeyDown = true;
                }

                if (kState.IsKeyDown(Keys.Left))
                {
                    if (Body.LinearVelocity.X > -5)
                    {
                        xForce = -50;
                    }

                    _animatedSprite.Effect = SpriteEffects.FlipHorizontally;
                    _animatedSprite.Repeat = true;
                    _animatedSprite.CurrentRow = (int)PlayerState.Run;
                    isDirectionKeyDown = true;
                }

                if (kState.IsKeyDown(Keys.Up))
                {
                    if ((gameTime.TotalGameTime - _lastJumpTime).TotalSeconds > 0.5f
                        && Body.LinearVelocity.Y <= 0 && Body.LinearVelocity.Y > -0.01)
                    {
                        yForce = -600;
                        _lastJumpTime = gameTime.TotalGameTime;
                        _animatedSprite.Repeat = false;
                        _animatedSprite.CurrentRow = (int)PlayerState.Jump;
                    }
                    isDirectionKeyDown = true;
                }

                if (kState.IsKeyDown(Keys.Down))
                {
                    if ((gameTime.TotalGameTime - _lastSlideTime).TotalSeconds > 1)
                    {
                        xForce = (_animatedSprite.Effect == SpriteEffects.None ? 1 : -1) * 800;
                        _lastSlideTime = gameTime.TotalGameTime;
                        _animatedSprite.Repeat = false;
                        _animatedSprite.CurrentRow = (int)PlayerState.Slide;
                    }
                    isDirectionKeyDown = true;
                }

                if (kState.IsKeyDown(Keys.X))
                {
                    _animatedSprite.Repeat = false;
                    _animatedSprite.CurrentRow = (int)PlayerState.Dead;
                    isDirectionKeyDown = true;
                }
            }

            if (!isDirectionKeyDown)
            {
                if (_animatedSprite.CurrentRow == (int)PlayerState.Dead)
                {
                    _animatedSprite.CurrentRow = (int)PlayerState.Dead;
                }
                else// if (_animatedSprite.AnimationCompleted)
                {
                    xForce = Body.LinearVelocity.X * -10;
                    _animatedSprite.Repeat = true;
                    _animatedSprite.CurrentRow = (int)PlayerState.Idle;
                }
            }

            _animatedSprite.Update();

            Body.ApplyForce(new Vector2(xForce, yForce));

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: BazingaCamera.GetCamera().TranslationMatrix);
            _animatedSprite.Draw(_spriteBatch, ConvertUnits.ToDisplayUnits(Body.Position), Body.Rotation, Origin);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
