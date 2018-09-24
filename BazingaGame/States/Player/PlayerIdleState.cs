﻿using BazingaGame.Animations;
using BazingaGame.Prefabs;
using FarseerPhysics;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Factories;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BazingaGame.Sounds;

namespace BazingaGame.States.Player
{
    /// <summary>
    /// Finite State Mchine Pattern
    /// http://gameprogrammingpatterns.com/state.html
    /// </summary>
    public class PlayerIdleState : IGameComponentState
    {
        private const int FixtureRadiusX = 31;
        private const int FixtureRadiusY = 65;
        private readonly Vector2 FixtureOffset = new Vector2(-9, 0);
        private BazingaPlayer player;

        public void EnterState(StatefulGameComponent target)
        {
            this.player = target as BazingaPlayer;
            var offset = FixtureOffset * new Vector2(player.Animation.IsFlippedHorizontally ? -1 : 1, 1);
            player.SetBodyFixture(FixtureRadiusX, FixtureRadiusY, offset);
            player.Animation.PlaySprite(SpriteState.Idle, true);
        }

        public IGameComponentState HandleInput(KeyboardState input)
        {
            
            if (input.IsKeyDown(Keys.Right) || input.IsKeyDown(Keys.Left))
            {
                return new PlayerRunningState();
            }
            else if (input.IsKeyDown(Keys.Up))
            {
                return new PlayerJumpState();
            }
            else if (input.IsKeyDown(Keys.LeftControl))
            {
                return new PlayerAttackState();
            }
            else
            {
                return null;
            }
        }

        public IGameComponentState Update(GameTime gameTime)
        {
            float xForce = 0;
            float yForce = 0;
            xForce = player.Body.LinearVelocity.X * -10;

            player.Body.ApplyForce(new Vector2(xForce, yForce));

            return null;
        }

        public void ExitState()
        {
            // Nop
        }
    }
}
