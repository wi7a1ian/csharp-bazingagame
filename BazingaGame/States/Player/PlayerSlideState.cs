using BazingaGame.Prefabs;
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
    class PlayerSlideState : IPlayerState
    {
        private const int FixtureRadiusX = 40;
        private const int FixtureRadiusY = 35;
        private readonly Vector2 FixtureOffset = new Vector2(-10, 29);

        private const float MinSlideVelocity = 0.6f;
        private const float SlideForceFactorX = 1.2f;
        private const float SlideForceFactorY = -1;
        private readonly TimeSpan MinSlideTimeSpan = TimeSpan.FromMilliseconds(500);
        private TimeSpan _slideStartTime = TimeSpan.Zero;

        public void Enter(BazingaPlayer player)
        {
            var offset = FixtureOffset * new Vector2(player.Animation.IsFlippedHorizontally ? -1 : 1, 1);

            player.SetBodyFixture(FixtureRadiusX, FixtureRadiusY, offset);
            player.Animation.Repeat = false;
            player.Animation.PlaySprite(Animations.SpriteState.Slide);
            player.Body.OnCollision += PlayerCollisionHandler;
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

        public IPlayerState HandleInput(BazingaPlayer player, KeyboardState input)
        {
            return null;
        }

        public IPlayerState Update(BazingaPlayer player, GameTime gameTime)
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

        public void Exit(BazingaPlayer player)
        {
            player.Body.OnCollision -= PlayerCollisionHandler;
        }

    }
}
