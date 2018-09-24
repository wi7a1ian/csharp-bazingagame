using BazingaGame.Prefabs;
using BazingaGame.Sounds;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BazingaGame.States.Player
{
    class PlayerSlideState : IGameComponentState
    {
        private const int FixtureRadiusX = 40;
        private const int FixtureRadiusY = 35;
        private readonly Vector2 FixtureOffset = new Vector2(-10, 29);

        private const float MinSlideVelocity = 0.6f;
        private const float SlideForceFactorX = 1.2f;
        private const float SlideForceFactorY = -1;
        private readonly TimeSpan MinSlideTimeSpan = TimeSpan.FromMilliseconds(500);
        private TimeSpan _slideStartTime = TimeSpan.Zero;

        private const string SoundEffect = @"Sounds/slide";

        private BazingaPlayer player;

        public void EnterState(StatefulGameComponent target)
        {
            player = target as BazingaPlayer;

            var offset = FixtureOffset * new Vector2(player.Animation.IsFlippedHorizontally ? -1 : 1, 1);

            player.SetBodyFixture(FixtureRadiusX, FixtureRadiusY, offset);
            player.Animation.Repeat = false;
            player.Animation.PlaySprite(Animations.SpriteState.Slide);
            player.Body.OnCollision += PlayerCollisionHandler;

            player.Sounds.PlaySound(SoundEffect, false);
        }

        /// <summary>
        /// We want to kick the boxes a bit during slide.
        /// </summary>
        /// <returns></returns>
        private bool PlayerCollisionHandler(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            if ( (fixtureB.CollisionCategories & Category.Cat3) == Category.Cat3)
            {
                fixtureB.Body.ApplyLinearImpulse(new Vector2(SlideForceFactorX * fixtureA.Body.LinearVelocity.X, SlideForceFactorY * fixtureA.Body.LinearVelocity.X));
            }

            return true;
        }

        public IGameComponentState HandleInput(KeyboardState input)
        {
            return null;
        }

        public IGameComponentState Update(GameTime gameTime)
        {
            if(_slideStartTime == TimeSpan.Zero)
            {
                _slideStartTime = gameTime.TotalGameTime;
            }

            if (Math.Abs(player.Body.LinearVelocity.X) <= MinSlideVelocity && gameTime.TotalGameTime - _slideStartTime >= MinSlideTimeSpan)
                return new PlayerIdleState();
            else
                return null;
        }

        public void ExitState()
        {
            player.Body.OnCollision -= PlayerCollisionHandler;
        }

    }
}
