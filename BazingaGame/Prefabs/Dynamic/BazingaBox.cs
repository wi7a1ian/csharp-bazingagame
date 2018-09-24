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
using BazingaGame.GameCamera;

namespace BazingaGame.Prefabs
{
    public sealed class BazingaBox : DrawableGameComponent
    {

        private float _scale = 1f;
        private SpriteBatch _spriteBatch;
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

        public BazingaBox(BazingaGame game, float initialX, float initialY)
            : base(game)
        {
            // Nop
            DrawOrder = 10;
            _initialX = initialX;
            _initialY = initialY;
        }

        public override void Initialize()
        {
            Texture = Game.Content.Load<Texture2D>("MapObjects/Crate");
            Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);

            Body = BodyFactory.CreateRectangle((Game as BazingaGame).World, ConvertUnits.ToSimUnits(Texture.Width), ConvertUnits.ToSimUnits(Texture.Height), 1f);
            Body.BodyType = BodyType.Dynamic;
            Body.Position = ConvertUnits.ToSimUnits(_initialX, _initialY);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            base.LoadContent();
        }

        public override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
        {
            _spriteBatch.Begin(transformMatrix: BazingaCamera.GetCamera().TranslationMatrix);
            _spriteBatch.Draw(Texture, ConvertUnits.ToDisplayUnits(Body.Position), null, Color.White, Body.Rotation, Origin, _scale, SpriteEffects.None, 1f);
            _spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}
