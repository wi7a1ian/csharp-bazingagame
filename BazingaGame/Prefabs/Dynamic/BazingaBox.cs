using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Factories;
using FarseerPhysics;
using BazingaGame.Display;

namespace BazingaGame.Prefabs
{
    public sealed class Box : GameObject
    {

        private float _scale = 1f;
        
        private float _initialX;
        private float _initialY;

        public Body Body { get; private set; }
        public Vector2 Origin { get; private set; }
        public Texture2D Texture{ get; private set; }

        public float X
        {
            get { return Body.Position.X; }
        }

        public float Y
        {
            get { return Body.Position.Y; }
        }

        public Box(BazingaGame game, float initialX, float initialY)
            : base(game)
        {
            // Nop
            DrawOrder = 10;
            _initialX = initialX;
            _initialY = initialY;
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            Texture = Game.Content.Load<Texture2D>("MapObjects/Crate");
            Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);

            Body = BodyFactory.CreateRectangle((Game as BazingaGame).World, ConvertUnits.ToSimUnits(Texture.Width), ConvertUnits.ToSimUnits(Texture.Height), 1f);
            Body.BodyType = BodyType.Dynamic;
            Body.Position = ConvertUnits.ToSimUnits(_initialX, _initialY);
            Body.Restitution = 0f;
            Body.CollisionCategories = Category.Cat3;
            Body.CollidesWith = Category.All;

            base.LoadContent();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            SpriteBatch.Begin(transformMatrix: Game.Camera.GetTransformMatrix());
            SpriteBatch.Draw(Texture, ConvertUnits.ToDisplayUnits(Body.Position), null, Color.White, Body.Rotation, Origin, _scale, SpriteEffects.None, 1f);
            SpriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
