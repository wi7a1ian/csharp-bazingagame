using BazingaGame.Animations;
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
    class PlayerJumpState : IGameComponentState
    {
        private const int FixtureRadiusX = 31;
        private const int FixtureRadiusY = 65;
        private const float JumpYForce = -3.1f;
        private const float JumpXForce = -90f;
        private const float AirWalkXForce = 0.1f;
        private const float MaxXVelocity = 1.0f;
        private readonly Vector2 FixtureOffset = new Vector2(-6, -3);
        private const string SoundEffect = @"Sounds/jump";

        private TimeSpan lastJumpTime = TimeSpan.Zero;
        private bool isOnTheFloor = false;

        private BazingaPlayer player;

        public void EnterState(StatefulGameComponent target)
        {
            player = target as BazingaPlayer;

            var xForce = JumpXForce * (player.Animation.IsFlippedHorizontally ? -1 : 1);

            player.Animation.PlaySprite(SpriteState.Jump, false);
            player.Body.ApplyLinearImpulse(new Vector2(0, JumpYForce));

            UpdateBodyFixture(player, player.Animation.IsFlippedHorizontally);

            if(Math.Abs(player.Body.LinearVelocity.X) >= 1)
                player.Body.ApplyForce(new Vector2(xForce, 0));

            player.Sounds.PlaySound(SoundEffect, false);
        }

        public IGameComponentState HandleInput(KeyboardState input)
        {
            if (input.IsKeyDown(Keys.Right))
            {
                if (player.Body.LinearVelocity.X < MaxXVelocity)
                {
                    UpdateBodyFixture(player, false);
                    player.Body.ApplyLinearImpulse(new Vector2(AirWalkXForce, 0));
                }
            }
            else if (input.IsKeyDown(Keys.Left))
            {
                if (player.Body.LinearVelocity.X > -MaxXVelocity)
                {
                    UpdateBodyFixture(player, true);
                    player.Body.ApplyLinearImpulse(new Vector2(-AirWalkXForce, 0));
                }
            }

            return null;
        }

        private void UpdateBodyFixture(BazingaPlayer player, bool flip)
        {
            player.Body.OnCollision -= OnGroundCollision;
            player.Animation.FlipHorizontally(flip);

            var offset = FixtureOffset * new Vector2(player.Animation.IsFlippedHorizontally ? -1 : 1, 1);
            player.SetBodyFixture(FixtureRadiusX, FixtureRadiusY, offset);

            player.Body.OnCollision += OnGroundCollision;
        }

        private bool OnGroundCollision(Fixture fixtureA, Fixture fixtureB, Contact contact)
        {
            bool isCollidingWithSolidObject = (fixtureB.CollisionCategories & BazingaCollisionGroups.SolidObject) > 0;
            if (contact.IsTouching && isCollidingWithSolidObject)
            {
                bool isLandingFromAbove = Math.Abs(contact.Manifold.LocalNormal.Y) >= 1;

                if (isLandingFromAbove)
                {
                    isOnTheFloor = true;
                }
            }

            return true;
        }

        public IGameComponentState Update(GameTime gameTime)
        {
            if (lastJumpTime == TimeSpan.Zero)
            {
                lastJumpTime = gameTime.TotalGameTime;
            }

            if (isOnTheFloor)
            {
                return new PlayerIdleState();
            }
            
            return null;
        }

        public void ExitState()
        {
            player.Body.OnCollision -= OnGroundCollision;
        }
    }
}
