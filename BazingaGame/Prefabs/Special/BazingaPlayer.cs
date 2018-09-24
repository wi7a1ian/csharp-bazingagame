using BazingaGame.Animations;
using BazingaGame.States.Player;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BazingaGame.Prefabs
{
    /// <summary>
    /// Finite State Mchine Pattern
    /// http://gameprogrammingpatterns.com/state.html
    /// </summary>
    public class BazingaPlayer : GameObject
    {
        public const float BodyDensity = 1.0f;
        private const int AnimationFps = 18;
        private const string SpriteTexture = "PlayerSprite";
        private readonly List<int> SpriteMap = new List<int>
        {
            10, 10, 10, 7, 8, 3, 5
        };

        private SpriteBatch _spriteBatch;
        private IPlayerState _state;

        private float _initialX;
        private float _initialY;

        public Body Body { get; private set; }
        public Vector2 Origin { get; private set; }
        public AnimatedSprite Animation { get; private set; }

        public BazingaPlayer(BazingaGame game, float initialX, float initialY)
            : base(game)
        {
            _initialX = initialX;
            _initialY = initialY;
            DrawOrder = 100;
        }

        public override void Initialize()
        {
            _state = new PlayerIdleState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Texture2D texture = this.Game.Content.Load<Texture2D>(SpriteTexture);

            Animation = new AnimatedSprite(texture, SpriteMap, AnimationFps);

            Origin = new Vector2(Animation.FrameWidth / 2f, Animation.FrameHeight / 2f);

            //Body = BodyFactory.CreateRectangle((Game as BazingaGame).World, ConvertUnits.ToSimUnits(Animation.FrameWidth), ConvertUnits.ToSimUnits(Animation.FrameHeight), 1f);
            Body = BodyFactory.CreateBody((Game as BazingaGame).World);
            Body.BodyType = BodyType.Dynamic;
            Body.Position = ConvertUnits.ToSimUnits(_initialX, _initialY);
            Body.FixedRotation = true;
            Body.Friction = 0.7f;
            Body.Restitution = 0.2f;
            Body.CollisionCategories = Category.Cat1;
            Body.CollidesWith = Category.All;

            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            _state.Enter(this);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState kState = Keyboard.GetState();

            HandleInput(kState);
            UpdateState(gameTime);
            Animation.Update(gameTime);

            base.Update(gameTime);
        }

        private void UpdateState(GameTime gameTime)
        {
            if (_state != null)
            {
                IPlayerState newState = _state.Update(this, gameTime);

                if (newState != null && _state != newState)
                {
                    _state.Exit(this);
                    _state = newState;
                    _state.Enter(this);
                }
            }
        }

        public void HandleInput(KeyboardState input)
        {
            IPlayerState newState = _state.HandleInput(this, input);

            if (newState != null && _state != newState)
            {
                if (_state != null)
                {
                    _state.Exit(this);
                }

                _state = newState;
                _state.Enter(this);
            }
        }

        public void SetBodyFixture(Shape shape)
        {
            // Remove all the old fixtures
            while (Body.FixtureList.Count > 0)
            {
                Body.DestroyFixture(Body.FixtureList[0]);
            }
            //Body.FixtureList.AsParallel().ForAll(x => Body.DestroyFixture(x));

            shape.Density = BodyDensity;
            Body.CreateFixture(shape);
        }

        public void SetBodyFixture(int xRadius, int yRadius, Vector2 offset)
        {
            // Remove all the old fixtures
            while(Body.FixtureList.Count > 0)
            {
                Body.DestroyFixture(Body.FixtureList[0]);
            }

            //FixtureFactory.AttachRectangle(ConvertUnits.ToSimUnits(62), ConvertUnits.ToSimUnits(110), 1.0f, ConvertUnits.ToSimUnits(-9, 2), player.Body);
            FixtureFactory.AttachEllipse(ConvertUnits.ToSimUnits(xRadius), ConvertUnits.ToSimUnits(yRadius), 6, BodyDensity, ConvertUnits.ToSimUnits(offset), Body);
        }

        public override void Draw(GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: Game.Camera.GetTransformMatrix());
            Animation.Draw(_spriteBatch, ConvertUnits.ToDisplayUnits(Body.Position), Body.Rotation, Origin);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
