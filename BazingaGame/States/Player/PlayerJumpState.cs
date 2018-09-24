using BazingaGame.Animations;
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
    class PlayerJumpState : IPlayerState
    {
        private const int FixtureRadiusX = 31;
        private const int FixtureRadiusY = 65;
        private const float JumpYForce = -3.1f;
        private const float JumpXForce = -90f;
        private readonly Vector2 FixtureOffset = new Vector2(-6, -3);
        private TimeSpan _lastJumpTime = TimeSpan.Zero;

        public void Enter(BazingaPlayer player)
        {
            var offset = FixtureOffset * new Vector2(player.Animation.IsFlippedHorizontally ? -1 : 1, 1);
            var xForce = JumpXForce * (player.Animation.IsFlippedHorizontally ? -1 : 1);

            player.SetBodyFixture(FixtureRadiusX, FixtureRadiusY, offset);
            player.Animation.PlaySprite(SpriteState.Jump, false);
            player.Body.ApplyLinearImpulse(new Vector2(0, JumpYForce));

            if(Math.Abs(player.Body.LinearVelocity.X) >= 1)
                player.Body.ApplyForce(new Vector2(xForce, 0));
        }

        public IPlayerState HandleInput(BazingaPlayer player, KeyboardState input)
        {
            return null;
        }

        public IPlayerState Update(BazingaPlayer player, GameTime gameTime)
        {
            if (_lastJumpTime == TimeSpan.Zero)
            {
                _lastJumpTime = gameTime.TotalGameTime;
            }

            // TODO: remove double jump

            //if ((gameTime.TotalGameTime - _lastJumpTime).TotalSeconds > 0.5f
            //        && player.Body.LinearVelocity.Y <= 0 && player.Body.LinearVelocity.Y > -0.01)
            if (player.Body.LinearVelocity.Y <= 0 && player.Body.LinearVelocity.Y > -0.01)
            {
                return new PlayerIdleState();
                //while (player.Body.ContactList.Next != null)
                //{
                //    if ((player.Body.ContactList.Contact.FixtureB.CollisionCategories & Category.Cat2) == Category.Cat2)
                //    {
                //        return new PlayerIdleState();
                //    }
                //}
            }
            
            return null;
        }

        public void Exit(BazingaPlayer player)
        {
            // Nop
        }
    }
}
