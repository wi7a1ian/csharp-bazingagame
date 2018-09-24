using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BazingaGame.Particles
{
    public class Particle: GameObject
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Vector2 Velocity { get; set; }
        public float Angle { get; set; }
        public float AngularVelocity { get; set; }
        public Color Color { get; set; }
        public float Size { get; set; }
        public int TTL { get; set; }
        private Vector2 Origin;
        public Body Body { get; private set; }
        private SpriteBatch _spriteBatch;

        public Particle(BazingaGame game, Texture2D texture, Vector2 position, Vector2 velocity,
            float angle, float angularVelocity, Color color, float size, int ttl)
            :base(game)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Angle = angle;
            AngularVelocity = angularVelocity;
            Color = color;
            Size = size;
            TTL = ttl;
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(this.Game.GraphicsDevice);

            Origin = new Vector2(Texture.Width / 2f, Texture.Height / 2f);

            //Body = BodyFactory.CreateRectangle(Game.World, ConvertUnits.ToSimUnits(Texture.Width), ConvertUnits.ToSimUnits(Texture.Height), 1f);
            Body = BodyFactory.CreateCircle(Game.World, ConvertUnits.ToSimUnits(Texture.Width / 1f), 1f);
            Body.BodyType = BodyType.Dynamic;
            Body.Position = ConvertUnits.ToSimUnits(Position);
            Body.Restitution = 0f;
            Body.Mass = 0.01f;

            base.LoadContent();
        }

        public void Draw(GameTime gameTime)
        {
            Rectangle sourceRectangle = new Rectangle(0, 0, Texture.Width, Texture.Height);
            //Vector2 origin = new Vector2(Texture.Width / 2, Texture.Height / 2);

            _spriteBatch.Begin(transformMatrix: Game.Camera.GetTransformMatrix());
            _spriteBatch.Draw(Texture, ConvertUnits.ToDisplayUnits(Body.Position), sourceRectangle, Color,
                Angle, Origin, Size, SpriteEffects.None, 1f);
            _spriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            TTL--;
            Position += Velocity;
            Angle += AngularVelocity;

            //Body.ApplyForce(new Vector2(0, 0));

            base.Update(gameTime);
        }

        //public void Update()
        //{
        //    TTL--;
        //    Position += Velocity;
        //    Angle += AngularVelocity;
        //}
    }
}
