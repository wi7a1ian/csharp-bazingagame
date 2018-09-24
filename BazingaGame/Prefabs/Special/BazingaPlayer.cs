using BazingaGame.Animations;
using BazingaGame.Sounds;
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
    public class BazingaPlayer : StatefulGameComponent
    {
        public const float BodyDensity = 1.0f;
        private const int AnimationFps = 18;
        private const string SpriteTexture = "PlayerSprite";
        private readonly List<int> SpriteMap = new List<int>
        {
            10, 10, 10, 7, 8, 3, 5
        };

        private float _initialX;
        private float _initialY;

        public Body Body { get; private set; }
        public Vector2 Origin { get; private set; }
        public AnimatedSprite Animation { get; private set; }
        public SoundManager Sounds { get; private set; }

        public BazingaPlayer(BazingaGame game, float initialX, float initialY)
            : base(game)
        {
            _initialX = initialX;
            _initialY = initialY;
            DrawOrder = 100;
        }

        public override void Initialize()
        {
            State = new PlayerIdleState();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            Texture2D texture = this.Game.Content.Load<Texture2D>(SpriteTexture);
            Animation = new AnimatedSprite(texture, SpriteMap, AnimationFps);

            Origin = new Vector2(Animation.FrameWidth / 2f, Animation.FrameHeight / 2f);

            SetDefaultBodyFixture();

            Sounds = new SoundManager(this.Game.Content);

            State.EnterState(this);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(transformMatrix: Game.Camera.GetTransformMatrix());
            Animation.Draw(SpriteBatch, ConvertUnits.ToDisplayUnits(Body.Position), Body.Rotation, Origin);
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        public void SetDefaultBodyFixture()
        {
            //Body = BodyFactory.CreateRectangle((Game as BazingaGame).World, ConvertUnits.ToSimUnits(Animation.FrameWidth), ConvertUnits.ToSimUnits(Animation.FrameHeight), 1f);
            Body = BodyFactory.CreateBody((Game as BazingaGame).World);
            Body.BodyType = BodyType.Dynamic;
            Body.Position = ConvertUnits.ToSimUnits(_initialX, _initialY);
            Body.FixedRotation = true;
            Body.Friction = 0.7f;
            Body.Restitution = 0.2f;
            Body.CollisionCategories = BazingaCollisionGroups.Player;
            Body.CollidesWith = BazingaCollisionGroups.SolidObject;
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
    }
}
