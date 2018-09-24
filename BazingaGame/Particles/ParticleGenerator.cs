using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BazingaGame.Particles
{
    public class ParticleEngine : GameObject
    {
        private Random random;
        public Vector2 EmitterLocation { get; set; }
        private List<Particle> particles;
        private Texture2D texture;
        private int particlesAdded = 0;
        private Particle prev = null;
        private bool _jointsAdded = false;

        public ParticleEngine(BazingaGame game, Texture2D texture, Vector2 location)
            : base(game)
        {
            EmitterLocation = location;
            this.texture = texture;
            this.particles = new List<Particle>();
            random = new Random();
        }

        public void Update()
        {
            int total = 1;

            

            if (particlesAdded <= 300)
            {
                
                //for (int i = 0; i < total; i++)
                //{
                //    var particle = GenerateNewParticle();
                //    Game.Components.Add(particle);
                //    particles.Add(particle);
                //    particlesAdded++;

                //    if (prev != null)
                //    {
                //        //DistanceJoint dj = JointFactory.CreateDistanceJoint(Game.World, prev.Body, particle.Body, Vector2.Zero, Vector2.Zero);
                //        //dj.Frequency = 4.0f;
                //        //dj.DampingRatio = 0.5f;
                //        //dj.Breakpoint = 2f;

                //        //DistanceJoint djf = JointFactory.CreateDistanceJoint(world, prev.Body, currentFixture, Vector2.Zero, Vector2.Zero);
                //        //dj.Frequency = 4.0f;
                //        //dj.DampingRatio = 0.5f;
                //    }

                //    prev = particle;
                //}
            }
            else if (!_jointsAdded)
            {
                //foreach (var particle in particles)
                //{
                //    foreach (var connectedParticle in particles.Where(p => p != particle))
                //    {
                //        DistanceJoint dj = JointFactory.CreateDistanceJoint(Game.World, particle.Body, connectedParticle.Body, Vector2.Zero, Vector2.Zero);
                //        dj.Frequency = 1.0f;
                //        dj.DampingRatio = 0.5f;
                //        dj.Breakpoint = 0.5f;
                //    }   
                //}

                _jointsAdded = true;
            }

            for (int particle = 0; particle < particles.Count; particle++)
            {
                //particles[particle].Update();
                if (particles[particle].TTL <= 0)
                {
                    Game.Components.Remove(particles[particle]);
                    particles[particle].Body.Dispose();
                    particles.RemoveAt(particle);
                    
                    
                    particle--;
                }
            }
        }

        private Particle GenerateNewParticle()
        {
            Vector2 position = EmitterLocation;
            Vector2 velocity = new Vector2(
                                    100f * (float)(random.NextDouble() * 2 - 1),
                                    100f * (float)(random.NextDouble() * 2 - 1));
            float angle = 0;
            float angularVelocity = 0.1f * (float)(random.NextDouble() * 2 - 1);
            //Color color = new Color(
            //            (float)random.NextDouble(),
            //            (float)random.NextDouble(),
            //            (float)random.NextDouble());
            Color color = new Color(
                        255,
                        255,
                        255
                        );
            //float size = (float)random.NextDouble();
            float size = 1f;
            int ttl = 2200 + random.Next(40);

            return new Particle(Game, texture, position, velocity, angle, angularVelocity, color, size, ttl);
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.Begin(transformMatrix: Game.Camera.GetTransformMatrix());
            for (int index = 0; index < particles.Count; index++)
            {
                particles[index].Draw(gameTime);
            }
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        //public void Draw(SpriteBatch spriteBatch)
        //{
        //    spriteBatch.Begin(transformMatrix: Game.Camera.GetTransformMatrix());
        //    for (int index = 0; index < particles.Count; index++)
        //    {
        //        particles[index].Draw(spriteBatch);
        //    }
        //    spriteBatch.End();
        //}
    }
}
